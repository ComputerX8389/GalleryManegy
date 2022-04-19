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
    public class DatabaseHandler
    {
        private readonly DatabaseContext DatabaseContext;
        private readonly Dictionary<SettingModel.SettingKeys, string> DefaultSettings;

        public UserModel CurrentUser { get; set; }

        /// <summary>
        /// Diffenrent ways to sort images
        /// </summary>
        public enum SortingOptions
        {
            CreationDate,
            ScannedDate,
            Imagesize
        }

        public DatabaseHandler()
        {
            DatabaseContext = new();

            // Set some default settings. Will only be used for new users.
            // Scould properly come from a config file
            DefaultSettings = new();
            DefaultSettings.Add(SettingModel.SettingKeys.GalleryPath, Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
            DefaultSettings.Add(SettingModel.SettingKeys.GalleryRowAmount, "6");
            DefaultSettings.Add(SettingModel.SettingKeys.ThumbnailSize, "120");
            DefaultSettings.Add(SettingModel.SettingKeys.SelectedOrder, SortingOptions.ScannedDate.ToString());

            // Create database
            DatabaseContext.Database.Migrate();
        }

        #region General
        /// <summary>
        /// Gets a setting for current user basen on given setting key
        /// </summary>
        /// <param name="key"> Setting to find </param>
        /// <returns></returns>
        public SettingModel GetSetting(SettingModel.SettingKeys key)
        {
            var setting = DatabaseContext.Settings.Where(i => i.User.Id == CurrentUser.Id).FirstOrDefault(s => s.Key == key);

            if (setting != null)
            {
                return setting;
            }
            else
            {
                setting = new SettingModel(key, DefaultSettings[key], CurrentUser);
                DatabaseContext.Settings.Add(setting);
                SaveChanges();
                return setting;
            }
        }

        /// <summary>
        /// Returns true if there are any users in database
        /// </summary>
        /// <returns></returns>
        public bool AnyUsers()
        {
            return DatabaseContext.Users.Any();
        }

        /// <summary>
        /// Attempt to login user. If success returns userModel
        /// </summary>
        /// <param name="Username"> Username </param>
        /// <param name="Password"> Password </param>
        /// <returns></returns>
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

        /// <summary>
        /// Create a new user. UserModel.Created will be overridden
        /// </summary>
        /// <param name="user"> User to add to database </param>
        /// <returns></returns>
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

        /// <summary>
        /// Save changes to any model that has been changed
        /// </summary>
        public void SaveChanges()
        {
            DatabaseContext.SaveChanges();
        }
        #endregion

        #region Images
        /// <summary>
        /// Gets all images for current user
        /// </summary>
        /// <returns></returns>
        public List<ImageModel> GetAllImages()
        {
            return DatabaseContext.Images.Where(i => i.User.Id == CurrentUser.Id).ToList();
        }

        /// <summary>
        /// Gets all images for current user. Where ImageModel.Unsupported is false
        /// </summary>
        /// <returns></returns>
        public List<ImageModel> GetSurportedImages()
        {
            return DatabaseContext.Images.Where(i => i.User.Id == CurrentUser.Id && i.Unsupported == false).ToList();
        }

        /// <summary>
        /// Gets all images for current user. Where ImageModel.Unsupported is false.
        /// Sorted by given sorting option
        /// </summary>
        /// <param name="sorting"> What colum to sort after </param>
        /// <returns></returns>
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
                    return DatabaseContext.Images.Where(i => i.User.Id == CurrentUser.Id && i.Unsupported == false).OrderByDescending(i => i.Created).ToList();
                case SortingOptions.ScannedDate:
                    return DatabaseContext.Images.Where(i => i.User.Id == CurrentUser.Id && i.Unsupported == false).OrderByDescending(i => i.Scanned).ToList();
                case SortingOptions.Imagesize:
                    return DatabaseContext.Images.Where(i => i.User.Id == CurrentUser.Id && i.Unsupported == false).OrderBy(i => i.Size).ToList();
                default:
                    return GetSurportedImages();
            }
        }

        /// <summary>
        /// Adds a new image. ImageModel.User will be overridden by current user
        /// </summary>
        /// <param name="imageModel"> Image to add </param>
        public void AddImage(ImageModel imageModel)
        {
            imageModel.User = CurrentUser;

            DatabaseContext.Images.Add(imageModel);
        }

        /// <summary>
        /// Gets first image for current user with matching fullname.
        /// </summary>
        /// <param name="fullname"> Full image path to find </param>
        /// <returns></returns>
        public ImageModel? GetImageByFullName(string fullname)
        {
            return DatabaseContext.Images.Where(i => i.User.Id == CurrentUser.Id).FirstOrDefault(i => i.FullName == fullname);
        }

        /// <summary>
        /// Remove images
        /// </summary>
        /// <param name="imageModel"> List of images to remove </param>
        public void RemoveRange(List<ImageModel> imageModel)
        {
            DatabaseContext.Images.RemoveRange(imageModel);
        }

        /// <summary>
        /// REMOVES ALL IMAGES FOR CURRENT USER.
        /// Use for starting a fresh scan.
        /// </summary>
        public void RemoveAllImages()
        {
            DatabaseContext.Images.RemoveRange(DatabaseContext.Images.Where(i => i.User.Id == CurrentUser.Id));
        }
        #endregion
    }
}
