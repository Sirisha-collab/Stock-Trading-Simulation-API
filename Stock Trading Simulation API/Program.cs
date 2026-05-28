using Stock_Trading_Simulation_API.Application;
using Stock_Trading_Simulation_API.Infrastrcuture;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IOrderBook, OrderBook>();
builder.Services.AddSingleton<OrderQueue>();
// Register MatchingEngine as concrete class
builder.Services.AddSingleton<MatchingEngine>();

// Map interface to same instance
builder.Services.AddSingleton<IMatchingEngine>(sp =>
    sp.GetRequiredService<MatchingEngine>());

builder.Services.AddSingleton<OrderProcessor>();
builder.Services.AddHostedService<EngineWorker>();
builder.Services.AddSwaggerGen();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Enable Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Map MVC Route for  UI
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
