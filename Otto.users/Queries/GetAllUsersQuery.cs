using MediatR;
using Otto.models;
using Otto.users.Services;

namespace Otto.users.Queries
{
    public class GetAllUsersQuery : IRequest<List<User>>
    {
    }

    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, List<User>>
    {
        private readonly UsersService _userService;
        public GetAllUsersHandler(UsersService userService)
        {
            _userService = userService;
        }
        public async Task<List<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var usersDtos = await _userService.GetUsersAsync();
            //var officesResponse = _mapperMapOfficesDtosToOfficesResponse(officesDtos);
            return usersDtos;
        }
    }
}
