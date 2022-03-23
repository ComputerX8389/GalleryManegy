using GalleryManegy.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GalleryManegy.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ImageModel ImageModel { get; set; }
        public ICommand ChangeNameCommand => _changeDisplayNameCommand;
        
        private readonly DelegateCommand _changeDisplayNameCommand;

        public MainWindowViewModel()
        {
            ImageModel = new ImageModel()
            {
                FileName = "Testing"
            };
            _changeDisplayNameCommand = new DelegateCommand(OnChangeName, CanChangeName);
        }

        private bool CanChangeName(object commandParameter)
        {
            return ImageModel.FileName != "Walter";
        }

        private void OnChangeName(object commandParameter)
        {
            ImageModel.FileName = "Walter";
            _changeDisplayNameCommand.InvokeCanExecuteChanged();
        }
    }
}
