using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neo4jSocial.Common
{
    public class Login
    {
        public void CookiePass(string user, string pass, HttpResponseBase response)
        {
            HttpCookie acc = new HttpCookie("acc");
            acc.Values["user"] = user;
            acc.Values["pass"] = pass;
            acc.Expires = DateTime.Now.AddDays(1);
            response.Cookies.Add(acc);
        }
        public bool AccessPass(HttpRequestBase request,ref string ses)
        {
            if (ses != "")
            {
                return true;
            }
            try
            {
                string acc = request.Cookies["acc"].Values["user"];
                if (acc != null)
                {
                    ses = acc ;
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}