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
    internal class GalleryViewModel : ViewModelBase, IViewModel
    {
        private Size CurrentSize;

        public ICommand PictureSelectedCommand => new DelegateCommand(PictureSelected);
        public ICommand StartScanCommand => new DelegateCommand(OnStartScan);

        private double _rowCount;
        public double RowCount { get { return _rowCount; } set
            {
                SetProperty(ref _rowCount, value);
                OnChangeSize(CurrentSize);
                SetProperty(nameof(ImagesInGrid));
            }
        }

        public ObservableCollection<ObservableCollection<ImageModel>> ImagesInGrid
        {
            get
            {
                var output = new ObservableCollection<ObservableCollection<ImageModel>>();
                var current = new ObservableCollection<ImageModel>();

                for (int i = 0; i < Images.Count; i++)
                {
                    current.Add(Images[i]);

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

        public DatabaseHandler DatabaseHandler { get; set; }
        public ImageModel CurrentImage { get; set; }

        private ObservableCollection<ImageModel> _images;
        public ObservableCollection<ImageModel> Images
        {
            get => _images;
            set
            {
                SetProperty(ref _images, value);

                SetProperty(nameof(ImagesInGrid));
            }
        }

        public Action<IViewModel.Commands, object?> SendCommand { get; set; }

        public GalleryViewModel() : base("Gallery")
        {
            RowWidth = 100;
            _rowCount = 4;
            _images = new();
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
            CurrentImage = image;
            SendCommand.Invoke(IViewModel.Commands.SelectedImage, image);
        }

        private void OnStartScan(object sender)
        {
            SendCommand.Invoke(IViewModel.Commands.StartScan, null);
        }
    }
}
