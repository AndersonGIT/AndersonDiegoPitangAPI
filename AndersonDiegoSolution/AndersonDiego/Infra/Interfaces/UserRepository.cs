using AndersonDiego.Models;
using System;

namespace AndersonDiego.Infra.Interfaces
{
    public interface IUserRepository
    {
        bool Insert(User pUser);
        bool VerifyIfExists(string pEmail);
        User Login(string pEmail, string pPassword);
        User GetUserById(Int64 pUserId);
        void SetLastLogin(Int64 pUserId);
    }
}
