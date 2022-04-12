using GalleryManegy;
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
        private DatabaseHandler DatabaseHandler;

        public ICommand PictureSelectedCommand => new DelegateCommand(PictureSelected);
        public ICommand StartScanCommand => new DelegateCommand(OnStartScan);
        public ICommand OpenSettingsCommand => new DelegateCommand(OpenSettings);

        private SettingModel? _rowSetting;
        public int RowCount 
        { 
            get 
            {
                if (_rowSetting != null)
                {
                    return _rowSetting.ValueAsInt;
                }
                else
                {
                    return 1;
                }
            }
            set
            {
                if (_rowSetting != null)
                {
                    _rowSetting.ValueAsInt = value;
                    DatabaseHandler.SaveChanges();
                }
                else
                {
                    Debug.WriteLine($"Cant save settting row setting");
                }
                SetProperty(nameof(RowCount));
                SetProperty(nameof(ImagesInGrid));
                OnChangeSize(CurrentSize);
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
            set { }
        }

        private double _rowWidth;
        public double RowWidth { get { return _rowWidth; } set => SetProperty(ref _rowWidth, value); }

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
            _images = new();
        }

        public void SetDependencies(DatabaseHandler databaseHandler, ObservableCollection<ImageModel> images, ImageModel? currentImage)
        {
            DatabaseHandler = databaseHandler;
            Images = images;
            
            _rowSetting = DatabaseHandler.GetSetting(SettingModel.SettingKeys.GalleryRowAmount);
            RowCount = _rowSetting.ValueAsInt;
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
            SendCommand.Invoke(IViewModel.Commands.SelectedImage, image);
        }

        private void OnStartScan(object sender)
        {
            SendCommand.Invoke(IViewModel.Commands.StartScan, null);
        }

        private void OpenSettings(object commandParameter)
        {
            SendCommand.Invoke(IViewModel.Commands.SelectedSettings, null);
        }
    }
}
