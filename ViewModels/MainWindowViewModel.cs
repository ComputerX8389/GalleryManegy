using GalleryManegy.Handlers;
using GalleryManegy.Models;
using GalleryManegy.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GalleryManegy.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private readonly DatabaseHandler DatabaseHandler;

        private UserModel _currentUser;
        public UserModel CurrentUser { get { return _currentUser; } set => SetProperty(ref _currentUser, value); }

        private FrameworkElement _currentView;
        public FrameworkElement CurrentView { get { return _currentView; } set => SetProperty(ref _currentView, value); }

        public MainWindowViewModel() : base("MainWindow")
        {
            DatabaseHandler = new();

            if (DatabaseHandler.AnyUsers())
            {
                SwitchView(new LoginView());
            }
            else
            {
                SwitchView(new RegisterView());
            }
        }

        public void SwitchView(FrameworkElement view, ImageModel? image = null)
        {
            CurrentView = view;
            if (CurrentView.DataContext is IViewModel viewModel)
            {
                viewModel.SetDependencies(DatabaseHandler, image);
                viewModel.SendCommand = RunCommand;
            }
            else
            {
                Debug.WriteLine($"Cant set dependecies on {view}. cant convert DataContext to IViewModel");
            }
        }

        private void RunCommand(IViewModel.Commands command, object? data)
        {
            switch (command)
            {
                case IViewModel.Commands.SelectedImage:
                    if (data is ImageModel image)
                    {
                        SwitchView(new PictureView(), image);
                    }
                    else
                    {
                        throw new Exception("Trying to switch to picture view without a image");
                    }
                    break;

                case IViewModel.Commands.SelectedGallery:
                    SwitchView(new GalleryView());
                    break;

                case IViewModel.Commands.UserLogin:
                    if (data is UserModel user)
                    {
                        CurrentUser = user;
                        DatabaseHandler.User = CurrentUser;
                        SwitchView(new GalleryView());
                    }
                    break;

                case IViewModel.Commands.SelectedSettings:
                    SwitchView(new SettingsView());
                    break;

                case IViewModel.Commands.SelectedRegister:
                    SwitchView(new RegisterView());
                    break;

                case IViewModel.Commands.UserRegistered:
                    SwitchView(new LoginView());
                    break;
            }
        }
    }
}
