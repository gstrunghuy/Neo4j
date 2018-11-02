using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neo4j.Driver.V1;
using Neo4jSocial.DAO;
using Neo4jSocial.Models;
using Neo4jSocial.Common;
using Neo4jSocial.DTO;

namespace Neo4jSocial.Controllers
{
    public class ProfileController : Controller
    {
        #region hiển thị trang Index
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
                ProfileDAO dao = new ProfileDAO();
                Profile node = dao.GetProfile(ses);

                ViewData["node"] = node;
                if (node.User == null)
                {
                    ViewBag.mode = "Add";
                }
                else
                {
                    ViewBag.mode = "Edit";
                }

                List<Status> lstStt = new List<Status>();
                lstStt = dao.GetAllPost(ses);
                ViewData["lstStt"] = lstStt;

                //COUNT POST
                ViewData["countStatus"] = dao.CountStatus(ses);
                ViewData["countShare"] = dao.CountShare(ses);
                ViewData["countCmt"] = dao.CountCmt(ses);

                //Get list friend
                List<Profile> lstF = new List<Profile>();
                lstF = dao.GetFriend(ses);
                ViewData["lstF"] = lstF;

                //Suggestion Friend
                List<SuggestionFriend> lstSF = new List<SuggestionFriend>();
                lstSF = dao.SuggestionFriend(ses);
                ViewData["lstSF"] = lstSF;

                return View();
            }
        }
        #endregion

        #region Cập nhật thông tin
        public ActionResult Update(string FirstName, string LastName, string City, string Birthday, string Interest, string Email, string Gender, string Job, string Company, string School, string mode, HttpPostedFileBase avatar)
        {
            bool reIndex = true;
            ProfileDAO dao = new ProfileDAO();
            Profile p = new Profile();
            if (Birthday != "")
            {
                DateTime dt = DateTime.Parse(Birthday);
                LocalDate bd = new LocalDate(dt);
                p.Birthday = bd;
            }
            p.City = City == "" ? "" : City;
            p.LastName = LastName == "" ? "" : LastName;
            p.FirstName = FirstName == "" ? "" : FirstName;
            p.Interest = Interest == "" ? "" : Interest;
            p.Email = Email == "" ? "" : Email;
            p.Gender = Gender == "" ? "" : Gender;
            p.Job = Job == "" ? "" : Job;
            p.Company = Company == "" ? "" : Company;
            p.School = School == "" ? "" : School;
            if (avatar != null && avatar.ContentLength > 0)
            {
                string user = Session["user"].ToString().ToUpper();
                // extract only the fielname
                var fileName = Path.GetFileName(avatar.FileName);
                p.Avatar = fileName;
                // store the file inside ~/Content/uploads folder
                bool hasPath = Directory.Exists(Server.MapPath("~/Content/Uploads/" + user));
                if (!hasPath)
                {
                    Directory.CreateDirectory(Server.MapPath("~/Content/Uploads/" + user));
                }
                var path = Path.Combine(Server.MapPath("~/Content/Uploads/" + user), fileName);
                avatar.SaveAs(path);
            }
            p.User = "";
            if (Session["user"] != null)
            {
                p.User = Session["user"].ToString();
                if (mode == "Edit")
                {
                    reIndex = dao.Edit(p);
                }
            }
            if (mode == "Add")
            {
                reIndex = dao.Add(p);
            }
            if (reIndex)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        #endregion

        #region Dòng thời gian
        public ActionResult Post(string Content, string Avatar, string DisplayName)
        {
            string user = Session["user"].ToString();
            ProfileDAO dao = new ProfileDAO();
            dao.ReplaceInput(ref Content);
            bool res = dao.Post(user, Content, Avatar, DisplayName);
            if (res)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        [HttpGet]
        public ActionResult Share(int id)
        {
            string user = Session["user"].ToString();
            ProfileDAO dao = new ProfileDAO();
            dao.Share(id, user);
            return RedirectToAction("Index");
        }
        #endregion
        #region Delete
        [HttpGet]
        public ActionResult DeleteShare(int id)
        {
            ProfileDAO dao = new ProfileDAO();
            dao.DeleteShare(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult DeleteSTT(int id)
        {
            ProfileDAO dao = new ProfileDAO();
            dao.DeleteSTT(id);
            return RedirectToAction("Index");
        }
        #endregion
    }
}