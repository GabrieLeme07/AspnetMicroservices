using Catalog.API.IOC;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
var host = builder.Host;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.DatabaseCollection();
services.RepositoriesCollection();

builder.Services.AddAuthentication("Bearer")
 .AddJwtBearer("Bearer", options =>
 {
     options.Authority = "http://localhost:9090";
     options.RequireHttpsMetadata = false;
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateAudience = false
     };
 });

services.AddSwaggerGen();
var app = builder.Build();

app
    .UseSwagger()
    .UseSwaggerUI()
    .UseAuthentication()
    .UseAuthorization();

app.MapControllers();

app.Run();
