using GSApp.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using NAudio.CoreAudioApi;
using Microsoft.Win32;
using System.Text;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO;

namespace GSApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for Audiomixer.xaml
    /// </summary>
    public partial class Audiomixer : UserControl
    {
        Storyboard storyboard = new Storyboard();
        List<AppProcessIdHelper> appProcessIdHelper = new List<AppProcessIdHelper>();
        AudioHelper audioHelper = new AudioHelper();
        private Slider _selectedSlider;
        MainWindow mainwindow;
        
        public Audiomixer()
        {
            InitializeComponent(); 
            mainwindow = (MainWindow)Application.Current.MainWindow;
            this.Loaded += AudioMixer_Loaded;
            this.Unloaded += AudioMixer_Unloaded;


        }

        //private void AudioMixer_Loaded(object sender, RoutedEventArgs e)
        //{
        //    getAudioApps();
        //    LoadSettings();
        //    getSliderValues();
        //}
        private void getSliderValues()
        {
            try
            {
                mainwindow.port.DataReceived += SerialPort_DataReceived;
            }
            catch
            {

            }
        }
        private void AudioMixer_Loaded(object sender, RoutedEventArgs e)
        {
            getAudioApps();
            LoadSettings();
            RegisterSerialPortEvents();
        }

        private void AudioMixer_Unloaded(object sender, RoutedEventArgs e)
        {
            UnregisterSerialPortEvents();
        }
        private void RegisterSerialPortEvents()
        {
            try
            {
                if (mainwindow.port != null && mainwindow.port.IsOpen)
                {
                    mainwindow.port.DataReceived += SerialPort_DataReceived;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("Fehler beim Registrieren der Serial-Port-Ereignisse: " + ex.Message);
#endif
            }
        }
        private void UnregisterSerialPortEvents()
        {
            try
            {
                if (mainwindow.port != null && mainwindow.port.IsOpen)
                {
                    mainwindow.port.DataReceived -= SerialPort_DataReceived;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("Fehler beim Abmelden der Serial-Port-Ereignisse: " + ex.Message);
#endif
            }
        }
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string data = mainwindow.port.ReadLine();
                double value = ExtractValueFromString(data);

                Dispatcher.Invoke(() =>
                {
                    if (data.Contains("S1"))
                    {
                        ChannelOneSlider.Value = value / 10;
                    }
                    if (data.Contains("S2"))
                    {
                        ChannelTwoSlider.Value = value / 10;
                    }
                    if (data.Contains("S3"))
                    {
                        ChannelThreeSlider.Value = value / 10;
                    }
                    if (data.Contains("S4"))
                    {
                        ChannelFourSlider.Value = value / 10;
                    }
                });
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("Fehler beim Lesen von Daten vom Com-Port: " + ex.Message);
#endif
            }
        }
        static double ExtractValueFromString(string input)
        {
            // Regex, um den Wert innerhalb der runden Klammern zu extrahieren
            Regex regex = new Regex(@"\((\d+(\.\d+)?)\)");
            Match match = regex.Match(input);

            if (match.Success)
            {
                // Extrahierter Wert als double zurückgeben
                return double.Parse(match.Groups[1].Value);
            }

            throw new ArgumentException("Kein gültiger Wert gefunden");
        }
        public void getAudioApps()
        {
            InstalledAppsComboBox.Items.Clear();
            List<Process> processes = GetAudioPlayingProcesses();

            Console.WriteLine("Processes playing audio:");
            foreach (var process in processes)
            {
                InstalledAppsComboBox.Items.Add(process.ProcessName);
                appProcessIdHelper.Add(new AppProcessIdHelper { pid = process.Id, appname = process.ProcessName });
                Console.WriteLine($"{process.ProcessName} (PID: {process.Id})");
            }
        }

        public void LoadSettings()
        {
            SetTextBoxFromSettings(SliderOneApp, Properties.Settings.Default.Slider1);
            SetTextBoxFromSettings(SliderTwoApp, Properties.Settings.Default.Slider2);
            SetTextBoxFromSettings(SliderThreeApp, Properties.Settings.Default.Slider3);
            SetTextBoxFromSettings(SliderFourApp, Properties.Settings.Default.Slider4);
            
            SliderOneOled.DrawText(Properties.Settings.Default.Slider1, 0,0, 12);
            SliderTwoOled.DrawText(Properties.Settings.Default.Slider2, 0, 0, 12);
            SliderThreeOled.DrawText(Properties.Settings.Default.Slider3, 0, 0, 12);
            SliderFourOled.DrawText(Properties.Settings.Default.Slider4, 0, 0, 12);
        }

        private void SetTextBoxFromSettings(TextBox sliderApp, string value)
        {
            var textBox = (TextBox)sliderApp.Template.FindName("SearchBar", sliderApp);
            if (textBox != null)
            {
                textBox.Text = value;
            }
        }

        private void SliderAnimation(Border slider)
        {
            storyboard = new Storyboard();
            DoubleAnimation opacityAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
            };

            Storyboard.SetTarget(opacityAnimation, slider);
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(Border.OpacityProperty));

            storyboard.Children.Add(opacityAnimation);
            storyboard.Begin();
        }

        private void SelectIconOledbtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            var imagePath = ofd.FileName;
            var imageArrays = ImageConverterHelper.ConvertImageToByteArray(imagePath, 128, 64);
            var sb = new StringBuilder();
            foreach (var image in imageArrays)
            {
                sb.Append("0x" + image.ToString("X2") + ", ");
            }
            Debug.Print(sb.ToString());
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        public static List<Process> GetAudioPlayingProcesses()
        {
            List<Process> audioPlayingProcesses = new List<Process>();

            NAudio.CoreAudioApi.MMDeviceEnumerator deviceEnumerator = new NAudio.CoreAudioApi.MMDeviceEnumerator();
            MMDevice defaultDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            var sessions = defaultDevice.AudioSessionManager.Sessions;

            for (int i = 0; i < sessions.Count; i++)
            {
                var session = sessions[i];
                if (session.State == NAudio.CoreAudioApi.Interfaces.AudioSessionState.AudioSessionStateActive)
                {
                    uint processId = (uint)session.GetProcessID;
                    try
                    {
                        Process process = Process.GetProcessById((int)processId);
                        audioPlayingProcesses.Add(process);
                    }
                    catch (ArgumentException)
                    {
                        // Process might have ended between the time we got the session and now.
                    }
                }
            }

            return audioPlayingProcesses;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (InstalledAppsComboBox.SelectedItem != null)
            {
                foreach (var p in appProcessIdHelper)
                {
                    if (p.appname == InstalledAppsComboBox.SelectedItem.ToString())
                    {
                        AudioHelper.SetApplicationVolume(p.pid, (float)ChannelOneSlider.Value * 10);
                    }
                }
            }
        }

        #region BorderSliderMouseDown
        private void Slider1Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectSlider(ChannelOneSlider, SliderOneApp, Slider1Border, Properties.Settings.Default.Slider1);
        }

        private void Slider2Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectSlider(ChannelTwoSlider, SliderTwoApp, Slider2Border, Properties.Settings.Default.Slider2);
        }

        private void Slider3Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectSlider(ChannelThreeSlider, SliderThreeApp, Slider3Border, Properties.Settings.Default.Slider3);
        }

        private void Slider4Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectSlider(ChannelFourSlider, SliderFourApp, Slider4Border, Properties.Settings.Default.Slider4);
        }

        private void SelectSlider(Slider slider, TextBox sliderApp, Border sliderBorder, string settingValue)
        {
            _selectedSlider = slider;
            InstalledAppsComboBox.SelectedItem = null;
            var textBox = (TextBox)sliderApp.Template.FindName("SearchBar", sliderApp);

            storyboard.Stop();
            SliderAnimation(sliderBorder);

            if (!string.IsNullOrEmpty(settingValue))
            {
                textBox.Text = settingValue;
            }
        }
        #endregion

        private void InstalledAppsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_selectedSlider != null && InstalledAppsComboBox.SelectedItem != null)
            {
                string selectedApp = InstalledAppsComboBox.SelectedItem.ToString();
                UpdateSettingAndTextBox(_selectedSlider, selectedApp);
            }
        }

        private void UpdateSettingAndTextBox(Slider slider, string selectedApp)
        {
            if (slider == ChannelOneSlider)
            {
                Properties.Settings.Default.Slider1 = selectedApp;
                SetTextBoxFromSettings(SliderOneApp, selectedApp);
            }
            else if (slider == ChannelTwoSlider)
            {
                Properties.Settings.Default.Slider2 = selectedApp;
                SetTextBoxFromSettings(SliderTwoApp, selectedApp);
            }
            else if (slider == ChannelThreeSlider)
            {
                Properties.Settings.Default.Slider3 = selectedApp;
                SetTextBoxFromSettings(SliderThreeApp, selectedApp);
            }
            else if (slider == ChannelFourSlider)
            {
                Properties.Settings.Default.Slider4 = selectedApp;
                SetTextBoxFromSettings(SliderFourApp, selectedApp);
            }
        }

        private void InstalledAppsComboBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            getAudioApps();
        }

        private void SelectIconCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = SelectIconCombobox.SelectedItem as ComboBoxItem;
            string test = selectedItem.Content.ToString();
            if(test == "Spotify")
            {
                SliderOneOled.ClearDisplay();
                SliderOneOled.DrawImage(ConvertBitmapToBitmapImage(Properties.Resources.spotifymonochrome),0,0);
            }
        }
        public BitmapImage ConvertBitmapToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                // Speichert das Bitmap in den MemoryStream
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;

                // Lädt das BitmapImage aus dem MemoryStream
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze(); // BitmapImage einfrieren, um es thread-safe zu machen
                return bitmapImage;
            }
        }
    }
}