using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Otto.models;
using Otto.users.Services;

namespace Otto.users.Commands
{
    public class UpdateUserCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Pass { get; set; }
        public string Mail { get; set; }
        public string Rol { get; set; }
        public string? TUserId { get; set; }
        public string? MUserId { get; set; }
        public ICollection<Token>? Tokens { get; set; }
    }

    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly UsersService _userService;
        private readonly IMapper _mapper;
        public UpdateUserHandler(UsersService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;

        }
        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request);

            if (!string.IsNullOrEmpty(request.Pass))
            {
                var passHasher = new PasswordHasher<User>();
                var hashPass = passHasher.HashPassword(user, user.Pass);
                user.Pass = hashPass;
            }

            var res = await _userService.UpdateUserAsync(request.Id, user);
            //var officesResponse = _mapperMapOfficesDtosToOfficesResponse(officesDtos);
            return res;
        }
    }
}
