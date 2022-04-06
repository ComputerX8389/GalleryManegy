using GalleryManegy.Handlers;
using GalleryManegy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GalleryManegy.ViewModels
{
    internal class LoginViewModel : ViewModelBase
    {
        public Action<UserModel> UserLogin{ get; set; }

        public ICommand LoginCommand => new DelegateCommand(Login);

        public DatabaseHandler DatabaseHandler { get; set; }

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

        public LoginViewModel() : base("Login")
        {

        }

        private void Login(object sender)
        {
            var user = DatabaseHandler.Login(Username, Password);

            if (user != null)
            {
                UserLogin.Invoke(user);
            }
            else
            {
                MessageBox.Show("Wrong username or password");
            }
        }
    }
}
