using Microsoft.Net.Http.Headers;
using Otto.models;
using Otto.products.DTO;
using Otto.products.Models.Responses;
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

        public async Task<StockResponse> GetStockByMUserIdAsync(long MUserId)
        {
            try
            {
                //Deberia estar dentro de una variable de entorno
                string baseUrl = "http://ottostocks.herokuapp.com";
                string endpoint = $"api/stock/GetStockOfSellerByMUserId/{MUserId}";
                string url = string.Join('/', baseUrl, endpoint);


                var httpRequestMessage = new HttpRequestMessage(
                    HttpMethod.Get, url)
                {
                    Headers =
                    {
                        { HeaderNames.Accept, "*/*" },
                    }
                };

                var httpClient = _httpClientFactory.CreateClient();
                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    using var contentStream =
                        await httpResponseMessage.Content.ReadAsStreamAsync();

                    var listStockDTO = await JsonSerializer.DeserializeAsync
                        <List<StockDTO>>(contentStream);

                    return new StockResponse(ResponseCode.OK, $"{ResponseCode.OK}", listStockDTO);

                }

                //si no lo encontro, verificar en donde leo la respuesta del servicio
                return new StockResponse(ResponseCode.WARNING, $"Ocurrio un error al consulat el stock del usuario con el id {MUserId}", null);


            }
            catch (Exception ex)
            {
                //verificar en donde leo la respuesta del servicio
                return new StockResponse(ResponseCode.ERROR, $"Error al obtener el stock del usuario con id {MUserId}. Ex : {ex}", null);

            }


        }
    }
}
