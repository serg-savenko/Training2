using LiteDB;
using my_new_app.Model;
using System.Collections.Generic;

namespace my_new_app
{
    public class UserRepository : IUserRepository
    {
        private LiteDatabase db;

        public UserRepository(LiteDatabase database)
        {
            this.db = database;
        }

        public IEnumerable<User> GetUsers()
        {
            return db.GetCollection<User>().FindAll();
        }

        public User GetUser(int id)
        {
            return db.GetCollection<User>().FindById(id);
        }

        public void AddUser(User u)
        {
            db.GetCollection<User>().Insert(u);
        }

        public void ModifyUser(User u)
        {
            db.GetCollection<User>().Update(u);
        }

        public void DeleteUser(int id)
        {
            db.GetCollection<User>().Delete(id);
        }

        public User AddBalance(int userId, Balance b)
        {
            db.Execute("update User set Balance = concat($.Balance, {\"Date\": @0, \"Amount\": @1}) where _id = @2", b.Date, b.Amount, userId);
            return GetUser(userId);
        }
    }
}
