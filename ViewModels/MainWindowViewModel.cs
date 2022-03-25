using GalleryManegy.Handlers;
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
        public UserModel UserModel { get; private set; }
        
        private readonly DelegateCommand _changeDisplayNameCommand;
        private readonly DatabaseContext DatabaseContext;
        private readonly FileScanner FileScanner;

        public MainWindowViewModel()
        {
            DatabaseContext = new();
            // Add temp user
            //DatabaseContext.Users.Add(new UserModel()
            //{
            //    FullName = "Tobias Steffensen",
            //    Username = "steff",
            //    Password = "123qwe123",
            //    Created = DateTime.Now,
            //    LastLogin = DateTime.Now,
            //});
            //DatabaseContext.SaveChanges();
            UserModel = DatabaseContext.Users.FirstOrDefault(U => U.Username == "steff");
            FileScanner = new(DatabaseContext, UserModel);

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
