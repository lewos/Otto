using Microsoft.EntityFrameworkCore;
using Otto.models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<OttoDbContext>();
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


app.MapGet("/api/companies", async (OttoDbContext db) =>
{
    var companies = await db.Companies.ToListAsync();
    //return GetListStockDTO(aux);
    return Results.Ok(companies);
});

app.MapGet("/api/companies/{id}", async (OttoDbContext db, int id) =>
{
    var company = await db.Companies.FindAsync(id);
    if (company is not null)
        return Results.Ok(company);
    return Results.NotFound();
});

app.MapGet("/api/companies/name/{id}", async (OttoDbContext db, string id) =>
{
    var company = await db.Companies.Where(c=> c.Name == id).FirstOrDefaultAsync();
    if (company is not null)
        return Results.Ok(company);
    return Results.NotFound();

});

app.MapGet("/api/companies/cuit/{id}", async (OttoDbContext db, string id) =>
{
    var company = await db.Companies.Where(c => c.CUIT == id).FirstOrDefaultAsync();
    if (company is not null)
        return Results.Ok(company);
    return Results.NotFound();
});

app.MapPost("/api/companies", async (OttoDbContext db, Company request) =>
{
    try
    {
        //check if userId exist
        var user = await db.Users.Where(u => u.Id == request.CreatedByUserId && u.Rol == "Administrador").FirstOrDefaultAsync();
        if (user is null)
            return Results.Conflict("El usuario no existe o no tiene un rol de Administrador");

        // check if cuit o name is alredy in db
        var company = await db.Companies.Where(c => c.Name == request.Name || c.CUIT == request.CUIT).FirstOrDefaultAsync();
        if (company is not null)
            return Results.Conflict("Nombre o CUIT ya se encuentran registrados");


        await db.Companies.AddAsync(request);
        await db.SaveChangesAsync();

        //update user's companyId
        user.CompanyId = request.Id;
        //db.Users.Update(user);
        await db.SaveChangesAsync();

        return Results.Created($"/api/companies/{request.Id}", request);
    }
    catch (Exception ex )
    {
        var a = ex;
        throw;
    }    
});

app.MapPut("/api/companies/{id}", async (OttoDbContext db, Company request, int id) =>
{
    var company = await db.Companies.FindAsync(id);
    if (company is null) return Results.NotFound();

    //// check if user is in company and is admin
    //var user = await db.Users.FindAsync(company.CreatedByUserId);

    UpdateFields(request, company);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/api/companies/{id}", async (OttoDbContext db, int id) =>
{
    // check if user is in company and is admin

    //update user's company id

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
    if (!string.IsNullOrEmpty(request.PostalCode))
        company.PostalCode = request.PostalCode;
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