
using System;
using System.Text.RegularExpressions;
using ConsoleDecoration;

namespace ChatAppClient
{
    class Program
    {
        private static int Connect(ClientLogic MainClient,string ip, string port)
        {
            if (!MainClient.IsConnected())
            {
                try
                {
                    MainClient.Connect(ip, int.Parse(port));
                    return 0;
                }
                catch (FormatException)
                {
                    return 1;
                }
                catch (System.IO.FileNotFoundException)
                {
                    return 2;
                }
            }
            return 3;
        }

        private static int Disconnect(ClientLogic MainClient)
        {
            if (MainClient.IsConnected())
            {
                MainClient.Disconnect();
                return 0;
            }
            return 1;
        }

        private static int Register(ClientLogic MainClient)
        {
            if (MainClient.IsConnected())
            {
                ColoredConsole.ColoredWrite("Username: ", ConsoleColor.Cyan);
                MainClient.Send("&&@@@///" + Console.ReadLine() + "&&@@@///", true);
                return 0;
            }
            return 1;
        }

        static void Main(string[] args)
        {
            String cmd;
            var MainClient = new ClientLogic();
            for (;;)
            {
                cmd = Console.ReadLine();
                switch (cmd)
                {
                    case "connect":
                        {
                            ColoredConsole.ColoredWrite("IP: ", ConsoleColor.Cyan);
                            string ip = Console.ReadLine();

                            ColoredConsole.ColoredWrite("Port: ", ConsoleColor.Cyan);
                            string port = Console.ReadLine();

                            int returnCode = Connect(MainClient, ip, port);
                            switch (returnCode)
                            {
                                case 0:
                                    {
                                        ColoredConsole.ColoredWriteLine("/// Connected To Server ///", ConsoleColor.Green);
                                        break;
                                    }
                                case 1:
                                    {
                                        ColoredConsole.ColoredWriteLine("/// The port is invalid ///", ConsoleColor.Red);
                                        break;
                                    }
                                case 2:
                                    {
                                        ColoredConsole.ColoredWriteLine("/// A dependency was not found (SimpleTCP.dll) ///", ConsoleColor.Red);
                                        Console.ReadLine();
                                        Environment.Exit(1);
                                        break;
                                    }
                                case 3:
                                    {
                                        ColoredConsole.ColoredWriteLine("/// You are already connected... ///", ConsoleColor.Red);
                                        break;
                                    }
                            }
                            break;
                        }
                    case "disconnect":
                        {
                            int returnCode = Disconnect(MainClient);
                            switch (returnCode)
                            {
                                case 0:
                                    {
                                        ColoredConsole.ColoredWriteLine("/// You successfully disconnected. ///", ConsoleColor.Yellow);
                                        break;
                                    }
                                case 1:
                                    {
                                        ColoredConsole.ColoredWriteLine("/// How can you disconnect if you aren't connected? ///", ConsoleColor.Red);
                                        break;
                                    }
                            }

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
                            int returnCode = Register(MainClient);
                            switch (returnCode)
                            {
                                case 0:
                                    {
                                        ColoredConsole.ColoredWriteLine("/// You successfully registered! ///", ConsoleColor.Green);
                                        break;
                                    }
                                case 1:
                                    {
                                        ColoredConsole.ColoredWriteLine("/// First you have to connect... ///", ConsoleColor.Red);
                                        break;
                                    }
                            }
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
}
 