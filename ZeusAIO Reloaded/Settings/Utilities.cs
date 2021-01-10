using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZeusAIO
{
    class Utilities
    {
        public static List<Color> colorss = new List<Color>();
        private static void Writeit(int n)
        {
            File.WriteAllText("theme.zeus", n.ToString());
        }

        public static void GetColors()
        {
            foreach (var colorValue in Enum.GetValues(typeof(KnownColor)))
            {
                Color coloor = Color.FromKnownColor((KnownColor)colorValue);
                colorss.Add(coloor);
            }
        }
        public static void SetCol()
        {
            int num = 0;
            for (int i = 0; i < colorss.Count; i++)
            {
                Console.Write("[");
                Console.Write((num).ToString(), mainmenu.Theme);
                Console.Write("] ");
                string coll = colorss[i].ToString();
                coll = coll.Replace("Color ", "");
                Console.Write(coll);
                Console.WriteLine();
                num++;
            }
            Console.Write(">");
            int numm = int.Parse(Console.ReadLine());
            mainmenu.Theme = colorss[numm];
            WriteNum(numm.ToString());
        }

        private static void WriteNum(string num)
        {
            if (!File.Exists("theme.zeus"))
            {
                File.Create("theme.zeus");
                File.WriteAllText("theme.zeus", "1");
            }
            else
            {
                File.WriteAllText("theme.zeus", num);
            }
        }
        public static void GetColor()
        {
            if (!File.Exists("theme.zeus"))
            {
                File.Create("theme.zeus");
                File.WriteAllText("theme.zeus", "1");
            }
            else
            {
                if (File.Exists("theme.zeus"))
                {
                    if (File.ReadAllText("theme.zeus") == null)
                    {
                        File.WriteAllText("theme.zeus", "1");
                    }
                    string color1 = File.ReadAllText("theme.zeus");
                    try
                    {
                        int num = Convert.ToInt32(color1);
                        mainmenu.Theme = colorss[num];
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine(err);
                        MessageBox.Show("error");
                    }
                }
            }
        }
    }
}
    
