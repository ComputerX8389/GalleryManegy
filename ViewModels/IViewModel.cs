using GalleryManegy.Handlers;
using GalleryManegy.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryManegy.ViewModels
{
    internal interface IViewModel
    {
        public DatabaseHandler DatabaseHandler { get; set; }
        public ObservableCollection<ImageModel> Images { get; set; }
        public ImageModel? CurrentImage { get; set; }
        public Action<Commands, object?> SendCommand { get; set; }

        public enum Commands
        {
            SelectedImage,
            SelectedGallery,
            UserLogin,
            UserRegistered,
            StartScan
        }
    }
}
