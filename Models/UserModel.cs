using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BCryptNet = BCrypt.Net.BCrypt;

namespace GalleryManegy.Models
{
    internal class UserModel : ModelBase
    {
        public int Id { get; set; }

        private string _fullName = "";
        public string FullName { get => _fullName; set => SetProperty(ref _fullName, value); }

        private string _username = "";
        public string Username { get => _username; set => SetProperty(ref _username, value); }

        private string _password = "";
        public string Password
        {
            get => _password;
            set
            {
                _password = BCryptNet.HashPassword(value);
            }
        }

        private DateTime _created;
        public DateTime Created { get => _created; set => SetProperty(ref _created, value); }

        private DateTime _lastLogin;
        public DateTime LastLogin { get => _lastLogin; set => SetProperty(ref _lastLogin, value); }

        public bool CheckPassword(string password)
        {
            return BCryptNet.Verify(password, Password);
        }
    }
}
