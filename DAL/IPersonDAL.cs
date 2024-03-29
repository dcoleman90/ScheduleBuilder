﻿using ScheduleBuilder.Model;
using System;
using System.Collections.Generic;

namespace ScheduleBuilder.DAL
{
    /// <summary>
    /// This interface is for the PersonDAL class
    /// </summary>
    public interface IPersonDAL
    {

        /// <summary>
        /// returns all persons equal too the accepted where clause which will be formated such as WHERE = etc
        /// </summary>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        List<Person> GetDesiredPersons(string whereClause);

        /// <summary>
        /// allows a Person to be added to the database
        /// </summary>
        /// <param name="lastName"></param>
        /// <param name="firstName"></param>
        /// <param name="dateOfBirth"></param>
        /// <param name="ssn"></param>
        /// <param name="gender"></param>
        /// <param name="phone"></param>
        /// <param name="streetAddress"></param>
        /// <param name="zipcode"></param>
        /// <param name="username"></param>
        /// <param name="email"></param>
        int AddPerson(string lastName
            , string firstName
            , DateTime dateOfBirth
            , string ssn
            , string gender
            , string phone
            , string streetAddress
            , string zipcode
            , string email);

        /// <summary>
        /// Allows the user to edit the accepted person value
        /// </summary>
        /// <param name="editPerson"></param>
        void EditPerson(Person person);

        /// <summary>
        /// Changes the person's status to seperated
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        Person SeperateEmployee(Person person);

        /// <summary>
        /// Allows the user to update thier password
        /// </summary>
        /// <param name="person"></param>
        void UpdatePassword(Person person);




    }
}
