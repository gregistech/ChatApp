using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDecoration
{
    public static class ColoredConsole
    {
        public static void ColoredWrite(string Msg, ConsoleColor FgColor, ConsoleColor BgColor = ConsoleColor.Black, bool ResetColor = true)
        {
            Console.ForegroundColor = FgColor;
            Console.BackgroundColor = BgColor;
            Console.Write(Msg);
            if (ResetColor)
                Console.ResetColor();
        }

        public static void ColoredWriteLine(string Msg, ConsoleColor FgColor, ConsoleColor BgColor = ConsoleColor.Black, bool ResetColor = true)
        {
            Console.ForegroundColor = FgColor;
            Console.BackgroundColor = BgColor;
            Console.WriteLine(Msg);

            if (ResetColor)
                Console.ResetColor();
        }
    }
}
