using System.Collections.Generic;

namespace PA.Services
{
    public interface IUsersService
    {
        User GetOne(int id);
        User GetOne(string username);
        List<User> GetAll();
        User Login(string username, string password);
        void Register(string username, string password, string email);
    }
}