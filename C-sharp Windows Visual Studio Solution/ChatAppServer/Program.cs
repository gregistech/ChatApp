using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace ChatAppServer
{
    class Program
    {
        static void Main(string[] args)
        {
            String cmd;
            var MainServer = new ServerLogic();
            for (; ; )
            {
                cmd = Console.ReadLine();
                switch (cmd)
                {
                    case "start":
                        {
                            Console.Write("IP: ");
                            string ip = Console.ReadLine();
                            MainServer.WriteToLog("IP: " + ip);

                            Console.Write("Port: ");
                            string port = Console.ReadLine();
                            MainServer.WriteToLog("Port: " + port);

                            MainServer.StartServer(ip,Int32.Parse(port));
                            break;
                        }
                    case "stop":
                        {
                            MainServer.StopServer();
                            break;
                        }
                }
            }
        }
    }
}