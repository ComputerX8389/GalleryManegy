using GalleryManegy.Handlers;
using GalleryManegy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GalleryManegy.ViewModels
{
    internal class RegisterViewModel : ViewModelBase
    {
        public Action UserRegistered { get; set; } 

        public ICommand RegisterCommand => RegisterCommandDelegate;
        private readonly DelegateCommand RegisterCommandDelegate;

        private string _fullname = "";
        public string Fullname
        {
            get => _fullname;
            set
            {
                SetProperty(ref _fullname, value);
                RegisterCommandDelegate.InvokeCanExecuteChanged();
            }
        }

        private string _username = "";
        public string Username
        {
            get => _username;
            set
            {
                SetProperty(ref _username, value);
                RegisterCommandDelegate.InvokeCanExecuteChanged();
            }
        }

        private string _password = "";
        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                RegisterCommandDelegate.InvokeCanExecuteChanged();
            }
        }

        private string _confirmPassword = "";
        public string ConfirmPassword { get => _confirmPassword; 
            set 
            {
                SetProperty(ref _confirmPassword, value); 
                RegisterCommandDelegate.InvokeCanExecuteChanged();
            } 
        }

        private readonly DatabaseHandler DatabaseHandler;

        public RegisterViewModel() : base("Register")
        {
            RegisterCommandDelegate = new DelegateCommand(Register, CanRegister);
            DatabaseHandler = new();
        }

        private void Register(object sender)
        {
            // If user somehow changes something before this runs
            if (CanRegister(this))
            {
                var user = new UserModel()
                {
                    FullName = Fullname,
                    Username = Username,
                    Password = Password,
                };

                if (DatabaseHandler.CreateUser(user))
                {
                    UserRegistered.Invoke();
                }
            }
        }

        private bool CanRegister(object sender)
        {
            if (Password != ConfirmPassword)
            {
                return false;
            }

            if (Password.Length == 0)
            {
                return false;
            }

            if (Fullname.Length == 0)
            {
                return false;
            }

            if (Username.Length == 0)
            {
                return false;
            }

            return true;
        }
    }
}
