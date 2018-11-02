using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neo4jSocial.DAO;

namespace Neo4jSocial.Controllers
{
    public class SignupController : Controller
    {
        // GET: Signup
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Signup(string user, string password, string repeatpass)
        {
            if (password!=repeatpass)
            {
                return Json(-1);
            }
            SignupDAO dao = new SignupDAO();
            dao.ReplaceInput(ref user);
            dao.ReplaceInput(ref password);
            user = user.Replace(" ", "");
            password = password.Replace(" ", "");
            bool cre = dao.CreateAccount(user, password);
            if (cre)
            {
                return Json(1);
            }
            else
            {

                return Json(0);
            }

        }

    }
}