using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"token/")), //用于定位资源的文件系统
    RequestPath = new PathString("/token") //请求地址
});

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"image/")), //用于定位资源的文件系统
    RequestPath = new PathString("/image") //请求地址
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/metadata/{tokenid}", (string tokenid) =>
{
    try
    {
        var FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("{0}.json", tokenid));
        using var stream = new StreamReader(FilePath);
        JsonTextReader reader = new JsonTextReader(stream);
        JObject OStream = (JObject)JToken.ReadFrom(reader);
        return OStream.ToString();
    }
    catch
    {
        return "";
    }
});

app.Run();
