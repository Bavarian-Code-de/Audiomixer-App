

using System.ComponentModel;
using System.Windows.Controls;

namespace GSApp.MVVM.ViewModel
{
    public class AudiomixerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string sliderOneApp;
        private string sliderTwoApp;
        private string sliderThreeApp;
        private string sliderFourApp;

        private double slider1;
        private double slider2;
        private double slider3;
        private double slider4;

        public string SliderOneApp
        {
            get { return sliderOneApp; }
            set
            {
                sliderOneApp = value;
                OnPropertyChanged(nameof(SliderOneApp));
            }
        }
        public string SliderTwoApp
        {
            get { return sliderTwoApp; }
            set
            {
                sliderTwoApp = value;
                OnPropertyChanged(nameof(SliderTwoApp));
            }
        }
        public string SliderThreeApp
        {
            get { return sliderThreeApp; }
            set
            {
                sliderThreeApp = value;
                OnPropertyChanged(nameof(SliderThreeApp));
            }
        }
        public string SliderFourApp
        {
            get { return sliderFourApp; }
            set
            {
                sliderFourApp = value;
                OnPropertyChanged(nameof(SliderFourApp));
            }
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public double Slider1
        {
            get { return slider1 ; }
            set
            {
                slider1 = value;
                OnPropertyChanged(nameof(Slider1));
            }
        }
        public double Slider2
        {
            get { return slider2; }
            set
            {
                slider2 = value;
                OnPropertyChanged(nameof(Slider2));
            }
        }
        public double Slider3
        {
            get { return slider3; }
            set
            {
                slider3 = value;
                OnPropertyChanged(nameof(Slider3));
            }
        }
        public double Slider4
        {
            get { return slider4; }
            set
            {
                slider1 = value;
                OnPropertyChanged(nameof(Slider4));
            }
        }
    }
}
