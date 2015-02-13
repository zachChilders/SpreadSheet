using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SimpleClient
{
    class Server
    {
        protected TcpListener server = null;
        protected Int32 port;

        protected NetworkStream ns = null;

        public Server()
        {
            port = 8009;
            server = new TcpListener(IPAddress.Any, port);
        }

        ~Server()
        {
            server.Stop();
        }

        public void Listen()
        {

            try
            {
                server.Start();

                Byte[] bytes = new Byte[256];
                String data = null;

                while (true)
                {
                    Console.Write("Waiting for connection...");

                    //This is a blocking connection
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;

                    ns = client.GetStream();

                    int i;
                    while (ns.DataAvailable)
                    {
                        i = ns.Read(bytes, 0, bytes.Length);
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Recieved: {0}", data);

                        respond(data);
                    }
                client.Close();
                }

            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

        }

        protected void respond(string data)
        {
            data = data.ToUpper();
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

            ns.Write(msg, 0, msg.Length);
            Console.WriteLine("Sent: {0}", data);
        }

    }
}
