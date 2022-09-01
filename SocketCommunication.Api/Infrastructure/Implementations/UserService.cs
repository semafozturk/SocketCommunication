using log4net;
using SocketCommunication.Api.Models;

namespace SocketCommunication.Api.Infrastructure.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserDbContext _context;
        //private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ILog log = LogManager.GetLogger(typeof(Program));

        public UserService(UserDbContext context)
        {
            _context = context;
        }
        #region Bütün Userları listeleme Endpointi için method
        #endregion
        public IEnumerable<User> GetAllUsers()
        {
            IEnumerable<User> userList = _context.Users.ToList();
            return userList;
        }
        #region Id'ye göre Userları listeleme Endpointi için method
        #endregion
        public User GetById(string tc)
        {

            try
            {
                var MyIni = new IniFile(@"C:\Users\sema.ozturk\source\repos\SocketCommunication_Async_withGetInfo_ServerBasewLog_allof3LogWorking_\SocketCommunication.Api\Variables.ini");

                User user = _context.Users.Where(x => x.UserId == tc).FirstOrDefault();
                var asd = MyIni.Read("userNotExist");
                if (user == null) log.Warn(tc + MyIni.Read("userNotExist"));
                else log.Warn(tc + VariableConfigApi.usersListed);
                return user;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return null;
        }
        public User GetByNameSurname(string name, string surname)
        {
            try
            {
                User user = _context.Users.Where(x => x.UserName == name && x.UserSurname == surname).FirstOrDefault();
                //if (user == null) log.Warn(tc + MyIni.Read("userNotExist"));
                //else log.Warn(tc + VariableConfigApi.usersListed);
                return user;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return null;
        }
        #region User ekleme Endpointi için method
        #endregion
        public void InsertUser(User user)
        {
            try
            {
                _context.Add(user);
                log.Info(user.UserId + VariableConfigApi.userAdded);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
        #region User güncelleme Endpointi için method
        #endregion
        public void UpdateUser(User user)
        {
            try
            {
                User willUpdateUser = _context.Users.Where(x => x.UserId == user.UserId).FirstOrDefault();
                _context.Remove(willUpdateUser);
                _context.Add(user);
                log.Info(user.UserId + VariableConfigApi.userUpdated);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
        #region User silme Endpointi için method
        #endregion
        public void DeleteUser(User user)
        {
            try
            {
                User willDelete = _context.Users.Where(x => x.UserId == user.UserId).FirstOrDefault();
                _context.Remove(willDelete);
                log.Info(VariableConfigApi.userDeleted + user.UserId + "-" + user.UserName + "-" + user.UserSurname);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
    }
}
