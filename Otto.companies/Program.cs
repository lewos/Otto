using Microsoft.EntityFrameworkCore;
using Otto.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<OttoDbContext>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.MapGet("/api/companies", async (OttoDbContext db) =>
{
    var companies = await db.Companies.ToListAsync();
    //return GetListStockDTO(aux);
    return companies;
});

app.MapGet("/api/companies/{id}", async (OttoDbContext db, int id) =>
{
    var company = await db.Companies.FindAsync(id);
    if (company is not null)
        return company;
    return null;
});

app.MapPost("/api/companies", async (OttoDbContext db, Company request) =>
{
    await db.Companies.AddAsync(request);
    await db.SaveChangesAsync();
    return Results.Created($"/api/companies/{request.Id}", request);
});

app.MapPut("/api/companies/{id}", async (OttoDbContext db, Company request, int id) =>
{
    //var updateStock = StockMapper.GetStock(dto);
    var company = await db.Companies.FindAsync(id);
    if (company is null) return Results.NotFound();
    UpdateFields(request, company);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/api/companies/{id}", async (OttoDbContext db, int id) =>
{
    var company = await db.Companies.FindAsync(id);
    if (company is null)
    {
        return Results.NotFound();
    }
    db.Companies.Remove(company);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.Run();


static void UpdateFields(Company request, Company? company)
{
    if (request.Id != null)
        company.Id = request.Id;
    if (!string.IsNullOrEmpty (request.Name))
        company.Name = request.Name;
    if (!string.IsNullOrEmpty(request.CUIT))
        company.CUIT = request.CUIT;
    if (!string.IsNullOrEmpty(request.Owner))
        company.Owner = request.Owner;
    if (!string.IsNullOrEmpty(request.Street))
        company.Street = request.Street;
    if (!string.IsNullOrEmpty(request.City))
        company.City = request.City;
    if (!string.IsNullOrEmpty(request.Province))
        company.Province = request.Province;
    if (!string.IsNullOrEmpty(request.Phone))
        company.Phone = request.Phone;
    if (!string.IsNullOrEmpty(request.Email))
        company.Email = request.Email;
    if (request.CreatedByUserId  != null)
        company.CreatedByUserId = request.CreatedByUserId;
}