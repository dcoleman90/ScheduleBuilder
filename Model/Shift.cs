﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScheduleBuilder.Model
{
    /// <summary>
    /// The Shift Model Class represents a shift at a place of employment and references a person, position, tasks
    /// and specific dates and times.
    /// </summary>
    public class Shift
    {
        //Shift Id 
        public int shiftID { get; set; }

        //Scheduled shift id
        public int scheduleShiftID { get; set; }

        //Scheduled person Id
        public int personID { get; set; }

        //Scheduled persons first name
        [Display(Name = "First Name")]
        public string personFirstName { get; set; }

        //Scheduled persons last name
        [Display(Name = "Last Name")]
        public string personLastName { get; set; }

        //Shift postion ID
        public int positionID { get; set; }

        //Shift postition name
        [Display(Name = "Position")]
        public string positionName { get; set; }

        //Shift start time
        [Display(Name = "Scheduled Start Time")]
        public DateTime scheduledStartTime { get; set; }

        //shift end time
        [Display(Name = "Scheduled End Time")]
        public DateTime scheduledEndTime { get; set; }

        //shift lunch start time
        [Display(Name = "Scheduled Break Start")]
        public DateTime? scheduledLunchBreakStart { get; set; }

        //shift lunch end time
        [Display(Name = "Scheduled Break End")]
        public DateTime? scheduledLunchBreakEnd { get; set; }

        //shift actual start time
        [Display(Name = "Actual Clock In")]
        public DateTime? actualStartTime { get; set; }

        //shift actual end time
        [Display(Name = "Actual Clock Out")]
        public DateTime? actualEndTime { get; set; }

        //shift actual lunch start time
        [Display(Name = "Actual Break Start")]
        public DateTime? actualLunchBreakStart { get; set; }

        //shift actual lunch end time
        [Display(Name = "Actual Break End")]
        public DateTime? actualLunchBreakEnd { get; set; }

        //List of Task
        public List<int> TaskIdList { get; set; }

        //Notes about the shift
        public string Notes { get; set; }
    }
}
