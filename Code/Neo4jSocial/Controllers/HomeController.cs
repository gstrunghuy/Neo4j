using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neo4jSocial.Common;

namespace Neo4jSocial.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            Login login = new Login();
            string ses = "";
            if (Session["user"]!=null)
            {
                ses = Session["user"].ToString();
            }
            if (login.AccessPass(Request,ref ses))
            {
                Session["user"] = ses;
                return View();
            }
            else
            {
                return RedirectToAction("../Signin/Index");
            }
        }
    }
}