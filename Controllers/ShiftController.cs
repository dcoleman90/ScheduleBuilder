﻿using Newtonsoft.Json;
using ScheduleBuilder.DAL;
using ScheduleBuilder.Model;
using ScheduleBuilder.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ScheduleBuilder.Controllers
{
    public class ShiftController : Controller
    {
        ShiftDAL shiftDAL = new ShiftDAL();
        PositionDAL positionDAL = new PositionDAL();
        PersonDAL personDAL = new PersonDAL();
        RoleDAL roleDAL = new RoleDAL();
        StatusDAL statusDAL = new StatusDAL();

        /// <summary>
        /// Load the Shifts tab view and content
        /// </summary>
        /// <returns>The shift tab view and content</returns>
        public ActionResult Shifts()
        {
            return View();
        }

        /// <summary>
        /// Gets all shifts from the database
        /// Called from app.js
        /// </summary>
        public ActionResult ViewAllShifts()
        {
            try
            {
                return Json(shiftDAL.GetAllShifts());
            }
            catch (Exception e)
            {
                this.Messagebox(e.ToString());
                return null;
            }
        }

        /// <summary>
        /// Adds a new shift to the database
        /// Called from app.js
        /// </summary>
        /// <param name="personID">The person's id as an integer</param>
        /// <param name="positionID">The position id as an integer</param>
        /// <param name="startdt">The shift start date time</param>
        /// <param name="enddt">The shift end date time</param>
        /// <param name="startlunchdt">The shift start lunch date time</param>
        /// <param name="endlunchdt">The shift lunch end date time</param>
        /// <param name="taskList">The task list assigned to the shift</param>
        /// <param name="notes">Shift notes</param>
        /// <returns>True if insert was successful, false otherwise</returns>
        public ActionResult AddShift(string personID, string positionID, string startdt, string enddt, string startlunchdt, string endlunchdt, string taskList, string notes)
        {
            JavaScriptSerializer thing = new JavaScriptSerializer();
            Dictionary<int, bool> otherThing = taskList == null ? new Dictionary<int, bool>() : JsonConvert.DeserializeObject<Dictionary<int, bool>>(taskList);
            Shift shift = new Shift();
            shift.personID = int.Parse(personID);
            shift.positionID = int.Parse(positionID);
            shift.scheduledStartTime = ConvertDateToC(long.Parse(startdt));
            shift.scheduledEndTime = ConvertDateToC(long.Parse(enddt));

            if (!string.IsNullOrEmpty(startlunchdt))
            {
                shift.scheduledLunchBreakStart = ConvertDateToC(long.Parse(startlunchdt));
            }
            else
            {
                shift.scheduledLunchBreakStart = null;
            }
            if (!string.IsNullOrEmpty(endlunchdt))
            {
                shift.scheduledLunchBreakEnd = ConvertDateToC(long.Parse(endlunchdt));
            }
            else
            {
                shift.scheduledLunchBreakEnd = null;
            }
            shift.Notes = notes;

            return Json(shiftDAL.AddShift(shift, otherThing));

        }

        /// <summary>
        /// Updates an existing shift
        /// Called from app.js file
        /// </summary>
        /// <param name="personID">The person's id as an integer</param>
        /// <param name="positionID">The position id</param>
        /// <param name="startdt">The shift start date time</param>
        /// <param name="enddt">The shift end date time</param>
        /// <param name="startlunchdt">The shift lunch start date time</param>
        /// <param name="endlunchdt">The shift lunch end date time</param>
        /// <param name="shiftID">The shift's id</param>
        /// <param name="scheduleshiftID">The shift hour's id</param>
        /// <param name="isDelete">Boolean to delete or not</param>
        /// <param name="taskList">The associated task list</param>
        /// <param name="notes">The shift notes</param>
        /// <returns>True if update is successful, false otherwise</returns>
        public ActionResult UpdateShift(string personID, string positionID, string startdt, string enddt, string startlunchdt, string endlunchdt, string shiftID, string scheduleshiftID, string isDelete, string taskList, string notes)
        {
            JavaScriptSerializer thing = new JavaScriptSerializer();
            Dictionary<int, bool> otherThing = taskList == null ? new Dictionary<int, bool>() : JsonConvert.DeserializeObject<Dictionary<int, bool>>(taskList);

            Shift shift = new Shift();
            shift.shiftID = int.Parse(shiftID);
            shift.scheduleShiftID = int.Parse(scheduleshiftID);
            shift.personID = int.Parse(personID);
            shift.positionID = int.Parse(positionID);
            shift.scheduledStartTime = ConvertDateToC(long.Parse(startdt));
            shift.scheduledEndTime = ConvertDateToC(long.Parse(enddt));
            if (!string.IsNullOrEmpty(startlunchdt))
            {
                shift.scheduledLunchBreakStart = ConvertDateToC(long.Parse(startlunchdt));
            }
            else
            {
                shift.scheduledLunchBreakStart = null;
            }
            if (!string.IsNullOrEmpty(endlunchdt))
            {
                shift.scheduledLunchBreakEnd = ConvertDateToC(long.Parse(endlunchdt));
            }
            else
            {
                shift.scheduledLunchBreakEnd = null;
            }
            shift.Notes = notes;

            //Delete or update shift accordingly
            if (string.Equals(isDelete, "delete"))
            {
                this.ContactPersonShiftChange("delete", shift);
                return Json(shiftDAL.DeleteShift(shift));
            }
            else
            {
                this.ContactPersonShiftChange("update", shift);
                return Json(shiftDAL.UpdateShift(shift, otherThing));
            }
        }

        /// <summary>
        /// Checks if user is scheduled - prevents double scheduling
        /// Called from app.js file
        /// </summary>
        /// <param name="personID">The person's id as an integer</param>
        /// <param name="startdt">The start of shift</param>
        /// <param name="enddt">The end of shift</param>
        /// <returns>True if scheduled, false otherwise</returns>
        public ActionResult CheckIfScheduled(int personID, string startdt, string enddt, string whereClause)
        {
            DateTime scheduledStartTime = ConvertDateToC(long.Parse(startdt));
            DateTime scheduledEndTime = ConvertDateToC(long.Parse(enddt));
            return Json(this.shiftDAL.CheckIfPersonIsScheduled(personID, scheduledStartTime, scheduledEndTime, whereClause));
        }

        /// <summary>
        /// Checks if user is scheduled - prevents double scheduling
        /// Called from app.js file
        /// </summary>
        /// <param name="personID">The person's id as an integer</param>
        /// <param name="startdt">The start of shift</param>
        /// <param name="enddt">The end of shift</param>
        /// <returns>True if scheduled, false otherwise</returns>
        public ActionResult CheckIfScheduledAdd(int personID, string startdt, string enddt)
        {
            DateTime scheduledStartTime = ConvertDateToC(long.Parse(startdt));
            DateTime scheduledEndTime = ConvertDateToC(long.Parse(enddt));
            return Json(this.shiftDAL.CheckIfScheduledAdd(personID, scheduledStartTime, scheduledEndTime));
        }

        /// <summary>
        /// Request time off
        /// </summary>
        /// <returns>The view for requesting time off</returns>
        public ActionResult RequestTimeOff()
        {
            ViewBag.failedRequest = "";
            ViewBag.successfulRequest = "";
            return View();
        }

        /// <summary>
        /// Accepts request off time
        /// </summary>
        /// <param name="personID">The person's id</param>
        /// <param name="positionID">The position's id</param>
        /// <param name="startDate">The start time for asking off</param>
        /// <param name="endDate">The end time for asking off</param>
        /// <param name="taskList">The task list</param>
        /// <param name="notes">Notes</param>
        /// <returns>True or false if success</returns>
        [HttpPost]
        public ActionResult RequestTimeOffFunction(string personID, string positionID, string startDate, string endDate, string taskList, string notes)
        {

            startDate = Request.Form["startDate"];
            if (startDate == "")
            {
                ViewBag.startError = "You must include a start date.";
                return View("RequestTimeOff");
            }
            endDate = Request.Form["endDate"];
            if (endDate == "")
            {
                ViewBag.startError = "You must include an end date.";
                return View("RequestTimeOff");
            }
            if (DateTime.Parse(endDate) < DateTime.Parse(startDate))
            {
                ViewBag.timingError = "Your end date must not be before your start date.";
                return View("RequestTimeOff");
            }
            if (DateTime.Parse(startDate) < DateTime.Now.AddHours(-4))
            {
                ViewBag.pastError = "You cannot request off in the past.";
                return View("RequestTimeOff");
            }

            var where = "";

            Shift shift = new Shift();
            shift.personID = int.Parse(Session["id"].ToString());
            shift.positionID = this.positionDAL.FindPositionIDByUnavailable();
            shift.scheduledStartTime = DateTime.Parse(startDate).AddHours(4);
            shift.scheduledEndTime = DateTime.Parse(endDate).AddHours(4);
            Dictionary<int, bool> otherThing = taskList == null ? new Dictionary<int, bool>() : JsonConvert.DeserializeObject<Dictionary<int, bool>>(taskList);
            bool checkIfAlreadyScheduled = this.shiftDAL.CheckIfPersonIsScheduled(shift.personID, shift.scheduledStartTime, shift.scheduledEndTime, where);
            if (checkIfAlreadyScheduled == false)
            {
                ViewBag.failedRequest = "You are already scheduled between " + startDate + " and " +
                    endDate + " or have requested this time off already. Please check your schedule.";
                return View("RequestTimeOff");
            }
            else
            {
                ViewBag.successfulRequest = "Your request beginning " + startDate + " and ending " + endDate + " has been submitted.";
                this.shiftDAL.AddShift(shift, otherThing);
                return View("RequestTimeOff");
            }
        }

        #region TimeCard

        /// <summary>
        /// Adds Timecard page
        /// </summary>
        /// <returns>View</returns>
        public ActionResult AddTimePunchPage()
        {
            string loggedInUserId = (Session["id"].ToString());
            string whereClause = " WHERE s.personId = " + loggedInUserId;
            Shift nearestShift = shiftDAL.GetNearestShift(whereClause);
            if (nearestShift.scheduledStartTime == DateTime.MinValue)
            {
                TempData["notice"] = "You have no scheduled shifts\n\n See Mangement";
                return RedirectToAction("Index", "Home");
            }

            return View(this.TimeUpdate(nearestShift));
        }

        /// <summary>
        /// Verify time is valid
        /// </summary>
        /// <param name="whereClause">Where clause for use in DAL</param>
        /// <returns>True or false if valid/invalid</returns>
        public bool ValidPunch(string whereClause)
        {
            bool validShift = true;

            if (shiftDAL.GetNearestShift(whereClause).scheduledStartTime == DateTime.MinValue)
            {
                validShift = false;
                TempData["notice"] = "You have no scheduled shifts\n\n See Mangement";
            }
            if (!(shiftDAL.GetNearestShift(whereClause).scheduledStartTime.ToUniversalTime() >= DateTime.Now.AddDays(-1).ToUniversalTime() && shiftDAL.GetNearestShift(whereClause).scheduledStartTime < DateTime.Now.AddDays(1).ToUniversalTime()))
            {
                validShift = false;
                TempData["notice"] = "You have no shifts scheduled today\n\n See Mangement";
            }
            //If Users clock in under 4 hours late
            else if (shiftDAL.GetNearestShift(whereClause).scheduledStartTime.AddHours(4).ToUniversalTime() < DateTime.Now.ToUniversalTime())
            {
                validShift = false;
                TempData["notice"] = "You are too late to clock in\n See Mangement";
            }
            else if ((shiftDAL.GetNearestShift(whereClause).scheduledStartTime.AddHours(-4).ToUniversalTime() > DateTime.Now.ToUniversalTime()))
            {
                validShift = false;
                TempData["notice"] = "You are too early to clock in\n See Mangement";
            }
            return validShift;
        }

        /// <summary>
        /// Refresh display
        /// </summary>
        /// <returns>Add time punch view</returns>
        public ActionResult RefreshPageDisplayError()
        {
            return RedirectToAction("AddTimePunchPage", "Shift");
        }

        /// <summary>
        /// Clocks user in retruns them to the time card page
        /// </summary>
        /// <returns>Time punch view</returns>
        public ActionResult ClockUserIn()
        {
            string loggedInUserId = (Session["id"].ToString());
            string whereClause = "WHERE p.id = " + loggedInUserId;
            if (this.ValidPunch(whereClause))
            {
                this.shiftDAL.ClockUserIn(shiftDAL.GetNearestShift(whereClause).scheduleShiftID, DateTime.Now.ToUniversalTime());
                return Redirect(Request.UrlReferrer.ToString());

            }
            else
            {
                return this.RefreshPageDisplayError();
            }
        }

        /// <summary>
        /// Clocks the user out returns them to the time card page
        /// </summary>
        /// <returns>Time punch View</returns>
        public ActionResult ClockUserOut()
        {
            //This needs to be cleaned up THREE LINES TO GET one ID not cool
            string loggedInUserId = (Session["id"].ToString());
            string whereClause = "WHERE p.id = " + loggedInUserId;
            this.shiftDAL.ClockUserOut(shiftDAL.GetNearestShift(whereClause).scheduleShiftID, DateTime.Now.ToUniversalTime());
            return Redirect(Request.UrlReferrer.ToString());
        }

        /// <summary>
        /// Clockt the user out for lunch
        /// </summary>
        /// <returns>Time Punch View</returns>
        public ActionResult ClockLunchStart()
        {
            string loggedInUserId = (Session["id"].ToString());
            string whereClause = "WHERE p.id = " + loggedInUserId;
            this.shiftDAL.ClockLunchStart(shiftDAL.GetNearestShift(whereClause).scheduleShiftID, DateTime.Now.ToUniversalTime());
            return Redirect(Request.UrlReferrer.ToString());
        }

        /// <summary>
        /// Clocks the user in from lunch
        /// </summary>
        /// <returns>Time Punch View</returns>
        public ActionResult ClockLunchEnd()
        {
            string loggedInUserId = (Session["id"].ToString());
            string whereClause = "WHERE p.id = " + loggedInUserId;
            this.shiftDAL.ClockLunchEnd(shiftDAL.GetNearestShift(whereClause).scheduleShiftID, DateTime.Now.ToUniversalTime());
            return Redirect(Request.UrlReferrer.ToString());
        }
        #endregion


        #region TimeCardEdit

        /// <summary>
        /// Get last two weeks worth of shifts for person with the accepted personid
        /// </summary>
        /// <param name="personid">The person's id as an integer</param>
        /// <returns>The time card edit view</returns>
        public ActionResult GetLastTwoWeeksOfShiftsForEdit(int personid)
        {
            string whereClause = $"Where p.id =  {personid}  and sh.scheduledStartTime > (DATEADD(day,- 14, GETDATE())) and sh.scheduledStartTime < (GETDATE())";
            List<Shift> shifts = this.shiftDAL.GetAllShifts(whereClause);
            List<TimeCardEditViewModel> timeCardEdits = this.CovertShiftToTimeCardView(shifts);
            Person person = this.personDAL.GetPersonByID(personid);
            ViewBag.FullName = person.GetFullName();
            return View(timeCardEdits);
        }

        //displays oldest first to insure that mistakes can be corrected
        private List<TimeCardEditViewModel> CovertShiftToTimeCardView(List<Shift> shifts)
        {
            List<TimeCardEditViewModel> timeCardEdits = new List<TimeCardEditViewModel>();
            foreach (Shift shift in shifts)
            {
                TimeCardEditViewModel timeCard = new TimeCardEditViewModel();
                timeCard.shiftId = shift.shiftID;
                timeCard.personFirstName = shift.personFirstName;
                timeCard.personLastName = shift.personLastName;
                timeCard.scheduledStartTime = shift.scheduledStartTime;
                timeCard.scheduledEndTime = shift.scheduledEndTime;
                timeCard.scheduledLunchBreakStart = shift.scheduledLunchBreakStart;
                timeCard.scheduledLunchBreakEnd = shift.scheduledLunchBreakEnd;
                timeCard.actualStartTime = shift.actualStartTime;
                timeCard.actualEndTime = shift.actualEndTime;
                timeCard.actualLunchBreakStart = shift.actualLunchBreakStart;
                timeCard.actualLunchBreakEnd = shift.actualLunchBreakEnd;
                timeCardEdits.Add(timeCard);
            }

            return timeCardEdits;
        }
        private Shift ConvertTimeCardEditViewModelToShift(TimeCardEditViewModel timeCardEditViewModel, Shift orignalShift)
        {
            Shift shift = orignalShift;
            shift.personFirstName = timeCardEditViewModel.personFirstName;
            shift.personLastName = timeCardEditViewModel.personLastName;
            shift.scheduledStartTime = timeCardEditViewModel.scheduledStartTime;
            shift.scheduledEndTime = timeCardEditViewModel.scheduledEndTime;
            shift.scheduledLunchBreakStart = timeCardEditViewModel.scheduledLunchBreakStart;
            shift.scheduledLunchBreakEnd = timeCardEditViewModel.scheduledLunchBreakEnd;
            shift.actualStartTime = timeCardEditViewModel.actualStartTime;
            shift.actualEndTime = timeCardEditViewModel.actualEndTime;
            shift.actualLunchBreakStart = timeCardEditViewModel.actualLunchBreakStart;
            shift.actualLunchBreakEnd = timeCardEditViewModel.actualLunchBreakEnd;
            return shift;
        }

        private bool EditTimeCardErrorCheck(TimeCardEditViewModel timeCardEditViewModel)
        {
            bool hasErrors = false;
            string alert = "There are errors in this edit The fields have been reset";
            DateTime temp;
            #region Insure Valid DateTimes
            if (!DateTime.TryParse(timeCardEditViewModel.scheduledStartTime.ToString(), out temp))
            {
                hasErrors = true;
                alert += "\n Scheduled Start Time is invalid datetime";
            }
            if (!DateTime.TryParse(timeCardEditViewModel.scheduledEndTime.ToString(), out temp))
            {
                hasErrors = true;
                alert += "\n Scheduled End Time is invalid datetime";
            }
            if (timeCardEditViewModel.scheduledStartTime == null || timeCardEditViewModel.scheduledEndTime == null)
            {
                hasErrors = true;
                alert += "\n Scheduled Start and End times cannot be empty";
            }
            if (timeCardEditViewModel.scheduledLunchBreakStart != null)
            {
                if (!DateTime.TryParse(timeCardEditViewModel.scheduledLunchBreakStart.ToString(), out temp))
                {
                    hasErrors = true;
                    alert += "\n Scheduled LunchBreak Start is invalid datetime";
                }
            }
            if (timeCardEditViewModel.scheduledLunchBreakEnd != null)
            {
                if (!DateTime.TryParse(timeCardEditViewModel.scheduledLunchBreakEnd.ToString(), out temp))
                {
                    hasErrors = true;
                    alert += "\n Scheduled LunchBreak End is invalid datetime";
                }
            }
            if (timeCardEditViewModel.actualStartTime != null)
            {
                if (!DateTime.TryParse(timeCardEditViewModel.actualStartTime.ToString(), out temp))
                {
                    hasErrors = true;
                    alert += "\n Actual Start Time is invalid datetime";
                }
            }
            if (timeCardEditViewModel.actualEndTime != null)
            {
                if (!DateTime.TryParse(timeCardEditViewModel.actualEndTime.ToString(), out temp))
                {
                    hasErrors = true;
                    alert += "\n Actual End Time is invalid datetime";
                }
            }
            if (timeCardEditViewModel.actualLunchBreakStart != null)
            {
                if (!DateTime.TryParse(timeCardEditViewModel.actualLunchBreakStart.ToString(), out temp))
                {
                    hasErrors = true;
                    alert += "\n Actual LunchBreak Start is invalid datetime";
                }
            }
            if (timeCardEditViewModel.actualLunchBreakEnd != null)
            {
                if (!DateTime.TryParse(timeCardEditViewModel.actualLunchBreakEnd.ToString(), out temp))
                {
                    hasErrors = true;
                    alert += "\n Actual LunchBreak End is invalid datetime";
                }
            }
            #endregion
            #region Insure Datetime logic
            if (timeCardEditViewModel.scheduledLunchBreakStart == null && timeCardEditViewModel.scheduledLunchBreakEnd != null)
            {
                hasErrors = true;
                alert += "\n If a Lunch break is scheduled it must have both a start and end time";
            }
            if (timeCardEditViewModel.scheduledLunchBreakEnd == null && timeCardEditViewModel.scheduledLunchBreakStart != null)
            {
                hasErrors = true;
                alert += "\n If a Lunch break is scheduled it must have both a start and end time";
            }
            if (timeCardEditViewModel.actualStartTime > timeCardEditViewModel.actualEndTime)
            {
                hasErrors = true;
                alert += "\n Actual start time must exist before end time";
            }
            if (timeCardEditViewModel.scheduledStartTime > timeCardEditViewModel.scheduledEndTime)
            {
                hasErrors = true;
                alert += "\n Scheduled start Time must exist before end time";
            }
            if (timeCardEditViewModel.scheduledLunchBreakStart > timeCardEditViewModel.scheduledLunchBreakEnd)
            {
                hasErrors = true;
                alert += "\n Scheduled Lunch start Time must exist before end time";
            }
            if (timeCardEditViewModel.actualLunchBreakStart > timeCardEditViewModel.actualLunchBreakEnd)
            {
                hasErrors = true;
                alert += "\n Actual Lunch start Time must exist before end time";
            }
            if (timeCardEditViewModel.scheduledLunchBreakStart > timeCardEditViewModel.scheduledEndTime)
            {
                hasErrors = true;
                alert += "\n No Scheduled values can exist before Scheduled end time";
            }
            if (timeCardEditViewModel.scheduledLunchBreakEnd > timeCardEditViewModel.scheduledEndTime)
            {
                hasErrors = true;
                alert += "\n No Scheduled values can exist before Scheduled end time";
            }
            if (timeCardEditViewModel.actualStartTime > timeCardEditViewModel.actualEndTime)
            {
                hasErrors = true;
                alert += "\n No actual values can exist before actual end time";
            }
            if (timeCardEditViewModel.actualLunchBreakEnd > timeCardEditViewModel.actualEndTime)
            {
                hasErrors = true;
                alert += "\n No actual values can exist before actual end time";
            }
            if (timeCardEditViewModel.actualLunchBreakStart > timeCardEditViewModel.actualEndTime)
            {
                hasErrors = true;
                alert += "\n No actual values can exist before actual end time";
            }
            if (timeCardEditViewModel.actualEndTime != null && timeCardEditViewModel.actualStartTime == null)
            {
                hasErrors = true;
                alert += "\n Actual Start time must exist for end time to exist";
            }
            if (timeCardEditViewModel.actualLunchBreakStart != null && timeCardEditViewModel.actualStartTime == null)
            {
                hasErrors = true;
                alert += "\n Actual Start time must exist for Lunch start time to exist";
            }
            if (timeCardEditViewModel.actualLunchBreakEnd != null && timeCardEditViewModel.actualStartTime == null)
            {
                hasErrors = true;
                alert += "\n Actual Start time must exist for Lunch end time to exist";
            }
            if (timeCardEditViewModel.actualLunchBreakEnd != null && timeCardEditViewModel.actualLunchBreakStart == null)
            {
                hasErrors = true;
                alert += "\n Actual Start Lunch must exist for Lunch End time to exist";
            }
            if (timeCardEditViewModel.actualEndTime != null && timeCardEditViewModel.actualStartTime == null)
            {
                hasErrors = true;
                alert += "\n Actual Start time must exist for end time to exist";
            }
            if (timeCardEditViewModel.actualLunchBreakEnd != null && timeCardEditViewModel.actualStartTime == null)
            {
                hasErrors = true;
                alert += "\n Actual Start Lunch must exist for Lunch End time to exist";
            }
            if (timeCardEditViewModel.actualLunchBreakStart < timeCardEditViewModel.actualStartTime)
            {
                hasErrors = true;
                alert += "\n Actual Start Lunch cannot exist before actual start time";
            }
            if (timeCardEditViewModel.scheduledLunchBreakStart < timeCardEditViewModel.scheduledStartTime)
            {
                hasErrors = true;
                alert += "\n Scheduled Start Lunch cannot exist before Scheduled start time";
            }
            if (timeCardEditViewModel.scheduledLunchBreakStart != null && timeCardEditViewModel.scheduledLunchBreakEnd == null)
            {
                hasErrors = true;
                alert += "\n Scheduled Lunch Breaks must have both start and end or not exist";
            }
            if (timeCardEditViewModel.scheduledLunchBreakStart == null && timeCardEditViewModel.scheduledLunchBreakEnd != null)
            {
                hasErrors = true;
                alert += "\n Scheduled Lunch Breaks must have both start and end or not exist";
            }
            if (timeCardEditViewModel.actualLunchBreakStart == null && timeCardEditViewModel.actualLunchBreakEnd != null)
            {
                hasErrors = true;
                alert += "\n Actual Lunch Breaks cannot end without starting";
            }
            if (timeCardEditViewModel.actualStartTime == null && timeCardEditViewModel.actualEndTime != null)
            {
                hasErrors = true;
                alert += "\n Actual Start time must exist before any other actual time";
            }
            if (timeCardEditViewModel.actualStartTime == null && timeCardEditViewModel.actualLunchBreakStart != null)
            {
                hasErrors = true;
                alert += "\n Actual Start time must exist before any other actual time";
            }
            if (timeCardEditViewModel.actualStartTime == null && timeCardEditViewModel.actualLunchBreakEnd != null)
            {
                hasErrors = true;
                alert += "\n Actual Start time must exist before any other actual time";
            }
            if (timeCardEditViewModel.actualEndTime != null && timeCardEditViewModel.actualLunchBreakStart != null && timeCardEditViewModel.actualLunchBreakEnd == null)
            {
                hasErrors = true;
                alert += "\n Shifts cannot end with an actual Start lunch and no actual end lunch";
            }
            #endregion



            if (hasErrors)
            {
                TempData["alert"] = MvcHtmlString.Create(HttpUtility.HtmlEncode(alert).Replace("\n", "<br />")); ;
            }

            return hasErrors;
        }

        /// <summary>
        /// Edit the time card
        /// </summary>
        /// <param name="shiftId">The shift id to be edited</param>
        /// <returns>Time card view</returns>
        public ActionResult EditTimecard(int shiftId)
        {
            string whereClause = $"WHERE s.id = {shiftId}";
            List<Shift> shift = this.shiftDAL.GetAllShifts(whereClause);
            List<TimeCardEditViewModel> selectedTimeCard = this.CovertShiftToTimeCardView(shift);
            ViewBag.FullName = shift[0].personFirstName + " " + shift[0].personLastName + "'s";
            return View(selectedTimeCard[0]);
        }

        /// <summary>
        /// Edit Time Card
        /// </summary>
        /// <param name="editedViewModel">The view model</param>
        /// <returns>Redirect to view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTimecard(TimeCardEditViewModel editedViewModel)
        {
            editedViewModel = this.AddUserAddedValuesToTimeCard(editedViewModel);
            if (EditTimeCardErrorCheck(editedViewModel))
            {
                return RedirectToAction("EditTimecard", "Shift", new { shiftId = editedViewModel.shiftId });
            }
            string whereClause = $"WHERE s.id ={editedViewModel.shiftId}";
            Shift orignalShift = this.shiftDAL.GetAllShifts(whereClause)[0];
            //test
            Shift tempOrignalShift = this.shiftDAL.GetAllShifts(whereClause)[0];
            tempOrignalShift.scheduleShiftID = Int32.MinValue + 500;

            Shift updatedShift = this.ConvertTimeCardEditViewModelToShift(editedViewModel, orignalShift);

            //Alerts and prevents user from 'saving' when no changes are made  
            if (!this.HasShiftChanged(updatedShift, tempOrignalShift))
            {
                TempData["alert"] = "No values where changed";
                return RedirectToAction("EditTimecard", "Shift", new { shiftId = editedViewModel.shiftId });

            }

            if (this.shiftDAL.EditShift(updatedShift, tempOrignalShift))
            {
                TempData["notice"] = "Shift Updated successfully";
            }
            else
            {
                TempData["alert"] = "There was an issue with the update";
            }
            var herefortest = updatedShift;
            return RedirectToAction("GetLastTwoWeeksOfShiftsForEdit", "Shift", new { personid = updatedShift.personID });
        }

        private bool HasShiftChanged(Shift updated, Shift orignal)
        {
            if (updated.scheduledStartTime != orignal.scheduledStartTime)
            {
                return true;
            }
            if (updated.scheduledEndTime != orignal.scheduledEndTime)
            {
                return true;
            }
            if (updated.scheduledLunchBreakStart != orignal.scheduledLunchBreakStart)
            {
                return true;
            }
            if (updated.scheduledLunchBreakEnd != orignal.scheduledLunchBreakEnd)
            {
                return true;
            }
            if (updated.actualStartTime != orignal.actualStartTime)
            {
                return true;
            }
            if (updated.actualEndTime != orignal.actualEndTime)
            {
                return true;
            }
            if (updated.actualLunchBreakStart != orignal.actualLunchBreakStart)
            {
                return true;
            }
            if (updated.actualLunchBreakEnd != orignal.actualLunchBreakEnd)
            {
                return true;
            }
            return false;
        }
        private TimeCardEditViewModel AddUserAddedValuesToTimeCard(TimeCardEditViewModel timeCardEditViewModel)
        {

            DateTime temp;
            if (DateTime.TryParse(Request.Form["scheduledLunchBreakStart"], out temp))
            {
                timeCardEditViewModel.scheduledLunchBreakStart = temp;
            }

            if (DateTime.TryParse(Request.Form["scheduledLunchBreakEnd"], out temp))
            {
                timeCardEditViewModel.scheduledLunchBreakEnd = temp;
            }

            if (DateTime.TryParse(Request.Form["actualStartTime"], out temp))
            {
                timeCardEditViewModel.actualStartTime = temp;
            }

            if (DateTime.TryParse(Request.Form["actualEndTime"], out temp))
            {
                timeCardEditViewModel.actualEndTime = temp;
            }

            if (DateTime.TryParse(Request.Form["actualLunchBreakStart"], out temp))
            {
                timeCardEditViewModel.actualLunchBreakStart = temp;
            }

            if (DateTime.TryParse(Request.Form["actualLunchBreakEnd"], out temp))
            {
                timeCardEditViewModel.actualLunchBreakEnd = temp;
            }

            return timeCardEditViewModel;
        }

        #endregion

        private void Messagebox(string xMessage)
        {
            Response.Write("<script>alert('" + xMessage + "')</script>");
        }

        private void ContactPersonShiftChange(string type, Shift shift)
        {
            string loggedInUserId = (Session["id"].ToString());
            Person loggedInUser = this.personDAL.GetDesiredPersons($"Where Id = {loggedInUserId}").FirstOrDefault();
            Person shiftChangePerson = this.personDAL.GetDesiredPersons($"Where Id = {shift.personID}").FirstOrDefault();
            Email email = new Email(shiftChangePerson);

            string subject = $"Your Shift has been {type}d";

            string body = $"Hello { shiftChangePerson.GetFullName()}, \n" +
                $"\nYou are recieving this email to let you know that { loggedInUser.GetFullName() } Has {type}d your shift on {shift.scheduledStartTime} \n" +
                $"\n Please log in to your account to see all schedule changes\n" +

                $"\n If this has been done in error please contact your Admin as soon as possible " +
                $"\n Hope you have a wonderful day";


            email.SendMessage(subject, body);

        }

        private DateTime ConvertDateToC(long jsDate)
        {
            var date = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return date.AddMilliseconds(jsDate);
        }

        private Shift TimeUpdate(Shift shift)
        {
            int timezone = -4;
            shift.scheduledStartTime = shift.scheduledStartTime.AddHours(timezone);
            shift.scheduledEndTime = shift.scheduledEndTime.AddHours(timezone);
            if (shift.scheduledLunchBreakStart != null)
            {
                DateTime temp = shift.scheduledLunchBreakStart ?? DateTime.MinValue; shift.scheduledLunchBreakStart = temp.AddHours(timezone);
            }
            if (shift.scheduledLunchBreakEnd != null)
            {
                DateTime temp = shift.scheduledLunchBreakEnd ?? DateTime.MinValue;
                shift.scheduledLunchBreakEnd = temp.AddHours(timezone);
            }
            if (shift.actualStartTime != null)
            {
                DateTime temp = shift.actualStartTime ?? DateTime.MinValue;
                shift.actualStartTime = temp.AddHours(timezone);
            }
            if (shift.actualEndTime != null)
            {
                DateTime temp = shift.actualEndTime ?? DateTime.MinValue;
                shift.actualEndTime = temp.AddHours(timezone);
            }
            if (shift.actualLunchBreakStart != null)
            {
                DateTime temp = shift.actualLunchBreakStart ?? DateTime.MinValue;
                shift.actualLunchBreakStart = temp.AddHours(timezone);
            }
            if (shift.actualLunchBreakEnd != null)
            {
                DateTime temp = shift.actualLunchBreakEnd ?? DateTime.MinValue;
                shift.actualLunchBreakEnd = temp.AddHours(timezone);
            }
            return shift;
        }

    }
}
