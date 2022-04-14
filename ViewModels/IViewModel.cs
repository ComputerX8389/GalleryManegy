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
        public void SetDependencies(DatabaseHandler DatabaseHandler, ImageModel? CurrentImage);

        public Action<Commands, object?> SendCommand { get; set; }

        public enum Commands
        {
            SelectedImage,
            SelectedGallery,
            SelectedSettings,
            UserLogin,
            UserRegistered,
        }
    }
}
