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
                after = CreateNormal(before);
                LoadedImage.Source = ToBitmapImage(before);
                SavedImage.Source = ToBitmapImage(after);
            }
        }

        private Bitmap CreateNormal(Bitmap image)
        {
            var result = new Bitmap(image);
            for(var x= 0; x< image.Width ; x++)
            {
                
                for(var y=0; y< image.Height; y++)
                {
                    if (x == 0 || x == image.Width - 1 || y == 0 || y == image.Height - 1)
                    {
                        result.SetPixel(x, y, System.Drawing.Color.FromArgb(image.GetPixel(x,y).A,System.Drawing.Color.Blue));
                    }
                    else
                    {
                        var horizontalChange = (image.GetPixel(x + 1, y).GetBrightness() - image.GetPixel(x - 1, y).GetBrightness()); // how much the pixel to the right is higher than the one to the left.
                        byte r = (byte)(127 + horizontalChange * 128);
                        var verticalChange = (image.GetPixel(x, y + 1).GetBrightness() - image.GetPixel(x, y - 1).GetBrightness()); // how much the pixel to the bottom is higher than the one to the top.
                        byte g = (byte)(127 + verticalChange * 128);
                        byte b = (byte)(255 - (Math.Abs(horizontalChange) + Math.Abs(verticalChange)) * 255);
                        result.SetPixel(x, y, System.Drawing.Color.FromArgb(image.GetPixel(x, y).A, r, g, b));
                    }
                }
            }
            return result;
        }
    }
}
