using MediatR;
using Otto.models;
using Otto.users.Services;

namespace Otto.users.Queries
{
    public class GetAllUsersByCompanyIdQuery : IRequest<List<User>>
    {
        public int Id { get; }
        public GetAllUsersByCompanyIdQuery(int id)
        {
            Id = id;
        }
    }
    public class GetAllUsersByCompanyIdHandler : IRequestHandler<GetAllUsersByCompanyIdQuery, List<User>>
    {
        private readonly UsersService _userService;
        public GetAllUsersByCompanyIdHandler(UsersService userService)
        {
            _userService = userService;
        }
        public async Task<List<User>> Handle(GetAllUsersByCompanyIdQuery request, CancellationToken cancellationToken)
        {
            var users = await _userService.GetByCompanyIdAsync(request.Id);
            //var officesResponse = _mapperMapOfficesDtosToOfficesResponse(officesDtos);
            return users == null ? null : users;
        }
    }
}
