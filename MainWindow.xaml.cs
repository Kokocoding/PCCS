using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using PCCS.Helper;

namespace PCCS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TcpHelper tcp;

        Byte[][] SendData = { 
            new byte[] { 0x00 },
            new byte[] { 0x01, 0x01, 0x01, 0x01, 0x01 },
            new byte[] { 0x02, 0x02, 0x02, 0x02, 0x02 },
            new byte[] { 0x03, 0x03, 0x03 },
            new byte[] { 0x04, 0x04, 0x04, 0x04, 0x04, 0x04 },
            new byte[] { 0x05, 0x05, 0x05, 0x05, 0x05 },
            new byte[] { 0x06, 0x06, 0x06, 0x06},
            new byte[] { 0x07, 0x07, 0x07 },
            new byte[] { 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08 },
            new byte[] { 0x09, 0x09, 0x09, 0x09, 0x09, 0x09, 0x09 },
            new byte[] { 0x10, 0x10, 0x10, 0x10, 0x10 },
            new byte[] { 0x11, 0x11, 0x11, 0x11, 0x11 },
            new byte[] { 0x12, 0x12, 0x12, 0x12 },
            new byte[] { 0x13, 0x13, 0x13, 0x13, 0x13 },
            new byte[] { 0x14, 0x14, 0x14, 0x14, 0x14 },
            new byte[] { 0x15, 0x15 },
            new byte[] { 0x16, 0x16, 0x16, 0x16, 0x16 }
        };

        public MainWindow()
        {
            InitializeComponent();
            tcp = new TcpHelper("192.168.1.200", 6001);
        }

        private Byte[] ComboByte(Byte[] data1, Byte[] data2)
        {

            // Create a new array to store the combined data
            byte[] combinedData = new byte[data1.Length + data2.Length];

            // Copy the elements from data1 to the combinedData array
            Buffer.BlockCopy(data1, 0, combinedData, 0, data1.Length);

            // Copy the elements from data2 to the combinedData array, starting from the end of data1
            Buffer.BlockCopy(data2, 0, combinedData, data1.Length, data2.Length);

            return combinedData;
        }

        private void Input(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            int tagNum = int.Parse(button.Tag.ToString());
            Byte[] data = { 0xFA, 0x00, 0x00, (byte)tagNum, 0x00, 0x03, (byte)SendData[tagNum].Length, 0xFD};
            
            tcp.SendData(ComboByte(data, SendData[tagNum]));
        }

        private void IRLearn(object sender, RoutedEventArgs e)
        {
            var toggleButton = sender as ToggleButton;
            Byte[] data;

            if (toggleButton != null)
            {
                if (toggleButton.IsChecked == true)
                {
                    data = new byte[] { 0xFA, 0x01, 0x03, 0xFF, 0xFF, 0xFD };
                }
                else
                {
                    data = new byte[] { 0xFA, 0x01, 0x03, 0x00, 0x00, 0xFD };
                }

                tcp.SendData(data);
            }
        }
        private void Relay(object sender, RoutedEventArgs e)
        {
            var toggleButton = sender as ToggleButton;
            int relayNumber = int.Parse(toggleButton.Tag.ToString());
            Byte[] data;

            if (toggleButton != null)
            {
                if (toggleButton.IsChecked == true)
                {
                    data = new byte[] { 0xFA, 0x01, 0x05, (byte)relayNumber, 0x01, 0xFD };
                }
                else
                {
                    data = new byte[] { 0xFA, 0x01, 0x05, (byte)relayNumber, 0x00, 0xFD };
                }

                tcp.SendData(data);
            }            
        }
    }
}
