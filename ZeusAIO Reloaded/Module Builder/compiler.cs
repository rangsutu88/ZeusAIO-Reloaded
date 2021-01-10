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
using ZeusAIO;

namespace Compiler
{
    class Config
    {
        public class configObject
        {
            public string Author { get; set; }
            public string ModuleName { get; set; }
            public string postUrl { get; set; }
            public string postData { get; set; }
            public string Badkeycheck { get; set; }
            public string goodkeycheck { get; set; }
            public string posttype { get; set; }
        }


        public static configObject config = new configObject();
        public static configObject renewconfig(Boolean AskToSave)
        {
            Console.Clear();
            Write.ascii();
            Colorful.Console.WriteLine("Author Name:", Color.White);
            config.Author = Console.ReadLine();
            Colorful.Console.WriteLine("Module Name:", Color.White);
            config.ModuleName = Console.ReadLine();
            Colorful.Console.WriteLine("Post Url:", Color.White);
            config.postUrl = Console.ReadLine();
            Colorful.Console.WriteLine("Post Data:", Color.White);
            config.postData = Console.ReadLine();
            Colorful.Console.WriteLine("Good Keycheck:", Color.White);
            config.goodkeycheck = Console.ReadLine();
            Colorful.Console.WriteLine("Bad Keycheck:", Color.White);
            config.Badkeycheck = Console.ReadLine();
            Colorful.Console.WriteLine("Post Type:", Color.White);
            config.posttype = Console.ReadLine();
        nigger:
            System.IO.File.WriteAllText("compiled.json", JsonConvert.SerializeObject(config));
            Console.WriteLine("Compiled Successfully...", Color.LawnGreen);
            Thread.Sleep(500);
            mainmenu.menu();
            return config;
        }
    }

}
