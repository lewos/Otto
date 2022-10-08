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
       
        //public async Task<bool> UpdateQuantity(UpdateQuantityDTO dto)
        //{
        //    try
        //    {
        //        //Deberia estar en una variable de entorno
        //        string baseUrl = "http://ottostocks.herokuapp.com";
        //        string endpoint = $"api/stock/UpdateQuantityByMItemId/{dto.MItemId}";
        //        string url = string.Join('/', baseUrl, endpoint);

        //        var json = JsonSerializer.Serialize(dto);
        //        var data = new StringContent(json, Encoding.UTF8, "application/json");

        //        var httpClient = _httpClientFactory.CreateClient();
        //        var httpResponseMessage = await httpClient.PostAsync(url, data);

        //        if (httpResponseMessage.IsSuccessStatusCode)
        //        {
        //            return true;
        //        }

        //        //si no lo encontro, verificar en donde leo la respuesta del servicio
        //        Console.WriteLine($"No se puedo actualizar la cantidad el stock del item {dto.MItemId}");
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        //verificar en donde leo la respuesta del servicio                
        //        Console.WriteLine($"Error al actualizar la cantidad el stock del item {dto.MItemId}. Ex : {ex}");
        //        return false;
        //    }
        //}

        public async Task<bool> UpdateQuantity(UpdateQuantityDTO dto) 
        {
            using (var db = new OttoDbContext())
            {
                var productInStock = await db.ProductsInStock.Where(p => p.MItemId == dto.MItemId).FirstOrDefaultAsync();
                productInStock.Quantity = dto.Quantity;
                var rowsAffected = await db.SaveChangesAsync();
                if(rowsAffected > 0)
                    return true;
                return false;
            }
        }
    }
}
