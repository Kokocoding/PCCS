using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PCCS.Helper
{
    public class TcpHelper
    {
        private TcpClient client;
        private NetworkStream stream;
        private string serverIP;
        private int serverPort;

        public TcpHelper(string serverIP = "192.168.1.31", int serverPort = 6001)
        {
            this.serverIP = serverIP;
            this.serverPort = serverPort;
            Connect();
        }

        public bool Connect()
        {
            try
            {
                client = new TcpClient();
                client.Connect(serverIP, serverPort);
                stream = client.GetStream();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection Error: " + ex.Message);
                return false;
            }
        }

        public void Disconnect()
        {
            stream?.Close();
            client?.Close();
        }

        public void SendData(byte[] data)
        {
            try
            {
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Send Error: " + ex.Message);
            }
        }
    }
}
