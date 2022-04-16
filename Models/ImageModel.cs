using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace GalleryManegy.Models
{
    internal class ImageModel : ModelBase
    {
        public ImageModel() { }

        public ImageModel(FileInfo file)
        {
            Scanned = DateTime.Now;
            Update(file);
        }

        public int Id { get; set; }

        private UserModel _user;
        public UserModel User { get { return _user; } set => SetProperty(ref _user, value); }

        private string _fileName = "";
        public string FileName { get => _fileName; set => SetProperty(ref _fileName, value); }

        private string? _fileLocation = "";
        public string? FileLocation { get => _fileLocation; set => SetProperty(ref _fileLocation, value); }

        private string _fullName = "";
        public string FullName { get => _fullName; set => SetProperty(ref _fullName, value); }

        private string _thumbnail = "";
        public string Thumbnail { get => _thumbnail; set => SetProperty(ref _thumbnail, value); }

        private int _width;
        public int Width { get => _width; set => SetProperty(ref _width, value); }

        private int _height;
        public int Height { get => _height; set => SetProperty(ref _height, value); }

        private string _fileType;
        public string FileType { get => _fileType; set => SetProperty(ref _fileType, value); }

        private DateTime _created;
        public DateTime Created { get => _created; set => SetProperty(ref _created, value); }

        private DateTime _modified;
        public DateTime Modified { get => _modified; set => SetProperty(ref _modified, value); }

        private DateTime _scanned;
        public DateTime Scanned { get => _scanned; set => SetProperty(ref _scanned, value); }

        private long _size;
        public long Size { get => _size; set => SetProperty(ref _size, value); }

        private bool _unsupported;
        public bool Unsupported { get => _unsupported; set => SetProperty(ref _unsupported, value); }

        // Todo Get from settings
        private readonly List<string> AllowedExtensions = new()
        {
            "PNG",
            "JPEG",
            "JPG"
        };

        internal void Update(ImageModel imagedb)
        {
            FileName = imagedb.FileName;
            FileLocation = imagedb.FileLocation;
            FullName = imagedb.FullName; 
            FileType = imagedb.FileType; 
            Created = imagedb.Created; 
            Modified = imagedb.Modified; 
            Size = imagedb.Size; 
            Unsupported = imagedb.Unsupported;
            Width = imagedb.Width;
            Height = imagedb.Height; 
        }

        public void Update(FileInfo file)
        {
            FileName = file.Name;
            // DirectoryName might be null for some reason, I dont belive it
            FileLocation = file.DirectoryName;
            FullName = file.FullName;
            FileType = file.Extension;
            Created = file.CreationTime;
            Modified = file.LastWriteTime;
            Size = file.Length;
            Unsupported = IsUnsupported(file.Extension);

            if (Unsupported == false)
            {
                var img = new BitmapImage(new Uri(file.FullName));

                Width = img.PixelWidth;
                Height = img.PixelHeight;
            }
        }

        private bool IsUnsupported(string extension)
        {
            foreach (var allowedExtension in AllowedExtensions)
            {
                if (extension.Remove(0, 1).ToLower() == allowedExtension.ToLower())
                {
                    return false;
                }
            }
            return true;
        }

        public override string ToString()
        {
            return FileName;
        }
    }
}
