using Microsoft.EntityFrameworkCore;
using Otto.models;
using Otto.models.Migrations;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<OttoDbContext>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(
  options => options.WithOrigins("*").AllowAnyMethod().AllowAnyHeader()
);


app.MapGet("/api/stockrequests", async (OttoDbContext db) =>
{
    try
    {
        var requests = await db.StockRequests.ToListAsync();
        return Results.Ok(getResponses(requests));

    }
    catch (Exception ex )
    {
        var a = ex;
        throw;
    }
});

app.MapGet("/api/stockrequests/{id}", async (OttoDbContext db, int id) =>
{
    var request = await db.StockRequests.FindAsync(id);
    if (request is not null)
        return Results.Ok(getResponse(request));
    return Results.NotFound();
});

app.MapGet("/api/stockrequests/company/{id}", async (OttoDbContext db, int id) =>
{
    var requests = await db.StockRequests.Where(r => r.CompanyId == id).ToListAsync();
    if (requests is not null)
        return Results.Ok(getResponses(requests));
    return Results.NotFound();
});

app.MapGet("/api/stockrequests/company/{id}/{state}", async (OttoDbContext db, string state, int id) =>
{
    if (Enum.TryParse<State>(state, true, out State newState))
    {
        var requests = await db.StockRequests.Where(r => r.CompanyId == id && r.State == newState).ToListAsync();
        if (requests is not null)
            return Results.Ok(getResponses(requests));
        return Results.NotFound();
    }
    else
    {
        var posibleStates = Enum.GetValues(typeof(State)).Cast<State>()
                        .Select(d => (d, (int)d))
                        .ToList();

        return Results.NotFound($"Los estados posibles son: {String.Join(',', posibleStates)}");
    }
});

app.MapGet("/api/stockrequests/user/{id}", async (OttoDbContext db, int id) =>
{
    var requests = await db.StockRequests.Where(r => r.UserId == id).ToListAsync();
    if (requests is not null)
        return Results.Ok(getResponses(requests));
    return Results.NotFound();
});

app.MapGet("/api/stockrequests/user/{id}/{state}", async (OttoDbContext db, string state, int id) =>
{
    if (Enum.TryParse<State>(state, true, out State newState))
    {
        var requests = await db.StockRequests.Where(r => r.UserId == id && r.State == newState).ToListAsync();
        if (requests is not null)
            return Results.Ok(getResponses(requests));
        return Results.NotFound();
    }
    else
    {
        var posibleStates = Enum.GetValues(typeof(State)).Cast<State>()
                        .Select(d => (d, (int)d))
                        .ToList();

        return Results.NotFound($"Los estados posibles son: {String.Join(',', posibleStates)}");
    }
});

app.MapPost("/api/stockrequests", async (OttoDbContext db, StockRequest request) =>
{
    //check if userId exist
    var user = await db.Users.Where(u => u.Id == request.UserId).FirstOrDefaultAsync();
    if (user is null)
        return Results.Conflict("El usuario no existe");

    //check if companyId exist
    var company = await db.Companies.Where(c => c.Id == request.CompanyId).FirstOrDefaultAsync();
    if (company is null)
        return Results.Conflict("El fullfilment no existe");

    //check if user belongs in that company
    if (user.CompanyId != company.Id)
        return Results.Conflict("El usuario no se encuentra dentro de este fullfilment");

    //check if request exist from that user to company id that is pending from the same channel
    var stockRequest = await db.StockRequests.Where(r => r.UserId == request.UserId &&
                                                        r.CompanyId == request.CompanyId &&
                                                        r.MItemId == request.MItemId && 
                                                        r.State == State.Pendiente &&
                                                        r.Origin == request.Origin).FirstOrDefaultAsync();
    if (stockRequest is not null)
        return Results.Conflict($"La solictud ya se encuentra creada y esta con un estado {stockRequest.State.ToString()}");

    request.Created = DateTime.Now;
    request.UserIdMail = user.Mail;
    request.UserName = user.Name;
    request.UserLastName = user.LastName;

    await db.StockRequests.AddAsync(request);
    await db.SaveChangesAsync();

    return Results.Created($"/api/stockrequests/{request.Id}", getResponse(request));
});

app.MapPut("/api/stockrequests/{id}", async (OttoDbContext db, StockRequest request, int id) =>
{
    var stockRequest = await db.StockRequests.FindAsync(id);
    if (stockRequest is null) return Results.NotFound();

    //// check if user is in company and is admin
    //var user = await db.Users.FindAsync(company.CreatedByUserId);

    UpdateFields(request, stockRequest);
    stockRequest.Modified = DateTime.Now;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapPut("/api/stockrequests/{id}/{state}", async (OttoDbContext db, StockRequestApplicant request, string state, int id) =>
{
    if (Enum.TryParse<State>(state, true, out State newState) && (newState == State.Confirmado || newState == State.Rechazado))
    {
        var stockRequest = await db.StockRequests.FindAsync(id);
        if (stockRequest is null) return Results.NotFound("No existe un request con ese id");

        //verificar que el usuario aprobador exista
        var user = await db.Users.FindAsync(request.ApplicantUserId);
        if (user is null) return Results.NotFound("No existe un aprobador con ese id");

        //verificar que el usuario que esta aprobando, pertenece al fullfilment
        if (user.CompanyId == stockRequest.CompanyId && user.Rol.Equals("Administrador"))
        {
            stockRequest.State = newState;
            stockRequest.Modified = DateTime.Now;
            await db.SaveChangesAsync();

            //create stock 
            var productInStock = getProductInStock(request, stockRequest);

            await db.ProductsInStock.AddAsync(productInStock);
            var rowsAffected = await db.SaveChangesAsync();
            if (rowsAffected > 0)
                return Results.NoContent();
            else 
            {
                stockRequest.State = State.Pendiente;
                stockRequest.Modified = DateTime.Now;
                await db.SaveChangesAsync();
                return Results.Conflict("Error al crear el inventario. Porfavor intentar en unos minutos");
            }
        }

        return Results.NotFound("El usuario aprobador no pertece a la misma compania que el solicitud o no tiene el rol apropiedo");

    }
    else
    {
        return Results.NotFound($"Los estados posibles son: {String.Join(',', new List<string> { State.Confirmado.ToString(), State.Rechazado.ToString() })}");
    }
});

app.MapDelete("/api/stockrequests/{id}", async (OttoDbContext db, int id) =>
{
    var joinRequest = await db.StockRequests.FindAsync(id);
    if (joinRequest is null)
        return Results.NotFound();

    db.StockRequests.Remove(joinRequest);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.Run();

static void UpdateFields(StockRequest request, StockRequest? stockRequest)
{
    if (stockRequest.Created != request.Created)
        stockRequest.Created = request.Created;
    if (stockRequest.Modified != request.Modified)
        stockRequest.Modified = request.Modified;
    if (stockRequest.Category != request.Category)
        stockRequest.Category = request.Category;
    if (stockRequest.Code != request.Code)
        stockRequest.Code = request.Code;
    if (stockRequest.Description != request.Description)
        stockRequest.Description = request.Description;
    if (stockRequest.MItemId != request.MItemId)
        stockRequest.MItemId = request.MItemId;
    if (stockRequest.MSellerId != request.MSellerId)
        stockRequest.MSellerId = request.MSellerId;
    if (stockRequest.Name != request.Name)
        stockRequest.Name = request.Name;
    if (stockRequest.Origin != request.Origin)
        stockRequest.Origin = request.Origin;
    if (stockRequest.Quantity != request.Quantity)
        stockRequest.Quantity = request.Quantity;
    if (stockRequest.SKU != request.SKU)
        stockRequest.SKU = request.SKU;
    if (stockRequest.State != request.State)
        stockRequest.State = request.State;
    if (stockRequest.TItemId != request.TItemId)
        stockRequest.TItemId = request.TItemId;
    if (stockRequest.TSellerId != request.TSellerId)
        stockRequest.TSellerId = request.TSellerId;
    if (stockRequest.UserId != request.UserId)
        stockRequest.UserId = request.UserId;

    if (stockRequest.UserName != request.UserName)
        stockRequest.UserName = request.UserName;
    if (stockRequest.UserLastName != request.UserLastName)
        stockRequest.UserLastName = request.UserLastName;

    if (stockRequest.UserIdMail != request.UserIdMail)
        stockRequest.UserIdMail = request.UserIdMail;
    if (stockRequest.CompanyId != request.CompanyId)
        stockRequest.CompanyId = request.CompanyId;
    if (stockRequest.Size != request.Size)
        stockRequest.Size = request.Size;
    if (stockRequest.Batch != request.Batch)
        stockRequest.Batch = request.Batch;
    if (stockRequest.Location != request.Location)
        stockRequest.Location = request.Location;
    if (stockRequest.EAN != request.EAN)
        stockRequest.EAN = request.EAN;
}

static IEnumerable<StockRequestResponse> getResponses(IEnumerable<StockRequest> joinRequests)
{
    var resultado = new List<StockRequestResponse>();
    foreach (var item in joinRequests)
    {
        resultado.Add(getResponse(item));
    }
    return resultado;
}

static StockRequestResponse getResponse(StockRequest stockRequest)
{
    var response = new StockRequestResponse();
    response.Id = stockRequest.Id;
    response.Created = stockRequest.Created;
    response.Modified = stockRequest.Modified;
    response.Category = stockRequest.Category;
    response.Code = stockRequest.Code;
    response.Description = stockRequest.Description;
    response.MItemId = stockRequest.MItemId;
    response.MSellerId = stockRequest.MSellerId;
    response.Name = stockRequest.Name;
    response.Origin = stockRequest.Origin;
    response.Quantity = stockRequest.Quantity;
    response.SKU = stockRequest.SKU;
    response.State = stockRequest.State.ToString();
    response.TItemId = stockRequest.TItemId;
    response.TSellerId = stockRequest.TSellerId;
    response.UserId = stockRequest.UserId;

    response.UserName = stockRequest.UserName;
    response.UserLastName = stockRequest.UserLastName;


    response.UserIdMail = stockRequest.UserIdMail;
    response.CompanyId = stockRequest.CompanyId;
    response.Size = stockRequest.Size;
    response.Batch = stockRequest.Batch;
    response.Location = stockRequest.Location;
    response.EAN = stockRequest.EAN;
    return response;
}


static ProductInStock getProductInStock(StockRequestApplicant request, StockRequest? stockRequest)
{
    var productInStock = new ProductInStock();
    productInStock.Name = stockRequest.Name;
    productInStock.Description = stockRequest.Description;
    productInStock.Origin = stockRequest.Origin;
    productInStock.Quantity = stockRequest.Quantity;
    productInStock.MSellerId = stockRequest.MSellerId;
    productInStock.TSellerId = stockRequest.TSellerId;
    productInStock.MItemId = stockRequest.MItemId;
    productInStock.TItemId = stockRequest.TItemId;
    productInStock.SKU = stockRequest.SKU;
    productInStock.Code = stockRequest.Code;
    productInStock.Category = stockRequest.Category;
    productInStock.State = stockRequest.State;
    productInStock.StateDescription = stockRequest.StateDescription;
    productInStock.UserId = stockRequest.UserId;

    productInStock.UserName = stockRequest.UserName;
    productInStock.UserLastName = stockRequest.UserLastName;

    productInStock.UserIdMail = stockRequest.UserIdMail;
    productInStock.CompanyId = stockRequest.CompanyId;
    productInStock.Size = stockRequest.Size;
    productInStock.Batch = stockRequest.Batch;
    productInStock.EAN = stockRequest.EAN;

    productInStock.Location = request.Location;

    return productInStock;
}

public class StockRequestApplicant
{
    [JsonPropertyName("modifiedByUserId")]
    public int ApplicantUserId { get; set; }

    [JsonPropertyName("location")]
    public string Location { get; set; }
}

public class StockRequestResponse
{
    public int Id { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? Modified { get; set; }
    public string? Category { get; set; }
    public string? Code { get; set; }
    public string? Description { get; set; }
    public string? MItemId { get; set; }
    public string? MSellerId { get; set; }
    public string? Name { get; set; }
    public string Origin { get; set; }
    public int Quantity { get; set; }
    public string? SKU { get; set; }
    public string? State { get; set; }
    public string? TItemId { get; set; }
    public string? TSellerId { get; set; }
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public string? UserLastName { get; set; }
    public string? UserIdMail { get; set; }
    public int CompanyId { get; set; }
    public string? Size { get; set; }
    public string? Batch { get; set; }
    public string? Location { get; set; }
    public string? EAN { get; set; }
}

