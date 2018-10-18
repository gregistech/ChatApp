using System;
using System.Text;
using System.Text.RegularExpressions;
using ConsoleDecoration;
using SimpleTCP;

namespace ChatAppClient
{
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
