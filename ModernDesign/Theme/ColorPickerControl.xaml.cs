using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GSApp.Theme
{
    /// <summary>
    /// Interaction logic for ColorPickerControl.xaml
    /// </summary>
    public partial class ColorPickerControl : UserControl
    {
        private bool isDragging = false;

        public ColorPickerControl()
        {
            InitializeComponent();
            Loaded += ColorPickerControl_Loaded;
        }

        private void ColorPickerControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateThumbColor(HueSlider.Value);
        }

        private void HueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateThumbColor(HueSlider.Value);
        }

        private void ColorRectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                UpdateSliderValue(e.GetPosition(ColorRectangle).X);
                isDragging = false;
                ColorRectangle.CaptureMouse();
            }
        }

        private void ColorRectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && e.LeftButton == MouseButtonState.Pressed)
            {
                UpdateSliderValue(e.GetPosition(ColorRectangle).X);
            }
        }

        private void ColorRectangle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isDragging)
            {
                isDragging = false;
                ColorRectangle.ReleaseMouseCapture();
            }
        }

        private void UpdateSliderValue(double positionX)
        {
            double hue = (positionX / ColorRectangle.ActualWidth) * 360;
            if (hue < 0) hue = 0;
            if (hue > 360) hue = 360;
            HueSlider.Value = hue;
        }

        private void UpdateThumbColor(double hue)
        {
            Color color = ConvertHsvToRgb(hue, 1.0, 1.0);
            var thumb = (Thumb)HueSlider.Template.FindName("SliderThumb", HueSlider);
            if (thumb != null)
            {
                thumb.Background = new SolidColorBrush(color);
            }
        }

        private Color ConvertHsvToRgb(double hue, double saturation, double value)
        {
            double chroma = value * saturation;
            double huePrime = hue / 60.0;
            double x = chroma * (1 - Math.Abs(huePrime % 2 - 1));

            double r = 0, g = 0, b = 0;
            if (0 <= huePrime && huePrime < 1)
            {
                r = chroma;
                g = x;
            }
            else if (1 <= huePrime && huePrime < 2)
            {
                r = x;
                g = chroma;
            }
            else if (2 <= huePrime && huePrime < 3)
            {
                g = chroma;
                b = x;
            }
            else if (3 <= huePrime && huePrime < 4)
            {
                g = x;
                b = chroma;
            }
            else if (4 <= huePrime && huePrime < 5)
            {
                r = x;
                b = chroma;
            }
            else if (5 <= huePrime && huePrime < 6)
            {
                r = chroma;
                b = x;
            }

            double m = value - chroma;
            r += m;
            g += m;
            b += m;

            return Color.FromRgb((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
        }
    }
}