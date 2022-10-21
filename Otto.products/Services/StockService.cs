using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Otto.models;
using Otto.models.Responses;
using Otto.products.DTO;
using System.Text.Json;

namespace Otto.products.Services
{
    public class StockService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public StockService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<StockResponse<ProductInStock>> GetStockByMUserIdAsync(string MUserId)
        {
            using (var db = new OttoDbContext())
            {
                var productsInStock = await db.ProductsInStock.Where(p => p.MSellerId == MUserId).ToListAsync();
                if(productsInStock is not null)
                    return new StockResponse<ProductInStock>(ResponseCode.OK, $"{ResponseCode.OK}", productsInStock);                
                return new StockResponse<ProductInStock>(ResponseCode.WARNING, $"Ocurrio un error al consular el stock del usuario con el id {MUserId}", null);
            }
        }

        public async Task<StockResponse<ProductInStock>> GetStockByTUserIdAsync(string TUserId)
        {
            using (var db = new OttoDbContext())
            {
                var productsInStock = await db.ProductsInStock.Where(p => p.TSellerId == TUserId).ToListAsync();
                if (productsInStock is not null)
                    return new StockResponse<ProductInStock>(ResponseCode.OK, $"{ResponseCode.OK}", productsInStock);
                return new StockResponse<ProductInStock>(ResponseCode.WARNING, $"Ocurrio un error al consular el stock del usuario con el id {TUserId}", null);
            }
        }
    }
}
