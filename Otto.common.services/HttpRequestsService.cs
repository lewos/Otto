using Microsoft.Net.Http.Headers;
using Otto.models;
using Otto.models.Responses;
using System.Text.Json;

namespace Otto.common.services
{
    public class HttpRequestsService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public HttpRequestsService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<BaseResponse<List<JoinRequest>>> GetRequest(long userId, string state, string urlService)
        {
            try
            {
                //Deberia estar en una variable de entorno
                string baseUrl = urlService;
                string endpoint = $"api/requests/user/{userId}/{state}";
                string url = string.Join('/', baseUrl, endpoint);

                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url)
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

                    var request = await JsonSerializer.DeserializeAsync
                        <List<JoinRequest>>(contentStream);

                    return new BaseResponse<List<JoinRequest>>(ResponseCode.OK, $"{ResponseCode.OK}", request);
                }
                //si no lo encontro, verificar en donde leo la respuesta del servicio
                return new BaseResponse<List<JoinRequest>>(ResponseCode.WARNING, $"No existe el request del usuario {userId}", null);
            }
            catch (Exception ex)
            {
                //verificar en donde leo la respuesta del servicio
                return new BaseResponse<List<JoinRequest>>(ResponseCode.ERROR, $"Error al obtener los request del usuario {userId}. Ex : {ex}", null);
            }
        }
    }
}
