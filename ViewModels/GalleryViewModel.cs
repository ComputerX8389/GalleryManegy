using GalleryManegy.Handlers;
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
        private readonly DatabaseHandler DatabaseHandler;
        private Size CurrentSize;

        public ICommand PictureSelectedCommand => new DelegateCommand(PictureSelected);
        public Action<ImageModel> SelectedPicture { get; set; }

        private double _rowCount;
        public double RowCount { get { return _rowCount; } set
            {
                SetProperty(ref _rowCount, value);
                OnChangeSize(CurrentSize);
                SetProperty(nameof(ImagesInGrid));
            }
        }

        private ObservableCollection<ImageModel> _allImages;
        public ObservableCollection<ImageModel> AllImages 
        {
            get => _allImages; 
            set
            {
                SetProperty(ref _allImages, value);

                SetProperty(nameof(ImagesInGrid));
            }
        }

        public ObservableCollection<ObservableCollection<ImageModel>> ImagesInGrid
        {
            get
            {
                var output = new ObservableCollection<ObservableCollection<ImageModel>>();
                var current = new ObservableCollection<ImageModel>();

                for (int i = 0; i < AllImages.Count; i++)
                {
                    current.Add(AllImages[i]);

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

                return output;
            }
            set
            {

            }
        }

        private double _rowWidth;
        public double RowWidth { get { return _rowWidth; } set => SetProperty(ref _rowWidth, value); }

        public GalleryViewModel() : base("Gallery")
        {
            RowWidth = 100;
            _rowCount = 4;
            _allImages = new();
        }
       

        public void OnChangeSize(Size newSize)
        {
            // - 5 to acount for scroolbar
            RowWidth = newSize.Width / RowCount - 5;
            CurrentSize = newSize;
        }

        private void PictureSelected(object command)
        {
            var image = (ImageModel)command;
            Debug.WriteLine("Picture selected: " + image.FileName);
            SelectedPicture.Invoke(image);
        }
    }
}
