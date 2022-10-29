using Microsoft.Net.Http.Headers;
using Otto.models.Responses;
using Otto.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading.Channels;

namespace Otto.common.services
{
    public class HttpTokenService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public HttpTokenService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> DeleteToken(string urlService,int userId, string channel, long channelUserId)
        {
            try
            {
                //Deberia estar en una variable de entorno
                string baseUrl = urlService;
                string endpoint = $"api/mtokens/user/{userId}/channel/{channel}/channelUserId/{channelUserId}";
                string url = string.Join('/', baseUrl, endpoint);

                var data = new Dictionary<string, string>
{
                };
                var serData = JsonSerializer.Serialize(data);
                var content = new StringContent(serData, Encoding.UTF8, "application/json");


                var httpClient = _httpClientFactory.CreateClient();
                var httpResponseMessage = await httpClient.PutAsync(url, content);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return true;
                }
                //si no lo encontro, verificar en donde leo la respuesta del servicio
                return false;
            }
            catch (Exception ex)
            {
                //verificar en donde leo la respuesta del servicio
                return false;
            }
        }
    }
}
