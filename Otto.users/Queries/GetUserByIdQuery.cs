using MediatR;
using Otto.models;
using Otto.users.Services;

namespace Otto.users.Queries
{
    public class GetUserByIdQuery : IRequest<User>
    {
        public int Id { get; }
        public GetUserByIdQuery(int id)
        {
            Id = id;
        }
    }
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, User>
    {
        private readonly UsersService _userService;
        public GetUserByIdHandler(UsersService userService)
        {
            _userService = userService;
        }

        public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var userDto = await _userService.GetUserByIdAsync(request.Id);
            //var officesResponse = _mapperMapOfficesDtosToOfficesResponse(officesDtos);
            return userDto == null ? null : userDto;
        }
    }
}
