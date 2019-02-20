using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EventManagementSystem.Service;
using Newtonsoft.Json;

namespace EventManagementSystem.Web.Helpers
{
    public static class GoogleReCaptchaHelper 
    {
        private static ParameterService ParameterService = new ParameterService();
        public static bool IsValidCaptcha()
        {
            var response = HttpContext.Current.Request["g-recaptcha-response"];
            var client = new WebClient();
            var secretKey = ParameterService.GetValueByKey("GoogleReCaptchaSecretKey");

            var reply = client.DownloadString($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={response}");
            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

            return captchaResponse.Success;
        }

        public static MvcHtmlString GoogleReCaptcha(this HtmlHelper helper)
        {
            var siteKey = ParameterService.GetValueByKey("GoogleReCaptchaSiteKey");
            return new MvcHtmlString($"<div class='g-recaptcha' data-sitekey='{siteKey}'></div><script src='https://www.google.com/recaptcha/api.js' async defer></script>");
        }

        public class CaptchaResponse
        {
            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("error-codes")]
            public List<string> ErrorCodes { get; set; }
        }
    }
}
