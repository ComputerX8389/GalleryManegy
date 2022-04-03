using GalleryManegy.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GalleryManegy.ViewModels
{
    internal class GalleryViewModel : ViewModelBase
    {
        private readonly DatabaseContext Context;
        private Size CurrentSize;

        private double _rowCount;
        public double RowCount { get { return _rowCount; } set
            {
                SetProperty(ref _rowCount, value);
                OnChangeSize(CurrentSize);
                SetImages();
            }
        }

        public Action<ImageModel> PictureSelected { get; set; }

        private ObservableCollection<ObservableCollection<ImageModel>> _images;
        public ObservableCollection<ObservableCollection<ImageModel>> Images { get => _images; set => SetProperty(ref _images, value); }

        private double _rowWidth;
        public double RowWidth { get { return _rowWidth; } set => SetProperty(ref _rowWidth, value); }

        public GalleryViewModel() : base("Gallery")
        {
            Context = new();

            RowWidth = 100;
            RowCount = 4;

            SetImages();
        }

        private void SetImages()
        {
            var output = new ObservableCollection<ObservableCollection<ImageModel>>();
            // Todo use loggedin user
            var images = Context.Images.Where(i => i.User.Id == 1 && i.Unsupported == false).ToList();
            var current = new ObservableCollection<ImageModel>();

            for (int i = 0; i < images.Count; i++)
            {
                current.Add(images[i]);

                if ((i + 1) % RowCount == 0)
                {
                    output.Add(current);
                    current = new ObservableCollection<ImageModel>();
                }
            }

            if (current.Count > 0)
            {
                output.Add(current);
            }

            Images = new(output);
        }

        public void OnChangeSize(Size newSize)
        {
            // - 5 to acount for scroolbar
            RowWidth = newSize.Width / RowCount - 5;
            CurrentSize = newSize;
        }
    }
}
