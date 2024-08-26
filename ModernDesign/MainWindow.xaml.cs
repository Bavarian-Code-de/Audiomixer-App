using GSApp.Helper;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
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

namespace GSApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<AppProcessIdHelper> appProcessIdHelper = new List<AppProcessIdHelper>();
        List<string> allApps = new List<string>();
        public SerialPort port;
        public MainWindow()
        {
            InitializeComponent();
            establishAudioMixerConnection();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void getInstalledAppsFromPc()
        {
            AppHelper appHelper = new AppHelper();
            appHelper.getAllInstalledApps(allApps, appProcessIdHelper);
        }
        private void establishAudioMixerConnection()
        {
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Caption like '%(COM%'");
            var portNames = SerialPort.GetPortNames();
            var ports = searcher.Get()
                                .Cast<ManagementBaseObject>()
                                .Select(p => p["Caption"].ToString())
                                .ToList();

            foreach (var portName in portNames)
            {
                var matchedPort = ports.FirstOrDefault(s => s.Contains(portName));
                if (matchedPort != null && matchedPort.Contains("Silicon"))
                {
                    port = new SerialPort(portName, 115200, Parity.None, 8, StopBits.One);
                    port.Open();
                    break; // Verbindung hergestellt, keine weiteren Ports prüfen
                }
            }
        }
    }
}
