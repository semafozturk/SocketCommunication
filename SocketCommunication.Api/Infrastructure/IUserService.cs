using SocketCommunication.Api.Models;

namespace SocketCommunication.Api.Infrastructure
{
    public interface IUserService
    {
        public IEnumerable<User> GetAllUsers();
        public User GetById(string tc);
        public User GetByNameSurname(string name, string surname);
        public void InsertUser(User user);
        public void UpdateUser(User user);
        public void DeleteUser(User user);
    }
}

