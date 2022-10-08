using AutoMapper;
using MediatR;
using Otto.models;
using Otto.users;
using Otto.users.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMediatR(typeof(Program).GetTypeInfo().Assembly);
builder.Services.AddDbContext<OttoDbContext>();

builder.Services.AddSingleton<UsersService>();

var mapperConfig = new MapperConfiguration(m =>
{
    m.AddProfile(new MappingProfile());
});


builder.Services.AddSingleton(mapperConfig.CreateMapper());

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

DBMigrations.PrepOtto(app);

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();