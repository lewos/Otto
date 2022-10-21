using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Otto.models;
using Otto.users.Services;

namespace Otto.users.Commands
{
    public class UpdateAfterTokenUserCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string? TUserId { get; set; }
        public string? MUserId { get; set; }
    }

    public class UpdateAfterTokenUserHandler : IRequestHandler<UpdateAfterTokenUserCommand, bool>
    {
        private readonly UsersService _userService;
        private readonly IMapper _mapper;
        public UpdateAfterTokenUserHandler(UsersService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;

        }
        public async Task<bool> Handle(UpdateAfterTokenUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request);

            var res = await _userService.UpdateUserAsync(request.Id, user);

            bool res2 = false;
            if(!string.IsNullOrEmpty(user.MUserId))
                res2 = await _userService.UpdateTokenUserIdByMUserIdAsync(user.Id, int.Parse(user.MUserId));
            else
                res2 = await _userService.UpdateTokenUserIdByTUserIdAsync(user.Id, int.Parse(user.TUserId));
            //var officesResponse = _mapperMapOfficesDtosToOfficesResponse(officesDtos);
            return res && res2;
        }
    }
}
