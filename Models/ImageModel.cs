using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryManegy.Models
{
    internal class ImageModel : ModelBase
    {
        public int Id;

        private UserModel _user;
        public UserModel User { get { return _user; } set => SetProperty(ref _user, value); }

        private string _fileName = "";
        public string FileName { get => _fileName; set => SetProperty(ref _fileName, value); }

        private string _fileLocation = "";
        public string FileLocation { get => _fileLocation; set => SetProperty(ref _fileLocation, value); }

        private int _width;
        public int Width { get => _width; set => SetProperty(ref _width, value); }

        private int _height;
        public int Height { get => _height; set => SetProperty(ref _height, value); }

        private int _bitDepth;
        public int BitDepth { get => _bitDepth; set => SetProperty(ref _bitDepth, value); }

        private FileTypes _fileType;
        public FileTypes FileType { get => _fileType; set => SetProperty(ref _fileType, value); }

        private DateTime _created;
        public DateTime Created { get => _created; set => SetProperty(ref _created, value); }

        private DateTime _modified;
        public DateTime Modified { get => _modified; set => SetProperty(ref _modified, value); }

        private DateTime _scanned;
        public DateTime Scanned { get => _scanned; set => SetProperty(ref _scanned, value); }

        private long _size;
        public long Size { get => _size; set => SetProperty(ref _size, value); }

        internal enum FileTypes
        {
            PNG,
            JPEG,
            BMP,
        }
    }
}
