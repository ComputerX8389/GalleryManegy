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
    public interface IViewModel
    {
        /// <summary>
        /// Set DatabaseHandler and ImageModel dependencies. This is expected to called right after the construktor
        /// </summary>
        /// <param name="DatabaseHandler"> DatabaseHandler </param>
        /// <param name="CurrentImage"> Selected image </param>
        public void SetDependencies(DatabaseHandler DatabaseHandler, ImageModel? CurrentImage);

        /// <summary>
        /// Command action. Will be called with diffenrent commands from ViewModel
        /// </summary>
        public Action<Commands, object?> SendCommand { get; set; }

        /// <summary>
        /// All diffenrent commands that can be sendt with SendCommand
        /// </summary>
        public enum Commands
        {
            SelectedImage,
            SelectedGallery,
            SelectedSettings,
            UserLogin,
            UserRegistered,
            SelectedRegister,
        }
    }
}
