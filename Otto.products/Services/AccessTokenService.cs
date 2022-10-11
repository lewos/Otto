using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Net.Http.Headers;
using Otto.models;
using Otto.models.Responses;
using System.IO;
using System.Text.Json;

namespace Otto.products.Services
{
    public class AccessTokenService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _memoryCache;
        private readonly MemoryCacheEntryOptions _cacheEntryOptions;

        public AccessTokenService(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
        {
            _httpClientFactory = httpClientFactory;
            _memoryCache = memoryCache;
            _cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(20)
                .SetSlidingExpiration(TimeSpan.FromSeconds(3000));
        }


        public async Task<MAccessTokenResponse<Token>> GetTokenCacheAsync(long MUserId)
        {
            var key = $"AccessToken_{MUserId}";
            if (!_memoryCache.TryGetValue(key, out MAccessTokenResponse<Token> response))
            {
                var mAccessTokenResponse = await GetToken(MUserId);
                if (mAccessTokenResponse.res == ResponseCode.OK)
                    _memoryCache.Set(key, mAccessTokenResponse, _cacheEntryOptions);

                return mAccessTokenResponse;
            }
            return response;
        }

        public async Task<MAccessTokenResponse<Token>> GetToken(long MUserId)
        {
            using (var db = new OttoDbContext())
            {
                var mToken = await db.Tokens.Where(t => t.MUserId == MUserId).FirstOrDefaultAsync();
                if (mToken is null)
                    return new MAccessTokenResponse<Token>(ResponseCode.WARNING, $"No existe el token del usuario {MUserId}", null);

                return new MAccessTokenResponse<Token>(ResponseCode.OK, $"{ResponseCode.OK}", mToken);
            }
        }

        public async Task<MAccessTokenResponse<Token>> GetTokenAfterRefresh(long MUserId)
        {
            try
            {
                //Deberia estar en una variable de entorno
                string baseUrl = Environment.GetEnvironmentVariable("URL_OTTO_TOKENS"); ;
                string endpoint = "api/MTokens/RefreshByMUserId";
                string url = string.Join('/', baseUrl, endpoint, MUserId);

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

                    var mToken = await JsonSerializer.DeserializeAsync
                        <Token>(contentStream);

                    return new MAccessTokenResponse<Token>(ResponseCode.OK, $"{ResponseCode.OK}", mToken);
                }
                //si no lo encontro, verificar en donde leo la respuesta del servicio
                return new MAccessTokenResponse<Token>(ResponseCode.WARNING, $"No existe el token del usuario {MUserId}", null);
            }
            catch (Exception ex)
            {
                //verificar en donde leo la respuesta del servicio
                return new MAccessTokenResponse<Token>(ResponseCode.ERROR, $"Error al obtener el token del usuario {MUserId}. Ex : {ex}", null);
            }
        }

        public async Task<MAccessTokenResponse<Token>> CreateNewRegisterAsync(Token token)
        {
            using (var db = new OttoDbContext())
            {
                db.Tokens.Add(token);
                var rowsAffected = await db.SaveChangesAsync();
                return rowsAffected > 0
                    ? new MAccessTokenResponse<Token>(ResponseCode.OK, $"{ResponseCode.OK}", token)
                    : new MAccessTokenResponse<Token>(ResponseCode.WARNING, $"No existe el token del usuario", null);
            }
        }

    }
}
