using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net;

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
            WebRequest w_req = WebRequest.Create(furl);
            w_req.Credentials = new NetworkCredential(user, passwd);
            var r = (HttpWebResponse) w_req.GetResponse();

            if(r.StatusCode == HttpStatusCode.OK)
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
