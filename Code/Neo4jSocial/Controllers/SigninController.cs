using Neo4jSocial.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neo4jSocial.Common;

namespace Neo4jSocial.Controllers
{
    public class SigninController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
      
        public JsonResult  Signin(string user,string pass, string savePass)
        {
            SigninDAO dao = new SigninDAO();
            dao.ReplaceInput(ref user);
            dao.ReplaceInput(ref pass);
            user = user.Replace(" ", "");
            pass = pass.Replace(" ", "");
            bool cre = dao.LoginAccount(user, pass);
            if (savePass=="true")
            {
                Login login = new Login();
                login.CookiePass(user, pass, Response);
            }
            if (cre)
            {
                Session["user"] = user;
                return Json(1);
            }
            else
            {
                return Json(-1);
            }
        }
        public ActionResult Logout()
        {
            Session.RemoveAll();
            //string[] myCookies = Request.Cookies.AllKeys;
            //foreach (string c in myCookies)
            //{
            //    Response.Cookies[c].Expires = DateTime.Now.AddDays(-1);
            //}
            Response.Cookies["acc"].Expires = DateTime.Now.AddDays(-1);
            return RedirectToAction("Index");
        }

    }
}