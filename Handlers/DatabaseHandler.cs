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
        private readonly Dictionary<SettingModel.SettingKeys, string> DefaultSettings;

        public UserModel User { get; set; }

        public enum SortingOptions
        {
            CreationDate,
            ScannedDate,
            Imagesize
        }

        public DatabaseHandler()
        {
            DatabaseContext = new();
            DefaultSettings = new();
            DefaultSettings.Add(SettingModel.SettingKeys.GalleryPath, Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
            DefaultSettings.Add(SettingModel.SettingKeys.GalleryRowAmount, "6");
            DefaultSettings.Add(SettingModel.SettingKeys.ThumbnailSize, "120");
            DefaultSettings.Add(SettingModel.SettingKeys.SelectedOrder, SortingOptions.ScannedDate.ToString());

            DatabaseContext.Database.Migrate();
        }

        #region General
        public SettingModel GetSetting(SettingModel.SettingKeys key)
        {
            var setting = DatabaseContext.Settings.Where(i => i.User.Id == User.Id).FirstOrDefault(s => s.Key == key);

            if (setting != null)
            {
                return setting;
            }
            else
            {
                setting = new SettingModel(key, DefaultSettings[key], User);
                DatabaseContext.Settings.Add(setting);
                SaveChanges();
                return setting;
            }
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

        public void SaveChanges()
        {
            DatabaseContext.SaveChanges();
        }
        #endregion

        #region Images
        public List<ImageModel> GetAllImages()
        {
            return DatabaseContext.Images.Where(i => i.User.Id == User.Id).ToList();
        }

        public List<ImageModel> GetSurportedImages()
        {
            return DatabaseContext.Images.Where(i => i.User.Id == User.Id && i.Unsupported == false).ToList();
        }

        public List<ImageModel> GetSurportedImages(SortingOptions sorting)
        {
            // Does not for some reason. Need to change out the whole quary
            //var quary = DatabaseContext.Images.Where(i => i.User.Id == User.Id && i.Unsupported == false);
            //switch (sorting)
            //{
            //    case SortingOptions.CreationDate:
            //        quary.OrderBy(i => i.Created);
            //        break;
            //    case SortingOptions.ScannedDate:
            //        quary.OrderBy(i => i.Scanned);
            //        break;
            //    case SortingOptions.Imagesize:
            //        quary.OrderBy(i => i.Size);
            //        break;
            //}
            //var result = quary.ToList();
            //return result;

            switch (sorting)
            {
                case SortingOptions.CreationDate:
                    return DatabaseContext.Images.Where(i => i.User.Id == User.Id && i.Unsupported == false).OrderByDescending(i => i.Created).ToList();
                case SortingOptions.ScannedDate:
                    return DatabaseContext.Images.Where(i => i.User.Id == User.Id && i.Unsupported == false).OrderByDescending(i => i.Scanned).ToList();
                case SortingOptions.Imagesize:
                    return DatabaseContext.Images.Where(i => i.User.Id == User.Id && i.Unsupported == false).OrderBy(i => i.Size).ToList();
                default:
                    return GetSurportedImages();
            }
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

        public void RemoveAllImages()
        {
            DatabaseContext.Images.RemoveRange(DatabaseContext.Images.Where(i => i.User.Id == User.Id));
        }
        #endregion
    }
}
