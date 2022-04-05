using GalleryManegy.Handlers;
using GalleryManegy.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GalleryManegy.ViewModels
{
    internal class PictureViewModel : ViewModelBase
    {
        private ImageModel _currentImage;
        public ImageModel CurrentImage { get => _currentImage; set => SetProperty(ref _currentImage, value); }

        private ObservableCollection<ImageModel> _images;
        public ObservableCollection<ImageModel> Images { get => _images; set => SetProperty(ref _images, value); }

        public ICommand ExitPictureCommand => new DelegateCommand(ExitPicture);
        public ICommand PictureRightCommand => new DelegateCommand(PictureRight);
        public ICommand PictureLeftCommand => new DelegateCommand(PictureLeft);
        public Action ExitPictureAction { get; set; }

        public PictureViewModel() : base("Picture") { }

        private void ExitPicture(object sender)
        {
            ExitPictureAction.Invoke();
        }

        private void PictureRight(object sender)
        {
            var index = Images.IndexOf(CurrentImage) + 1;

            if (index < Images.Count)
            {
                CurrentImage = Images[index];
            }
        }

        private void PictureLeft(object sender)
        {
            var index = Images.IndexOf(CurrentImage) - 1;

            if (index >= 0)
            {
                CurrentImage = Images[index];
            }
        }
    }
}
