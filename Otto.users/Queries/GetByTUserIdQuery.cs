using MediatR;
using Otto.Models;
using Otto.users.Services;

namespace Otto.users.Queries
{
    public class GetByTUserIdQuery : IRequest<User>
    {
        public string Id { get; }
        public GetByTUserIdQuery(string id)
        {
            Id = id;
        }
    }
    public class GetByTUserIdHandler : IRequestHandler<GetByTUserIdQuery, User>
    {
        private readonly UsersService _userService;
        public GetByTUserIdHandler(UsersService userService)
        {
            _userService = userService;
        }
        public async Task<User> Handle(GetByTUserIdQuery request, CancellationToken cancellationToken)
        {
            var userDto = await _userService.GetByTUserIdAsync(request.Id);
            //var officesResponse = _mapperMapOfficesDtosToOfficesResponse(officesDtos);
            return userDto == null ? null : userDto;
        }
    }
}
