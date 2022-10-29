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

//ȫ�������Ҫ��֤
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

#region Autofacע��
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
// �� appsettings.json �xȡ IpRateLimiting �O�� 
builder.Services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
// �� appsettings.json �xȡ Ip Rule �O��
builder.Services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));
// ע�� counter and IP Rules 
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
// Rate Limit configuration �O��
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
        Description = "DID�ӿ�"
    });
    //��Ӱ�ȫ����
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "������token,��ʽΪ Bearer xxxxxxxx��ע���м�����пո�",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    //��Ӱ�ȫҪ��
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

#region jwt��֤
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    //ȡ��˽Կ
    var secretByte = Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretKey"]);
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        //��֤������
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        //��֤������
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Authentication:Audience"],
        //��֤�Ƿ����
        ValidateLifetime = true,
        //��֤˽Կ
        IssuerSigningKey = new SymmetricSecurityKey(secretByte)
        //ע�����ǻ������ʱ�䣬�ܵ���Чʱ��������ʱ�����jwt�Ĺ���ʱ�䣬��������ã�Ĭ����5����
        //ClockSkew = TimeSpan.FromSeconds(4)
    };
});
#endregion

#region MemoryCache
builder.Services.AddMemoryCache();
#endregion

#region ����
builder.Services.AddCors(c =>
{
    if (AppSettings.GetValue<bool>("Cors", "EnableAllIPs"))
    {
        //���������������
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

//builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);//�������Ϊ���ж�

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

//��̬�ļ�
var upload = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Images/");
if (!Directory.Exists(upload)) Directory.CreateDirectory(upload);
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(upload), //���ڶ�λ��Դ���ļ�ϵͳ
    RequestPath = new PathString("/Images" +
    "") //�����ַ
});

//����
if (AppSettings.GetValue<bool>("Middleware", "IpLimit", "Enabled"))
{
    app.UseIpRateLimiting();
}

// CORS����
app.UseCors(AppSettings.GetValue("Cors", "PolicyName"));
//���jwt��֤
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

#region �쳣����
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
            var error = new Response { Code = 1, Message = "����������!" };
            var errObj = JsonConvert.SerializeObject(error);
            await context.Response.WriteAsync(errObj).ConfigureAwait(false);
        }
    });
});
#endregion

app.Run();
