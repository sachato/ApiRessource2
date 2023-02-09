using ApiRessource2.Models;

namespace ApiRessource2.Repositories
{
    public class UserRepository
    {

        //Get All Users
        public IEnumerable<User> GetUsers()
        {
            using (DataContext context = new DataContext())
            {
                return context.Users.ToList();
            }

        }

        public User GetUsers(int id)
        {
            using (DataContext context = new DataContext())
            {
                return context.Users.Where(e=>e.Id == id).FirstOrDefault();
            }

        }

        public User PostUsers(User user)
        {
            using (DataContext context = new DataContext())
            {
                user.CreationDate = DateTime.Now;
                user.IsConfirmed = false;
                user.IsDeleted = false;
                user.IdRole = 1;
                context.Add(user);
                context.SaveChanges();
                return user;
            }

        }
    }
}
