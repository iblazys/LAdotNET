using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LAdotNET.Utils
{
    public class ConsoleUtils
    {
        public enum ServerType { LOGINSERVER, GAMESERVER, WORLDSERVER };

        private const string TitlePrefix = "LAdotNET: Loginserver";

        private static readonly string[] Logo = new string[]
        {
            @"                                                  ",
            @" __     _____    _       _    _____  _____  _____ ",
            @"|  |   |  _  | _| | ___ | |_ |   | ||   __||_   _|",
            @"|  |__ |     || . || . ||  _|| | | ||   __|  | |  ",
            @"|_____||__|__||___||___||_|  |_|___||_____|  |_|  ",
        };

        private static readonly string[] Credits = new string[]
        {
            $"LostArk server emulator for 1.6.2.3",
        };

        /// <summary>
        /// Writes logo and credits to Console.
        /// </summary>
        /// <param name="color">Color of the logo.</param>
        public static void WriteHeader(ConsoleColor color, ServerType server)
        {
            if (server == ServerType.LOGINSERVER)
                Console.Title = "LAdotNET: Loginserver";
            else if(server == ServerType.GAMESERVER)
                Console.Title = "LAdotNET: Gameserver";
            else
                Console.Title = "LAdotNET: Worldserver";

            Console.ForegroundColor = color;
            WriteLinesCentered(Logo);

            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.White;
            WriteLinesCentered(Credits);

            Console.ResetColor();
            WriteSeperator();
            Console.WriteLine();
        }

        /// <summary>
        /// Writes seperator in form of 80 underscores to Console.
        /// </summary>
        public static void WriteSeperator()
        {
            Console.WriteLine("".PadLeft(Console.WindowWidth, '_'));
        }

        /// <summary>
        /// Writes lines to Console, centering them as a group.
        /// </summary>
        /// <param name="lines"></param>
        private static void WriteLinesCentered(string[] lines)
        {
            var longestLine = lines.Max(a => a.Length);
            foreach (var line in lines)
                WriteLineCentered(line, longestLine);
        }

        /// <summary>
        /// Writes line to Console, centering it either with the string's
        /// length or the given length as reference.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="referenceLength">Set to greater than 0, to use it as reference length, to align a text group.</param>
        private static void WriteLineCentered(string line, int referenceLength = -1)
        {
            if (referenceLength < 0)
                referenceLength = line.Length;

            Console.WriteLine(line.PadLeft(line.Length + Console.WindowWidth / 2 - referenceLength / 2));
        }

        /// <summary>
        /// Prefixes window title with an asterisk.
        /// </summary>
        public static void LoadingTitle()
        {
            if (!Console.Title.StartsWith("* "))
                Console.Title = "* " + Console.Title;
        }

        /// <summary>
        /// Removes asterisks and spaces that were prepended to the window title.
        /// </summary>
        public static void RunningTitle()
        {
            Console.Title = Console.Title.TrimStart('*', ' ');
        }
    }
}
