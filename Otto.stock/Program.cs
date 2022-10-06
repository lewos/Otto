using Microsoft.EntityFrameworkCore;
using Otto.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OttoDbContext>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

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

app.MapGet("/api/stock/GetStockOfSellerById/{id}", async (OttoDbContext db, string id) =>
{
    var products = await db.ProductsInStock.Where(s => s.SellerId == id).ToListAsync();
    if (products is not null)
        return products;
    return null;
});


app.MapGet("/api/stock/GetStockOfSellerByMUserId/{id}", async (OttoDbContext db, string id) =>
{
    var items = await db.ProductsInStock.Where(s => s.MSellerId == id).ToListAsync();
    if (items is not null)
        return items ;
    return null;
});

app.MapGet("/api/stock/GetStockOfSellerByMItemId/{id}", async (OttoDbContext db, string id) =>
{
    var item = await db.ProductsInStock.Where(s => s.MItemId == id).FirstOrDefaultAsync();
    if (item is not null)
        return item;
    return null;
});

app.MapGet("/api/stock/GetPendingStock", async (OttoDbContext db) =>
{
    var items = await db.ProductsInStock.Where(s => s.State == State.Pendiente).ToListAsync();
    if (items is not null)
        return items;
    return null;
});



app.MapPost("/api/stock", async (OttoDbContext db, ProductInStock request) =>
{
    await db.ProductsInStock.AddAsync(request);
    await db.SaveChangesAsync();
    return Results.Created($"/api/stock/{request.Id}", request);
});

app.MapPut("/api/stock/{id}", async (OttoDbContext db, ProductInStock request, int id) =>
{
    var product = await db.ProductsInStock.FindAsync(id);
    if (product is null) return Results.NotFound();
    UpdateFields(request, product);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapPut("/api/stock/UpdateQuantityByMItemId/{id}", async (OttoDbContext db, UpdateQuantityDTO dto, string id) =>
{
    var stock = await db.ProductsInStock.Where(s => s.MItemId == id).FirstOrDefaultAsync();
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
    stock.Name = request.Name;
    stock.Description = request.Description;
    stock.Quantity = request.Quantity;
    stock.Origin = request.Origin;
    stock.SellerId = request.SellerId;
    stock.SellerIdMail = request.SellerIdMail;
    stock.MSellerId = request.MSellerId;
    stock.TSellerId = request.TSellerId;
    stock.MItemId = request.MItemId;
    stock.TItemId = request.TItemId;
    stock.SKU = request.SKU;
    stock.Code = request.Code;
    stock.Category = request.Category;
    stock.State = request.State;
    stock.StateDescription = request.StateDescription;
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