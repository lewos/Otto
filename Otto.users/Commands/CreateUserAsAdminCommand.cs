using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Otto.models;
using Otto.users.Services;
using System.Net;
using System.Net.Mail;

namespace Otto.users.Commands
{
    public class CreateUserAsAdminCommand : IRequest<Response<CreateUserCommandResponse>>
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Pass { get; set; }
        public string Mail { get; set; }
        public string Rol { get; set; }
        public int CompanyId { get; set; }
    }

    public class CreateUserAsAdminCommandHandler : IRequestHandler<CreateUserAsAdminCommand, Response<CreateUserCommandResponse>>
    {
        private readonly UsersService _userService;
        private readonly IMapper _mapper;

        public CreateUserAsAdminCommandHandler(UsersService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<Response<CreateUserCommandResponse>> Handle(CreateUserAsAdminCommand request, CancellationToken cancellationToken)
        {

            var user = _mapper.Map<User>(request);

            var passHasher = new PasswordHasher<User>();

            var hashPass = passHasher.HashPassword(user, user.Pass);
            user.Pass = hashPass;

            var userDto = await _userService.CreateUserAsync(user);

            //Mandar mail
            //TODO comento temporal
            //SendMail(userDto);

            var content = _mapper.Map<CreateUserCommandResponse>(userDto);

            var response = new Response<CreateUserCommandResponse>(content);

            return response;

        }

        private void SendMail(User userDto)
        {
            try
            {
                var toMail = userDto.Mail;
                var fromMail = "otto.mail.soporte@gmail.com";
                var subject = "Usuario Otto";
                var pass = Environment.GetEnvironmentVariable("MAIL_PASS");

                var body = $" usuario: {userDto.Mail} " +
                          $" contraseña: {userDto.Pass}";

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromMail, pass),
                    EnableSsl = true,
                };

                smtpClient.Send(fromMail, toMail, subject, body);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
    }
}
