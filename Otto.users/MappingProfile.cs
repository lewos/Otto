using AutoMapper;
using Otto.Models;
using Otto.users.Commands;

namespace Otto.users
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            // request to entity model
            CreateMap<CreateUserCommand, User>();
            CreateMap <UpdateUserCommand, User>();

            //entity model to response
            CreateMap<User, CreateUserCommandResponse>();
            CreateMap<User, LoginUserCommandResponse>();
        }
    }
}
