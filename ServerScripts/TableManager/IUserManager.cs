using System;
using System.Collections.Generic;
using yukiServer.Table;

namespace yukiServer
{
    interface IUserManager
    {
        void Add(User user);
        void Update(User user);
        void Remove(User user);
        User GetById(int id);
        User GetByUsername(string username);
        ICollection<User> GetAllUsers();
        bool VerifyAccount(string username, string password);
    }
}
