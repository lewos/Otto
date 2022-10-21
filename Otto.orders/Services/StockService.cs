using Microsoft.EntityFrameworkCore;
using Otto.models;
using Otto.orders.DTOs;

namespace Otto.orders.Services
{
    public class StockService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public StockService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> UpdateQuantityByMItemId(int quantity ,string mItemId , int userId, int companyId) 
        {
            using (var db = new OttoDbContext())
            {
                var productInStock = await db.ProductsInStock.Where(p => p.MItemId == mItemId &&
                                                                         p.UserId == userId &&
                                                                         p.CompanyId == companyId).FirstOrDefaultAsync();

                productInStock.Quantity = productInStock.Quantity - quantity;
                var rowsAffected = await db.SaveChangesAsync();
                if(rowsAffected > 0)
                    return true;
                return false;
            }
        }

        public async Task<bool> UpdateQuantityByTItemId(int quantity, string tItemId, int userId, int companyId)
        {
            using (var db = new OttoDbContext())
            {
                var productInStock = await db.ProductsInStock.Where(p => p.TItemId == tItemId &&
                                                                         p.UserId == userId &&
                                                                         p.CompanyId == companyId).FirstOrDefaultAsync();

                productInStock.Quantity = productInStock.Quantity - quantity;
                var rowsAffected = await db.SaveChangesAsync();
                if (rowsAffected > 0)
                    return true;
                return false;
            }
        }

        public async Task<Tuple<bool,int>> GetProductInStockByItemId(string itemId, int userId, bool isTiendanube = false)
        {

            using (var db = new OttoDbContext())
            {
                var productInStock = isTiendanube 
                    ? await db.ProductsInStock.Where(p => p.TItemId == itemId &&
                                                          p.UserId == userId).FirstOrDefaultAsync()
                    
                    : await db.ProductsInStock.Where(p => p.MItemId == itemId && 
                                                          p.UserId == userId).FirstOrDefaultAsync();
                if (productInStock is null)
                    return new Tuple<bool, int>(false, 0);
                return new Tuple<bool, int>(true,productInStock.Id);
            }
        }


        public async Task<Tuple<bool, ProductInStock>> GetProductInStockById(int id)
        {
            using (var db = new OttoDbContext())
            {
                var productInStock = await db.ProductsInStock.Where(p => p.Id == id).FirstOrDefaultAsync();
                if (productInStock is null)
                    return new Tuple<bool, ProductInStock>(false, null);
                return new Tuple<bool, ProductInStock>(true, productInStock);
            }
        }


    }
}
