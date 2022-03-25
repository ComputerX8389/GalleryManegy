using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryManegy.Models
{
    internal class UnsupportedFile : ModelBase
    {
        public UnsupportedFile() { }

        public UnsupportedFile(FileInfo file, UserModel user)
        {
            FullName = file.FullName;
            Scanned = DateTime.Now;
            User = user;
        }

        public int Id { get; set; }

        private UserModel _user;
        public UserModel User { get { return _user; } set => SetProperty(ref _user, value); }

        private string _fullName = "";
        public string FullName { get => _fullName; set => SetProperty(ref _fullName, value); }

        private DateTime _scanned;
        public DateTime Scanned { get => _scanned; set => SetProperty(ref _scanned, value); }
    }
}
