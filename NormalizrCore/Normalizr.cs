namespace NormalizrCore
{
    using System;
    using System.Drawing;
    public static class Normalizr
    {
        public static Bitmap CreateNormal(Bitmap image)
        {
            var result = new Bitmap(image);
            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    if (x == 0 || x == image.Width - 1 || y == 0 || y == image.Height - 1)
                    {
                        result.SetPixel(x, y, System.Drawing.Color.FromArgb(image.GetPixel(x, y).A, System.Drawing.Color.Blue));
                    }
                    else
                    {
                        var horizontalChange = (image.GetPixel(x + 1, y).GetBrightness() - image.GetPixel(x - 1, y).GetBrightness()); // how much the pixel to the right is higher than the one to the left.
                        var r = (byte)(127 + horizontalChange * 128);
                        var verticalChange = (image.GetPixel(x, y + 1).GetBrightness() - image.GetPixel(x, y - 1).GetBrightness()); // how much the pixel to the bottom is higher than the one to the top.
                        var g = (byte)(127 + verticalChange * 128);
                        var b = (byte)(255 - (Math.Abs(horizontalChange) + Math.Abs(verticalChange)) * 255);
                        result.SetPixel(x, y, Color.FromArgb(image.GetPixel(x, y).A, r, g, b));
                    }
                }
            }
            return result;
        }
    }
}
