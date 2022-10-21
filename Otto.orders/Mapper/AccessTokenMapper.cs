using Otto.models;
using Otto.orders.DTOs;

namespace Otto.orders.Mapper
{
    public static class AccessTokenMapper
    {
        public static MTokenDTO GetMTokenDTO(MCodeForTokenDTO dto)
        {
            var utcNow = DateTime.UtcNow;
            return new MTokenDTO
            {
                //TODO, este dato no lo tengo en este momento, se tiene que actualizar desde el front
                //UserId = dto.UserId,
                //BusinessId = dto.,
                MUserId = dto.UserId,
                AccessToken = dto.AccessToken,
                RefreshToken = dto.RefreshToken,
                Type = dto.Type,
                Created = utcNow,
                Modified = utcNow,
                Active = true,
                ExpiresAt = utcNow + TimeSpan.FromSeconds((double)dto.ExpiresIn),
                ExpiresIn = dto.ExpiresIn,
            };
        }
        public static Token GetToken(MCodeForTokenDTO dto)
        {
            var utcNow = DateTime.UtcNow;
            return new Token
            {
                //TODO, este dato no lo tengo en este momento, se tiene que actualizar desde el front
                //UserId = dto.UserId,
                //BusinessId = dto.,
                MUserId = dto.UserId,
                AccessToken = dto.AccessToken,
                RefreshToken = dto.RefreshToken,
                Type = dto.Type,
                Created = utcNow,
                Modified = utcNow,
                Active = true,
                ExpiresAt = utcNow + TimeSpan.FromSeconds((double)dto.ExpiresIn),
                ExpiresIn = dto.ExpiresIn,
            };
        }


        public static Token GetToken(TCodeForTokenDTO dto)
        {
            var utcNow = DateTime.UtcNow;
            return new Token
            {
                //TODO, este dato no lo tengo en este momento, se tiene que actualizar desde el front
                //UserId = dto.UserId,
                //BusinessId = dto.,
                TUserId = dto.UserId,
                AccessToken = dto.AccessToken,
                Type = dto.Type,
                Created = utcNow,
                Modified = utcNow,
                Active = true,
                ExpiresAt = utcNow + TimeSpan.FromSeconds((double)dto.ExpiresIn),
                ExpiresIn = dto.ExpiresIn,
            };
        }
    }
}
