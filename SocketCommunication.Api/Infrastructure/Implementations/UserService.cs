using SocketCommunication.Api.Models;

namespace SocketCommunication.Api.Infrastructure.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserDbContext _context;

        public UserService(UserDbContext context)
        {
            _context = context;
        }
        public IEnumerable<User> GetAllUsers()
        {
            IEnumerable<User> userList = _context.Users.ToList();
            return userList;
        }

        public User GetById(string tc)
        {
            User user = _context.Users.Where(x => x.UserId == tc).FirstOrDefault();
            return user;
        }

        public void InsertUser(User user)
        {
            _context.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            User willUpdateUser=_context.Users.Where(x => x.UserId == user.UserId).FirstOrDefault();
            _context.Remove(willUpdateUser);
            _context.Add(user);
            _context.SaveChanges();
        }
        public void DeleteUser(User user)
        {
            User willDelete= _context.Users.Where(x => x.UserId == user.UserId).FirstOrDefault();
            _context.Remove(willDelete);
        }
    }
}
