using Stock_Trading_Simulation_API.Application;
using Stock_Trading_Simulation_API.Infrastrcuture;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// OpenAPI / Swagger
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// Core services
builder.Services.AddSingleton<IOrderBook, OrderBook>();
builder.Services.AddSingleton<IMatchingEngine, MatchingEngine>();
builder.Services.AddSingleton<OrderProcessor>();

// Background worker
builder.Services.AddHostedService<EngineWorker>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Stocks}/{action=Index}/{id?}");

app.Run();
