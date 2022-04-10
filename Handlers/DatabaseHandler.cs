using GalleryManegy.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public void UpdateImageListToMatchDatabase(ObservableCollection<ImageModel> List)
        {
            var allImagesDb = GetSurportedImages();
            foreach (var imagedb in allImagesDb)
            {
                var imgList = List.FirstOrDefault(i => i.Id == imagedb.Id);

                if (imgList != null)
                {
                    imgList.Update(imagedb);
                }
                else
                {
                    List.Add(imagedb);
                }
            }

            List<ImageModel> toRemove = new();

            foreach (var imageList in List)
            {
                if (!allImagesDb.Where(i => i.Id == imageList.Id).Any())
                {
                    toRemove.Add(imageList);
                }
            }

            foreach (var image in toRemove)
            {
                List.Remove(image);
            }
        }

        public List<ImageModel> GetAllImages()
        {
            return DatabaseContext.Images.Where(i => i.User.Id == User.Id).ToList();
        }

        public List<ImageModel> GetSurportedImages()
        {
            return DatabaseContext.Images.Where(i => i.User.Id == User.Id && i.Unsupported == false).ToList();
        }

        public void AddImage(ImageModel imageModel)
        {
            imageModel.User = User;

            DatabaseContext.Images.Add(imageModel);
        }

        public ImageModel? GetImageByFullName(string fullname)
        {
            return DatabaseContext.Images.FirstOrDefault(i => i.FullName == fullname);
        }

        public void RemoveImage(ImageModel imageModel)
        {
            DatabaseContext.Images.Remove(imageModel);
        }

        public void RemoveRange(List<ImageModel> imageModel)
        {
            DatabaseContext.Images.RemoveRange(imageModel);
        }

        public void SaveChanges()
        {
            DatabaseContext.SaveChanges();
        }
    }
}
