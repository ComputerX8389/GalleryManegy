using GalleryManegy.Handlers;
using GalleryManegy.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GalleryManegy.ViewModels
{
    public class LoginViewModel : ViewModelBase, IViewModel
    {
        private DatabaseHandler DatabaseHandler;

        public ICommand LoginCommand => new DelegateCommand(Login);
        public ICommand RegisterCommand => new DelegateCommand(Register);

        private string _username = "";
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string _password = "";
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        public ObservableCollection<ImageModel> Images { get; set; }
        public ImageModel? CurrentImage { get; set; }
        public Action<IViewModel.Commands, object?> SendCommand { get; set; }

        public LoginViewModel() : base("Login")
        {

        }

        private void Login(object sender)
        {
            var user = DatabaseHandler.Login(Username, Password);

            if (user != null)
            {
                SendCommand.Invoke(IViewModel.Commands.UserLogin, user);
            }
            else
            {
                MessageBox.Show("Wrong username or password");
            }
        }

        private void Register(object sender)
        {
            SendCommand.Invoke(IViewModel.Commands.SelectedRegister, null);
        }

        public void SetDependencies(DatabaseHandler databaseHandler, ImageModel? currentImage)
        {
            DatabaseHandler = databaseHandler;
        }
    }
}
