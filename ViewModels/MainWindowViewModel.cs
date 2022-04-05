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
        public ImageModel ImageModel { get; set; }
        public UserModel UserModel { get; private set; }
        
        private FrameworkElement _currentView;
        public FrameworkElement CurrentView { get { return _currentView; } set => SetProperty(ref _currentView, value); }

        private ObservableCollection<ImageModel> _images;
        public ObservableCollection<ImageModel> Images { get => _images; set => SetProperty(ref _images, value); }

        private readonly FileScanner FileScanner;
        private readonly DatabaseHandler DatabaseHandler;

        public MainWindowViewModel() : base("MainWindow")
        {
            DatabaseHandler = new();
            Images = new(DatabaseHandler.GetSurportedImages());
            UserModel = DatabaseHandler.GetUser();
            //FileScanner = new(DatabaseContext, UserModel);

            ImageModel = new ImageModel()
            {
                FileName = "Testing"
            };

            SwitchView("GalleryView");
        }

        public void SwitchView(string viewName, ImageModel? image = null)
        {
            switch (viewName)
            {
                case "PictureView":
                    if (image != null)
                    {
                        CurrentView = new PictureView();
                        var pictureViewModel = (PictureViewModel)CurrentView.DataContext;
                        pictureViewModel.CurrentImage = image;
                        pictureViewModel.Images = Images;
                        pictureViewModel.ExitPictureAction = OnPictureExit;
                    }
                    break;

                default:
                    CurrentView = new GalleryView();
                    var galleryViewModel = (GalleryViewModel)CurrentView.DataContext;
                    galleryViewModel.AllImages = Images;
                    galleryViewModel.SelectedPicture = OnPictureSelected;
                    break;
            }
        }

        private void OnPictureExit()
        {
            SwitchView("GalleryView");
        }

        private void OnPictureSelected(ImageModel image)
        {
            SwitchView("PictureView", image);
        }
    }
}
