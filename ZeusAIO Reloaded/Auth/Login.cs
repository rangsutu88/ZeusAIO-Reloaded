using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Console = Colorful.Console;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Net;

namespace ZeusAIO
{
    class Login
    {

        [STAThread]
        public static void Main()
        {
            Utilities.GetColors();
            Utilities.GetColor();
            Console.Title = "ZeusAIO - YoBoi";
            ZeusAIO.mainmenu.menu();
        }
    }
}

      