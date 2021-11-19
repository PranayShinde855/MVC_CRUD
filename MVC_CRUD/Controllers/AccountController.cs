using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_CRUD.Models;
using MVC_CRUD.Filters;
using MVC_CRUD.Services;

namespace MVC_CRUD.Controllers
{
    [CustomAuthenticationFilter]
    [CustomAuthorizationFilter("Admin")]
    public class AccountController : Controller
    {
        private DBModel db = new DBModel();

        // GET: Account
        public ActionResult Index()
        {
            return View(db.users.ToList());
        }

        // GET: Account/SingUp
        public ActionResult SingUp()
        {
            return View();
        }

        // POST: Account/SingUp
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SingUp([Bind(Include = "Id,UserName,Password,RoleId")] User user)
        {
            if (ModelState.IsValid)
            {
                db.users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

 
       
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


            [HttpPost]
        public ActionResult Login(User userModel)
        {
            using(DBModel db = new DBModel())
            {
                User user = db.users.Where(u => u.UserName == userModel.UserName && u.Password == userModel.Password).FirstOrDefault();
                if(user != null)
                {
                    Session["UserName"] = user.UserName;
                    Session["UserId"] = user.Password;
                    return RedirectToAction("Index", "Products");
                }
                return RedirectToAction("Index", "Account");
            }
            
        }

        public ActionResult LogOut(User userModel)
        {
            Session["UserName"] = string.Empty;
            Session["Password"] = string.Empty;
            return RedirectToAction("Login", "Account");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
