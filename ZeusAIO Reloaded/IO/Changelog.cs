using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Console = Colorful.Console;

namespace ZeusAIO.IO
{
    class Changelog
    {
       public static void changelog()
        {
            string version = App.GrabVariable("8MbBR0bA8Ju5e0MmSwSn3XitUPs41");
            string b1 = App.GrabVariable("mK00MR86pPrWIB7CDOu3rDO8B1Gmn");
            string b2 = App.GrabVariable("2WpLL7D4kbHKLBraLKjY2s0wz0fMJ");
            string b3 = App.GrabVariable("uktB99TyHJOVPm4pHmuDlrRi1XZ7H");
            string b4 = App.GrabVariable("GEvTU8uPqx1sVTgK00N8oNySEHDKo");
            Console.Clear();
            Write.ascii();
            Console.WriteLine("                                  <Welcome Back to ZeusAIO Reloaded>" , Color.Magenta);
            Console.WriteLine("                                          Whats New in " + version , Color.Green);
            Console.WriteLine();
            Console.WriteLine("- " + b1);
            Console.WriteLine("- " + b2);
            Console.WriteLine("- " + b3);
            Console.WriteLine("- " + b4);
            Console.WriteLine();
            Console.WriteLine("- Press any key to continue...");
            Thread.Sleep(200);
            Console.ReadKey();
            File.Delete("Db.zeus");
            ZeusAIO.mainmenu.menu();


        }
        public static string Server = App.GrabVariable("nN05P3UuTuojls9kLEJi5OiplzkVt");

    }
}
