using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using xNet;

namespace AntiCaptchas
{
    class TwoCaptcha
    {
        public static string GetCaptchaKey(string websiteUrl, string websiteKey, string antiCaptchaKey)
        {
        a:
            HttpRequest request = new HttpRequest();

            string captchaKey;

            HttpResponse response;

            string responseData = "";

            request.AddHeader("Content-Type", "application/json");
            try
            {
                request.ConnectTimeout = 20000;
                request.KeepAliveTimeout = 20000;
                request.ReadWriteTimeout = 20000;
                response = request.Start(xNet.HttpMethod.GET, new Uri("https://2captcha.com/in.php?key="+ antiCaptchaKey + "&method=userrecaptcha&googlekey="+ websiteKey + "&pageurl=" + websiteUrl), null);
                responseData = response.ToString();
            }
            catch
            {
                goto a;
            }

            if (responseData.Contains("ERROR_WRONG_USER_KEY") || responseData.Contains("ERROR_ZERO_BALANCE"))
            {
                return "WrongKey/Empty balance";
            }
            else if (!responseData.Contains("OK|"))
            {
                goto a;
            }

            string taskId = responseData.Split('|')[1];

            response = null;

            Thread.Sleep(20000);
            b:
            Thread.Sleep(5000);
            try
            {
                request.AddHeader("Content-Type", "application/json");
                response = request.Start(xNet.HttpMethod.GET, new Uri("https://2captcha.com/res.php?key=" + antiCaptchaKey + "&action=get&id="+taskId), null);
                responseData = response.ToString();
            }
            catch
            {
                goto b;
            }


            if (responseData.Contains("OK|"))
            {
                captchaKey = responseData.Split('|')[1];
                //Console.WriteLine(captchaKey);
            }
            else if (responseData.Contains("CAPCHA_NOT_READY"))
            {
                goto b;
            }
            else
            {
                goto a;
            }
            return captchaKey;
        }

        public static string GetReCaptchaV3Key(string websiteUrl, string websiteKey, string antiCaptchaKey, string score, string action)
        {
        a:
            HttpRequest request = new HttpRequest();

            string captchaKey;

            HttpResponse response;

            string responseData = "";

            request.AddHeader("Content-Type", "application/json");
            try
            {
                request.ConnectTimeout = 20000;
                request.KeepAliveTimeout = 20000;
                request.ReadWriteTimeout = 20000;
                response = request.Start(xNet.HttpMethod.GET, new Uri("https://2captcha.com/in.php?key=" + antiCaptchaKey + "&method=userrecaptcha&googlekey=" + websiteKey + "&pageurl=" + websiteUrl + "&version=v3&action="+action+"&min_score=" + score), null);
                responseData = response.ToString();
            }
            catch
            {
                goto a;
            }

            if (responseData.Contains("ERROR_WRONG_USER_KEY") || responseData.Contains("ERROR_ZERO_BALANCE"))
            {
                return "WrongKey/Empty balance";
            }
            else if (!responseData.Contains("OK|"))
            {
                goto a;
            }

            string taskId = responseData.Split('|')[1];

            response = null;

        b:
            Thread.Sleep(5000);
            try
            {
                request.AddHeader("Content-Type", "application/json");
                response = request.Start(xNet.HttpMethod.GET, new Uri("https://2captcha.com/res.php?key=" + antiCaptchaKey + "&action=get&taskinfo=1&id=" + taskId), null);
                responseData = response.ToString();
                Console.WriteLine(responseData);
            }
            catch
            {
                goto b;
            }


            if (responseData.Contains("OK|"))
            {
                captchaKey = responseData.Split('|')[1];
                //Console.WriteLine(captchaKey);
            }
            else if (responseData.Contains("CAPCHA_NOT_READY"))
            {
                goto b;
            }
            else if (responseData.Contains("ERROR_CAPTCHA_UNSOLVABLE"))
            {
                goto a;
            }
            else
            {
                goto a;
            }
            return captchaKey;
        }
    }
}
