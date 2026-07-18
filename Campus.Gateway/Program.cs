using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("Ocelot.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddOcelot(builder.Configuration);

if (builder.Environment.IsDevelopment())
{
    // Bypass SSL certificate validation for downstream self-signed certs
    ServicePointManager.ServerCertificateValidationCallback +=
        (sender, cert, chain, sslPolicyErrors) => true;
}

var app = builder.Build();

//app.MapGet("/", () => "Hello World!");
await app.UseOcelot();

app.Run();
