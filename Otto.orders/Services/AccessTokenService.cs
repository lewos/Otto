using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Otto.models;
using Otto.models.Responses;

namespace Otto.orders.Services
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
                if(mAccessTokenResponse.res== ResponseCode.OK)
                    _memoryCache.Set(key, mAccessTokenResponse, _cacheEntryOptions);

                return mAccessTokenResponse;
            }
            return response;
        }

        //public async Task<MAccessTokenResponse> GetToken(long MUserId)
        //{
        //    try
        //    {
        //        //Deberia estar en una variable de entorno
        //        string baseUrl = "https://ottomtokens.herokuapp.com";
        //        string endpoint = "api/MTokens/ByMUserId";
        //        string url = string.Join('/', baseUrl, endpoint, MUserId);


        //        var httpRequestMessage = new HttpRequestMessage(
        //            HttpMethod.Get, url)
        //        {
        //            Headers =
        //            {
        //                { HeaderNames.Accept, "*/*" },
        //            }
        //        };

        //        var httpClient = _httpClientFactory.CreateClient();
        //        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        //        if (httpResponseMessage.IsSuccessStatusCode)
        //        {
        //            using var contentStream =
        //                await httpResponseMessage.Content.ReadAsStreamAsync();

        //            var mToken = await JsonSerializer.DeserializeAsync
        //                <MTokenDTO>(contentStream);

        //            return new MAccessTokenResponse(ResponseCode.OK, $"{ResponseCode.OK}", mToken);

        //        }
        //        //si no lo encontro, verificar en donde leo la respuesta del servicio
        //        return new MAccessTokenResponse(ResponseCode.WARNING, $"No existe el token del usuario {MUserId}", null);
        //    }
        //    catch (Exception ex)
        //    {
        //        //verificar en donde leo la respuesta del servicio
        //        return new MAccessTokenResponse(ResponseCode.ERROR, $"Error al obtener el token del usuario {MUserId}. Ex : {ex}", null);

        //    }
        //}

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

        //public async Task<MAccessTokenResponse> GetTokenAfterRefresh(long MUserId)
        //{

        //    try
        //    {
        //        //Deberia estar en una variable de entorno
        //        string baseUrl = "https://ottomtokens.herokuapp.com";
        //        string endpoint = "api/MTokens/RefreshByMUserId";
        //        string url = string.Join('/', baseUrl, endpoint, MUserId);

        //        var httpRequestMessage = new HttpRequestMessage(
        //            HttpMethod.Get, url)
        //        {
        //            Headers =
        //            {
        //                { HeaderNames.Accept, "*/*" },
        //            }
        //        };

        //        var httpClient = _httpClientFactory.CreateClient();
        //        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        //        if (httpResponseMessage.IsSuccessStatusCode)
        //        {
        //            using var contentStream =
        //                await httpResponseMessage.Content.ReadAsStreamAsync();

        //            var mToken = await JsonSerializer.DeserializeAsync
        //                <MTokenDTO>(contentStream);

        //            return new MAccessTokenResponse(ResponseCode.OK, $"{ResponseCode.OK}", mToken);
        //        }
        //        //si no lo encontro, verificar en donde leo la respuesta del servicio
        //        return new MAccessTokenResponse(ResponseCode.WARNING, $"No existe el token del usuario {MUserId}", null);
        //    }
        //    catch (Exception ex)
        //    {
        //        //verificar en donde leo la respuesta del servicio
        //        return new MAccessTokenResponse(ResponseCode.ERROR, $"Error al obtener el token del usuario {MUserId}. Ex : {ex}", null);
        //    }
        //}

        public async Task<MAccessTokenResponse<Token>> GetTokenAfterRefresh(long MUserId)
        {
            using (var db = new OttoDbContext())
            {
                var mToken = await db.Tokens.Where(t => t.MUserId == MUserId).FirstOrDefaultAsync();
                if(mToken is null)
                    return new MAccessTokenResponse<Token>(ResponseCode.WARNING, $"No existe el token del usuario {MUserId}", null);

                return new MAccessTokenResponse<Token>(ResponseCode.OK, $"{ResponseCode.OK}", mToken);
            }            
        }

        //public async Task<MAccessTokenResponse> CreateNewRegisterAsync(MTokenDTO dto)
        //{

        //    try
        //    {
        //        //Deberia estar en una variable de entorno
        //        string baseUrl = "https://ottomtokens.herokuapp.com";
        //        string endpoint = "api/MTokens";
        //        string url = string.Join('/', baseUrl, endpoint);

        //        var json = JsonSerializer.Serialize(dto);
        //        var data = new StringContent(json, Encoding.UTF8, "application/json");

        //        var httpClient = _httpClientFactory.CreateClient();
        //        var httpResponseMessage = await httpClient.PostAsync(url, data);

        //        if (httpResponseMessage.IsSuccessStatusCode)
        //        {
        //            using var contentStream =
        //                await httpResponseMessage.Content.ReadAsStreamAsync();

        //            var mToken = await JsonSerializer.DeserializeAsync
        //                <MTokenDTO>(contentStream);

        //            return new MAccessTokenResponse(ResponseCode.OK, $"{ResponseCode.OK}", mToken);
        //        }
        //        //si no lo encontro, verificar en donde leo la respuesta del servicio
        //        return new MAccessTokenResponse(ResponseCode.WARNING, $"No existe el token del usuario", null);
        //    }
        //    catch (Exception ex)
        //    {
        //        //verificar en donde leo la respuesta del servicio
        //        return new MAccessTokenResponse(ResponseCode.ERROR, $"Error al obtener el token del usuario. Ex : {ex}", null);
        //    }
        //}

        public async Task<MAccessTokenResponse<Token>> CreateNewRegisterAsync(Token token, string channnel = null)
        {
            using (var db = new OttoDbContext())
            {
                var tokenInDb = await db.Tokens.Where((t) => t.MUserId == token.MUserId).FirstOrDefaultAsync();

                //update
                if (tokenInDb is not null)
                {
                    tokenInDb.AccessToken = token.AccessToken;
                    tokenInDb.RefreshToken = token.RefreshToken;
                    tokenInDb.Created = token.Created;
                    tokenInDb.Modified = token.Modified;
                    tokenInDb.ExpiresAt = token.ExpiresAt;
                    tokenInDb.ExpiresIn = token.ExpiresIn;
                    tokenInDb.ExpiresIn = token.ExpiresIn;
                    if (!string.IsNullOrEmpty(channnel))
                        tokenInDb.SalesChannel = channnel;


                    db.Tokens.Update(tokenInDb);
                    var rowsAffected = await db.SaveChangesAsync();
                    return rowsAffected > 0
                        ? new MAccessTokenResponse<Token>(ResponseCode.OK, $"{ResponseCode.OK}", tokenInDb)
                        : new MAccessTokenResponse<Token>(ResponseCode.WARNING, $"No existe el token del usuario", null);
                }
                //create
                else 
                {
                    if (!string.IsNullOrEmpty(channnel))
                        token.SalesChannel = channnel;

                    db.Tokens.Add(token);
                    var rowsAffected = await db.SaveChangesAsync();
                    return rowsAffected > 0
                        ? new MAccessTokenResponse<Token>(ResponseCode.OK, $"{ResponseCode.OK}", token)
                        : new MAccessTokenResponse<Token>(ResponseCode.WARNING, $"No existe el token del usuario", null);                
                }
            }            
        }

    }
}
