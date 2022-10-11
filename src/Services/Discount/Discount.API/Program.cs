using Discount.API.Extensions;
using Discount.API.Repositories;
using Microsoft.IdentityModel.Tokens;

var host = WebApplication.CreateBuilder(args);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

var app = builder.Build();

app.MigrateDatabase<Program>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
