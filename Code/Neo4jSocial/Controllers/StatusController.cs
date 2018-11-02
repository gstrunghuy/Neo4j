using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neo4jSocial.Common;
using Neo4jSocial.DAO;

namespace Neo4jSocial.Controllers
{
    public class StatusController : Controller
    {
        // GET: Comment
        [HttpGet]
        public ActionResult Index(int id)
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
                StatusDAO dao = new StatusDAO();
                ViewBag.Post = dao.GetPost(id);
                ViewBag.LstCmt = dao.GetAllComment(id.ToString());
                return View();
            }                
        }
        
        public ActionResult Comment(string Content, string Avatar, string DisplayName, int IdPost)
        {
            string user = Session["user"].ToString();
            StatusDAO dao = new StatusDAO();
            dao.ReplaceInput(ref Content);
            bool res = dao.Comment(user, Content, Avatar, DisplayName, IdPost);
            if (res)
            {
                return RedirectToAction("Index/"+IdPost);
            }
            else
            {
                return View("~/Views/Shared/Error.cshtml");
            }
        }
    }
}