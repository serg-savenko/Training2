using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using my_new_app.Model;

namespace my_new_app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserWithBalanceController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> logger;
        private readonly IUserRepository userRepository;

        public UserWithBalanceController(IUserRepository userRepository, ILogger<WeatherForecastController> logger)
        {
            this.logger = logger;
            this.userRepository = userRepository;
        }

        [HttpGet]
        public IEnumerable<User> GetAllUsers()
        {
            return userRepository.GetUsers();
        }

        [HttpGet("{id}")]
        public User GetUserById(int id)
        {
            return userRepository.GetUser(id);
        }

        [HttpDelete("{id}")]
        public void DeleteUserById(int id)
        {
            userRepository.DeleteUser(id);
        }

        [HttpPost]
        public User AddNewUser(User u)
        {
            userRepository.AddUser(u);
            return u;
        }

        [HttpPut]
        public User EditUser(User u)
        {
            userRepository.ModifyUser(u);
            return u;
        }

        [HttpPost("{userId}/balance")]
        public User AddBalanceToUser(int userId, Balance b)
        {
            return userRepository.AddBalance(userId, b);
        }
    }
}
