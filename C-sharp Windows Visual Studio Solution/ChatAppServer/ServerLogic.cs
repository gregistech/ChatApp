using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace ChatAppServer
{
    class ServerLogic
    {
        SimpleTcpServer server;
        Dictionary<TcpClient, string> ClientUsernames = new Dictionary<TcpClient, string>();

        private void Server_DataReceived(object sender, SimpleTCP.Message e)
        {
            String msg = e.MessageString.Remove(e.MessageString.Length - 1);
            if (Regex.IsMatch(msg, "^&&@@@///.*&&@@@///$"))
            {

                int asd1 = msg.IndexOf("/") + "/".Length;
                int asd2 = msg.LastIndexOf("&");
                String result = msg.Substring(asd1 + 2, asd2 - asd1 - 3);
                ClientRegister(e.TcpClient, result);
            }
            else
            {
                if (ClientUsernames.ContainsKey(e.TcpClient))
                {
                    if (!string.IsNullOrWhiteSpace(msg))
                    {
                        Console.WriteLine("/// Message Received From " + ClientUsernames[e.TcpClient] + ": \" " + msg + " \" ///");
                        Console.WriteLine("/// Broadcasting it to all client! ///");

                        server.BroadcastLine("ˇ|//msg///" + "[" + DateTime.Now + "] " + ClientUsernames[e.TcpClient] + @" \\\|:|/// " + msg + "ˇ|//msg///");

                        WriteToLog("/// Message Received From " + ClientUsernames[e.TcpClient] + ": \" " + msg + " \" ///");
                        WriteToLog("/// Broadcasting it to all client! ///");
                    }
                    else
                    {
                        Console.WriteLine("/// Message Received From " + ClientUsernames[e.TcpClient] + ": \" " + msg + " \" ///");
                        Console.WriteLine("/// String is empty, not broadcasting it to clients! ///");

                        WriteToLog("/// Message Received From " + ClientUsernames[e.TcpClient] + ": \" " + msg + " \" ///");
                        WriteToLog("/// String is empty, not broadcasting it to clients! ///");
                    }
                }
                else
                {
                    Console.WriteLine(msg);
                    e.ReplyLine("Please register. (register command)");
                }
            }
        }

        private void ClientRegister(TcpClient client, string result)
        {
            if (!ClientUsernames.ContainsKey(client))
            {
                ClientUsernames[client] = result;
                Console.WriteLine("Client registered from " + new Uri("http://" + client.Client.RemoteEndPoint.ToString()).Host.ToString() + " (Current Username: " + ClientUsernames[client] + ")");

                server.BroadcastLine("Client registered from " + new Uri("http://" + client.Client.RemoteEndPoint.ToString()).Host.ToString() + " (Current Username: " + ClientUsernames[client] + ")");

                WriteToLog("Client registered from " + new Uri("http://" + client.Client.RemoteEndPoint.ToString()).Host.ToString() + " (Current Username: " + ClientUsernames[client] + ")");
            }
            else
            {
                Console.WriteLine("User is already registered, his/her name is " + ClientUsernames[client]);

                server.BroadcastLine("User is already registered, his/her name is " + ClientUsernames[client]);

                WriteToLog("User is already registered, his/her name is " + ClientUsernames[client]);
            }
        }

        public void StartServer(string IP, int port)
        {
            server = new SimpleTcpServer
            {
                Delimiter = 0x13,
                StringEncoder = Encoding.UTF8
            };
            server.DataReceived += Server_DataReceived;
            System.Net.IPAddress ip = System.Net.IPAddress.Parse(IP);
            try
            {
                server.Start(ip, port);
            }
            catch (SocketException)
            {
                Console.WriteLine("/// A server is running on this port, please use another one port, or stop the other server. ///");
                WriteToLog("/// A server is running on this port, please use another one port, or stop the other server. ///");
            }
            Console.WriteLine("/// Session started ///");
            WriteToLog("/// Session started ///");
        }

        public void StopServer()
        {
            if (server.IsStarted)
            {
                server.Stop();
                Console.WriteLine("/// Session ended ///");
                WriteToLog("/// Session ended ///");
            }
        }

        public void WriteToLog(string LogText)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"./log.txt", true))
            {
                file.WriteLine("\n" + "[" + DateTime.Now + "]" + LogText);
            }
        }
    }
}
