using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System;
using System.Threading;
using Colorful;
using Console = Colorful.Console;
using Newtonsoft.Json;

namespace ZeusAIO
{
    class Config
    {
        public class configObject
        {
            public string LogorCui { get; set; }
            public string DiscordID { get; set; }
            public int RefreshRate { get; set; }
            public int Retries { get; set; }
            public string Botstatus { get; set; }
            public string anti_captcha_service { get; set; }
            public string anti_captcha_key { get; set; }
            public string ignoredis { get; set; }
        }


        public static configObject config = new configObject();
        public static configObject renewconfig(Boolean AskToSave)
        {
            Console.Clear();
            Write.ascii();
            Colorful.Console.WriteLine("Enable Discord bot [y/n]: ", Color.White);
            config.Botstatus = Console.ReadLine();
            if (config.Botstatus == "y")
            {
                Colorful.Console.WriteLine("Please Enter you Discord ID [Than You can do [!s] for Stats on Discord]: ", Color.White);
                config.DiscordID = Console.ReadLine();
            }
            Colorful.Console.WriteLine("Choose your ZeusAIO experience | [1] for CUI | [2] for LOG: ", Color.White);
            config.LogorCui = Console.ReadLine();
            Colorful.Console.WriteLine("Please Enter your Desired refresh rate [For CUI Mode/ how fast the menu Refreshes]: ", Color.White);
            config.RefreshRate = int.Parse(Console.ReadLine());
            Colorful.Console.WriteLine("Please Enter your Desired Retries Amount: ", Color.White);
            config.Retries = int.Parse(Console.ReadLine());
            Colorful.Console.WriteLine("Do you want change ZeusAIO Theme? y/n", Color.White);
            string a = Console.ReadLine();
            if (a == "y")
            {
                Utilities.SetCol();
                goto LMFAO;
            }
            else if (a == "n")
            {
                goto LMFAO;
            }
            LMFAO:
            Colorful.Console.WriteLine("Do you want to setup Anticaptcha [y/n]", Color.White);
            string aaa = Console.ReadLine();
            if (aaa == "y")
            {
                Colorful.Console.WriteLine("[1] 2Captcha", Color.White);
                Colorful.Console.WriteLine("[2] AntiCaptcha", Color.White);
                string aaaa = Console.ReadLine();
                if (aaaa == "1")
                {
                    config.anti_captcha_service = "2Captcha";
                    Colorful.Console.WriteLine("Enter Key:", Color.White);
                    config.anti_captcha_key = Console.ReadLine();
                }
                else if (aaaa == "2")
                {
                    config.anti_captcha_service = "AntiCaptcha";
                    Colorful.Console.WriteLine("Enter Key:", Color.White);
                    config.anti_captcha_key = Console.ReadLine();
                }
                goto nigger;
            }
            else if (aaa == "n")
            {
                goto nigger;
            }
            config.LogorCui = Console.ReadLine();
        nigger:
            System.IO.File.WriteAllText("config.json", JsonConvert.SerializeObject(config));
            Console.WriteLine("Config saved!", Color.LawnGreen);
            Thread.Sleep(300);
            mainmenu.menu();
            return config;
        }
    }
   
}
   