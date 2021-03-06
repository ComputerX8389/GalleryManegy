using GalleryManegy.Handlers;
using GalleryManegy.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace GalleryManegy.ViewModels
{
    public class SettingsViewModel : ViewModelBase, IViewModel
    {
        private DatabaseHandler DatabaseHandler;
        private int OriginalThumbnailSize;

        public Action<IViewModel.Commands, object?> SendCommand { get; set; }

        public ICommand SaveAndExitCommand => new DelegateCommand(SaveAndExit);
        public ICommand OpenFolderDialogCommand => new DelegateCommand(OpenFolderDialog);

        private SettingModel _galleryPath;
        public SettingModel GalleryPath { get { return _galleryPath; } set => SetProperty(ref _galleryPath, value); }

        private SettingModel _thumbnailSize;
        public SettingModel ThumbnailSize { get { return _thumbnailSize; } set => SetProperty(ref _thumbnailSize, value); }

        public SettingsViewModel() : base("Settings") { }

        public void SetDependencies(DatabaseHandler databaseHandler, ImageModel? currentImage)
        {
            DatabaseHandler = databaseHandler;
            GalleryPath = DatabaseHandler.GetSetting(SettingModel.SettingKeys.GalleryPath);
            ThumbnailSize = DatabaseHandler.GetSetting(SettingModel.SettingKeys.ThumbnailSize);
            OriginalThumbnailSize = ThumbnailSize.ValueAsInt;
        }

        private void SaveAndExit(object sender)
        {
            // Rescan all images to create new thumbnails, if Thumbnail Size changed
            if (OriginalThumbnailSize != ThumbnailSize.ValueAsInt)
            {
                DatabaseHandler.RemoveAllImages();
            }
            DatabaseHandler.SaveChanges();
            SendCommand.Invoke(IViewModel.Commands.SelectedGallery, null);
        }

        private void OpenFolderDialog(object sender)
        {
            var dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                var path = dialog.SelectedPath;
                GalleryPath.Value = path;
            }
        }
    }
}
