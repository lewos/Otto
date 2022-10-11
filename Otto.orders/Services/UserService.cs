using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Net.Http.Headers;
using Otto.models;
using Otto.models.Responses;
using Otto.orders.DTOs;
using System.Text.Json;

namespace Otto.orders.Services
{
    public class UserService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _memoryCache;
        private readonly MemoryCacheEntryOptions _cacheEntryOptions;

        public UserService(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
        {
            _httpClientFactory = httpClientFactory;
            _memoryCache = memoryCache;
            _cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(20)
                .SetSlidingExpiration(TimeSpan.FromSeconds(3000));
        }


        public async Task<UserResponse<User>> GetUserByMIdCacheAsync(string MUserId)
        {
            var key = $"UserByMId_{MUserId}";
            if (!_memoryCache.TryGetValue(key, out UserResponse<User> response))
            {
                var userResponse = await GetUserByMIdAsync(MUserId);
                _memoryCache.Set(key, userResponse, _cacheEntryOptions);

                return userResponse;
            }
            return response;
        }

        public async Task<UserResponse<User>> GetUserByMIdAsync(string MUserId) 
        {
            using (var db = new OttoDbContext())
            {
                var user = await db.Users.Where(t => t.MUserId == MUserId).FirstOrDefaultAsync();
                if (user is null)
                    return new UserResponse<User>(ResponseCode.WARNING, $"No existe el usuario con el id {MUserId}", null);

                return new UserResponse<User>(ResponseCode.OK, $"{ResponseCode.OK}", user);

            }
        }
    }
}
