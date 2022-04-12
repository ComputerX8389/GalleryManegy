using GalleryManegy.Handlers;
using GalleryManegy.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GalleryManegy.ViewModels
{
    internal class SettingsViewModel : ViewModelBase, IViewModel
    {
        private DatabaseHandler DatabaseHandler;

        public Action<IViewModel.Commands, object?> SendCommand { get; set; }

        public ICommand SaveAndExitCommand => new DelegateCommand(SaveAndExit);
        public ICommand OpenFolderDialogCommand => new DelegateCommand(OpenFolderDialog);

        private SettingModel _galleryPath;
        public SettingModel GalleryPath { get { return _galleryPath; } set => SetProperty(ref _galleryPath, value); }

        public SettingsViewModel() : base("Settings") { }

        public void SetDependencies(DatabaseHandler databaseHandler, ObservableCollection<ImageModel> images, ImageModel? currentImage)
        {
            DatabaseHandler = databaseHandler;
            GalleryPath = DatabaseHandler.GetSetting(SettingModel.SettingKeys.GalleryPath);
        }

        private void SaveAndExit(object sender)
        {
            DatabaseHandler.SaveChanges();
            //SendCommand.Invoke(IViewModel.Commands.SelectedGallery, null);
        }

        private void OpenFolderDialog(object sender)
        {

        }
    }
}
