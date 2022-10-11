using Otto.m.tokens.DTOs;
using Otto.models;

namespace Otto.m.tokens.Mapper
{
    public static class MTokenMapper
    {
        public static Token GetMToken(MTokenDTO dto) 
        {
            return new Token {
                ExpiresAt = dto.ExpiresAt,
                AccessToken = dto.AccessToken,
                Active = dto.Active,
                //BusinessId = dto.BusinessId,
                Created = dto.Created,
                Id = (int)dto.Id,
                Modified= dto.Modified,
                MUserId = dto.MUserId,
                RefreshToken= dto.RefreshToken,
                Type = dto.Type,
                UserId = dto.UserId
            };
        }


        public static MTokenDTO GetMTokenDTO(Token token)
        {
            return new MTokenDTO
            {
                ExpiresAt = token.ExpiresAt,
                AccessToken = token.AccessToken,
                Active = token.Active,
                //BusinessId = token.BusinessId,
                Created = token.Created,
                Id = token.Id,
                Modified = token.Modified,
                MUserId = token.MUserId,
                RefreshToken = token.RefreshToken,
                Type = token.Type,
                UserId = token.UserId
            };
        }
    }
}
