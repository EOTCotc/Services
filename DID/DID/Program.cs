using AspNetCoreRateLimit;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DID.Common;
using DID.Models.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Net;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = new ConfigurationBuilder()
                                    .AddJsonFile("appsettings.json")
                                    .AddJsonFile("Config/IpRateLimit.json").Build();
// Add services to the container.

//全局添加需要认证
builder.Services.AddControllers().AddMvcOptions(options => options.Filters.Add(new AuthorizeFilter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton(new AppSettings(configuration));
builder.Services.AddScoped<ICurrentUser, CurrentUser>();

#region log4net
builder.Services.AddLogging(cfg =>
{
    cfg.AddLog4Net("Config/log4net.config");
});
#endregion

#region Autofac注入
builder.Host
.UseServiceProviderFactory(new AutofacServiceProviderFactory())
.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterAssemblyTypes(Assembly.Load("DID.Services"))
    .Where(t => t.Name.EndsWith("Service"))
    .AsImplementedInterfaces();
    builder.RegisterAssemblyTypes(Assembly.Load("Dao.Services"))
   .Where(t => t.Name.EndsWith("Service"))
   .AsImplementedInterfaces();
    builder.RegisterAssemblyTypes(Assembly.Load("App.Services"))
  .Where(t => t.Name.EndsWith("Service"))
  .AsImplementedInterfaces();
});
#endregion

#region IpRateLimit
// 從 appsettings.json 讀取 IpRateLimiting 設定 
builder.Services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
// 從 appsettings.json 讀取 Ip Rule 設定
builder.Services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));
// 注入 counter and IP Rules 
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
// Rate Limit configuration 設定
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
#endregion

#region Swagger
builder.Services.AddSwaggerGen(options =>
{

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "DID",
        Description = "DID接口"
    });
    //添加安全定义
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "请输入token,格式为 Bearer xxxxxxxx（注意中间必须有空格）",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    //添加安全要求
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme{
                Reference =new OpenApiReference{
                    Type = ReferenceType.SecurityScheme,
                    Id ="Bearer"
                }
            },new string[]{ }
        }
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), true);

    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"Dao.Controllers.xml"), true);
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"Dao.Entity.xml"), true);
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"Dao.Models.xml"), true);
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"Dao.Services.xml"), true);

    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"DID.Common.xml"), true);
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"DID.Entity.xml"), true);
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"DID.Models.xml"), true);
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"DID.Services.xml"), true);

    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"App.Controllers.xml"), true);
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"App.Entity.xml"), true);
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"App.Models.xml"), true);
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"App.Services.xml"), true);
});
#endregion

#region jwt认证
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    //取出私钥
    var secretByte = Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretKey"]);
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        //验证发布者
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        //验证接收者
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Authentication:Audience"],
        //验证是否过期
        ValidateLifetime = true,
        //验证私钥
        IssuerSigningKey = new SymmetricSecurityKey(secretByte)
        //注意这是缓冲过期时间，总的有效时间等于这个时间加上jwt的过期时间，如果不配置，默认是5分钟
        //ClockSkew = TimeSpan.FromSeconds(4)
    };
});
#endregion

#region MemoryCache
builder.Services.AddMemoryCache();
#endregion

#region 跨域
builder.Services.AddCors(c =>
{
    if (AppSettings.GetValue<bool>("Cors", "EnableAllIPs"))
    {
        //允许任意跨域请求
        c.AddPolicy(AppSettings.GetValue("Cors", "PolicyName"),
            policy =>
            {
                policy
                    .SetIsOriginAllowed(host => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
    }
    else
    {
        c.AddPolicy(AppSettings.GetValue("Cors", "PolicyName"),
            policy =>
            {
                policy
                    .WithOrigins(AppSettings.GetValue("Cors", "IPs").Split(','))
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    }
});
#endregion

//builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);//请求参数为空判断

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

//静态文件
var upload = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Images/");
if (!Directory.Exists(upload)) Directory.CreateDirectory(upload);
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(upload), //用于定位资源的文件系统
    RequestPath = new PathString("/Images" +
    "") //请求地址
});

//限流
if (AppSettings.GetValue<bool>("Middleware", "IpLimit", "Enabled"))
{
    app.UseIpRateLimiting();
}

// CORS跨域
app.UseCors(AppSettings.GetValue("Cors", "PolicyName"));
//添加jwt验证
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

#region 异常处理
app.UseExceptionHandler(builder =>
{
    builder.Run(async context =>
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        var exception = context.Features.Get<IExceptionHandlerFeature>();
        if (exception != null)
        {
            app.Logger.LogError(exception.Error.Message);
            var error = new Response { Code = 1, Message = "服务器错误!" };
            var errObj = JsonConvert.SerializeObject(error);
            await context.Response.WriteAsync(errObj).ConfigureAwait(false);
        }
    });
});
#endregion

app.Run();
