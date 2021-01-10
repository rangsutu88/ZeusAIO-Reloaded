using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using xNet;

namespace AntiCaptchas
{
    class AntiCaptcha
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
                response = request.Start(HttpMethod.POST, new Uri("http://api.anti-captcha.com/createTask"), new BytesContent(Encoding.Default.GetBytes("{\"clientKey\":\"" + antiCaptchaKey + "\",\"task\":{\"type\":\"NoCaptchaTaskProxyless\",\"websiteURL\":\"" + websiteUrl + "\",\"websiteKey\":\"" + websiteKey + "\"}}")));
                responseData = response.ToString();
            }
            catch
            {
                //Console.WriteLine("e1");
                goto a;
            }

            if (responseData.Contains("ERROR_KEY_DOES_NOT_EXIST") || responseData.Contains("ERROR_ZERO_BALANCE"))
            {
                return "WrongKey/Empty balance";
            }
            else if (!responseData.Contains("\"taskId\":"))
            {
                goto a;
            }

            string taskId = responseData.Split('"')[4].Replace(":", "").Replace("}", "").Replace(" ", "");
            if(taskId == "")
            {
                taskId = responseData.Split('"')[12].Replace(":", "").Replace("}", "").Replace(" ", "");//{"errorId":0,"errorCode":"","errorDescription":"","taskId":77}
            }
            response = null;

            Thread.Sleep(5000);
        b:
            Thread.Sleep(1000);
            request.AddHeader("Content-Type", "application/json");
            try
            {
                response = request.Start(HttpMethod.POST, new Uri("https://api.anti-captcha.com/getTaskResult"), new BytesContent(Encoding.Default.GetBytes("{\"clientKey\":\"" + antiCaptchaKey + "\",\"taskId\":" + taskId + "}")));
                responseData = response.ToString();
            }
            catch
            {
                //Console.WriteLine("e2");
                goto b;
            }


            if (responseData.Contains("gRecaptchaResponse"))
            {
                captchaKey = responseData.Split('"')[11];
                //Console.WriteLine(captchaKey);
            }
            else if (responseData.Contains("processing"))
            {
                goto b;
            }
            else
            {
                goto a;
            }
            return captchaKey;
        }
        /*
        public static string GetReCaptchaV3Key(string websiteUrl, string websiteKey, string antiCaptchaKey, string score)
        {
        a:
            HttpRequest request = new HttpRequest();

            string captchaKey;

            HttpResponse response;

            string responseData = "";

            request.AddHeader("Content-Type", "application/json");
            try
            {
                response = request.Start(HttpMethod.POST, new Uri("http://api.anti-captcha.com/createTask"), new BytesContent(Encoding.Default.GetBytes("{\"clientKey\":\"" + antiCaptchaKey + "\",\"task\":{\"type\":\"RecaptchaV3TaskProxyless\",\"websiteURL\":\"" + websiteUrl + "\",\"websiteKey\":\"" + websiteKey + "\"}}")));
                responseData = response.ToString();
            }
            catch
            {
                //Console.WriteLine("e1");
                goto a;
            }

            if (responseData.Contains("ERROR_KEY_DOES_NOT_EXIST") || responseData.Contains("ERROR_ZERO_BALANCE"))
            {
                return "WrongKey/Empty balance";
            }
            else if (!responseData.Contains("\"taskId\":"))
            {
                goto a;
            }

            string taskId = responseData.Split('"')[4].Replace(":", "").Replace("}", "").Replace(" ", "");
            if (taskId == "")
            {
                taskId = responseData.Split('"')[12].Replace(":", "").Replace("}", "").Replace(" ", "");//{"errorId":0,"errorCode":"","errorDescription":"","taskId":77}
            }
            response = null;

            Thread.Sleep(5000);
        b:
            Thread.Sleep(1000);
            request.AddHeader("Content-Type", "application/json");
            try
            {
                response = request.Start(HttpMethod.POST, new Uri("https://api.anti-captcha.com/getTaskResult"), new BytesContent(Encoding.Default.GetBytes("{\"clientKey\":\"" + antiCaptchaKey + "\",\"taskId\":" + taskId + "}")));
                responseData = response.ToString();
            }
            catch
            {
                //Console.WriteLine("e2");
                goto b;
            }


            if (responseData.Contains("gRecaptchaResponse"))
            {
                captchaKey = responseData.Split('"')[11];
                //Console.WriteLine(captchaKey);
            }
            else if (responseData.Contains("processing"))
            {
                goto b;
            }
            else
            {
                goto a;
            }
            return captchaKey;
        }*/
    }
}
