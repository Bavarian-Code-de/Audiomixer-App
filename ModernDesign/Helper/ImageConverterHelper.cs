using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GSApp.Helper
{
    class ImageConverterHelper
    {
        public static Bitmap ResizeBitmap(string path, int width, int height)
        {
            using (Bitmap original = new Bitmap(path))
            {
                return new Bitmap(original, width, height);
            }
        }

        public static byte[] ConvertImageToByteArray(string imagePath, int width, int height)
        {
            // Create and resize image to fit OLED 
            using (Bitmap bitmap = ResizeBitmap(imagePath, width, height))
            {
                // Create a byte array to hold the image data
                byte[] imageData = new byte[width * height / 8];

                // Iterate through each pixel and convert to monochrome
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        var color = bitmap.GetPixel(x, y);
                        bool pixelOn = color.R < 128; // Threshold for converting to black or white

                        int byteIndex = (y * width + x) / 8;
                        int bitIndex = x % 8;

                        if (pixelOn)
                        {
                            imageData[byteIndex] |= (byte)(0x80 >> bitIndex);
                        }
                        else
                        {
                            imageData[byteIndex] &= (byte)~(0x80 >> bitIndex);
                        }
                    }
                }

                return imageData;
            }
        }

        public static Bitmap ConvertByteArrayToBitmap(byte[] imageData, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format1bppIndexed);

            // Set the palette to use only black and white colors
            ColorPalette palette = bitmap.Palette;
            palette.Entries[0] = System.Drawing.Color.Black;
            palette.Entries[1] = System.Drawing.Color.White;
            bitmap.Palette = palette;

            // Lock the bitmap's bits
            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bitmap.PixelFormat);

            // Copy the image data to the bitmap
            IntPtr ptr = bmpData.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(imageData, 0, ptr, imageData.Length);

            // Unlock the bits
            bitmap.UnlockBits(bmpData);

            return bitmap;
        }

        public static ImageSource ConvertBitmapToImageSource(Bitmap bitmap)
        {
            // Convert Bitmap to MemoryStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, ImageFormat.Png);
                memoryStream.Position = 0;

                // Convert MemoryStream to BitmapImage
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }
    }
}