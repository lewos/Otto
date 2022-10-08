using MediatR;
using Microsoft.EntityFrameworkCore;
using Otto.models;
using Otto.users.Commands;

namespace Otto.users
{
    public static class DBMigrations
    {
        public static void PrepOtto(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<OttoDbContext>(), serviceScope.ServiceProvider.GetService<IMediator>());
            }
        }

        public static void SeedData(OttoDbContext ottoDbContext, IMediator mediator)
        {
            Console.WriteLine("Appling Migrations...");

            ottoDbContext.Database.Migrate();

            if (!ottoDbContext.Users.Any())
            {
                Console.WriteLine("Adding data - seeding");

                var command = new CreateUserCommand { Mail = "prueba@gmail.com", Name = "Administrador", Pass = "admin", Rol = "Administrador" };
                mediator.Send(command);
            }
            else 
            {
                Console.WriteLine("Alredy have data -  not seeding");
            }
        }
    }
}
