﻿using ScheduleBuilder.DAL;
using ScheduleManager.Model;
using System.Data;
using System.Web.Mvc;
using System.Web.Security;

namespace ScheduleBuilder.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult PeopleDirectory()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Person person)
        {
            ViewBag.Error = "Invalid Username or Password";
            if (person.Username == null || person.Username == " " || person.Password == null || person.Password == " ")
            {
                return View();
            }
            DataTable dataTable = LoginDAL.GetLogin(person.Username, person.Password);
            if (dataTable.Rows.Count > 0)
            {
                if ((string)dataTable.Rows[0]["roleTitle"] == "Employee")
                {
                    return View("Index");
                }
                else if ((string)dataTable.Rows[0]["roleTitle"] == "Manager")
                {
                    return View("Index");
                }
                else if ((string)dataTable.Rows[0]["roleTitle"] == "Administrator")
                {
                    return View("Index");
                }
            }
            return View();
        }
    }
}