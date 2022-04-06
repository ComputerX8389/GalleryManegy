using GalleryManegy.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryManegy.Handlers
{
    internal class DatabaseHandler
    {
        private readonly DatabaseContext DatabaseContext;

        public UserModel User { get; set; }

        public DatabaseHandler()
        {
            DatabaseContext = new DatabaseContext();
        }

        public UserModel? GetUser()
        {
            if (DatabaseContext.Users.Any())
            {
                return DatabaseContext.Users.FirstOrDefault(U => U.Id == User.Id);
            }
            else
            {
                return null;
            }
        }

        public bool AnyUsers()
        {
            return DatabaseContext.Users.Any();
        }

        public UserModel? Login(string Username, string Password)
        {
            var user = DatabaseContext.Users.FirstOrDefault(u => u.Username == Username);

            if (user != null)
            {
                if (user.CheckPassword(Password))
                {
                    user.LastLogin = DateTime.Now;
                    DatabaseContext.SaveChanges();
                    return user;
                }
            }

            return null;
        }

        public bool CreateUser(UserModel user)
        {
            if (DatabaseContext.Users.Where(u => u.Username == user.Username).Any())
            {
                return false;
            }
            else
            {
                user.Created = DateTime.Now;
                DatabaseContext.Users.Add(user);
                DatabaseContext.SaveChanges();
                return true;
            }
        }

        public List<ImageModel> GetSurportedImages()
        {
            // Todo use loggedin user
            return DatabaseContext.Images.Where(i => i.User.Id == 1 && i.Unsupported == false).ToList();
        }
    }
}
