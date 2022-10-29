using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Otto.common.services;
using Otto.models;
using Otto.users.Services;

namespace Otto.users.Commands
{
    public class UnlinkSalesChannelCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Channel { get; set; }
    }

    public class UnlinkSalesChannelCommandHandler : IRequestHandler<UnlinkSalesChannelCommand, bool>
    {
        private readonly UsersService _userService;
        private readonly IMapper _mapper;
        private readonly HttpTokenService _httpTokenService;

        public UnlinkSalesChannelCommandHandler(UsersService userService, IMapper mapper, HttpTokenService httpTokenService)
        {
            _userService = userService;
            _mapper = mapper;
            _httpTokenService = httpTokenService;

        }
        public async Task<bool> Handle(UnlinkSalesChannelCommand request, CancellationToken cancellationToken)
        {

            var user = await _userService.GetUserByIdAsync(request.Id);
            if (user == null)
                return false;


            var urlService = Environment.GetEnvironmentVariable("URL_OTTO_TOKENS");
            var servResult = await _httpTokenService.DeleteToken(urlService, user.Id, request.Channel, int.Parse(user.MUserId));


            if (request.Channel == "Mercadolibre")
                user.MUserId = null;
            else
                user.TUserId = null;


            var res = await _userService.UpdateUserAsync(request.Id, user);

            
            return res && servResult;
        }
    }
}
