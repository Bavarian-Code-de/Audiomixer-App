using GSApp.Helper;
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
using Microsoft.Win32;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;
using System.Windows.Media.Animation;

namespace GSApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for Audiomixer.xaml
    /// </summary>
    public partial class Audiomixer : UserControl
    {
        Storyboard storyboard = new Storyboard();
        List<AppProcessIdHelper> appProcessIdHelper = new List<AppProcessIdHelper>();
        List<string> allApps = new List<string>();
        AudioHelper audioHelper = new AudioHelper();
        string activeSlider;
        public Audiomixer()
        {
            InitializeComponent();
            //getInstalledApps();
            getAudioApps();
        }

        public void getAudioApps()
        {
            List<Process> processes = GetAudioPlayingProcesses();

            Console.WriteLine("Processes playing audio:");
            foreach (var process in processes)
            {
                InstalledAppsComboBox.Items.Add(process.ProcessName);
                appProcessIdHelper.Add(new AppProcessIdHelper { pid = process.Id, appname = process.ProcessName });
                Console.WriteLine($"{process.ProcessName} (PID: {process.Id})");
            }
        }
        public void getInstalledApps()
        {
            AppHelper appHelper = new AppHelper();
            appHelper.getAllInstalledApps(allApps, appProcessIdHelper);
            appProcessIdHelper = appHelper.getAllInstalledApps(allApps, appProcessIdHelper);
            foreach(var item in allApps)
            {
                InstalledAppsComboBox.Items.Add(item);
            }
        }
        private void SliderAnimation(Border slider)
        {
            storyboard = new Storyboard();
            // Create a DoubleAnimation to animate the Opacity property
            DoubleAnimation opacityAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
            };

            // Set the target property and target object for the animation
            Storyboard.SetTarget(opacityAnimation, slider);
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(Border.OpacityProperty));

            // Add the animation to the storyboard
            storyboard.Children.Add(opacityAnimation);

            // Start the storyboard
            storyboard.Begin();
        }


        private void SelectIconOledbtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            var imagePath = ofd.FileName;
            var imageArrays = ImageConverterHelper.ConvertImageToByteArray(imagePath, 128, 64);
            var sb = new StringBuilder();
            foreach(var image in imageArrays)
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
                if (session.State == AudioSessionState.AudioSessionStateActive)
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
                var processes = Process.GetProcessesByName(InstalledAppsComboBox.SelectedItem.ToString());
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
            activeSlider ="Slider1";
            storyboard.Stop();
            SliderAnimation(Slider1Border);
        }

        private void Slider2Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            activeSlider = "Slider2";
            storyboard.Stop();
            SliderAnimation(Slider2Border);
        }

        private void Slider3Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            activeSlider = "Slider3";
            storyboard.Stop();
            SliderAnimation(Slider3Border);
        }

        private void Slider4Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            activeSlider = "Slider4";
            storyboard.Stop();
            SliderAnimation(Slider4Border);
        }
        #endregion

        private void InstalledAppsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (activeSlider)
            {
                case "Slider1":
                    Slider1Border.Tag = InstalledAppsComboBox.SelectedItem.ToString();
                    break;
                case "Slider2":
                    Slider2Border.Tag = InstalledAppsComboBox.SelectedItem.ToString();
                    break;
                case "Slider3":
                    Slider3Border.Tag = InstalledAppsComboBox.SelectedItem.ToString();
                    break;
                case "Slider4":
                    Slider4Border.Tag = InstalledAppsComboBox.SelectedItem.ToString();
                    break;
            }
        }
    }
}
