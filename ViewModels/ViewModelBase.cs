using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GalleryManegy.ViewModels
{

    internal class ViewModelBase : INotifyPropertyChanged
    {
        public string _fileName;
        public string FileName { get { return _fileName; } set => SetProperty(ref _fileName, value); }

        public ViewModelBase(string filename)
        {
            FileName = filename;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string? propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }
            return false;
        }
    }
}
