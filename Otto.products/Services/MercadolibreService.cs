using Otto.models;
using Otto.products.DTO;
using Otto.products.Models;
using Otto.products.Models.Responses;
using System.Text.Json;

namespace Otto.products.Services
{
    public class MercadolibreService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public MercadolibreService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<MItemsSearchResponse> GetMItemsBySelledIdAsync(long MUserId, string AccessToken)
        {
            try
            {
                //Deberia estar en una variable de entorno
                string baseUrl = "https://api.mercadolibre.com";
                string endpoint = $"users/{MUserId}/items/search";
                string url = string.Join('/', baseUrl, endpoint);


                var httpRequestMessage = new HttpRequestMessage(
                    HttpMethod.Get, url);

                httpRequestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);

                var httpClient = _httpClientFactory.CreateClient();
                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    using var contentStream =
                        await httpResponseMessage.Content.ReadAsStreamAsync();

                    var mItemsSearchDTO = await JsonSerializer.DeserializeAsync
                        <MItemsSearchDTO>(contentStream);

                    //comentar
                    //string jsonString = JsonSerializer.Serialize(mOrder);
                    //Console.WriteLine(jsonString);

                    return new MItemsSearchResponse(ResponseCode.OK, $"{ResponseCode.OK}", mItemsSearchDTO);

                }
                //si no lo encontro, verificar en donde leo la respuesta del servicio
                return new MItemsSearchResponse(ResponseCode.WARNING, $"No se obtuvo una respuesta correcta al consultar los productos del usuario {MUserId}", null);


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los productos del usuario {MUserId}. Ex : {ex}");
                //verificar en donde leo la respuesta del servicio
                return new MItemsSearchResponse(ResponseCode.ERROR, $"Error al obtener los productos del usuario {MUserId}. Ex : {ex}", null);

            }
        }

        public async Task<MProductsSearchResponse> GetMProductsByItemsAsync(string concatItems, string AccessToken)
        {
            try
            {
                //Deberia estar en una variable de entorno
                string baseUrl = "https://api.mercadolibre.com";
                string endpoint = $"items?ids={concatItems}&attributes=id,title,price,available_quantity,permalink,thumbnail,secure_thumbnail,shipping,attributes";
                string url = string.Join('/', baseUrl, endpoint);


                var httpRequestMessage = new HttpRequestMessage(
                    HttpMethod.Get, url);

                httpRequestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);

                var httpClient = _httpClientFactory.CreateClient();
                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    using var contentStream =
                        await httpResponseMessage.Content.ReadAsStreamAsync();

                    var mProductsSearchDTO = await JsonSerializer.DeserializeAsync
                        <List<MProductsSearchDTO>>(contentStream);

                    //comentar
                    //string jsonString = JsonSerializer.Serialize(mOrder);
                    //Console.WriteLine(jsonString);

                    return new MProductsSearchResponse(ResponseCode.OK, $"{ResponseCode.OK}", mProductsSearchDTO);

                }
                //si no lo encontro, verificar en donde leo la respuesta del servicio
                return new MProductsSearchResponse(ResponseCode.WARNING, $"No se obtuvo una respuesta correcta al consultar los items {concatItems}", null);


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los items {concatItems}. Ex : {ex}");
                //verificar en donde leo la respuesta del servicio
                return new MProductsSearchResponse(ResponseCode.ERROR, $"Error al obtener los items {concatItems}. Ex : {ex}", null);

            }
        }

    }
}
