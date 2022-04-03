using GalleryManegy.Handlers;
using GalleryManegy.Models;
using GalleryManegy.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GalleryManegy.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ImageModel ImageModel { get; set; }
        public UserModel UserModel { get; private set; }
        public ICommand SwitchViewCommand => new DelegateCommand(OnSwirchView);

        private FrameworkElement _currentView;
        public FrameworkElement CurrentView { get { return _currentView; } set => SetProperty(ref _currentView, value); }

        private readonly DatabaseContext DatabaseContext;
        private readonly FileScanner FileScanner;

        public MainWindowViewModel() : base("MainWindow")
        {
            DatabaseContext = new();
            // Add temp user
            //DatabaseContext.Users.Add(new UserModel()
            //{
            //    FullName = "Tobias Steffensen",
            //    Username = "steff",
            //    Password = "123qwe123",
            //    Created = DateTime.Now,
            //    LastLogin = DateTime.Now,
            //});
            //DatabaseContext.SaveChanges();
            UserModel = DatabaseContext.Users.FirstOrDefault(U => U.Username == "steff");
            //FileScanner = new(DatabaseContext, UserModel);

            ImageModel = new ImageModel()
            {
                FileName = "Testing"
            };

            SwitchView("PictureSecond");
        }

        public void SwitchView(string viewName)
        {
            switch (viewName)
            {
                case "PictureView":
                    CurrentView = new PictureView();
                    break;

                default:
                    CurrentView = new GalleryView();
                    break;
            }
        }

        private void OnSwirchView(object commandParameter)
        {
            SwitchView("PictureView");
        }
    }
}
