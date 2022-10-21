using Microsoft.Extensions.Caching.Memory;
using Otto.models.Responses;
using Otto.models;
using Otto.orders.DTOs;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using System.Text;

namespace Otto.orders.Services
{
    public class TiendanubeService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MemoryCacheEntryOptions _cacheEntryOptions;

        public TiendanubeService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<TCodeForTokenDTO> GetTokenWithCodeAsync(string code)
        {
            try
            {
                //{
                //    "client_id": "5472",
                //    "client_secret": "3lKXs4vGP8S4zjpeOkvzSBgmdULuOk0GIlZhWx2O6o8lVpET",
                //    "grant_type": "authorization_code",
                //    "code": "93af63d8aa71d63b99e9447603cd70523f21db07"
                //}
                
                string appId = Environment.GetEnvironmentVariable("T_APP_ID");
                string clientSecret = Environment.GetEnvironmentVariable("T_CLIENT_SECRET");
                
                var data = new[]
                {
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("client_id", appId),
                    new KeyValuePair<string, string>("client_secret", clientSecret),
                    new KeyValuePair<string, string>("code", code),
                };

                //Deberia estar en una variable de entorno
                string baseUrl = "https://www.tiendanube.com";
                string endpoint = $"apps/authorize/token";
                string url = string.Join('/', baseUrl, endpoint);


                var httpRequestMessage = new HttpRequestMessage(
                    HttpMethod.Post, url);


                var httpClient = _httpClientFactory.CreateClient();
                var httpResponseMessage = await httpClient.PostAsync(url, new FormUrlEncodedContent(data));

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    using var contentStream =
                        await httpResponseMessage.Content.ReadAsStreamAsync();

                    //TODO borrar, solo debug
                    var body = await httpResponseMessage.Content.ReadAsStringAsync();
                    Console.WriteLine($"respuesta al obtener el token de Tiendanube: {body}");

                    var dto = await JsonSerializer.DeserializeAsync
                        <TCodeForTokenDTO>(contentStream);

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

        public async Task<BaseResponse<TWebHookDTO>> CreateOrderWebHook(long UserId, string AccessToken)
        {
            try
            {
                //Deberia estar en una variable de entorno
                string baseUrl = "https://api.tiendanube.com";
                string endpoint = $"v1/{UserId}/webhooks";
                string url = string.Join('/', baseUrl, endpoint);

                var tUserAgent = Environment.GetEnvironmentVariable("T_USER_AGENT") ?? "(Otto(leo.carmi@gmail.com))";
                var urlEnpointTOrders = Environment.GetEnvironmentVariable("T_ORDERS_URL_WEBHOOK") ?? "https://139.144.172.25/api/TOrders";


                var data = new Dictionary<string, string>
{
                    {"url",$"{urlEnpointTOrders}"},
                    {"event","order/paid"}
                };
                var serData = JsonSerializer.Serialize(data);
                var content = new StringContent(serData, Encoding.UTF8, "application/json");


                var httpClient = _httpClientFactory.CreateClient();

                httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(tUserAgent));
                httpClient.DefaultRequestHeaders.Add("Authentication", $"bearer {AccessToken}");

                var httpResponseMessage = await httpClient.PostAsync(url, content);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                    //TODO borrar, solo debug
                    var body = await httpResponseMessage.Content.ReadAsStringAsync();
                    Console.WriteLine($"respuesta al obtener el token de Tiendanube: {body}");

                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.Converters.Add(new DateTimeConverterUsingDateTimeParseAsFallback());

                    var itemsSearchDTO = await JsonSerializer.DeserializeAsync
                        <TWebHookDTO>(contentStream, options);

                    return new BaseResponse<TWebHookDTO>(ResponseCode.OK, $"{ResponseCode.OK}", itemsSearchDTO);

                }
                //si no lo encontro, verificar en donde leo la respuesta del servicio
                return new BaseResponse<TWebHookDTO>(ResponseCode.WARNING, $"No se obtuvo una respuesta correcta al consultar los productos del usuario {UserId}", null);


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los productos del usuario {UserId}. Ex : {ex}");
                //verificar en donde leo la respuesta del servicio
                return new BaseResponse<TWebHookDTO>(ResponseCode.ERROR, $"Error al obtener los productos del usuario {UserId}. Ex : {ex}", null);

            }
        }

        public async Task<BaseResponse<TOrderDTO>> GetTOrderAsync(long storeId, string resource, long id, string AccessToken)
        {
            try
            {
                //Deberia estar en una variable de entorno
                string baseUrl = "https://api.tiendanube.com";
                string endpoint = $"v1/{storeId}/orders/{id}";
                string url = string.Join('/', baseUrl, endpoint);


                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                httpRequestMessage.Headers.Add("Authentication", $"bearer {AccessToken}");

                var tUserAgent = Environment.GetEnvironmentVariable("T_USER_AGENT") ?? "(Otto(leo.carmi@gmail.com))";

                var httpClient = _httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(tUserAgent));
                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                    ////TODO borrar, solo debug
                    //var body = await httpResponseMessage.Content.ReadAsStringAsync();
                    //Console.WriteLine($"respuesta al obtener el token de Tiendanube: {body}");

                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.Converters.Add(new DateTimeConverterUsingDateTimeParseAsFallback());

                    var tOrderDTO = await JsonSerializer.DeserializeAsync
                        <TOrderDTO>(contentStream, options);

                    return new BaseResponse<TOrderDTO>(ResponseCode.OK, $"{ResponseCode.OK}", tOrderDTO);

                }
                //si no lo encontro, verificar en donde leo la respuesta del servicio
                return new BaseResponse<TOrderDTO>(ResponseCode.WARNING, $"No se obtuvo una respuesta correcta al consultar los productos del usuario {storeId}", null);


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los productos del usuario {storeId}. Ex : {ex}");
                //verificar en donde leo la respuesta del servicio
                return new BaseResponse<TOrderDTO>(ResponseCode.ERROR, $"Error al obtener los productos del usuario {storeId}. Ex : {ex}", null);

            }
        }

        public async Task<BaseResponse<TOrderFulfill>> FullfillOrderAsync(int storeId, string orderId, string accessToken)
        {
            try
            {
                //Deberia estar en una variable de entorno
                string baseUrl = "https://api.tiendanube.com";
                string endpoint = $"v1/{storeId}/orders/{orderId}/fulfill";
                string url = string.Join('/', baseUrl, endpoint);

                var tUserAgent = Environment.GetEnvironmentVariable("T_USER_AGENT") ?? "(Otto(leo.carmi@gmail.com))";                

                var data = new Dictionary<string, string>
{
                    {"shipping_tracking_number","ABC1234"},
                    {"shipping_tracking_url","https://shipping.com/tracking/ABC1234"},
                    {"notify_customer","true"}
                };
                var serData = JsonSerializer.Serialize(data);
                var content = new StringContent(serData, Encoding.UTF8, "application/json");


                var httpClient = _httpClientFactory.CreateClient();

                httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(tUserAgent));
                httpClient.DefaultRequestHeaders.Add("Authentication", $"bearer {accessToken}");

                var httpResponseMessage = await httpClient.PostAsync(url, content);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                    //TODO borrar, solo debug
                    var body = await httpResponseMessage.Content.ReadAsStringAsync();
                    Console.WriteLine($"respuesta al obtener el token de Tiendanube: {body}");

                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.Converters.Add(new DateTimeConverterUsingDateTimeParseAsFallback());

                    var orderDTO = await JsonSerializer.DeserializeAsync
                        <TOrderFulfill>(contentStream, options);

                    return new BaseResponse<TOrderFulfill>(ResponseCode.OK, $"{ResponseCode.OK}", orderDTO);

                }
                //si no lo encontro, verificar en donde leo la respuesta del servicio
                return new BaseResponse<TOrderFulfill>(ResponseCode.WARNING, $"No se obtuvo una respuesta correcta al actualizar el estado de la orden del usuario {storeId}", null);


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los productos del usuario {storeId}. Ex : {ex}");
                //verificar en donde leo la respuesta del servicio
                return new BaseResponse<TOrderFulfill>(ResponseCode.ERROR, $"Error al actualizar el estado de la orden del usuario {storeId}. Ex : {ex}", null);

            }
        }

        public class DateTimeConverterUsingDateTimeParseAsFallback : JsonConverter<DateTime>
        {
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                Debug.Assert(typeToConvert == typeof(DateTime));

                if (!reader.TryGetDateTime(out DateTime value))
                {
                    value = DateTime.Parse(reader.GetString()!);
                }

                return value;
            }

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString("dd/MM/yyyy"));
            }
        }
    }
}
