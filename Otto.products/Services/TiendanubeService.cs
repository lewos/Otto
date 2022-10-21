using Otto.models.Responses;
using Otto.models;
using Otto.products.DTO;
using System.Text.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Otto.products.Services
{
    public class TiendanubeService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public TiendanubeService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ItemsSearchResponse<TItemsSearchDTO>> GetTItemsBySelledIdAsync(long UserId, string AccessToken)
        {
            try
            {
                //Deberia estar en una variable de entorno
                string baseUrl = "https://api.tiendanube.com";
                string endpoint = $"v1/{UserId}/products";
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

                    //TODO borrar, solo debug
                    var body = await httpResponseMessage.Content.ReadAsStringAsync();
                    Console.WriteLine($"respuesta al obtener el token de Tiendanube: {body}");

                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.Converters.Add(new DateTimeConverterUsingDateTimeParseAsFallback());

                    var itemsSearchDTO = await JsonSerializer.DeserializeAsync
                        <List<TItemSearchDTO>>(contentStream, options);

                    return new ItemsSearchResponse<TItemsSearchDTO>(ResponseCode.OK, $"{ResponseCode.OK}", new TItemsSearchDTO {Items = itemsSearchDTO });

                }
                //si no lo encontro, verificar en donde leo la respuesta del servicio
                return new ItemsSearchResponse<TItemsSearchDTO>(ResponseCode.WARNING, $"No se obtuvo una respuesta correcta al consultar los productos del usuario {UserId}", null);


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los productos del usuario {UserId}. Ex : {ex}");
                //verificar en donde leo la respuesta del servicio
                return new ItemsSearchResponse<TItemsSearchDTO>(ResponseCode.ERROR, $"Error al obtener los productos del usuario {UserId}. Ex : {ex}", null);

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
