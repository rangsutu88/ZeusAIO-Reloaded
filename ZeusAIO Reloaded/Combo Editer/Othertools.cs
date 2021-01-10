using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Console = Colorful.Console;

namespace ZeusAIO
{
    class Comboediter
    {
        public static ConcurrentQueue<string> Combo = new ConcurrentQueue<string>();

        public static void otherone()
        {
            Console.Clear();
            Write.ascii();
            Console.WriteLine();
            Colorful.Console.Write("[", Color.Lavender);
            Colorful.Console.Write("1", ZeusAIO.mainmenu.Theme);
            Colorful.Console.Write("]", Color.Lavender);
            Colorful.Console.Write(" Combo Editor\n", Color.White);
            Colorful.Console.Write("[", Color.Lavender);
            Colorful.Console.Write("2", ZeusAIO.mainmenu.Theme);
            Colorful.Console.Write("]", Color.Lavender);
            Colorful.Console.Write(" Proxy Scraper\n", Color.White);
            Colorful.Console.Write("[", Color.Lavender);
            Colorful.Console.Write("3", ZeusAIO.mainmenu.Theme);
            Colorful.Console.Write("]", Color.Lavender);
            Colorful.Console.Write(" Proxy Checker\n", Color.White);
            Colorful.Console.Write("[", Color.Lavender);
            Colorful.Console.Write("4", ZeusAIO.mainmenu.Theme);
            Colorful.Console.Write("]", Color.Lavender);
            Colorful.Console.Write(" Injectable Checker\n", Color.White);
            Colorful.Console.Write("[", Color.Lavender);
            Colorful.Console.Write("5", ZeusAIO.mainmenu.Theme);
            Colorful.Console.Write("]", Color.Lavender);
            Colorful.Console.Write(" Bing Parser\n", Color.White);
            Colorful.Console.Write("[", Color.Lavender);
            Colorful.Console.Write("6", ZeusAIO.mainmenu.Theme);
            Colorful.Console.Write("]", Color.Lavender);
            Colorful.Console.Write(" Back\n", Color.White);
            Console.Write(">");
            string gayfcuk = Console.ReadLine();
            if (gayfcuk == "1")
            {
                mainmenu();
            }
            else if (gayfcuk == "2")
            {
                ZeusAIO.mainmenu.menu();
            }
            else
            {
                otherone();
            }
        }
        public static void mainmenu()
        {

            Console.Clear();
            Write.ascii();
            Console.WriteLine();
            Console.Write(">");


        }
    }
}

