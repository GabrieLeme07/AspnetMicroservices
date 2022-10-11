using IdentityServer.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddIdentityServer()
 .AddInMemoryClients(Config.Clients)
 .AddInMemoryIdentityResources(Config.IdentityResources)
 .AddInMemoryApiResources(Config.ApiResources)
 .AddInMemoryApiScopes(Config.ApiScopes)
 .AddTestUsers(Config.TestUsers)
 .AddDeveloperSigningCredential();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseIdentityServer();
app.UseAuthorization();

app.MapControllers();

app.Run();
