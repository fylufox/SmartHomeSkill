using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;

namespace SmartHomeSkill
{
    class PowerApi
    {
        private string url;
        private string user;
        private string passwd;
        public PowerApi(string url,string user,string passwd)
        {
            this.url = url;
            this.user = user;
            this.passwd = passwd;
        }

        public bool PowerOn()
        {
            return RequestAPI(url + "/api/home/wol.php");
        }

        public bool PowerOff()
        {
            return RequestAPI(url + "/api/home/poweroff.php");
        }

        private bool RequestAPI(string furl)
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(furl)
            };
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", user, passwd))));
            using (var httpclient = new HttpClient())
            {
                var response = httpclient.SendAsync(request);
                var status = response.Result.StatusCode;
                if (status == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
