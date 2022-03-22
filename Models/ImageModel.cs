using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryManegy.Models
{
    internal class ImageModel : ModelBase
    {
        private string _fullPath;
        public string FullPath
        {
            get => _fullPath;
            set => SetProperty(ref _fullPath, value);
        }
    }
}
