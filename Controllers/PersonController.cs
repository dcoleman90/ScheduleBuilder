﻿using ScheduleBuilder.Model;
using ScheduleBuilder.DAL;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System;

namespace ScheduleBuilder.Controllers
{
    /// <summary>
    /// This class insures proper access to the DAL 
    /// and controls all manipulation of the person object(s)
    /// </summary>
    public class PersonController : Controller
    {
        PersonDAL personDAL = new PersonDAL();
        RoleDAL roleDAL = new RoleDAL();

        /// <summary>
        /// Adds a person to the database
        /// </summary>
        /// <param name="addedPerson"></param>
        public ActionResult AddPerson()
        {
            ViewBag.Message = "Add Employee";
            return View();          
        }



        #region Return specified Persons
        /// <summary>
        /// returns a list of all persons - no where clause specified
        /// </summary>
        /// <returns></returns>
        //public static List<Person> GetAllPeoples()
        //{

        //    string whereClause = "";
        //    return PersonDAL.GetDesiredPersons(whereClause);

        //}

        public ActionResult GetAllPeoples()
        {
            string whereClause = "";
            return View(this.personDAL.GetDesiredPersons(whereClause));
        }

        public ActionResult Edit(int id)
        {
            string whereClause = "";
            Person person = this.personDAL.GetDesiredPersons(whereClause).Where(p => p.Id == id).FirstOrDefault();
            return View(person);
        }

        [HttpPost]
        public ActionResult Edit(Person person)
        {
            return RedirectToAction("GetAllPeoples");
        }

        public ActionResult Details(int id)
        {
            string whereClause = "";
            Person person = this.personDAL.GetDesiredPersons(whereClause).Where(p => p.Id == id).FirstOrDefault();
            return View(person);
        }

        public ActionResult Delete()
        {
            return View();
        }

        /// <summary>
        /// Returns list of persons based upon inputed statusid
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public List<Person> GetAllPeopleByStatusId(int statusId)
        {
            string whereClause = "WHERE statusId = " + statusId.ToString();
            return this.personDAL.GetDesiredPersons(whereClause);
        }

        /// <summary>
        /// Return a json list of all active employees that can be scheduled to work.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAllActivePeople()
        {
            try
            {
                string whereClause = "WHERE statusId = 1 OR statusId = 5";
                return Json(this.personDAL.GetDesiredPersons(whereClause));
            }
            catch (Exception e)
            {
                return null;
            }
            

        }

        /// <summary>
        /// Returns list of persons based upon inputed RoleId
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<Person> GetAllPeopleByRoleId(int roleId)
        {
            string whereClause = "WHERE roleId = " + roleId.ToString();
            return this.personDAL.GetDesiredPersons(whereClause);
        }

        public ActionResult GetAllPeopleById()
        {
            return View(); 
        }

        public ActionResult GetAllPeopleById(Person model)
        {
            string whereClause = "WHERE Id = " + model.Id.ToString();
            List<Person> selectedEmployee = this.personDAL.GetDesiredPersons(whereClause);
            return View(selectedEmployee[0]);


        }

        /// <summary>
        /// Returns list of persons based upon inputed first name
        /// </summary>
        /// <param name="FirstName"></param>
        /// <returns></returns>
        public List<Person> GetAllPeopleByFirstName(string FirstName)
        {
            string whereClause = "WHERE first_name = " + FirstName;
            return this.personDAL.GetDesiredPersons(whereClause);
        }

        /// <summary>
        /// Returns list of persons based upon inputed last name
        /// </summary>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public List<Person> GetAllPeopleByLastName(string lastName)
        {
            string whereClause = "WHERE last_name = " + lastName;
            return this.personDAL.GetDesiredPersons(whereClause);
        }

        /// <summary>
        /// Returns list of persons based upon inputed first and last name
        /// </summary>
        /// <param name="FirstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public List<Person> GetAllPeopleByFirstAndLastName(string FirstName, string lastName)
        {
            string whereClause = "WHERE last_name = " + lastName + " And first_name = " + FirstName;
            return this.personDAL.GetDesiredPersons(whereClause);
        }
        #endregion

        #region Return roles
        /// <summary>
        /// Gets all the role values
        /// </summary>
        /// <returns></returns>
        public List<Role> GetRoles()
        {
            return roleDAL.GetRoles();
        }

        /// <summary>
        /// Gets the role by the roleID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetRoleByID(int id)
        {
            return roleDAL.GetRoleByID(id);
        }

        public void SetRole(int id)
        {

            ViewBag.userRoleTitle = this.GetRoleByID(id);

        }
        #endregion
    }
}
