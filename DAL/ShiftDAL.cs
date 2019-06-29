﻿using ScheduleBuilder.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ScheduleBuilder.DAL
{
    /// <summary>
    /// This class provides access to the database 
    /// It is conserned with Person 
    /// </summary>
    public class ShiftDAL : IShiftDAL
    {
        /// <summary>
        /// Get all shifts in the database
        /// </summary>
        /// <returns>A list of shift objects</returns>
        public List<Shift> GetAllShifts()
        {
            SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection();
            List<Shift> shiftList = new List<Shift>();

            string selectStatement = "SELECT s.id, s.scheduleShiftId, s.personId, s.positionId, sh.scheduledStartTime, sh.scheduledEndTime, " +
                "sh.scheduledLunchBreakStartTime, sh.scheduledLunchBreakEndTime, sh.actualStartTime, sh.actualEndTime, sh.actualLunchBreakStart, " +
                "sh.acutalLunchBreakEnd, p.first_name, p.last_name, ps.position_title " +
                "FROM shift AS s " +
                "JOIN shiftHours AS sh ON s.scheduleShiftId = sh.id " +
                "JOIN person AS p ON s.personId = p.id " +
                "JOIN position AS ps ON s.positionId = ps.id";

            using (connection)
            {
                connection.Open();

                using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                {
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Shift shift = new Shift();
                            shift.shiftID = int.Parse(reader["id"].ToString());
                            shift.scheduleShiftID = int.Parse(reader["scheduleShiftId"].ToString());
                            shift.personID = int.Parse(reader["personId"].ToString());
                            shift.positionID = int.Parse(reader["positionId"].ToString());
                            shift.scheduledStartTime = (DateTime)reader["scheduledStartTime"];
                            shift.scheduledEndTime = (DateTime)reader["scheduledEndTime"];
                            shift.scheduledLunchBreakStart = reader["scheduledLunchBreakStartTime"] as DateTime?;
                            shift.scheduledLunchBreakEnd = reader["scheduledLunchBreakEndTime"] as DateTime?;
                            shift.actualStartTime = reader["actualStartTime"] as DateTime?;
                            shift.actualEndTime = reader["actualEndTime"] as DateTime?;
                            shift.actualLunchBreakStart = reader["actualLunchBreakStart"] as DateTime?;
                            shift.actualLunchBreakEnd = reader["acutalLunchBreakEnd"] as DateTime?;
                            shift.positionName = reader["position_title"].ToString();
                            shift.personLastName = reader["last_name"].ToString();
                            shift.personFirstName = reader["first_name"].ToString();
                            shiftList.Add(shift);
                        }
                    }
                }
            }
            return shiftList;

        }

        /// <summary>
        /// Returns list equal to the accepted where statement
        /// </summary>
        /// <returns></returns>
        public List<Shift> GetAllShifts(string whereClause)
        {
            SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection();
            List<Shift> shiftList = new List<Shift>();

            string selectStatement = "SELECT s.id, s.scheduleShiftId, s.personId, s.positionId, sh.scheduledStartTime, sh.scheduledEndTime, " +
                "sh.scheduledLunchBreakStartTime, sh.scheduledLunchBreakEndTime, sh.actualStartTime, sh.actualEndTime, sh.actualLunchBreakStart, " +
                "sh.acutalLunchBreakEnd, p.first_name, p.last_name, ps.position_title " +
                "FROM shift AS s " +
                "JOIN shiftHours AS sh ON s.scheduleShiftId = sh.id " +
                "JOIN person AS p ON s.personId = p.id " +
                "JOIN position AS ps ON s.positionId = ps.id " + whereClause;

            using (connection)
            {
                connection.Open();

                using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                {
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Shift shift = new Shift();
                            shift.shiftID = int.Parse(reader["id"].ToString());
                            shift.scheduleShiftID = int.Parse(reader["scheduleShiftId"].ToString());
                            shift.personID = int.Parse(reader["personId"].ToString());
                            shift.positionID = int.Parse(reader["positionId"].ToString());
                            shift.scheduledStartTime = (DateTime)reader["scheduledStartTime"];
                            shift.scheduledEndTime = (DateTime)reader["scheduledEndTime"];
                            shift.scheduledLunchBreakStart = reader["scheduledLunchBreakStartTime"] as DateTime?;
                            shift.scheduledLunchBreakEnd = reader["scheduledLunchBreakEndTime"] as DateTime?;
                            shift.actualStartTime = reader["actualStartTime"] as DateTime?;
                            shift.actualEndTime = reader["actualEndTime"] as DateTime?;
                            shift.actualLunchBreakStart = reader["actualLunchBreakStart"] as DateTime?;
                            shift.actualLunchBreakEnd = reader["acutalLunchBreakEnd"] as DateTime?;
                            shift.positionName = reader["position_title"].ToString();
                            shift.personLastName = reader["last_name"].ToString();
                            shift.personFirstName = reader["first_name"].ToString();
                            shiftList.Add(shift);
                        }
                    }
                }
            }
            return shiftList;

        }

        public Shift GetNearestShift(string whereClause)
        {
            SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection();
            Shift shift = new Shift();
            string query = $"SELECT TOP 1 s.id" +
                $", s.scheduleShiftId" +
                $", s.personId" +
                $", s.positionId" +
                $", sh.scheduledStartTime" +
                $", sh.scheduledEndTime" +
                $", sh.scheduledLunchBreakStartTime" +
                $", sh.scheduledLunchBreakEndTime" +
                $", sh.actualStartTime" +
                $", sh.actualEndTime" +
                $", sh.actualLunchBreakStart" +
                $", sh.acutalLunchBreakEnd" +
                $", p.first_name" +
                $", p.last_name" +
                $", ps.position_title " +
                $" FROM shift AS s " +
                $" JOIN shiftHours AS sh ON s.scheduleShiftId = sh.id " +
                $" JOIN person AS p ON s.personId = p.id " +
                $" JOIN position AS ps ON s.positionId = ps.id "
                +  whereClause +
                $"  ORDER BY ABS(DATEDIFF(MINUTE, scheduledStartTime, GETDATE()))";

            using (connection)
            {
                connection.Open();

                using (SqlCommand selectCommand = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            shift.shiftID = int.Parse(reader["id"].ToString());
                            shift.scheduleShiftID = int.Parse(reader["scheduleShiftId"].ToString());
                        
                            shift.positionID = int.Parse(reader["positionId"].ToString());
                            shift.scheduledStartTime = (DateTime)reader["scheduledStartTime"];
                            shift.scheduledEndTime = (DateTime)reader["scheduledEndTime"];
                            shift.scheduledLunchBreakStart = reader["scheduledLunchBreakStartTime"] as DateTime?;
                            shift.scheduledLunchBreakEnd = reader["scheduledLunchBreakEndTime"] as DateTime?;
                            shift.actualStartTime = reader["actualStartTime"] as DateTime?;
                            shift.actualEndTime = reader["actualEndTime"] as DateTime?;
                            shift.actualLunchBreakStart = reader["actualLunchBreakStart"] as DateTime?;
                            shift.actualLunchBreakEnd = reader["acutalLunchBreakEnd"] as DateTime?;
                            shift.positionName = reader["position_title"].ToString();
                            shift.personLastName = reader["last_name"].ToString();
                            shift.personFirstName = reader["first_name"].ToString();
                            shift.personID = int.Parse(reader["personId"].ToString());
                        }
                    }
                }
            }
            return shift;

        }

        /// <summary>
        /// Add a shift
        /// </summary>
        /// <param name="shift">A shift object to be sent to db</param>
        /// <returns>A boolean value of success or not</returns>
        public bool AddShift(Shift shift)
        {
            int shiftHoursResult = 0;
            int shiftResult = 0;

            string insertShiftHoursStatement =
            "INSERT INTO shiftHours([scheduledStartTime], [scheduledEndTime], [scheduledLunchBreakStartTime], [ScheduledLunchBreakEndTime])" +
            "VALUES(@scheduledStartTime,@scheduledEndTime,@scheduledLunchBreakStartTime,@scheduledLunchBreakEndTime); SELECT SCOPE_IDENTITY()";

            string insertShiftStatement =
            "INSERT INTO shift([scheduleShiftID],[personId],[positionId])" +
            "VALUES(@scheduleShiftID,@personId,@positionId)";

            //create new shift and shift hours entries
            using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
            {
                int pk = -1;
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    using (SqlCommand insertCommand = new SqlCommand(insertShiftHoursStatement, connection))
                    {
                        insertCommand.Transaction = transaction;
                        insertCommand.Parameters.AddWithValue("@scheduledStartTime", shift.scheduledStartTime);
                        insertCommand.Parameters.AddWithValue("@scheduledEndTime", shift.scheduledEndTime);
                        insertCommand.Parameters.AddWithValue("@scheduledLunchBreakStartTime", ((object)shift.scheduledLunchBreakStart) ?? DBNull.Value);
                        insertCommand.Parameters.AddWithValue("@scheduledLunchBreakEndTime", ((object)shift.scheduledLunchBreakEnd) ?? DBNull.Value);
                        
                        pk = Convert.ToInt32(insertCommand.ExecuteScalar());
                        shiftHoursResult = 1;
                    }
                    using (SqlCommand insertShiftCommand = new SqlCommand(insertShiftStatement, connection))
                    {
                        insertShiftCommand.Transaction = transaction;
                        insertShiftCommand.Parameters.AddWithValue("@scheduleShiftId", pk);
                        insertShiftCommand.Parameters.AddWithValue("@personId", shift.personID);
                        insertShiftCommand.Parameters.AddWithValue("@positionId", shift.positionID);

                        shiftResult = insertShiftCommand.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch
                {
                    
                    transaction.Rollback();
                }
            }
            return (shiftHoursResult == 1 && shiftResult >= 1 ? true : false);
        }

        /// <summary>
        /// Update an existing shift/shift hours
        /// </summary>
        /// <param name="shift">The shift object to be updated</param>
        /// <returns>A boolean value based on the success or failure of the update</returns>
        public bool UpdateShift(Shift shift)
        {
            int shiftHoursResult = 0;
            int shiftResult = 0;

            string updateShiftHoursStatement =
            "UPDATE shiftHours SET [scheduledStartTime] = @scheduledStartTime, [scheduledEndTime] = @scheduledEndTime, [scheduledLunchBreakStartTime] = @scheduledLunchBreakStartTime, " +
            "[scheduledLunchBreakEndTime] = @scheduledLunchBreakEndTime " +
            "WHERE id = @id";

            string updateShiftStatement =
            "UPDATE shift SET [personId] = @personId, [positionId] = @positionId " +
            "WHERE id = @id";
            //update shift and shift hour entries
            using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    using (SqlCommand updateShiftHoursCommand = new SqlCommand(updateShiftHoursStatement, connection))
                    {
                        updateShiftHoursCommand.Transaction = transaction;
                        updateShiftHoursCommand.Parameters.AddWithValue("@scheduledStartTime", shift.scheduledStartTime);
                        updateShiftHoursCommand.Parameters.AddWithValue("@scheduledEndTime", shift.scheduledEndTime);
                        updateShiftHoursCommand.Parameters.AddWithValue("@scheduledLunchBreakStartTime", ((object)shift.scheduledLunchBreakStart) ?? DBNull.Value);
                        updateShiftHoursCommand.Parameters.AddWithValue("@scheduledLunchBreakEndTime", ((object)shift.scheduledLunchBreakEnd) ?? DBNull.Value);
                        updateShiftHoursCommand.Parameters.AddWithValue("@id", shift.scheduleShiftID);

                        shiftHoursResult = updateShiftHoursCommand.ExecuteNonQuery();
                    }
                    using (SqlCommand updateShiftCommand = new SqlCommand(updateShiftStatement, connection))
                    {
                        updateShiftCommand.Transaction = transaction;
                        updateShiftCommand.Parameters.AddWithValue("@id", shift.shiftID);
                        updateShiftCommand.Parameters.AddWithValue("@personId", shift.personID);
                        updateShiftCommand.Parameters.AddWithValue("@positionId", shift.positionID);

                        shiftResult = updateShiftCommand.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }

            return (shiftHoursResult == 1 && shiftResult >= 1 ? true : false);
        }

        /// <summary>
        /// Delete a shift
        /// </summary>
        /// <param name="shift"></param>
        /// <returns></returns>
        public bool DeleteShift(Shift shift)
        {
            int shiftHoursResult = 0;
            int shiftResult = 0;
            int taskResult = 0;

            string deleteShiftHoursStatement =
            "DELETE FROM shiftHours " +
            "WHERE id = @id";

            string deleteShiftStatement =
            "DELETE FROM shift " +
            "WHERE id = @id";

            string deleteTaskStatement =
            "DELETE FROM assignedTask " +
            "WHERE shiftId = @shiftId";

            //delete shift and shift hour entries
            using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    using (SqlCommand deleteTaskCommand = new SqlCommand(deleteTaskStatement, connection))
                    {
                        deleteTaskCommand.Transaction = transaction;
                        deleteTaskCommand.Parameters.AddWithValue("@shiftId", shift.shiftID);

                        taskResult = deleteTaskCommand.ExecuteNonQuery();
                    }
                    using (SqlCommand deleteShiftCommand = new SqlCommand(deleteShiftStatement, connection))
                    {
                        deleteShiftCommand.Transaction = transaction;
                        deleteShiftCommand.Parameters.AddWithValue("@id", shift.shiftID);

                        shiftResult = deleteShiftCommand.ExecuteNonQuery();
                    }
                    using (SqlCommand deleteShiftHoursCommand = new SqlCommand(deleteShiftHoursStatement, connection))
                    {
                        deleteShiftHoursCommand.Transaction = transaction;
                        deleteShiftHoursCommand.Parameters.AddWithValue("@id", shift.scheduleShiftID);

                        shiftHoursResult = deleteShiftHoursCommand.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }

            return (shiftHoursResult == 1 && shiftResult >= 1 ? true : false);
        }

        #region TimeCard 
        public void ClockUserIn(int shiftHoursId, DateTime clockIn)
        {
            string insert = $" UPDATE ShiftHours" +
                            $" SET actualStartTime = @clockInTime" +
                            $" WHERE id = @shiftId";
            using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
            {
                connection.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insert, connection))
                    {

                        command.Parameters.AddWithValue("@shiftId", shiftHoursId);
                        command.Parameters.AddWithValue("@clockInTime", clockIn);

                        command.ExecuteNonQuery();   
                    }
                    connection.Close();
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }
        }


        public void ClockUserOut(int scheduleShiftID, DateTime now)
        {
            string insert = $" UPDATE ShiftHours" +
                           $" SET actualEndTime = @clockOutTime" +
                           $" WHERE id = @shiftId";
            using (SqlConnection connection = ScheduleBuilder_DB_Connection.GetConnection())
            {
                connection.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(insert, connection))
                    {
                        command.Parameters.AddWithValue("@shiftId", scheduleShiftID);
                     
                        command.Parameters.AddWithValue("@clockOutTime", now);
                        
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }
        }
        #endregion
    }
}
