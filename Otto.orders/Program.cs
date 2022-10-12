using Otto.models;
using Otto.orders.Models;
using Otto.orders.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<OttoDbContext>();
builder.Services.AddSingleton<QueueTasks>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddHostedService<QueueService>();
builder.Services.AddScoped<AccessTokenService>();

builder.Services.AddMemoryCache();

builder.Services.AddScoped<MercadolibreService>();
builder.Services.AddScoped<MOrdersService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<StockService>();
builder.Services.AddHttpClient();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(
  options => options.WithOrigins("*").AllowAnyMethod().AllowAnyHeader()
);

app.UseAuthorization();

app.MapControllers();

app.Run();
