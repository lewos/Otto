using MediatR;
using Otto.models;
using Otto.users.Services;

namespace Otto.users.Queries
{
    public class GetAllUsersByCompanyIdAndRolQuery : IRequest<List<User>>
    {
        public int Id { get; }
        public string Rol { get; }

        public GetAllUsersByCompanyIdAndRolQuery(int id, string rol)
        {
            Id = id;
            Rol = rol;
        }

    }

    public class GetAllUsersByCompanyIdAndRolHandler : IRequestHandler<GetAllUsersByCompanyIdAndRolQuery, List<User>>
    {
        private readonly UsersService _userService;
        public GetAllUsersByCompanyIdAndRolHandler(UsersService userService)
        {
            _userService = userService;
        }
        public async Task<List<User>> Handle(GetAllUsersByCompanyIdAndRolQuery request, CancellationToken cancellationToken)
        {
            var usersDtos = await _userService.GetByCompanyIdAndRolAsync(request.Id, request.Rol);
            return usersDtos;
        }
    }
}
