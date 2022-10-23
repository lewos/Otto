using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Otto.common.services;
using Otto.models;
using Otto.users.Services;
using System.Text.Json.Serialization;

namespace Otto.users.Commands
{
    public class LoginUserCommand : IRequest<LoginUserCommandResponse>
    {
        [JsonPropertyName("mail")]
        public string Mail { get; set; }
        [JsonPropertyName("pass")]
        public string Pass { get; set; }
    }

    public class LoginUserCommandResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Mail { get; set; }
        public string Rol { get; set; }
        public int LoginCount { get; set; }
        public string? TUserId { get; set; }
        public string? MUserId { get; set; }
        public int? CompanyId { get; set; }
        public bool HasPendingRequests { get; set; } = false;
    }

    public class LoginUserHandler : IRequestHandler<LoginUserCommand, LoginUserCommandResponse>
    {

        private readonly UsersService _userService;
        private readonly IMapper _mapper;
        private readonly HttpRequestsService _httpRequestsService;
        public LoginUserHandler(UsersService userService, IMapper mapper, HttpRequestsService httpRequestsService)
        {
            _userService = userService;
            _mapper = mapper;
            _httpRequestsService = httpRequestsService;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserByMail(request.Mail);
            if (user != null)
            {
                var passHasher = new PasswordHasher<User>();
                var verifyResult = passHasher.VerifyHashedPassword(user, user.Pass, request.Pass);
                if (verifyResult == PasswordVerificationResult.Success)
                {
                    //update login count 
                    user.LoginCount = user.LoginCount + 1;
                    var rowsAffected = await _userService.UpdateUserAsync(user.Id, user);

                    var response = _mapper.Map<LoginUserCommandResponse>(user);

                    var urlRequestsService =Environment.GetEnvironmentVariable("URL_OTTO_REQUESTS");

                    //request pendientes
                    var requestResponse = await _httpRequestsService.GetRequest(user.Id, "pendiente", urlRequestsService);

                    if(requestResponse.res == ResponseCode.OK && requestResponse.content.Count > 0)
                        response.HasPendingRequests = true;

                    return response;
                }
            }
            return null;

            //var officesResponse = _mapperMapOfficesDtosToOfficesResponse(officesDtos);
            //if (userDto!=null && !string.IsNullOrEmpty(userDto.Pass))
            //    userDto.Pass = "";
            //return userDto;
        }
    }
}
