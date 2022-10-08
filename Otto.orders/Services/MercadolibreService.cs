using Microsoft.Extensions.Caching.Memory;
using Otto.models;
using Otto.models.Responses;
using Otto.orders.DTOs;
using System.Text.Json;

namespace Otto.orders.Services
{
    public class MercadolibreService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MemoryCacheEntryOptions _cacheEntryOptions;

        public MercadolibreService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        
        public async Task<MOrderResponse<MOrderDTO>> GetMOrderAsync(long MUserId, string Resource, string AccessToken)
        {
            try
            {
                //Deberia estar en una variable de entorno
                string baseUrl = "https://api.mercadolibre.com";
                string endpoint = Resource.Substring(1);
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

                    var mOrder = await JsonSerializer.DeserializeAsync
                        <MOrderDTO>(contentStream);

                    //comentar
                    //string jsonString = JsonSerializer.Serialize(mOrder);
                    //Console.WriteLine(jsonString);

                    return new MOrderResponse<MOrderDTO>(ResponseCode.OK, $"{ResponseCode.OK}", mOrder);

                }
                //si no lo encontro, verificar en donde leo la respuesta del servicio
                return new MOrderResponse<MOrderDTO>(ResponseCode.WARNING, $"No existe la orden {Resource} del usuario {MUserId}", null);


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener la orden {Resource} del usuario {MUserId}. Ex : {ex}");
                //verificar en donde leo la respuesta del servicio
                return new MOrderResponse<MOrderDTO>(ResponseCode.ERROR, $"Error al obtener la orden {Resource} del usuario {MUserId}. Ex : {ex}", null);

            }
        }
        public async Task<MItemResponse<MItemDTO>> GetMItemAsync(long MUserId, string Resource, string AccessToken)
        {
            try
            {
                //Deberia estar en una variable de entorno
                string baseUrl = "https://api.mercadolibre.com";
                string endpoint = Resource;
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

                    var mItem = await JsonSerializer.DeserializeAsync
                        <MItemDTO>(contentStream);


                    return new MItemResponse<MItemDTO>(ResponseCode.OK, $"{ResponseCode.OK}", mItem);

                }

                //si no lo encontro, verificar en donde leo la respuesta del servicio
                return new MItemResponse<MItemDTO>(ResponseCode.WARNING, $"No existe la orden {Resource} del usuario {MUserId}", null);


            }
            catch (Exception ex)
            {
                //verificar en donde leo la respuesta del servicio
                return new MItemResponse<MItemDTO>(ResponseCode.ERROR, $"Error al obtener la orden {Resource} del usuario {MUserId}. Ex : {ex}", null);

            }


        }
        public async Task<MUnreadNotificationsResponse<MissedFeedsDTO>> GetUnreadNotificationsAsync(long MUserId, string Resource, string AccessToken) 
        {
            try
            {
                //Deberia estar en una variable de entorno
                string baseUrl = "https://api.mercadolibre.com";
                string endpoint = $"missed_feeds?app_id={Resource}";
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

                    var missedFeedsDTO = await JsonSerializer.DeserializeAsync
                        <MissedFeedsDTO>(contentStream);

                    //comentar
                    //string jsonString = JsonSerializer.Serialize(missedFeedsDTO);
                    //Console.WriteLine(jsonString);

                    return new MUnreadNotificationsResponse<MissedFeedsDTO>(ResponseCode.OK, $"{ResponseCode.OK}", missedFeedsDTO);

                }
                //si no lo encontro, verificar en donde leo la respuesta del servicio
                return new MUnreadNotificationsResponse<MissedFeedsDTO>(ResponseCode.WARNING, $"Ocurrio un error al consultar las notificaciones no leidas {Resource} del usuario {MUserId}", null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las notificaciones no leidas {Resource} del usuario {MUserId}. Ex : {ex}");
                //verificar en donde leo la respuesta del servicio
                return new MUnreadNotificationsResponse<MissedFeedsDTO>(ResponseCode.ERROR, $"Error al obtener las notificaciones no leidas {Resource} del usuario {MUserId}. Ex : {ex}", null);
            }

        }
        public async Task<string> GetPrintOrderAsync(long Resource, string AccessToken, bool pdf = true)
        {
            try
            {
                //Deberia estar en una variable de entorno
                string baseUrl = "https://api.mercadolibre.com";
                //Por ahora solo busco pdf
                string responseType = pdf ? "pdf" : "zpl2";
                string endpoint = $"shipment_labels?shipment_ids={Resource}&response_type={responseType}";
                string url = string.Join('/', baseUrl, endpoint);

                var pegaleAca = $"curl -X GET -H 'Authorization: Bearer {AccessToken}' {url}";
                return pegaleAca;

            }
            catch (Exception ex )
            {
                var aver = ex;
                throw;
            }
        }

        public async Task<MCodeForTokenDTO> GetTokenWithCodeAsync(string code)
        {
            try
            {
                long mUserId = long.Parse(Environment.GetEnvironmentVariable("APP_MUSER_ID_OWNER"));
                string appId = Environment.GetEnvironmentVariable("APP_ID");
                string clientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET");
                string redirectUri = Environment.GetEnvironmentVariable("REDIRECT_URI");

                var data = new[]
                {
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("client_id", appId),
                    new KeyValuePair<string, string>("client_secret", clientSecret),
                    new KeyValuePair<string, string>("code", code),
                    new KeyValuePair<string, string>("redirect_uri", redirectUri),
                };                

                //Deberia estar en una variable de entorno
                string baseUrl = "https://api.mercadolibre.com";
                string endpoint = $"oauth/token";
                string url = string.Join('/', baseUrl, endpoint);


                var httpRequestMessage = new HttpRequestMessage(
                    HttpMethod.Post, url);


                var httpClient = _httpClientFactory.CreateClient();
                var httpResponseMessage = await httpClient.PostAsync(url, new FormUrlEncodedContent(data));

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    using var contentStream =
                        await httpResponseMessage.Content.ReadAsStreamAsync();

                    var dto = await JsonSerializer.DeserializeAsync
                        <MCodeForTokenDTO>(contentStream);

                    //comentar
                    string jsonString = JsonSerializer.Serialize(dto);
                    Console.WriteLine(jsonString);

                    return dto;

                }
                //si no lo encontro, verificar en donde leo la respuesta del servicio
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error, Ex : {ex}");
                //verificar en donde leo la respuesta del servicio
                return null;
            }
        }
    }
}
