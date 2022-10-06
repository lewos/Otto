using MediatR;
using Otto.users.Services;

namespace Otto.users.Commands
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public long Id { get; }
        public DeleteUserCommand(long id)
        {
            Id = id;
        }
    }

    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly UsersService _userService;
        public DeleteUserHandler(UsersService userService)
        {
            _userService = userService;
        }
        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var userDto = await _userService.DeleteUserAsync(request.Id);
            //var officesResponse = _mapperMapOfficesDtosToOfficesResponse(officesDtos);
            return userDto;
        }
    }
}
