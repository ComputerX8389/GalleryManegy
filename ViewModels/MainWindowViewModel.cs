using GalleryManegy.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GalleryManegy.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ImageModel ImageModel { get; set; }
        private readonly DelegateCommand _changeDisplayNameCommand;
        public ICommand ChangeNameCommand => _changeDisplayNameCommand;
        public MainWindowViewModel()
        {
            ImageModel = new ImageModel()
            {
                FullPath = "Testing"
            };
            _changeDisplayNameCommand = new DelegateCommand(OnChangeName, CanChangeName);
        }


        private bool CanChangeName(object commandParameter)
        {
            return ImageModel.FullPath != "Walter";
        }

        private void OnChangeName(object commandParameter)
        {
            ImageModel.FullPath = "Walter";
            _changeDisplayNameCommand.InvokeCanExecuteChanged();
        }
    }
}
