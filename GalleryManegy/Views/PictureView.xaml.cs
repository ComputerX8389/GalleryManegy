using GalleryManegy.Models;
using GalleryManegy.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GalleryManegy.Views
{
    /// <summary>
    /// Interaction logic for PictureView.xaml
    /// </summary>
    public partial class PictureView : UserControl
    {
        internal PictureView()
        {
            InitializeComponent();

            // Key bindings dont work if UserControl is not focused.
            // And you cant focus it by clicking on it
            Focusable = true;
            Loaded += (s, e) => Keyboard.Focus(this);
        }
    }
}
