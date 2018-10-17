
using System;
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
}
 