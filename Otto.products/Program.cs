using Otto.products.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<AccessTokenService>();

builder.Services.AddMemoryCache();

builder.Services.AddScoped<MercadolibreService>();
builder.Services.AddScoped<ProductsService>();
builder.Services.AddScoped<StockService>();

builder.Services.AddScoped<TiendanubeService>();

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
