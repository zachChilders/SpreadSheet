using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace SimpleClient
{
    class Client
    {
        private TcpClient t = null;
        private NetworkStream ns = null;
        private Byte [] readBuffer = null;

        private string address
        {
            get;
            set;
        }
        private int port
        {
            get;
            set;
        }
        private int numBytes
        {
            get;
            set;
        }

        public Client(String addr, int port)
        {
            this.address = addr;
            this.port = port;
        }

        ~Client()
        {
            ns.Close();
            t.Close();
        }

        public void Connect()
        {
            t = new TcpClient();
            IPAddress ipAddress = IPAddress.Parse(this.address);
            Console.WriteLine(ipAddress);
            IPEndPoint ipep = new IPEndPoint(ipAddress, this.port);
            try
            {
                t.Connect(ipep);
                ns = t.GetStream();
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.ErrorCode);
            }
        }


        public void Send(string request)
        {
            Byte[] bytes = Encoding.ASCII.GetBytes(request);
            try
            {
                ns.Write(bytes, 0, bytes.Length);
            }
            catch(NullReferenceException)
            {
                Console.WriteLine("Something broke somewhere.");
            }
        }

        public void Recieve ()
        {
            readBuffer = new Byte[1024];

            if (ns.CanRead)
            {
                do
                {
                    this.numBytes = ns.Read(readBuffer, 0, readBuffer.Length);
                    string data = System.Text.Encoding.ASCII.GetString(readBuffer, 0, numBytes);
                    Console.WriteLine("Recieved: {0}", data);
                } while (ns.DataAvailable);
            }
            else
            {
                Console.WriteLine("Unable to read.");
            }
        }
    }
}
