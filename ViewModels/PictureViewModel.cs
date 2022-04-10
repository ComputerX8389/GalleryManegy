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
    internal class PictureViewModel : ViewModelBase, IViewModel
    {
        private ImageModel? _currentImage;
        public ImageModel? CurrentImage { get => _currentImage; set => SetProperty(ref _currentImage, value); }

        private ObservableCollection<ImageModel> _images;
        public ObservableCollection<ImageModel> Images { get => _images; set => SetProperty(ref _images, value); }

        public ICommand ExitPictureCommand => new DelegateCommand(ExitPicture);
        public ICommand PictureRightCommand => new DelegateCommand(PictureRight);
        public ICommand PictureLeftCommand => new DelegateCommand(PictureLeft);

        public DatabaseHandler DatabaseHandler { get; set; }
        public Action<IViewModel.Commands, object?> SendCommand { get; set; }

        public PictureViewModel() : base("Picture") { }

        private void ExitPicture(object sender)
        {
            SendCommand.Invoke(IViewModel.Commands.SelectedGallery, null);
        }

        private void PictureRight(object sender)
        {
            if (CurrentImage == null)
                return;
            
            var index = Images.IndexOf(CurrentImage) + 1;

            if (index < Images.Count)
            {
                CurrentImage = Images[index];
            }
        }

        private void PictureLeft(object sender)
        {
            if (CurrentImage == null)
                return;

            var index = Images.IndexOf(CurrentImage) - 1;

            if (index >= 0)
            {
                CurrentImage = Images[index];
            }
        }
    }
}
