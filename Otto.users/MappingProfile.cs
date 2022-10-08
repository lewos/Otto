using AutoMapper;
using Otto.models;
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
            CreateMap <UpdateAfterTokenUserCommand, User>();


            //entity model to response
            CreateMap<User, CreateUserCommandResponse>();
            CreateMap<User, LoginUserCommandResponse>();
        }
    }
}
