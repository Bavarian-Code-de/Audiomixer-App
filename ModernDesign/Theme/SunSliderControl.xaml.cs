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
    /// Interaction logic for SunSliderControl.xaml
    /// </summary>
    public partial class SunSliderControl : UserControl
    {
        public SunSliderControl()
        {
            InitializeComponent();
        }

            private void BrightnessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                UpdateThumbColor(e.NewValue);
            }

            private void UpdateThumbColor(double value)
            {
                // Convert slider value (0-100) to a brightness factor (0-1)
                double brightnessFactor = value / 100;

                // Calculate the new color based on the brightness factor
                byte grayValue = (byte)(brightnessFactor * 255);
                Color newColor = Color.FromRgb(grayValue, grayValue, grayValue);

                // Find the Thumb and update its color
                if (BrightnessSlider.Template.FindName("PART_Thumb", BrightnessSlider) is Thumb thumb)
                {
                    if (thumb.Template.FindName("ThumbEllipse", thumb) is Ellipse thumbEllipse)
                    {
                        thumbEllipse.Fill = new SolidColorBrush(newColor);
                    }
                }
            }
        }
}