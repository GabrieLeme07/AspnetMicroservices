using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder
        .Configuration.GetValue<string>("CacheSettings:ConnectionString");
});

#region IOC

//General Configs
builder.Services.AddScoped<IBasketRepository, BasketRepository>();

//Grpc Configs
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options
    => options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]));
builder.Services.AddScoped<DiscountGrpcService>();

//MassTransit-RabbitMQ Configs
builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((context, configuration) =>
    {
        configuration.Host(builder.Configuration["EventBusSettings:HostAddress"]);
    });
});

builder.Services.AddAutoMapper(typeof(Program));

#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
