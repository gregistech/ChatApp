using SimpleTCP;
using System;
using System.Text;
using System.Text.RegularExpressions;
using ConsoleDecoration;

namespace ChatAppClient
{
    class Program
    {
        static void Main(string[] args)
        {
            String cmd;
            var MainClient = new ClientLogic();
            string ip;
            for (; ; )
            {
                cmd = Console.ReadLine();
                switch (cmd)
                {
                    case "connect":
                        {
                            if (!MainClient.IsConnected())
                            {
                                ColoredConsole.ColoredWrite("IP: ", ConsoleColor.Cyan);
                                ip = Console.ReadLine();

                                ColoredConsole.ColoredWrite("Port: ", ConsoleColor.Cyan);
                                string port = Console.ReadLine();

                                try
                                {
                                    MainClient.Connect(ip, Int32.Parse(port));
                                }
                                catch (FormatException)
                                {
                                    ColoredConsole.ColoredWriteLine("/// The port is invalid ///", ConsoleColor.Red);
                                }
                                catch (System.IO.FileNotFoundException)
                                {
                                    ColoredConsole.ColoredWriteLine("/// A dependency was not found (SimpleTCP.dll) ///", ConsoleColor.Red);
                                    Console.ReadLine();
                                    Environment.Exit(1);
                                }
                                break;
                            }
                            ColoredConsole.ColoredWriteLine("/// You are already connected... ///", ConsoleColor.Red);
                            break;
                        }
                    case "disconnect":
                        {
                            if (MainClient.IsConnected())
                            {
                                MainClient.Disconnect();
                                ColoredConsole.ColoredWriteLine("/// You successfully disconnected. ///", ConsoleColor.Yellow);
                                break;
                            }
                            ColoredConsole.ColoredWriteLine("/// How can you disconnect if you aren't connected? ///", ConsoleColor.Red);
                            break;
                        }
                    case "exit":
                        {
                            ColoredConsole.ColoredWriteLine("/// You are going to exit... ///", ConsoleColor.Red);
                            Console.ReadLine();
                            Environment.Exit(0);
                            break;
                        }
                    case "send":
                        {
                            if (MainClient.IsConnected())
                            {
                                while (cmd != "exit")
                                {
                                    MainClient.SetMode(1);
                                    if (!MainClient.WritedMessage)
                                        ColoredConsole.ColoredWrite("Message: ", ConsoleColor.Cyan);
                                    MainClient.WritedMessage = false;
                                    cmd = Console.ReadLine();
                                    if (!Regex.IsMatch(cmd, "^/"))
                                    {
                                        if (!string.IsNullOrWhiteSpace(cmd))
                                            MainClient.Send(cmd, true);
                                        else
                                            ColoredConsole.ColoredWriteLine("/// Write something... ///", ConsoleColor.Red);
                                    }
                                    else
                                    {
                                        cmd = cmd.Substring(1);
                                        switch (cmd)
                                        {
                                            case "clear":
                                                {
                                                    Console.Clear();
                                                    break;
                                                }
                                        }
                                    }
                                }
                                MainClient.SetMode();
                                MainClient.WritedMessage = false;
                                break;
                            }
                            ColoredConsole.ColoredWriteLine("/// First you have to connect... ///", ConsoleColor.Red);
                            break;
                        }
                    case "register":
                        {
                            if (MainClient.IsConnected())
                            {
                                ColoredConsole.ColoredWrite("Username: ", ConsoleColor.Cyan);
                                MainClient.Send("&&@@@///" + Console.ReadLine() + "&&@@@///", true);
                                break;
                            }
                            ColoredConsole.ColoredWriteLine("/// First you have to connect... ///", ConsoleColor.Red);
                            break;
                        }
                    case "clear":
                        {
                            Console.Clear();
                            break;
                        }
                }
            }
        }
    }

    class ClientLogic
    {
        SimpleTcpClient Client;
        private bool Connected = false;
        private int ClientMode = 0;
        public bool WritedMessage = false;

        public bool IsConnected()
        {
            return Connected;
        }

        public void SetMode(int mode = 0)
        {
            ClientMode = mode;
        }

        public void Disconnect()
        {
            Client.Disconnect();
            Connected = false;
        }

        public void Connect(string ip, int port)
        {
            Client = new SimpleTcpClient
            {
                StringEncoder = Encoding.Unicode
            };
            Client.DataReceived += Client_DataReceived;
            try
            {
                Client.Connect(ip, port);
                Connected = true;
                ColoredConsole.ColoredWriteLine("/// Connected To Server ///", ConsoleColor.Green);
            }
            catch (System.Net.Sockets.SocketException)
            {
                ColoredConsole.ColoredWriteLine("/// Connection timed out ///", ConsoleColor.Red);
            }
            catch (ArgumentOutOfRangeException)
            {
                ColoredConsole.ColoredWriteLine("/// The port is invalid ///", ConsoleColor.Red);
            }
        }

        private void Client_DataReceived(object sender, SimpleTCP.Message e)
        {
            string msg = e.MessageString;

            string gotmsgregex = "ˇ|//msg///";

            string msgregex = msg.Remove(msg.Length - 1);

            if (Regex.IsMatch(msgregex, gotmsgregex))
            {
                msgregex = msgregex.Replace("ˇ|//msg///", "");
                String[] StringSeperators = new String[] { @"\\\|:|///" };
                String[] parameters = msgregex.Split(StringSeperators, StringSplitOptions.None);
                ColoredConsole.ColoredWrite("\n" + parameters[0] + ": ", ConsoleColor.DarkYellow);

                Console.Write(parameters[1]);
                Console.WriteLine();
                if (ClientMode == 1)
                {
                    ColoredConsole.ColoredWrite("Message: ", ConsoleColor.Cyan);
                    WritedMessage = true;
                }

            }
            else
                Console.WriteLine(msgregex);
        }

        public void Send(string msg, bool GetReply)
        {
            if (IsConnected())
            {
                try
                {
                    if (GetReply)
                    {
                        try
                        {
                            Client.WriteLineAndGetReply(msg, TimeSpan.FromSeconds(3));
                        }
                        catch (System.IO.IOException)
                        {
                            ColoredConsole.ColoredWriteLine("/// Connection to the server was lost! ///", ConsoleColor.Red);
                            Disconnect();
                        }
                    }
                    else
                    {
                        try
                        {
                            Client.WriteLine(msg);
                        }
                        catch (System.IO.IOException)
                        {
                            ColoredConsole.ColoredWriteLine("/// Connection to the server was lost! ///", ConsoleColor.Red);
                            Disconnect();
                        }
                    }
                }
                catch (InvalidOperationException)
                {
                    ColoredConsole.ColoredWriteLine("/// First connect to a server ///", ConsoleColor.Yellow);
                }
                catch (NullReferenceException)
                {
                    ColoredConsole.ColoredWriteLine("/// First connect to a server ///", ConsoleColor.Yellow);
                }
            }
            else
                ColoredConsole.ColoredWriteLine("/// First connect to a server ///", ConsoleColor.Yellow);
        }

    }
}
