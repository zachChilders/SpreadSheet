using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleClient
{
    class main
    {

        public static void Main()
        {
            Console.WriteLine("What am I? \n Client (1) | Server (2)");
            char choice = Convert.ToChar(Console.Read());

            switch (choice)
            {
                case '1':
                    Client c = new Client("127.0.0.1", 8008);
                    c.Connect();
                    c.Send("DickButt");
                    c.Recieve();
                    break;
                case '2':
                    LoadBalancer s = new LoadBalancer();
                    s.Listen();
                    break;

                default:
                    break;
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}
