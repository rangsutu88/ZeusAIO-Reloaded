using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Console = Colorful.Console;


namespace ZeusAIO
{
    internal class Import
    {
        public static void LoadCombos()
        {
            Console.WriteLine();
            Console.WriteLine("Please upload combos:", Color.White);
            System.Windows.Forms.OpenFileDialog op = new System.Windows.Forms.OpenFileDialog();
            string fileName;
            do
            {
                op.Title = "Load Combos";
                op.DefaultExt = "txt";
                op.Filter = "Text Files|*.txt";
                op.RestoreDirectory = true;
                op.ShowDialog();
                fileName = op.FileName;
            }
            while (!File.Exists(fileName));

            try
            {
                mainmenu.combos = File.ReadAllLines(fileName);
            }
            catch { }
            ZeusAIO.mainmenu.comboTotal = mainmenu.combos.Count();
            Console.WriteLine("Sucessfully Loaded: " + mainmenu.comboTotal, Color.White);
            Thread.Sleep(1000);
        }

        public static void LoadProxies()
        {
            Console.WriteLine();
            Console.WriteLine("Please upload proxies:", Color.White);

            System.Windows.Forms.OpenFileDialog op = new System.Windows.Forms.OpenFileDialog();
            string fileName;
            do
            {
                op.Title = "Load Proxies";
                op.DefaultExt = "txt";
                op.Filter = "Text Files|*.txt";
                op.RestoreDirectory = true;
                op.ShowDialog();
                fileName = op.FileName;
            }
            while (!File.Exists(fileName));

            try
            {
                ZeusAIO.mainmenu.proxies = File.ReadAllLines(fileName);
            }
            catch { }

            mainmenu.proxiesCount = mainmenu.proxies.Count();
            Console.WriteLine("Successfully Loaded: " + mainmenu.proxiesCount, Color.White);
            Thread.Sleep(1000);
        }
    }
}

   