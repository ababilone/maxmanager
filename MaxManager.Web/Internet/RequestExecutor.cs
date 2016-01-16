using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace MaxControl.Internet
{
    public class RequestExecutor
    {
        private readonly WebClientExtended _webClientExtended;

        public RequestExecutor()
        {
            _webClientExtended = new WebClientExtended();
        }

        public String Post(String url, Dictionary<String, String> parameters)
        {
            var formatedParameters = parameters.Aggregate("", (current, parameter) => current + String.Format("{0}={1}&", parameter.Key, HttpUtility.UrlEncode(parameter.Value)));

            _webClientExtended.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            return _webClientExtended.UploadString(url, formatedParameters);
        }

        public String Get(String url)
        {
            return _webClientExtended.DownloadString(url);
        }

        public String GetSessionId(String url)
        {
            var cookieCollection = _webClientExtended.CookieContainer.GetCookies(new Uri(url));
            if (cookieCollection["JSESSIONID"] != null)
                return cookieCollection["JSESSIONID"].Value;

            return "";
        }
    }
}