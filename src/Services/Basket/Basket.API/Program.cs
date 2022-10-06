using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Discount.Grpc.Protos;

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

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options
    => options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]));

builder.Services.AddScoped<DiscountGrpcService>();

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
