using MediatR;
using Otto.Models;
using Otto.users.Services;

namespace Otto.users.Queries
{
    public class GetByMUserIdQuery : IRequest<User>
    {
        public string Id { get; }
        public GetByMUserIdQuery(string id)
        {
            Id = id;
        }
    }
    public class GetByMUserIdHandler : IRequestHandler<GetByMUserIdQuery, User>
    {
        private readonly UsersService _userService;
        public GetByMUserIdHandler(UsersService userService)
        {
            _userService = userService;
        }
        public async Task<User> Handle(GetByMUserIdQuery request, CancellationToken cancellationToken)
        {
            var userDto = await _userService.GetByMUserIdAsync(request.Id);
            //var officesResponse = _mapperMapOfficesDtosToOfficesResponse(officesDtos);
            return userDto == null ? null : userDto;
        }
    }
}
