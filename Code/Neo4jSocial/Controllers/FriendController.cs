using Neo4jSocial.Common;
using Neo4jSocial.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Neo4jSocial.Controllers
{
    public class FriendController : Controller
    {
        // GET: Friend
        public ActionResult Index()
        {
            Login common = new Login();
            string ses = "";
            if (Session["user"] != null)
            {
                ses = Session["user"].ToString();
            }
            bool IsLogin = common.AccessPass(Request, ref ses);

            if (!IsLogin)
            {
                return RedirectToAction("../Signin/Index");
            }
            else
            {
                Session["user"] = ses;
                return View();
            }
        }
        [HttpPost]
        public ActionResult Search(string Info, string Key)
        {
            FriendDao dao = new FriendDao();
            ViewData["lst"] = dao.FindFriend(Key, Info);
            return View();
        }
        public void AddFriend(string User)
        {
            string me = Session["user"].ToString();
            FriendDao dao = new FriendDao();
            dao.AddFriend(User, me);
        }
        public ActionResult Accept(string User)
        {
            string me = Session["user"].ToString();
            FriendDao dao = new FriendDao();
            dao.AddFriend(User,me);
            return RedirectToAction("../Profile/");
        }
    }
}