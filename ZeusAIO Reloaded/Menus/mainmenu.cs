using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeusAIO.IO;
using Console = Colorful.Console;

namespace ZeusAIO
{
    class mainmenu
    {
        public static Color Theme = Color.LawnGreen;
        public static string options = "";
        public static string bots = "";
        public static string bots2 = "";
        static List<Func<string[], string, bool>> pickedModules = new List<Func<string[], string, bool>>();
        public static List<string> pickedModulesNames = new List<string>();
        public static void menu()
        {
            string fileName = "config.json";
            ZeusAIO.Config.config = File.Exists(fileName) ? JsonConvert.DeserializeObject<Config.configObject>(File.ReadAllText(fileName)) : Config.renewconfig(true);
            if (ZeusAIO.Config.config.LogorCui == "1")
            {
                options = "Cui";
            }
            else if (ZeusAIO.Config.config.LogorCui == "2")
            {
                options = "Log";
            }
            if (Config.config.Botstatus == "y")
            {
                bots = "Enabled";
            }
            else if (Config.config.Botstatus == "n")
            {
                bots = "Disabled";
            }
            if (Config.config.Botstatus == "y")
            {
                bots2 = Config.config.DiscordID;
            }
            else if (Config.config.Botstatus == "n")
            {
                bots2 = "Bot is Disabled";
            }
            Export.Initialize();
            Console.Clear();
            Write.ascii();
            Console.WriteLine("Welcome Back: " + "UHQ UHQ UHQ person", Color.White);
            Console.WriteLine("Latest Discord Server: (" + Changelog.Server + ")", Color.White);
            Console.WriteLine();
            Console.WriteLine("The Settings file is located in (config.json)", Color.White);
            Console.WriteLine();
            Console.WriteLine("Discord ID: " + bots2, Color.White);
            Console.WriteLine("Refresh Rate: " + Config.config.RefreshRate, Color.White);
            Console.WriteLine("Anticaptcha Service: " + Config.config.anti_captcha_service, Color.White);
            Console.WriteLine("Anticaptcha Key: " + "*******************", Color.White);
            Console.WriteLine("User interface: " + options, Color.White);
            Console.WriteLine("Discord bot: " + bots, Color.White);
            Console.WriteLine();
            Console.Write("[", Color.Lavender);
            Console.Write("1", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Modules\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("2", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Change Settings\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("3", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Other tools\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("4", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Module Builder\n", Color.White);
            Console.Write(">", Color.White);
            string inputmenu = Console.ReadLine();
            if (inputmenu == "1")
            {
                settingsmodules();
            }
            else if (inputmenu == "2")
            {
                Config.renewconfig(true);
            }
            else if (inputmenu == "3")
            {
                ZeusAIO.Comboediter.otherone();
            }
            else if (inputmenu == "4")
            {
                hahah:
                Console.Clear();
                Write.ascii();
                Console.WriteLine();
                Console.WriteLine("[!] Important Notice: This Compiler is in Beta Stage, many bugs *WILL* arise which will be fixed in every update" , Color.Magenta);
                Console.WriteLine();
                Console.Write("[", Color.Lavender);
                Console.Write("1", Theme);
                Console.Write("]", Color.Lavender);
                Console.Write(" Build Module\n", Color.White);
                Console.Write("[", Color.Lavender);
                Console.Write("2", Theme);
                Console.Write("]", Color.Lavender);
                Console.Write(" Run Compiled Module (compiled.json)\n", Color.White);
                Console.Write("[", Color.Lavender);
                Console.Write("3", Theme);
                Console.Write("]", Color.Lavender);
                Console.Write(" Back\n", Color.White);
                Console.Write(">");
                string aaaa = Console.ReadLine();
                if (aaaa == "1")
                {
                    Compiler.Config.renewconfig(true);
                }
                else if (aaaa == "2")
                {
                    string fileName2 = "compiled.json";
                    Compiler.Config.config = File.Exists(fileName) ? JsonConvert.DeserializeObject<Compiler.Config.configObject>(File.ReadAllText(fileName2)) : Compiler.Config.renewconfig(true);
                    gay = "yep";
                    pickedModulesNames.Add("Custom");
                    settingsmodules();
                }
                else if (aaaa == "3")
                {
                    menu();

                }
                else
                {
                    goto hahah;
                }
               
            }
            else
            {
                menu();
            }
        }
        public static void settingsmodules()
        {
        ThreadsInput:
            Console.Clear();
           Write.ascii();
            Console.WriteLine("Please select threads: ", Color.White);
            while (globalThreads <= 0)

                try
                {
                    globalThreads = int.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Error Parsing Integor ", Color.Red);
                    Thread.Sleep(500);
                    goto ThreadsInput;
                }
            ProxyTypeInput:
            Console.WriteLine("Please select proxy type:");
            Console.Write("[", Color.Lavender);
            Console.Write("1", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Http/s\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("2", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Socks4\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("3", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Socks5\n", Color.White);
            string proxytype = Console.ReadLine();

            if (proxytype == "1")
            {
                proxyProtocol = "HTTP";
                Console.WriteLine();
                Console.WriteLine("Using HTTP/S", Color.White);
            }
            else if (proxytype == "2")
            {
                proxyProtocol = "SOCKS4";
                Console.WriteLine();
                Console.WriteLine("Using SOCKS4", Color.White);
            }
            else if (proxytype == "3")
            {
                proxyProtocol = "SOCKS5";
                Console.WriteLine();
                Console.WriteLine("Using SOCKS5", Color.White);
            }
            else
            {
                Console.WriteLine("Invalid Input!", Color.Red);
                goto ProxyTypeInput;
            }
            Import.LoadCombos();
            Import.LoadProxies();
            if (gay == "yep")
            {
                Thread.Sleep(500);
                pickedModules.Add(CompilerRunner.Check.CheckAccount);
                Console.Clear();
                Write.ascii();
                Console.WriteLine();
                Console.WriteLine("Author: " + Compiler.Config.config.Author);
                Console.WriteLine("Module Name: " + Compiler.Config.config.ModuleName);
                Console.WriteLine();
                Console.WriteLine("[+] Press any key to start..." , Color.Magenta);
                Console.ReadKey();
                start();
            }
            modules();

        }
        public static void modules()
        {
            if (Config.config.Botstatus == "y")
            {
                new Thread(BotShits).Start();
            }
            Console.Clear();
            Write.ascii();
            Console.WriteLine("[1] Adfly");
            Console.WriteLine("[2] Albertsons");
            Console.WriteLine("[3] Aliexpress (Valid Mail)");
            Console.WriteLine("[4] Antipublic");
            Console.WriteLine("[5] Apowersoft");
            Console.WriteLine("[6] Avira");
            Console.WriteLine("[7] Azure");
            Console.WriteLine("[8] Bagelboy");
            Console.WriteLine("[9] Beeg.com");
            Console.WriteLine("[10] Bitesquad");
            Console.WriteLine("[11] Bitlaunch.io");
            Console.WriteLine("[12] Blim");
            Console.WriteLine("[13] Buffalo Wild Wings");
            Console.WriteLine("[14] Cbs");
            Console.WriteLine("[15] Coinbase");
            Console.WriteLine("[16] Coins.ph");
            Console.WriteLine("[17] Coldstone");
            Console.WriteLine("[18] CrunchyRoll");
            Console.WriteLine("[19] Dc Universe");
            Console.WriteLine("[20] Disney+");
            Console.WriteLine("[21] Dominos (US)");
            Console.WriteLine("[22] Doordash");
            Console.WriteLine("[23] Duolingo");
            Console.WriteLine("[24] Easyjet.com");
            Console.WriteLine("[25] Ebay (Valid Mail)");
            Console.WriteLine("[26] Elastic Email");
            Console.WriteLine("[27] Facebook");
            Console.WriteLine("[28] Flimin");
            Console.WriteLine("[29] Fitbit");
            Console.WriteLine("[30] Flight Club");
            Console.WriteLine("[31] Foap");
            Console.WriteLine("[32] Forever 21");
            Console.WriteLine("[33] Fox");
            Console.WriteLine("[34] Funimation");
            Console.WriteLine("[35] Fwrd");
            Console.WriteLine("[36] Gamefly");
            Console.WriteLine("[37] GetUpSide");
            Console.WriteLine("[38] Gfuel");
            Console.WriteLine("[39] Godaddy");
            Console.WriteLine("[40] Goose vpn");
            Console.WriteLine("[41] Gucci");
            Console.WriteLine("[42] Headspace");
            Console.WriteLine("[43] Hide My Ass");
            Console.WriteLine("[44] Hola vpn");
            Console.WriteLine("[45] Hotspot shield");
            Console.WriteLine("[46] Hulu");
            Console.WriteLine("[47] Ib vpn");
            Console.WriteLine("[48] Imvu");
            Console.WriteLine("[49] Instagram");
            Console.WriteLine("[50] IP vanish");
            Console.WriteLine("[51] Kfc (AUS)");
            Console.WriteLine("[52] Later.com");
            Console.WriteLine("[53] Luminati");
            Console.WriteLine("[54] Mail Access");
            Console.WriteLine("[55] Mcafee");
            Console.WriteLine("[56] Md5 Dehasher");
            Console.WriteLine("[57] Microsoft/Edu Mail Access");
            Console.WriteLine("[58] Minecraft");
            Console.WriteLine("[59] Mingle.com");
            Console.WriteLine("[60] Mycanal");
            Console.WriteLine("[61] Napster");
            Console.WriteLine("[62] Nba");
            Console.WriteLine("[63] Netflix");
            Console.WriteLine("[64] Nord vpn");
            Console.WriteLine("[65] Origin");
            Console.WriteLine("[66] Outback steak house");
            Console.WriteLine("[67] Pandora");
            Console.WriteLine("[68] Patreon.com");
            Console.WriteLine("[69] Pornhub");
            Console.WriteLine("[70] Pinterest");
            Console.WriteLine("[71] Pizzahut uk (Valid Mail)");
            Console.WriteLine("[72] Plex tv");
            Console.WriteLine("[73] Postmates fleet");
            Console.WriteLine("[74] Psn (Valid Mail)");
            Console.WriteLine("[75] Robinhood");
            Console.WriteLine("[76] Scribd");
            Console.WriteLine("[77] Shawacademy");
            Console.WriteLine("[78] Shemaroo.me");
            Console.WriteLine("[79] Shopify");
            Console.WriteLine("[80] Shuttershock");
            Console.WriteLine("[81] Slicelife");
            Console.WriteLine("[82] Sling tv");
            Console.WriteLine("[83] Smartproxy");
            Console.WriteLine("[84] Smoothie king");
            Console.WriteLine("[85] Surfshark");
            Console.WriteLine("[86] Symbolab");
            Console.WriteLine("[87] Tiger vpn");
            Console.WriteLine("[88] Tubi tv");
            Console.WriteLine("[89] Tunnel bear");
            Console.WriteLine("[90] Twitch");
            Console.WriteLine("[91] Twitch (Username Checker)");
            Console.WriteLine("[92] Udacity");
            Console.WriteLine("[93] Ufc tv");
            Console.WriteLine("[94] Ullu");
            Console.WriteLine("[95] Upcloud");
            Console.WriteLine("[96] Uplay");
            Console.WriteLine("[97] Valorant");
            Console.WriteLine("[98] Viaplay");
            Console.WriteLine("[99] Voot");
            Console.WriteLine("[100] Vortex.gg");
            Console.WriteLine("[101] Vyper vpn");
            Console.WriteLine("[102] Waves.com");
            Console.WriteLine("[103] Wish");
            Console.WriteLine("[104] Wordpress");
            Console.WriteLine("[105] Wwe");
            Console.WriteLine("[106] Xcams");
            Console.WriteLine("[107] Xvpn");
            Console.WriteLine("[108] Yahoo");
            Console.WriteLine("[109] Zee5");
            Console.WriteLine("[110] Aha.com");
            Console.WriteLine("[111] Bitdefender");
            Console.WriteLine("[112] Epix now");
            Console.WriteLine("[113] Kaspersky");
            Console.WriteLine("[114] Etsy");
            Console.WriteLine("[115] ClaroVideo");
            Console.WriteLine("[116] Venmo");
            Console.WriteLine("[117] Apple");
            Console.WriteLine("[118] Onlyfans (Captcha Key Needed)");
            Console.WriteLine("[119] Spotify");
            Console.WriteLine("[120] Heaspace (UK)");
            Console.WriteLine("[111] Sony Vegas");
            List<string> modules = new List<string>();
            string[] i = Console.ReadLine().Split(',');
            foreach (string j in i)
            {
                modules.Add(j);
            }
            if (modules.Contains("1"))
            {
                pickedModules.Add(Adfly.Check.CheckAccount);
                pickedModulesNames.Add("Adfly");
            }
            if (modules.Contains("2"))
            {
                pickedModules.Add(Albertsons.Check.CheckAccount);
                pickedModulesNames.Add("Albertsons");
            }
            if (modules.Contains("3"))
            {
                pickedModules.Add(Aliexpress.Check.CheckAccount);
                pickedModulesNames.Add("Aliexpress");
            }
            if (modules.Contains("4"))
            {
                pickedModules.Add(Antipublic.Check.CheckAccount);
                pickedModulesNames.Add("Antipublic");
            }
            if (modules.Contains("5"))
            {
                pickedModules.Add(Apowersoft.Check.CheckAccount);
                pickedModulesNames.Add("Apowersoft");
            }
            if (modules.Contains("6"))
            {
                pickedModules.Add(Avira.Check.CheckAccount);
                pickedModulesNames.Add("Avira");
            }
            if (modules.Contains("7"))
            {
                pickedModules.Add(Azure.Check.CheckAccount);
                pickedModulesNames.Add("Azure");
            }
            if (modules.Contains("8"))
            {
                pickedModules.Add(Bagelboy.Check.CheckAccount);
                pickedModulesNames.Add("Bagelboy");
            }
            if (modules.Contains("9"))
            {
                pickedModules.Add(Beeg.Check.CheckAccount);
                pickedModulesNames.Add("Beeg.com");
            }
            if (modules.Contains("10"))
            {
                pickedModules.Add(Bitesquad.Check.CheckAccount);
                pickedModulesNames.Add("Bitesquad");
            }
            if (modules.Contains("11"))
            {
                pickedModules.Add(Bitlaunch.Check.CheckAccount);
                pickedModulesNames.Add("Bitlaunch.io");
            }
            if (modules.Contains("12"))
            {
                pickedModules.Add(Blim.Check.CheckAccount);
                pickedModulesNames.Add("Blim");
            }
            if (modules.Contains("13"))
            {
                pickedModules.Add(BwwChecker.Check.CheckAccount);
                pickedModulesNames.Add("Buffalo Wild Wings");
            }
            if (modules.Contains("14"))
            {
                pickedModules.Add(Cbs.Check.CheckAccount);
                pickedModulesNames.Add("Cbs");
            }
            if (modules.Contains("15"))
            {
                pickedModules.Add(Coinbase.Check.CheckAccount);
                pickedModulesNames.Add("Coinbase");
            }
            if (modules.Contains("16"))
            {
                pickedModules.Add(coinsph.Check.CheckAccount);
                pickedModulesNames.Add("Coins.ph");
            }
            if (modules.Contains("17"))
            {
                pickedModules.Add(ColdStoneCreamery.Check.CheckAccount);
                pickedModulesNames.Add("Coldstone");
            }
            if (modules.Contains("18"))
            {
                pickedModules.Add(CrunchyRoll.Check.CheckAccount);
                pickedModulesNames.Add("CrunchyRoll");
            }
            if (modules.Contains("19"))
            {
                pickedModules.Add(DCuniverse.Check.CheckAccount);
                pickedModulesNames.Add("Dc Universe");
            }
            if (modules.Contains("20"))
            {
                pickedModules.Add(Disneyplus.Check.CheckAccount);
                pickedModulesNames.Add("Disney+");
            }
            if (modules.Contains("21"))
            {
                pickedModules.Add(DominosUS.Check.CheckAccount);
                pickedModulesNames.Add("Dominos (US)");
            }
            if (modules.Contains("22"))
            {
                pickedModules.Add(Doordash.Check.CheckAccount);
                pickedModulesNames.Add("Doordash");
            }
            if (modules.Contains("23"))
            {
                pickedModules.Add(Dualingo.Check.CheckAccount);
                pickedModulesNames.Add("Duolingo");
            }
            if (modules.Contains("24"))
            {
                pickedModules.Add(Easyjet.Check.CheckAccount);
                pickedModulesNames.Add("Easyjet.com");
            }
            if (modules.Contains("25"))
            {
                pickedModules.Add(EbayVm.Check.CheckAccount);
                pickedModulesNames.Add("Ebay (Valid Mail)");
            }
            if (modules.Contains("26"))
            {
                pickedModules.Add(Elasticemail.Check.CheckAccount);
                pickedModulesNames.Add("Elastic Email");
            }
            if (modules.Contains("27"))
            {
                pickedModules.Add(Facebook.Check.CheckAccount);
                pickedModulesNames.Add("Facebook");
            }
            if (modules.Contains("28"))
            {
                pickedModules.Add(Flimin.Check.CheckAccount);
                pickedModulesNames.Add("Flimin");
            }
            if (modules.Contains("29"))
            {
                pickedModules.Add(Fitbit.Check.CheckAccount);
                pickedModulesNames.Add("Fitbit");
            }
            if (modules.Contains("30"))
            {
                pickedModules.Add(Flightclub.Check.CheckAccount);
                pickedModulesNames.Add("Flight Club");
            }
            if (modules.Contains("31"))
            {
                pickedModules.Add(Foap.Check.CheckAccount);
                pickedModulesNames.Add("Foap");
            }
            if (modules.Contains("32"))
            {
                pickedModules.Add(Forever21.Check.CheckAccount);
                pickedModulesNames.Add("Forever 21");
            }
            if (modules.Contains("33"))
            {
                pickedModules.Add(Fox.Check.CheckAccount);
                pickedModulesNames.Add("Fox");
            }
            if (modules.Contains("34"))
            {
                pickedModules.Add(Funimation.Check.CheckAccount);
                pickedModulesNames.Add("Funimation");
            }
            if (modules.Contains("35"))
            {
                pickedModules.Add(Fwrd.Check.CheckAccount);
                pickedModulesNames.Add("Fwrd");
            }
            if (modules.Contains("36"))
            {
                pickedModules.Add(Gamefly.Check.CheckAccount);
                pickedModulesNames.Add("Gamefly");
            }
            if (modules.Contains("37"))
            {
                pickedModules.Add(GetUpside.Check.CheckAccount);
                pickedModulesNames.Add("GetUpSide");
            }
            if (modules.Contains("38"))
            {
                pickedModules.Add(Gfuel.Check.CheckAccount);
                pickedModulesNames.Add("Gfuel");
            }
            if (modules.Contains("39"))
            {
                pickedModules.Add(Godaddy.Check.CheckAccount);
                pickedModulesNames.Add("Godaddy");
            }
            if (modules.Contains("40"))
            {
                pickedModules.Add(GooseVpn.Check.CheckAccount);
                pickedModulesNames.Add("Goose vpn");
            }
            if (modules.Contains("41"))
            {
                pickedModules.Add(Gucci.Check.CheckAccount);
                pickedModulesNames.Add("Gucci");
            }
            if (modules.Contains("42"))
            {
                pickedModules.Add(Headspace.Check.CheckAccount);
                pickedModulesNames.Add("Headspace");
            }
            if (modules.Contains("43"))
            {
                pickedModules.Add(HideMyAss.Check.CheckAccount);
                pickedModulesNames.Add("Hide My Ass");
            }
            if (modules.Contains("44"))
            {
                pickedModules.Add(HolaVpn.Check.CheckAccount);
                pickedModulesNames.Add("Hola vpn");
            }
            if (modules.Contains("45"))
            {
                pickedModules.Add(Hotspotshield.Check.CheckAccount);
                pickedModulesNames.Add("Hotspot shield");
            }
            if (modules.Contains("46"))
            {
                pickedModules.Add(Hulu.Check.CheckAccount);
                pickedModulesNames.Add("Hulu");
            }
            if (modules.Contains("47"))
            {
                pickedModules.Add(IbVpn.Check.CheckAccount);
                pickedModulesNames.Add("Ib vpn");
            }
            if (modules.Contains("48"))
            {
                pickedModules.Add(Imvu.Check.CheckAccount);
                pickedModulesNames.Add("Imvu");
            }
            if (modules.Contains("49"))
            {
                pickedModules.Add(Instagram.Check.CheckAccount);
                pickedModulesNames.Add("Instagram");
            }
            if (modules.Contains("50"))
            {
                pickedModules.Add(IpVanish.Check.CheckAccount);
                pickedModulesNames.Add("IP vanish");
            }
            if (modules.Contains("51"))
            {
                pickedModules.Add(Kfc.Check.CheckAccount);
                pickedModulesNames.Add("Kfc (AUS)");
            }
            if (modules.Contains("52"))
            {
                pickedModules.Add(Later.Check.CheckAccount);
                pickedModulesNames.Add("Later.com");
            }
            if (modules.Contains("53"))
            {
                pickedModules.Add(Luminati.Check.CheckAccount);
                pickedModulesNames.Add("Luminati");
            }
            if (modules.Contains("54"))
            {
                pickedModules.Add(Mailaccess.Check.CheckAccount);
                pickedModulesNames.Add("Mail Access");
            }
            if (modules.Contains("55"))
            {
                pickedModules.Add(McAfee.Check.CheckAccount);
                pickedModulesNames.Add("Mcafee");
            }
            if (modules.Contains("56"))
            {
                pickedModules.Add(Md5Hash.Check.CheckAccount);
                pickedModulesNames.Add("Md5 Dehasher");
            }
            if (modules.Contains("57"))
            {
                pickedModules.Add(Microsoftandedumailaccess.Check.CheckAccount);
                pickedModulesNames.Add("Microsoft/Edu Mail Access");
            }
            if (modules.Contains("58"))
            {
                pickedModules.Add(Minecraft.Check.CheckAccount);
                pickedModulesNames.Add("Minecraft");
            }
            if (modules.Contains("59"))
            {
                pickedModules.Add(Mingle.Check.CheckAccount);
                pickedModulesNames.Add("Mingle.com");
            }
            if (modules.Contains("60"))
            {
                pickedModules.Add(MyCanal.Check.CheckAccount);
                pickedModulesNames.Add("Mycanal");
            }
            if (modules.Contains("61"))
            {
                pickedModules.Add(Napster.Check.CheckAccount);
                pickedModulesNames.Add("Napster");
            }
            if (modules.Contains("62"))
            {
                pickedModules.Add(Nba.Check.CheckAccount);
                pickedModulesNames.Add("Nba");
            }
            if (modules.Contains("63"))
            {
                pickedModules.Add(Netflix2.Check.CheckAccount);
                pickedModulesNames.Add("Netflix");
            }
            if (modules.Contains("64"))
            {
                pickedModules.Add(NordVpn.Check.CheckAccount);
                pickedModulesNames.Add("Nord vpn");
            }
            if (modules.Contains("65"))
            {
                pickedModules.Add(Origin.Check.CheckAccount);
                pickedModulesNames.Add("Origin");
            }
            if (modules.Contains("66"))
            {
                pickedModules.Add(OutbackSteakHouse.Check.CheckAccount);
                pickedModulesNames.Add("Outback steak house");
            }
            if (modules.Contains("67"))
            {
                pickedModules.Add(Pandora.Check.CheckAccount);
                pickedModulesNames.Add("Pandora");
            }
            if (modules.Contains("68"))
            {
                pickedModules.Add(Patreon.Check.CheckAccount);
                pickedModulesNames.Add("Patreon.com");
            }
            if (modules.Contains("69"))
            {
                pickedModules.Add(Phub.Check.CheckAccount);
                pickedModulesNames.Add("Pornhub");
            }
            if (modules.Contains("70"))
            {
                pickedModules.Add(Pinterest.Check.CheckAccount);
                pickedModulesNames.Add("Pinterest");
            }
            if (modules.Contains("71"))
            {
                pickedModules.Add(pizzahutukvm.Check.CheckAccount);
                pickedModulesNames.Add("Pizzahut uk (Valid Mail)");
            }
            if (modules.Contains("72"))
            {
                pickedModules.Add(Plex.Check.CheckAccount);
                pickedModulesNames.Add("Plex tv");
            }
            if (modules.Contains("73"))
            {
                pickedModules.Add(Postmatesfleet.Check.CheckAccount);
                pickedModulesNames.Add("Postmates fleet");
            }
            if (modules.Contains("74"))
            {
                pickedModules.Add(PsnValidMail.Check.CheckAccount);
                pickedModulesNames.Add("Psn Mail acces");
            }
            if (modules.Contains("75"))
            {
                pickedModules.Add(Robinhood.Check.CheckAccount);
                pickedModulesNames.Add("Robinhood");
            }
            if (modules.Contains("76"))
            {
                pickedModules.Add(Scribd.Check.CheckAccount);
                pickedModulesNames.Add("Scribd");
            }
            if (modules.Contains("77"))
            {
                pickedModules.Add(Shawacademy.Check.CheckAccount);
                pickedModulesNames.Add("Shawacademy");
            }
            if (modules.Contains("78"))
            {
                pickedModules.Add(Shemaroo.Check.CheckAccount);
                pickedModulesNames.Add("Shemaroo.me");
            }
            if (modules.Contains("79"))
            {
                pickedModules.Add(Shopify.Check.CheckAccount);
                pickedModulesNames.Add("Shopify");
            }
            if (modules.Contains("80"))
            {
                pickedModules.Add(Shuttershock.Check.CheckAccount);
                pickedModulesNames.Add("Shuttershock");
            }
            if (modules.Contains("81"))
            {
                pickedModules.Add(SliceLife.Check.CheckAccount);
                pickedModulesNames.Add("Slicelife");
            }
            if (modules.Contains("82"))
            {
                pickedModules.Add(Sling.Check.CheckAccount);
                pickedModulesNames.Add("Sling tv");
            }
            if (modules.Contains("83"))
            {
                pickedModules.Add(Smartproxy.Check.CheckAccount);
                pickedModulesNames.Add("Smartproxy");
            }
            if (modules.Contains("84"))
            {
                pickedModules.Add(SmoothieKing.Check.CheckAccount);
                pickedModulesNames.Add("Smoothie king");
            }
            if (modules.Contains("85"))
            {
                pickedModules.Add(Surfshark.Check.CheckAccount);
                pickedModulesNames.Add("Surfshark");
            }
            if (modules.Contains("86"))
            {
                pickedModules.Add(Symbolab.Check.CheckAccount);
                pickedModulesNames.Add("Symbolab");
            }
            if (modules.Contains("87"))
            {
                pickedModules.Add(TigerVpn.Check.CheckAccount);
                pickedModulesNames.Add("Tiger vpn");
            }
            if (modules.Contains("88"))
            {
                pickedModules.Add(TubiTV.Check.CheckAccount);
                pickedModulesNames.Add("Tubi tv");
            }
            if (modules.Contains("89"))
            {
                pickedModules.Add(Tunnelbear.Check.CheckAccount);
                pickedModulesNames.Add("Tunnel bear");
            }
            if (modules.Contains("90"))
            {
                pickedModules.Add(Twitch.Check.CheckAccount);
                pickedModulesNames.Add("Twitch");
            }
            if (modules.Contains("91"))
            {
                pickedModules.Add(Twitchlegacy.Check.CheckAccount);
                pickedModulesNames.Add("Twitch (Username Checker)");
            }
            if (modules.Contains("92"))
            {
                pickedModules.Add(Udacity.Check.CheckAccount);
                pickedModulesNames.Add("Udacity");
            }
            if (modules.Contains("93"))
            {
                pickedModules.Add(UfcTV.Check.CheckAccount);
                pickedModulesNames.Add("Ufc tv");
            }
            if (modules.Contains("94"))
            {
                pickedModules.Add(Ullu.Check.CheckAccount);
                pickedModulesNames.Add("Ullu");
            }
            if (modules.Contains("95"))
            {
                pickedModules.Add(Upcloud.Check.CheckAccount);
                pickedModulesNames.Add("Upcloud");
            }
            if (modules.Contains("96"))
            {
                pickedModules.Add(Uplay.Check.CheckAccount);
                pickedModulesNames.Add("Uplay");
            }
            if (modules.Contains("97"))
            {
                pickedModules.Add(Valorant.Check.CheckAccount);
                pickedModulesNames.Add("Zenmate");
            }
            if (modules.Contains("98"))
            {
                pickedModules.Add(Viaplay.Check.CheckAccount);
                pickedModulesNames.Add("Viaplay");
            }
            if (modules.Contains("99"))
            {
                pickedModules.Add(Voot.Check.CheckAccount);
                pickedModulesNames.Add("Voot");
            }
            if (modules.Contains("100"))
            {
                pickedModules.Add(Vortexgg.Check.CheckAccount);
                pickedModulesNames.Add("Vortex.gg");
            }
            if (modules.Contains("101"))
            {
                pickedModules.Add(VyperVpn.Check.CheckAccount);
                pickedModulesNames.Add("Vyper vpn");
            }
            if (modules.Contains("102"))
            {
                pickedModules.Add(Waves.Check.CheckAccount);
                pickedModulesNames.Add("Waves.com");
            }
            if (modules.Contains("103"))
            {
                pickedModules.Add(Wish.Check.CheckAccount);
                pickedModulesNames.Add("Wish");
            }
            if (modules.Contains("104"))
            {
                pickedModules.Add(Wordpress.Check.CheckAccount);
                pickedModulesNames.Add("Wordpress");
            }
            if (modules.Contains("105"))
            {
                pickedModules.Add(Wwe.Check.CheckAccount);
                pickedModulesNames.Add("Wwe");
            }
            if (modules.Contains("106"))
            {
                pickedModules.Add(Xcams.Check.CheckAccount);
                pickedModulesNames.Add("Xcams");
            }
            if (modules.Contains("107"))
            {
                pickedModules.Add(XVPN.Check.CheckAccount);
                pickedModulesNames.Add("Xvpn");
            }
            if (modules.Contains("108"))
            {
                pickedModules.Add(Yahoo.Check.CheckAccount);
                pickedModulesNames.Add("Yahoo");
            }
            if (modules.Contains("109"))
            {
                pickedModules.Add(Zee5.Check.CheckAccount);
                pickedModulesNames.Add("Zee5");
            }
            if (modules.Contains("110"))
            {
                pickedModules.Add(Aha.Check.CheckAccount);
                pickedModulesNames.Add("Aha.com");
            }
            if (modules.Contains("111"))
            {
                pickedModules.Add(Bitdefender.Check.CheckAccount);
                pickedModulesNames.Add("Bitdefender");
            }
            if (modules.Contains("112"))
            {
                pickedModules.Add(Epixnow.Check.CheckAccount);
                pickedModulesNames.Add("Epix now");
            }
            if (modules.Contains("113"))
            {
                pickedModules.Add(Kaspersky.Check.CheckAccount);
                pickedModulesNames.Add("Kaspersky");
            }
            if (modules.Contains("114"))
            {
                pickedModules.Add(Etsy.Check.CheckAccount);
                pickedModulesNames.Add("Etsy");
            }
            if (modules.Contains("115"))
            {
                pickedModules.Add(Clarovideo.Check.CheckAccount);
                pickedModulesNames.Add("ClaroVideo");
            }
            if (modules.Contains("116"))
            {
                pickedModules.Add(Venmo.Check.CheckAccount);
                pickedModulesNames.Add("Venmo");
            }
            if (modules.Contains("117"))
            {
                pickedModules.Add(Apple.Check.CheckAccount);
                pickedModulesNames.Add("Apple");
            }
            if (modules.Contains("118"))
            {
                pickedModules.Add(Onlyfans.Check.CheckAccount);
                pickedModulesNames.Add("Onlyfans");
            }
            if (modules.Contains("119"))
            {
                pickedModules.Add(Spotify.Check.CheckAccount);
                pickedModulesNames.Add("Spotify");
            }
            if (modules.Contains("120"))
            {
                pickedModules.Add(Headspace_UK.Check.CheckAccount);
                pickedModulesNames.Add("Headspace UK");
            }
            if (modules.Contains("121"))
            {
                pickedModules.Add(Sonyvegas.Check.CheckAccount);
                pickedModulesNames.Add("Sony Vegas");
            }
            start();
        }
        public static void BotShits()
        {
            DiscordBot prog = new DiscordBot();
            prog.MainAsync().GetAwaiter().GetResult();
        }
    
        public static void start()
        {
            for (int i = 1; i <= globalThreads; i++)
            {
                new Thread(() =>
                {
                    Random r = new Random();
                    for (; ; )
                    {
                        while (true)
                        {
                            if (comboIndex >= combos.Count())
                            {
                                return;
                            }
                            int localIndex = comboIndex;
                            Interlocked.Increment(ref comboIndex);

                            string[] comboArray = combos.ElementAt(localIndex).Split(':');
                            string proxy = proxies.ElementAt(r.Next(proxiesCount));

                            foreach (Func<string[], string, bool> checkFunction in pickedModules.Distinct())
                            {
                                checkFunction(comboArray, proxy);
                            }
                            checks++;
                        }
                    }
                }).Start();
            }
            if (Config.config.LogorCui == "1")
            {
                CUI();
            }
            else if (Config.config.LogorCui == "2")
            {
                Console.Clear();
               Write.ascii();
                Console.WriteLine();
                Console.WriteLine("------------------------------------------------------------------------------------------------------------------------" , Theme);
                LOG();
            }
        }



        static void CUI()
        {

            int lastChecks = checks;
            for (; ; )
            {
                cpm = checks - lastChecks;
                lastChecks = checks;
                Console.Clear();
               Write.ascii();
                Colorful.Console.Title = "ZeusAIO" + " | Modules: " + string.Join(", ", pickedModulesNames) + " | Hits - " + hits + " | Frees - " + frees + " | Bads - " + (checks - hits - frees) + " | Checked - " + checks + "/" + comboTotal + " | Errors - " + errors + " | Retries - " + realretries  + " | Cpm - " + cpm * 60;
                Console.WriteLine();
                Console.WriteLine("[+] Modules: " + string.Join(", ", pickedModulesNames), Color.White);
                Console.WriteLine();
                Console.Write("[+] Hits", Color.White);
                Console.Write(" : ", Color.NavajoWhite);
                Console.Write(hits, Color.Green);
                Console.WriteLine();
                Console.Write("[+] Frees", Color.White);
                Console.Write(" : ", Color.NavajoWhite);
                Console.Write(frees, Color.OrangeRed);
                Console.WriteLine();
                Console.Write("[+] Bads", Color.White);
                Console.Write(" : ", Color.NavajoWhite);
                Console.Write(checks - hits - frees, Color.Red);
                Console.WriteLine();
                Console.Write("[+] Checked", Color.White);
                Console.Write(" : ", Color.NavajoWhite);
                Console.Write(checks, Color.Orange);
                Console.Write("/", Color.NavajoWhite);
                Console.Write(comboTotal, Color.DarkOrange);
                Console.WriteLine();
                Console.Write("[+] Threads", Color.White);
                Console.Write(" : ", Color.NavajoWhite);
                Console.Write(globalThreads, Color.Chocolate);
                Console.Write("/", Color.NavajoWhite);
                Console.Write(globalThreads, Color.Chocolate);
                Console.WriteLine();
                Console.Write("[+] Errors", Color.White);
                Console.Write(" : ", Color.NavajoWhite);
                Console.Write(errors, Color.DimGray);
                Console.WriteLine();
                Console.Write("[+] Retries", Color.White);
                Console.Write(" : ", Color.NavajoWhite);
                Console.Write(realretries, Color.YellowGreen);
                Console.WriteLine();
                Console.Write("[+] Cpm", Color.White);
                Console.Write(" : ", Color.NavajoWhite);
                Console.Write(cpm * 60, Color.DeepSkyBlue);
                Console.WriteLine();
                Thread.Sleep(Config.config.RefreshRate);
                if (checks >= comboTotal)
                {
                    Colorful.Console.Title = "ZeusAIO" + " | Modules: " + string.Join(", ", pickedModulesNames) + " | Hits: " + hits + " | Finished Checking...";
                    Console.WriteLine("Finished Checking...", Theme);
                    Thread.Sleep(-1);
                }
            }

        }

        static void LOG()
        {

            int lastChecks = checks;
            for (; ; )
            {
                cpm = checks - lastChecks;
                lastChecks = checks;
                Colorful.Console.Title = "ZeusAIO" + " | Modules: " + string.Join(", ", pickedModulesNames) + " | Hits - " + hits + " | Frees - " + frees + " | Bads - " + (checks - hits - frees) + " | Checked - " + checks + "/" + comboTotal + " | Errors - " + errors + " | Retries - " + realretries + " | Cpm - " + cpm * 60;
                Thread.Sleep(1000);
                if (checks >= comboTotal)
                {
                    Colorful.Console.Title = "ZeusAIO" + " | Modules: " + string.Join(", ", pickedModulesNames) + " | Hits: " + hits + " | Finished Checking...";
                    Console.WriteLine("Finished Checking...", Theme);
                    Thread.Sleep(-1);
                }
            }
        }


        public static int globalThreads = -1;
        public static int globalRetries = -1;
        public static string proxyProtocol = "";
        public static int hits = 0;
        public static int frees = 0;
        public static int errors = 0;
        public static int realretries = 0;
        public static int cpm = 0;
        public static int checks = 0;
        public static IEnumerable<string> combos;
        public static int comboTotal = 0;
        public static IEnumerable<string> proxies;
        public static int proxiesCount = 0;
        public static int comboIndex = 0;
        public static int Modules = 0;
        public static string gay = "";
     
    }
}
