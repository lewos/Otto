using Microsoft.EntityFrameworkCore;
using Otto.models;

var builder = WebApplication.CreateBuilder(args);

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

app.MapGet("/api/stock", async (OttoDbContext db) =>
{
    return await db.ProductsInStock.ToListAsync();    
});

app.MapGet("/api/stock/{id}", async (OttoDbContext db, int id) =>
{
    var productInStock = await db.ProductsInStock.FindAsync(id);
    if (productInStock is not null)
        return productInStock;
    return null;
});

app.MapGet("/api/stock/MUserId/{id}", async (OttoDbContext db, string id) =>
{
    var items = await db.ProductsInStock.Where(s => s.MSellerId == id).ToListAsync();
    if (items is not null)
        return items ;
    return null;
});

app.MapGet("/api/stock/TUserId/{id}", async (OttoDbContext db, string id) =>
{
    var items = await db.ProductsInStock.Where(s => s.TSellerId == id).ToListAsync();
    if (items is not null)
        return items;
    return null;
});

app.MapGet("/api/stock/MItemId/{id}", async (OttoDbContext db, string id) =>
{
    var item = await db.ProductsInStock.Where(s => s.MItemId == id).FirstOrDefaultAsync();
    if (item is not null)
        return item;
    return null;
});

app.MapGet("/api/stock/TItemId/{id}", async (OttoDbContext db, string id) =>
{
    var item = await db.ProductsInStock.Where(s => s.TItemId == id).FirstOrDefaultAsync();
    if (item is not null)
        return item;
    return null;
});

app.MapGet("/api/stock/company/{id}", async (OttoDbContext db, int id) =>
{
    var products = await db.ProductsInStock.Where(s => s.CompanyId == id).ToListAsync();
    if (products is not null)
        return products;
    return null;
});

app.MapGet("/api/stock/user/{id}", async (OttoDbContext db, int id) =>
{
    var products = await db.ProductsInStock.Where(s => s.UserId == id).ToListAsync();
    if (products is not null)
        return products;
    return null;
});

app.MapPost("/api/stock", async (OttoDbContext db, ProductInStock request) =>
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

    //if origin is Mercadolibre, get user search mUserId
    if (!string.IsNullOrEmpty(request.Origin) && request.Origin.Equals("Mercadolibre") && !string.IsNullOrEmpty(user.MUserId)) 
    {
        request.MSellerId = user.MUserId;
        request.UserIdMail = user.Mail;
    }
    else if (!string.IsNullOrEmpty(request.Origin) && request.Origin.Equals("Tiendanube") && !string.IsNullOrEmpty(user.TUserId))
    {
        request.TSellerId = user.TUserId;
        request.UserIdMail = user.Mail;
    }
    else
        return Results.Conflict("El canal de venta no esta habilitado o el usuario no posee los permisos para ese canal. Los canales habilitados son Mercadolibre, Tiendanube ");

    //TODO  if same user, company, location and item id and seller, updatequantity

    await db.ProductsInStock.AddAsync(request);
    await db.SaveChangesAsync();
    return Results.Created($"/api/stock/{request.Id}", request);
});

app.MapPut("/api/stock/{id}", async (OttoDbContext db, ProductInStock request, int id) =>
{
    try
    {
        var product = await db.ProductsInStock.FindAsync(id);
        if (product is null) return Results.NotFound();
        UpdateFields(request, product);
        await db.SaveChangesAsync();
        return Results.NoContent();

    }
    catch (Exception ex )
    {
        var a = ex;
        throw;
    }
    
});

app.MapPut("/api/stock/UpdateQuantityById/{id}", async (OttoDbContext db, UpdateQuantityDTO dto, int id) =>
{
    var stock = await db.ProductsInStock.Where(s => s.Id == id).FirstOrDefaultAsync();
    if (stock is null) return Results.NotFound();
    UpdateQuantity(dto.Quantity, stock);
    await db.SaveChangesAsync();
    return Results.NoContent();
});


app.MapDelete("/api/stock/{id}", async (OttoDbContext db, int id) =>
{
    var stock = await db.ProductsInStock.FindAsync(id);
    if (stock is null)
    {
        return Results.NotFound();
    }
    db.ProductsInStock.Remove(stock);
    await db.SaveChangesAsync();
    return Results.Ok();
});


app.Run();

static void UpdateFields(ProductInStock request, ProductInStock? stock)
{
    stock.Id = request.Id;
    stock.Name = !string.IsNullOrEmpty(request.Name) ? request.Name : stock.Name;
    stock.Description = !string.IsNullOrEmpty(request.Description) ? request.Description : stock.Description;
    stock.Origin = !string.IsNullOrEmpty(request.Origin) ? request.Origin : stock.Origin;
    stock.Quantity = request.Quantity;
    stock.MSellerId = !string.IsNullOrEmpty(request.MSellerId) ? request.MSellerId : stock.MSellerId;
    stock.TSellerId = !string.IsNullOrEmpty(request.TSellerId) ? request.TSellerId : stock.TSellerId;
    stock.MItemId = !string.IsNullOrEmpty(request.MItemId) ? request.MItemId : stock.MItemId;
    stock.TItemId = !string.IsNullOrEmpty(request.TItemId) ? request.TItemId : stock.TItemId;
    stock.SKU = !string.IsNullOrEmpty(request.SKU) ? request.SKU : stock.SKU;
    stock.Code = !string.IsNullOrEmpty(request.Code) ? request.Code : stock.Code;
    stock.Category = !string.IsNullOrEmpty(request.Category) ? request.Category : stock.Category;
    stock.State = request.State;
    stock.StateDescription = !string.IsNullOrEmpty(request.StateDescription) ? request.StateDescription : stock.StateDescription;
    stock.UserId = request.UserId;
    stock.UserName = !string.IsNullOrEmpty(request.UserName) ? request.UserName : stock.UserName;
    stock.UserLastName = !string.IsNullOrEmpty(request.UserLastName) ? request.UserLastName : stock.UserLastName;
    stock.UserIdMail = !string.IsNullOrEmpty(request.UserIdMail) ? request.UserIdMail : stock.UserIdMail;
    stock.CompanyId = request.CompanyId;
    stock.Size = !string.IsNullOrEmpty(request.Size) ? request.Size : stock.Size;
    stock.Batch = !string.IsNullOrEmpty(request.Batch) ? request.Batch : stock.Batch;
    stock.Location = !string.IsNullOrEmpty(request.Location) ? request.Location : stock.Location;
    stock.EAN = !string.IsNullOrEmpty(request.EAN) ? request.EAN : stock.EAN;
}


static void UpdateQuantity(int QuantityToRest, ProductInStock? stock)
{
    stock.Quantity = stock.Quantity - QuantityToRest;
}

//static List<StockDTO> GetListStockDTO(List<Stock> aux)
//{
//    var res = new List<StockDTO>();
//    foreach (var item in aux)
//        res.Add(StockMapper.GetStockDTO(item));
//    return res;
//}

public class UpdateQuantityDTO
{
    public int Quantity { get; set; }
    public string? MItemId { get; set; }
}