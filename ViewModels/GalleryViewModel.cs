using GalleryManegy.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryManegy.ViewModels
{
    internal class GalleryViewModel : ViewModelBase
    {
        private readonly DatabaseContext Context;

        public ObservableCollection<ImageModel> Images { get; set; }

        public GalleryViewModel() : base("Gallery")
        {
            Context = new();
            // Todo use loggedin user
            Images = new(Context.Images.Where(i => i.User.Id == 1).ToList());
        }

    }
}
