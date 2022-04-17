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
        private DatabaseHandler? DatabaseHandler;
        private FileScanner? FileScanner;
        private SettingModel OrderSetting;

        public List<ComboDataModel> SortingOptions { get; private set; } = new()
        {
            new(DatabaseHandler.SortingOptions.CreationDate, "Creation date"),
            new(DatabaseHandler.SortingOptions.ScannedDate, "Scanned date"),
            new(DatabaseHandler.SortingOptions.Imagesize, "Image size")
        };

        public ICommand PictureSelectedCommand => new DelegateCommand(PictureSelected);
        public ICommand StartScanCommand => new DelegateCommand(OnStartScan);
        public ICommand OpenSettingsCommand => new DelegateCommand(OpenSettings);

        private bool _scanning;
        public bool Scanning { get => _scanning; set => SetProperty(ref _scanning, value); }

        private ComboDataModel _selectedOrder;
        public ComboDataModel SelectedOrder { get => _selectedOrder;
            set
            {
                SetProperty(ref _selectedOrder, value);
                OrderSetting.Value = value.SortingOption.ToString();

                if (DatabaseHandler != null)
                {
                    DatabaseHandler.SaveChanges();
                    var list = DatabaseHandler.GetSurportedImages(value.SortingOption);
                    Images = new(list);
                }
            }
        }

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

        public void SetDependencies(DatabaseHandler databaseHandler, ImageModel? currentImage)
        {
            DatabaseHandler = databaseHandler;
            FileScanner = new(DatabaseHandler);
            
            _rowSetting = DatabaseHandler.GetSetting(SettingModel.SettingKeys.GalleryRowAmount);
            RowCount = _rowSetting.ValueAsInt;

            // Convert string back to enum
            OrderSetting = DatabaseHandler.GetSetting(SettingModel.SettingKeys.SelectedOrder);
            Enum.TryParse(OrderSetting.Value, out DatabaseHandler.SortingOptions selectedorderkey);
            SelectedOrder = SortingOptions.FirstOrDefault(s => s.SortingOption == selectedorderkey);

            ScanForImages();
        }

        public void OnChangeSize(Size newSize)
        {
            // - 5 to acount for scroolbar
            RowWidth = newSize.Width / RowCount;
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
            //SendCommand.Invoke(IViewModel.Commands.StartScan, null);
            ScanForImages();
        }

        private void ScanForImages()
        {
            if (FileScanner != null && DatabaseHandler != null)
            {
                Scanning = true;
                FileScanner.ScanAsync().ContinueWith((sender) =>
                {
                    Scanning = false;
                    Images = new(DatabaseHandler.GetSurportedImages());
                });
            }
        }

        private void OpenSettings(object commandParameter)
        {
            SendCommand.Invoke(IViewModel.Commands.SelectedSettings, null);
        }
    }
}
