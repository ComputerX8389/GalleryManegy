using GalleryManegy.Handlers;
using GalleryManegy.Models;
using GalleryManegy.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace GalleryManegyTests
{
    [TestClass]
    public class ViewModelTests
    {
        [TestMethod]
        public void LoginTest()
        {
            // Wait for callbacks: https://stackoverflow.com/questions/2113697/unit-testing-asynchronous-function
            var completion = new ManualResetEvent(false);

            string username = $"test {DateTime.Now}";
            string password = $"1234";

            var db = new DatabaseHandler();
            var loginVM = new LoginViewModel();
            loginVM.SetDependencies(db, null);

            CreateTestUser(username, password, db);

            loginVM.Username = username;
            loginVM.Password = password;
            loginVM.SendCommand = (command, data) =>
            {
                Assert.AreEqual(IViewModel.Commands.UserLogin, command);
                if (data is UserModel user)
                {
                    Assert.AreEqual(IViewModel.Commands.UserLogin, command);
                    Assert.AreEqual(username, user.Username);
                    Assert.AreNotEqual(password, user.Password);
                }
                else
                {
                    Assert.Fail("Login callback data is not a userModel");
                }
                completion.Set();
            };
            loginVM.LoginCommand.Execute(this);
            completion.WaitOne();
        }

        private void CreateTestUser(string username, string password, DatabaseHandler db)
        {
            var completion = new ManualResetEvent(false);

            var registerVM = new RegisterViewModel();
            registerVM.SetDependencies(db, null);
            registerVM.Fullname = username + " testson";
            registerVM.Username = username;
            registerVM.Password = password;
            registerVM.ConfirmPassword = password;
            registerVM.SendCommand = (command, data) =>
            {
                Assert.AreEqual(IViewModel.Commands.UserRegistered, command);
                completion.Set();
            };
            registerVM.RegisterCommand.Execute(this);
            completion.WaitOne(2000);
        }

        [TestMethod]
        public async Task GalleryTest()
        {
            string username = $"test {DateTime.Now}";
            string password = $"1234";

            string imagedir = Path.Combine(Directory.GetCurrentDirectory(), "Test Images");

            var db = new DatabaseHandler();
            CreateTestUser(username, password, db);
            var user = db.Login(username, password);

            if (user == null)
            {
                Assert.Fail($"Could not login with user {username}");
                return;
            }

            db.CurrentUser = user;

            var gallerSetting = db.GetSetting(SettingModel.SettingKeys.GalleryPath);
            gallerSetting.Value = imagedir;
            db.SaveChanges();

            var galleryVM = new GalleryViewModel();
            galleryVM.SetDependencies(db, null);

            var startTime = DateTime.Now;
            while (galleryVM.Images.Count == 0)
            {
                await Task.Delay(100);
                if (DateTime.Now - startTime > new TimeSpan(0,1,0))
                {
                    Assert.Fail("GallerViewModel took too long to find images");
                }
            }
            Debug.WriteLine($"ImageCount {galleryVM.Images.Count}");
            Assert.AreEqual(10, galleryVM.Images.Count);

            galleryVM.RowCount = 2;

            Assert.AreEqual(5, galleryVM.ImagesInGrid.Count);

            galleryVM.RowCount = 5;

            Assert.AreEqual(2, galleryVM.ImagesInGrid.Count);
        }
    }
}