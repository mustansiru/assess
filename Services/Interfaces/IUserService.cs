using System.Collections.Generic;
using assess.Models;

namespace assess.Services.Intefaces
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
    }
}