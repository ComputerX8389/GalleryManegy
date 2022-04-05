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

        public DatabaseHandler()
        {
            DatabaseContext = new DatabaseContext();
        }

        public UserModel? GetUser()
        {
            return DatabaseContext.Users.FirstOrDefault(U => U.Username == "steff");
        }

        public List<ImageModel> GetSurportedImages()
        {
            // Todo use loggedin user
            return DatabaseContext.Images.Where(i => i.User.Id == 1 && i.Unsupported == false).ToList();
        }
    }
}
