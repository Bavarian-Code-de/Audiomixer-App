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

namespace GSApp.Controls
{
    /// <summary>
    /// Interaction logic for Oled.xaml
    /// </summary>
    public partial class Oled : UserControl
    {
        private const int DefaultWidth = 64;
        private const int DefaultHeight = 32;

        private WriteableBitmap _bitmap;

        public Oled()
        {
            InitializeComponent();
        }

        public void ClearDisplay()
        {
            DisplayText.Text = string.Empty;  // Leert den Text
            DisplayImage.Source = null;       // Leert das Bild
        }

        public void DrawText(string text, int x, int y, int fontSize = 12)
        {
            DisplayText.Margin = new Thickness(x, y, 0, 0);  // Setzt die Position
            DisplayText.FontSize = fontSize;  // Setzt die Schriftgröße
            DisplayText.Text = text;          // Setzt den Text
        }

        public void DrawImage(BitmapImage image, int x, int y)
        {
            DisplayImage.Margin = new Thickness(x, y, 0, 0);  // Setzt die Position des Bildes
            DisplayImage.Source = image;                      // Setzt das Bild
        }
    }
}