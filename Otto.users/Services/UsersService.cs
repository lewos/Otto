using Otto.Models;

namespace Otto.users.Services
{
    public class UsersService
    {
        public UsersService()
        {

        }

        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                using (var db = new OttoDbContext())
                {
                    var users = db.Users.ToList();
                    return users;
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                throw;
            }
        }
        public async Task<User> GetUserByIdAsync(int id)
        {
            using (var db = new OttoDbContext())
            {
                var user = db.Users
                    .Where(u => u.Id == id)
                    .FirstOrDefault();
                return user;
            }
        }
        public async Task<User> GetByMUserIdAsync(string id)
        {
            using (var db = new OttoDbContext())
            {
                var user = db.Users
                    .Where(u => u.MUserId == id)
                    .FirstOrDefault();
                return user;
            }
        }
        public async Task<User> GetByTUserIdAsync(string id)
        {
            using (var db = new OttoDbContext())
            {
                var user = db.Users
                    .Where(u => u.TUserId == id)
                    .FirstOrDefault();
                return user;
            }
        }
        public async Task<User> GetUserByMailPassAsync(string mail, string pass)
        {
            using (var db = new OttoDbContext())
            {
                var user = db.Users
                    .Where(u => u.Mail == mail &&
                                u.Pass == pass)
                    .FirstOrDefault();
                return user;
            }
        }
        public async Task<User> GetUserByMail(string mail)
        {
            using (var db = new OttoDbContext())
            {
                var user = db.Users
                    .Where(u => u.Mail == mail)
                    .FirstOrDefault();
                return user;
            }
        }
        public async Task<User> CreateUserAsync(User user)
        {
            var userInDb = await GetUserByMail(user.Mail);

            var mailNotInUse = userInDb == null;
            if (mailNotInUse)
            {
                using (var db = new OttoDbContext())
                {
                    db.Users.Add(user);
                    var rowsAffected = await db.SaveChangesAsync();
                    return rowsAffected > 0
                        ? user
                        : null;
                }
            }
            return null;
        }
        public async Task<bool> UpdateUserAsync(int id, User user)
        {
            var inDBUser = await GetUserByIdAsync(id);
            UpdateEntity(inDBUser, user);

            using (var db = new OttoDbContext())
            {
                db.Users.Update(inDBUser);
                var rowsAffected = await db.SaveChangesAsync();
                return rowsAffected > 0;
            }
        }
        public async Task<bool> DeleteUserAsync(long id)
        {
            using (var db = new OttoDbContext())
            {
                var user = db.Users.Where(u => u.Id == id).First();
                db.Users.Remove(user);
                var rowsAffected = await db.SaveChangesAsync();
                return rowsAffected > 0;
            }
        }

        private void UpdateEntity(User inDBUser, User user)
        {
            if (!string.IsNullOrEmpty((user.Name)))
                inDBUser.Name = inDBUser.Name != user.Name ? user.Name : inDBUser.Name;
            if (!string.IsNullOrEmpty((user.Pass)))
                inDBUser.Pass = inDBUser.Pass != user.Pass ? user.Pass : inDBUser.Pass;
            if (!string.IsNullOrEmpty((user.Mail)))
                inDBUser.Mail = inDBUser.Mail != user.Mail ? user.Mail : inDBUser.Mail;
            if (!string.IsNullOrEmpty((user.Rol)))
                inDBUser.Rol = inDBUser.Rol != user.Rol ? user.Rol : inDBUser.Rol;
            if (!string.IsNullOrEmpty((user.TUserId)))
                inDBUser.TUserId = inDBUser.TUserId != user.TUserId ? user.TUserId : inDBUser.TUserId;
            if (!string.IsNullOrEmpty((user.MUserId)))
                inDBUser.MUserId = inDBUser.MUserId != user.MUserId ? user.MUserId : inDBUser.MUserId;
        }
    }
}
