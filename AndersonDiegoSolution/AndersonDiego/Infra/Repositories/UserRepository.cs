using AndersonDiego.Infra.Interfaces;
using AndersonDiego.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AndersonDiego.Infra.Repositories
{
    public class UserRepository : IUserRepository
    {
        static List<User> usersDataBase = new List<User>();

        public bool Insert(User pUser)
        {
            pUser.CreatedAt = DateTime.Now;
            pUser.UserId = usersDataBase?.Count > 0 ? usersDataBase.Max(u => u.UserId) +1 : 1;
            usersDataBase.Add(pUser);

            return usersDataBase.Contains(pUser);
        }

        public bool VerifyIfExists(string pEmail)
        {
            return usersDataBase.Exists(u => u.Email == pEmail);
        }

        public User GetUser(Int64 pUserId)
        {
            return usersDataBase.FirstOrDefault(u => u.UserId == pUserId);
        }

        public User Login(string pEmail, string pPassword)
        {
            string userSerialized = string.Empty;

            User userFound = usersDataBase.FirstOrDefault(u => u.Email.Equals(pEmail) && u.Password.Equals(pPassword));            

            if (userFound != null)
            {
                //Intializing a new Object to avoid reference on last login set up.
                //userFound = new User()
                //{
                //    UserId = userFound.UserId,
                //    FirstName = userFound.FirstName,
                //    LastName = userFound.LastName,
                //    Email = userFound.Email,
                //    Password = userFound.Password,
                //    Phones = userFound.Phones,
                //    CreatedAt = userFound.CreatedAt,
                //    LastLogin = userFound.LastLogin
                //};
                
                SetLastLogin(userFound.UserId);
            }

            return userFound;
        }

        public User GetUserById(Int64 pUserId)
        {
            return usersDataBase.Find(u => u.UserId.Equals(pUserId));
        }

        public void SetLastLogin(Int64 pUserId)
        {
            usersDataBase.Find(u => u.UserId.Equals(pUserId)).LastLogin = DateTime.Now;
        }
    }
}