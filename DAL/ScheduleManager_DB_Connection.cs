﻿
using System.Configuration;
using System.Data.SqlClient;


namespace ScheduleManager.DAL
{
    public static class ScheduleManager_DB_Connection
    {
        public static SqlConnection GetConnection()
        {
#if DEBUG
            string connectionString = "Data Source =localhost;Initial Catalog=ScheduleManager;" +
            "Integrated Security=True;";
#else
            string connectionString = "Server=tcp:scheduledbcs6920.database.windows.net,1433;Initial Catalog = ScheduleDB;" +
                " Persist Security Info = False; User ID = cs6920;Password = Yoder1245!@; MultipleActiveResultSets = False;" +
                " Encrypt = True; TrustServerCertificate = False;Connection Timeout = 30; ";
#endif


            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }


    }
}
