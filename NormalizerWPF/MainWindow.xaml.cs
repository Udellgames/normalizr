using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace NormalMapMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Bitmap before;

        private Bitmap after;
        public MainWindow()
        {
            InitializeComponent();
        }

        private BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            var sfd = new SaveFileDialog()
            {
                DefaultExt = "png"
            };

            if(sfd.ShowDialog() == true)
            {
                after.Save(sfd.FileName);
            }
        }

        private void OpenClick(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "png",
                Filter = "Images |*.png"
            };
            if (ofd.ShowDialog() == true)
            {
                before = new Bitmap(ofd.FileName);
                after = NormalizrCore.Normalizr.CreateNormal(before);
                LoadedImage.Source = ToBitmapImage(before);
                SavedImage.Source = ToBitmapImage(after);
            }
        }
    }
}
