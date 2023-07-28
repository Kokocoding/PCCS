using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PCCS.Helper
{
    public class PortHelper
    {
        public SerialPort sp = new SerialPort();
        public List<string> PortCombo = new List<string>();
        public PortHelper()
        {
            OpenPort();
            PortCombo = DetectUsedPortName();
        }

        public void OpenPort()
        {
            if (sp.IsOpen)
            {
                sp.Close();
            }
            sp.PortName = "COM4";
            sp.BaudRate = 9600;
            sp.Parity = Parity.None;
            sp.DataBits = 8;
            sp.StopBits = StopBits.One;
            try
            {
                sp.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void PortWrite(Byte[] data)
        {
            if (!sp.IsOpen)
            {
                try
                {
                    sp.Open();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            if (sp.IsOpen)
            {
                sp.Write(data, 0, data.Length);
            }
        }

        public List<string> DetectUsedPortName()
        {
            List<string> portCom = new List<string>();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Caption LIKE '%(COM%'");
            foreach (ManagementObject port in searcher.Get())
            {
                string portName = port["Caption"].ToString();
                if (portName.Contains("COM"))
                {
                    int startIndex = portName.IndexOf("(COM") + 1;
                    int endIndex = portName.IndexOf(")", startIndex);
                    string detectedPortName = portName.Substring(startIndex, endIndex - startIndex);
                    if (SerialPort.GetPortNames().Contains(detectedPortName))
                    {
                        portCom.Add(detectedPortName);
                    }
                }
            }
            return portCom;
        }
    }
}
