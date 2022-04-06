using GalleryManegy.Handlers;
using GalleryManegy.Models;
using GalleryManegy.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
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
        private UserModel _currentUser;
        public UserModel CurrentUser { get { return _currentUser; } set => SetProperty(ref _currentUser, value); }

        private FrameworkElement _currentView;
        public FrameworkElement CurrentView { get { return _currentView; } set => SetProperty(ref _currentView, value); }

        private ObservableCollection<ImageModel> _images;
        public ObservableCollection<ImageModel> Images { get => _images; set => SetProperty(ref _images, value); }

        private readonly FileScanner FileScanner;
        private readonly DatabaseHandler DatabaseHandler;
        private bool ImagesSetup;

        public MainWindowViewModel() : base("MainWindow")
        {
            DatabaseHandler = new();
            FileScanner = new(DatabaseHandler);

            if (DatabaseHandler.AnyUsers())
            {
                SwitchView(new LoginView());
            }
            else
            {
                SwitchView(new RegisterView());
            }
        }

        private void SetUpImages()
        {
            if (ImagesSetup == false)
            {
                Images = new(DatabaseHandler.GetSurportedImages());
                ImagesSetup = true;

                Debug.WriteLine("Scanner starting");
                FileScanner.ScanAsync().ContinueWith((sender) =>
                {
                    Debug.WriteLine("Scanner done");
                    // TODO bind gallerview to images properly
                    Images = new(DatabaseHandler.GetSurportedImages());
                });
            }
        }

        public void SwitchView(FrameworkElement view)
        {
            switch (view)
            {
                case PictureView pictureView:
                    throw new Exception("Trying to change to pictureView without a pixture");

                case GalleryView galleryView:
                    CurrentView = galleryView;
                    var galleryViewModel = (GalleryViewModel)CurrentView.DataContext;
                    SetUpImages();
                    galleryViewModel.AllImages = Images;
                    galleryViewModel.SelectedPicture = OnPictureSelected;
                    break;

                case LoginView loginView:
                    CurrentView = loginView;
                    var loginViewModel = (LoginViewModel)CurrentView.DataContext;
                    loginViewModel.DatabaseHandler = DatabaseHandler;
                    loginViewModel.UserLogin = OnUserLogin;
                    break;

                case RegisterView registerView:
                    CurrentView = registerView;
                    var registerViewModel = (RegisterViewModel)CurrentView.DataContext;
                    registerViewModel.UserRegistered = OnUserRegister;
                    break;

                default:
                    CurrentView = new LoginView();
                    break;
            }
        }

        public void SwitchView(PictureView view, ImageModel image)
        {
            if (image != null)
            {
                CurrentView = view;
                var pictureViewModel = (PictureViewModel)CurrentView.DataContext;
                pictureViewModel.CurrentImage = image;
                pictureViewModel.Images = Images;
                pictureViewModel.ExitPictureAction = OnPictureExit;
            }
        }

        private void OnPictureExit()
        {
            SwitchView(new GalleryView());
        }

        private void OnPictureSelected(ImageModel image)
        {
            SwitchView(new PictureView(), image);
        }

        private void OnUserRegister()
        {
            SwitchView(new LoginView());
        }

        private void OnUserLogin(UserModel user)
        {
            CurrentUser = user;
            DatabaseHandler.User = CurrentUser;
            SwitchView(new GalleryView());
        }
    }
}
