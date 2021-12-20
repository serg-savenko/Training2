using my_new_app.Model;
using System.Collections.Generic;

namespace my_new_app
{
    public interface IUserRepository
    {
        void AddUser(User u);
        void DeleteUser(int id);
        void ModifyUser(User u);
        User GetUser(int id);
        IEnumerable<User> GetUsers();
        User AddBalance(int userId, Balance b);
    }
}