using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace GSApp.Helper
{
    class ComConnectionHelper
    {
        string[] portnames;
        public bool isConnected = false;
        public SerialPort port;

        public void SendBytesToComPort(byte[] img)
        {
            SerialPort port = new SerialPort();
            port.PortName = "COM3";
            port.Parity = Parity.None;
            port.BaudRate = 9600;
            port.DataBits = 8;
            port.StopBits = StopBits.One;

            port.Open();
            port.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            //port.Read();


        }

        private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort port = (SerialPort)sender;

            int imgbytes = port.BytesToRead;
            byte[] buffer = new byte[imgbytes];

            if (port.BytesToRead > 1)
            {
                port.Read(buffer, 0, imgbytes);
            }
#if DEBUG
            foreach (byte item in buffer)
            {
                Console.WriteLine(item);
                Console.ReadKey();
            }
#endif
        }

        public bool GetAvailableComPorts()
        {
            bool IsPortOpen = false;
            portnames = SerialPort.GetPortNames();
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Caption like '%(COM%'"))
            {
                int i = 0;
                var portnames = SerialPort.GetPortNames();
                var ports = searcher.Get().Cast<ManagementBaseObject>().ToList().Select(p => p["Caption"].ToString());

                var portList = portnames.Select(n => n + " - " + ports.FirstOrDefault(s => s.Contains(n))).ToList();

                foreach (string s in portList)
                {
                    if (s.Contains("ESP") || s.Contains("340"))
                    {
                        port = new SerialPort(portnames[i], 9600, Parity.None, 8, StopBits.One);
                        port.Open();
                        IsPortOpen = true;
                    }
                    i++;
                }
                return IsPortOpen;
            }
        }

    }
}
