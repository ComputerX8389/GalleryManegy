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
    public class MainWindowViewModel : ViewModelBase
    {
        private DatabaseHandler DatabaseHandler;

        private UserModel _currentUser;
        public UserModel CurrentUser { get { return _currentUser; } set => SetProperty(ref _currentUser, value); }

        private string _currentTitle;
        public string CurrentTitle { get { return _currentTitle; } set => SetProperty(ref _currentTitle, value); }

        private FrameworkElement _currentView;
        public FrameworkElement CurrentView { get { return _currentView; } set => SetProperty(ref _currentView, value); }

        public MainWindowViewModel() : base("MainWindow")
        {
            CurrentTitle = "Connecting to database...";
            SwitchView(new LoadingView());
            Task.Run(() =>
            {
                try
                {
                    DatabaseHandler = new();
                }
                catch (Exception exe)
                {
                    MessageBox.Show("Cant open database");
                    Debug.WriteLine(exe.Message);
                    Environment.Exit(0);
                }
            }).ContinueWith(_ =>
            {
                if (DatabaseHandler.AnyUsers())
                {
                    SwitchView(new LoginView());
                }
                else
                {
                    SwitchView(new RegisterView());
                }
                // Fix for strange sta problem: https://stackoverflow.com/questions/63874479/task-delayn-system-invalidoperationexception-the-calling-thread-must-be-sta
            }, TaskScheduler.FromCurrentSynchronizationContext());

        }

        public void SwitchView(FrameworkElement view, ImageModel? image = null)
        {
            CurrentView = view;
            if (CurrentView.DataContext is IViewModel viewModel)
            {
                viewModel.SetDependencies(DatabaseHandler, image);
                viewModel.SendCommand = RunCommand;
                if (CurrentView.DataContext is ViewModelBase viewModelBase)
                {
                    CurrentTitle = viewModelBase.FileName;
                }
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
