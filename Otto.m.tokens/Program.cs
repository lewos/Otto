using Otto.m.tokens.Services;
using Otto.models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<MTokenService>();
builder.Services.AddDbContext<OttoDbContext>();

builder.Services.AddScoped<RefreshService>();
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
