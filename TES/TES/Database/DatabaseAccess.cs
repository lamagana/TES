using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace TES
{
    public static class DatabaseAccess
    {
        private static string message;

        private static SqlConnection connection;

        static DatabaseAccess()
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                builder.DataSource = "tesprojectdatabase.database.windows.net";
                builder.UserID = "tesProjectAdmin";
                builder.Password = "!QAZ2wsx";
                builder.InitialCatalog = "tesproject";

                connection = new SqlConnection(builder.ConnectionString);
                connection.Open();
                message = "Connected to the database\n";
            }
            catch (SqlException ex)
            {
                message = MethodInfo.GetCurrentMethod().DeclaringType.Name + " " + MethodInfo.GetCurrentMethod().Name + " " + ex.Message;

                Console.WriteLine("Failed to connect to database");
                Console.WriteLine(MethodInfo.GetCurrentMethod().DeclaringType.Name + " " +
                                   MethodInfo.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        public static bool SelectUserFirstNameLastName_SQL(int pk, out string errorMessage, out string FirstName, out string LastName)
        {
            errorMessage = "";
            FirstName = "";
            LastName = "";
            try
            {
                if (connection != null)
                {
                    //Checks if user's role request already exists in the database
                    string sql = $"SELECT FirstName, LastName FROM Users WHERE UserId = '{pk}'";
                    SqlCommand command = new SqlCommand(sql, connection);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader.HasRows)
                            {
                                FirstName = reader.GetString(0);
                                LastName = reader.GetString(1);
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Selecting Role of User from the database failed";
                Console.WriteLine(MethodInfo.GetCurrentMethod().DeclaringType.Name + " " +
                                  MethodInfo.GetCurrentMethod().Name + " " + ex.Message);
                return false;
            }

            return false;
        }

        public static bool SelectRoleRequest_SQL(int pk, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (connection != null)
                {
                    //Checks if user's role request already exists in the database
                    string sql = $"SELECT * FROM RoleRequest WHERE UserId = '{pk}'";
                    SqlCommand command = new SqlCommand(sql, connection);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Selecting Role of User from the database failed";
                Console.WriteLine(MethodInfo.GetCurrentMethod().DeclaringType.Name + " " +
                                  MethodInfo.GetCurrentMethod().Name + " " + ex.Message);
                return false;
            }

            return false;
        }

        public static bool InsertRoleRequest_SQL(int role, int pk, out string errorMessage)
        {
            errorMessage = "";
            int currentRole = 0;
            try
            {
                if (connection != null)
                {
                    //Checks if user's role request already exists in the database
                    string sql = $"SELECT * FROM RoleRequest WHERE UserId = '{pk}'";
                    SqlCommand command = new SqlCommand(sql, connection);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            errorMessage = "Request already made for User.";
                            return false;
                        }
                    }

                    string sql1 = $"SELECT RoleId FROM Users WHERE UserId = {pk}";
                    SqlCommand command1 = new SqlCommand(sql1, connection);
                    using (SqlDataReader reader = command1.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader.HasRows)
                            {
                                currentRole = reader.GetInt32(0);
                            }
                            else
                            {
                                errorMessage = "No user found";
                                return false;
                            }
                        }
                    }


                    //Inserts the new role request 
                    string sql2 = $"INSERT INTO RoleRequest (UserId, CurrentRoleId, RequestedRoleId) VALUES ( {pk}, {currentRole}, {role})";
                    SqlCommand command2 = new SqlCommand(sql2, connection);
                    command2.ExecuteNonQuery();

                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                errorMessage = "Inserting New User in the database failed";
                Console.WriteLine(MethodInfo.GetCurrentMethod().DeclaringType.Name + " " +
                                  MethodInfo.GetCurrentMethod().Name + " " + ex.Message);
                return false;
            }

            return true;
        }

        public static bool InsertNewUser_SQL(string firstname, string lastname, string email, string password, out int userId, out string errorMessage)
        {
            errorMessage = "";
            userId = 0;
            if (string.IsNullOrWhiteSpace(firstname) || string.IsNullOrWhiteSpace(lastname) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                if (string.IsNullOrWhiteSpace(firstname))
                    errorMessage += "Please enter a first name.\n";
                if (string.IsNullOrWhiteSpace(lastname))
                    errorMessage += "Please enter a last name.\n";
                if (string.IsNullOrWhiteSpace(email))
                    errorMessage += "Please enter a email.\n";
                if (string.IsNullOrWhiteSpace(password))
                    errorMessage += "Please enter a password.\n";
                return false;
            }

            try
            {
                if (connection != null)
                {
                    //Checks if email already exists in the database
                    string sql1 = $"SELECT * FROM Users WHERE Email = '{email}'";
                    SqlCommand command = new SqlCommand(sql1, connection);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            errorMessage = "Email is already in use.";
                            return false;
                        }
                    }

                    //Inserts the new user 
                    string sql2 = $"INSERT INTO Users (FirstName, LastName, Password, Email, RoleId) VALUES ( '{firstname}', '{lastname}', '{password}', '{email}', 0)";
                    SqlCommand command1 = new SqlCommand(sql2, connection);
                    command1.ExecuteNonQuery();

                    string sql3 = $"SELECT UserId FROM Users WHERE Email = '{email}' AND Password = '{password}'";
                    SqlCommand command3 = new SqlCommand(sql3, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader.HasRows)
                            {
                                userId = reader.GetInt32(0);
                                return true;
                            }
                        }
                        errorMessage = "Invalid username/password.";
                    }

                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                errorMessage = "Inserting New User in the database failed";
                Console.WriteLine(MethodInfo.GetCurrentMethod().DeclaringType.Name + " " +
                                  MethodInfo.GetCurrentMethod().Name + " " + ex.Message);
                return false;
            }

            return true;
        }

        public static bool SelectUser_SQL(string email, string password, out int UserID, out int Role, out string errorMessage)
        {
            errorMessage = "";
            errorMessage += message;
            UserID = 0;
            Role = 0;

            try
            {
                if (connection != null)
                {
                    string sql = $"SELECT UserId, RoleId FROM Users WHERE Email = '{email}' AND Password = '{password}'";
                    SqlCommand command = new SqlCommand(sql, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader.HasRows)
                            {
                                UserID = reader.GetInt32(0);
                                Role = reader.GetInt32(1);
                                return true;
                            }
                        }
                        errorMessage = "Invalid username/password.";
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage += "Verifying credentials from the database failed\n";
                errorMessage += MethodInfo.GetCurrentMethod().DeclaringType.Name + " " +
                                  MethodInfo.GetCurrentMethod().Name + " " + ex.Message;
                Console.WriteLine(MethodInfo.GetCurrentMethod().DeclaringType.Name + " " +
                                  MethodInfo.GetCurrentMethod().Name + " " + ex.Message);
                return false;
            }
            return false;
        }

        public static bool SelectUsersProjects_SQL(int userId, out string errorMessage, out List<Project> list)
        {
            errorMessage = "";
            list = new List<Project>();
            try
            {

                if (connection != null)
                {
                    //string sql = $"SELECT ProjectId, Description FROM Projects";
                    //Requests all the projects that a user is a part of
                    string sql = $"Select p.ProjectId, Description " +
                                 $"FROM Users i " +
                                 $"INNER JOIN Groups_Users gu " +
                                 $"ON i.UserId = gu.UserId " +
                                 $"INNER JOIN Groups g " +
                                 $"ON gu.GroupId = g.GroupId " +
                                 $"INNER JOIN Projects p " +
                                 $"ON g.ProjectId = p.ProjectId " +
                                 $"WHERE i.UserId = {userId}";
                    SqlCommand command = new SqlCommand(sql, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int projectId = reader.GetInt32(0);
                            string description = reader.GetString(1);
                            list.Add(new Project(projectId, description));
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Failed to retrieve projects from the database";
                Console.WriteLine(MethodInfo.GetCurrentMethod().DeclaringType.Name + " " +
                                  MethodInfo.GetCurrentMethod().Name + " " + ex.Message);
                return false;
            }

            return false;

        }

        public static bool SelectProjectById_SQL(int projectId, out string errorMessage, out Project project)
        {
            errorMessage = string.Empty;
            project = null;

            try
            {
                if (connection != null)
                {
                    string sql = $"SELECT projectId, Description FROM Projects WHERE ProjectId = {projectId}";
                    SqlCommand command = new SqlCommand(sql, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int Id = reader.GetInt32(0);
                            string description = reader.GetString(1);

                            project = new Project(Id, description);
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Failed to retrieve projects from the database";
                Console.WriteLine(MethodInfo.GetCurrentMethod().DeclaringType.Name + " " +
                                  MethodInfo.GetCurrentMethod().Name + " " + ex.Message);
                return false;
            }

            return false;
        }

        public static bool InsertTime_SQL(int UserId, int ProjectId, DateTime StartTime, out string errorMessage)
        {
            // out string value that will share an error message if the query fails.
            errorMessage = "";

            // Make sure that the connection is established.
            if (connection != null)
            {
                // SQL string that will get the UserId based on the current user's ID.
                string selectSQL = $"SELECT UserID FROM Users WHERE UserId = '{UserId}'";

                // SqlCommand object to execute the SELECT statement.
                SqlCommand selectCommand = new SqlCommand(selectSQL, connection);

                // Using the SqlDataReader class, create a reader object to check returned rows.
                using (SqlDataReader reader = selectCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // If the reader object has no rows, something went wrong. Set the value of the errorMessage.
                        if (!reader.HasRows)
                        {
                            errorMessage = "User Not Found.";
                            return false;
                        }
                    }
                }

                // SQL string that will insert the start time value.
                string insertSQL = $"INSERT INTO TimeSheet (UserId, ProjectId, GroupId, StartTime) VALUES ('{UserId}', '{ProjectId}', 2, '{StartTime}')";

                // SqlCommand object to execute the INSERT statement.
                SqlCommand insertCommand = new SqlCommand(insertSQL, connection);

                // Using the SqlDataReader class, create a reader object to check returned rows.
                using (SqlDataReader reader = insertCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // If the reader object has no rows, something went wrond. Set the value of the errorMessage. 
                        if (!reader.HasRows)
                        {
                            errorMessage = "User Not Found.";
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public static bool UpdateStopTime_SQL(int UserId, int ProjectId, DateTime StopTime, out string errorMessage)
        {
            // Out string value that will display an error if the query fails.
            errorMessage = "";

            // Make sure the connection is established.
            if (connection != null)
            {
                // SQL string that will select the latest TimeId created by the user's most recent start time insert.
                string selectSQL = $"SELECT MAX(TimeSheetId) FROM TimeSheet WHERE UserId = '{UserId}' AND ProjectId = '{ProjectId}'";

                // SqlCommand object that will execute the SELECT statement.
                SqlCommand selectCommand = new SqlCommand(selectSQL, connection);

                // This will be the TimeId value retrieved by the SELECT.
                int timeId = 0;

                // USing the SqlDataReader class, create a reader object to check returned rows.
                using (SqlDataReader reader = selectCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // If the reader has no rows, something went wrong. Set the value of the errorMessage.
                        if (!reader.HasRows)
                        {
                            errorMessage = "Time Entry Not found.";
                            return false;
                        }
                        else
                        {
                            // Get the TimeId from the SELECT.
                            timeId = reader.GetInt32(0);
                        }
                    }
                }

                // SQL string that will update the user's most recent row and set the StopTime field to the stop time.
                string updateSQL = $"UPDATE TimeSheet SET StopTime = '{StopTime}' WHERE TimeSheetId = {timeId}";

                SqlCommand updateCommand = new SqlCommand(updateSQL, connection);

                using (SqlDataReader reader = updateCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // If the reader has no rows, something went wrong. Set the value of the errorMessage.
                        if (!reader.HasRows)
                        {
                            errorMessage = "Time Entry Not found.";
                            return false;
                        }

                    }
                }
            }

            return true;
        }

        public static bool InsertNewRow_SQL(int UserId, int ProjectId, out string errorMessage)
        {
            errorMessage = "";
            try
            {

                String selectSQL = $"SELECT UserId FROM Users WHERE UserId = '{UserId}'";

                // SqlCommand object to execute the SELECT statement.
                SqlCommand selectCommand = new SqlCommand(selectSQL, connection);

                // Using the SqlDataReader class, create a reader object to check returned rows.
                using (SqlDataReader reader = selectCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // If the reader object has no rows, something went wrong. Set the value of the errorMessage.
                        if (!reader.HasRows)
                        {
                            errorMessage = "User Not Found.";
                            return false;
                        }
                    }
                }


                string insertSQL = $"INSERT INTO TimeSheet (UserId, ProjectId, GroupId) VALUES ('{UserId}', '{ProjectId}', 2)";

                SqlCommand insertCommand = new SqlCommand(insertSQL, connection);

                // Using the SqlDataReader class, create a reader object to check returned rows.
                using (SqlDataReader reader = insertCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // If the reader object has no rows, something went wrong. Set the value of the errorMessage.
                        if (!reader.HasRows)
                        {
                            errorMessage = "User Not Found.";
                            return false;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(MethodInfo.GetCurrentMethod().DeclaringType.Name + " " +
                                  MethodInfo.GetCurrentMethod().Name + " " + ex.Message);
            }

            return true;
        }

        public static bool ManualAddStartTime_SQL(int TimeSheetId, DateTime StartTime, out string errorMessage)
        {
            errorMessage = "";
            try
            {

                string updateSQL = $"UPDATE TimeSheet SET StartTime = '{StartTime}'WHERE TimeSheetId = '{TimeSheetId}'";

                SqlCommand updateCommand = new SqlCommand(updateSQL, connection);

                using (SqlDataReader reader = updateCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (!reader.HasRows)
                        {
                            errorMessage = "Somthing went wrong with updating";
                            return false;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(MethodInfo.GetCurrentMethod().DeclaringType.Name + " " +
                                  MethodInfo.GetCurrentMethod().Name + " " + ex.Message);
            }

            return true;
        }

        public static bool ManualAddStopTime_SQL(int TimeSheetId, DateTime StopTime, out string errorMessage)
        {
            errorMessage = "";
            try
            {

                string updateSQL = $"UPDATE TimeSheet SET StopTime = '{StopTime}'WHERE TimeSheetId = '{TimeSheetId}'";

                SqlCommand updateCommand = new SqlCommand(updateSQL, connection);

                using (SqlDataReader reader = updateCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (!reader.HasRows)
                        {
                            errorMessage = "Somthing went wrong with updating";
                            return false;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(MethodInfo.GetCurrentMethod().DeclaringType.Name + " " +
                                  MethodInfo.GetCurrentMethod().Name + " " + ex.Message);
            }

            return true;
        }

        public static DataTable SelectUncompletedRoleRequests_SQL()
        {
            DataTable table = new DataTable();
            try
            {
                string sql = $"SELECT * FROM RoleRequest left join Users on RoleRequest.UserId = Users.UserId WHERE Completed = 'FALSE'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(table);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(MethodInfo.GetCurrentMethod().DeclaringType.Name + " " +
                                  MethodInfo.GetCurrentMethod().Name + " " + ex.Message);
            }
            return table;
        }

        public static bool AcceptRoleRequest_SQL(int RoleRequestId, int RequestedRoleId)
        {
            try
            {
                if (connection != null)
                {

                    string sql = $"UPDATE RoleRequest SET CurrentRoleId = '{RequestedRoleId}', Completed = 'TRUE' WHERE RoleRequestId = '{RoleRequestId}'";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.ExecuteNonQuery();

                    int UserId = 0;
                    string sql1 = $"SELECT UserId FROM RoleRequest WHERE RoleRequestId = {RoleRequestId}";
                    SqlCommand command1 = new SqlCommand(sql1, connection);
                    using (SqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserId = reader.GetInt32(0);
                        }
                    }

                    string sql2 = $"UPDATE Users SET RoleId = {RequestedRoleId} WHERE UserId = {UserId}";
                    SqlCommand command2 = new SqlCommand(sql2, connection);
                    command2.ExecuteNonQuery();
                    return true;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(MethodInfo.GetCurrentMethod().DeclaringType.Name + " " +
                                  MethodInfo.GetCurrentMethod().Name + " " + ex.Message);
            }
            return false;
        }

        public static bool DeclineRoleRequest_SQL(int RoleRequestId)
        {
            try
            {
                string sql = $"DELETE FROM RoleRequest WHERE RoleRequestId = '{RoleRequestId}'";
                SqlCommand command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(MethodInfo.GetCurrentMethod().DeclaringType.Name + " " +
                                  MethodInfo.GetCurrentMethod().Name + " " + ex.Message);
            }
            return false;
        }

        public static double CalculateUsersTotalTimeByProject_SQL(int UserId, int ProjectId, out string errorMessage)
        {
            errorMessage = "";
            double timeHours = -1;
            try
            {
                double totalMinutes;
                if (connection != null)
                {
                    string selectSQL = $"SELECT DATEDIFF(minute, StartTime, StopTime) AS Minutes FROM TimeSheet WHERE UserId = '{UserId}' AND ProjectId = '{ProjectId}'";

                    SqlCommand selectCommand = new SqlCommand(selectSQL, connection);

                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (!reader.HasRows)
                            {
                                errorMessage = "Somthing went wrong with getting total time";
                                return -1;
                            }

                            totalMinutes = reader.GetDouble(0);


                            timeHours = totalMinutes / 60;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(MethodInfo.GetCurrentMethod().DeclaringType.Name + " " +
                                  MethodInfo.GetCurrentMethod().Name + " " + ex.Message);
            }
            return timeHours;
        }

        public static bool CheckIfAlreadyClockedIn(int UserId, int ProjectId, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                int timeId = 0;

                if (connection != null)
                {
                    string selectIdSQL = $"SELECT MAX(TimeSheetId) FROM TimeSheet WHERE UserId = '{UserId}' AND ProjectId = '{ProjectId}'";

                    SqlCommand selectIdCommand = new SqlCommand(selectIdSQL, connection);

                    using (SqlDataReader reader = selectIdCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (!reader.HasRows)
                            {
                                errorMessage = "Cannot find entry";
                            }

                            timeId = reader.GetInt32(0);
                        }
                    }

                    string selectTimeSQL = $"SELECT StartTime, StopTime FROM TimeSheet WHERE TimeSheetId = '{timeId}'";

                    SqlCommand selectTimeCommand = new SqlCommand(selectTimeSQL, connection);

                    using (SqlDataReader reader = selectTimeCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (!reader.HasRows)
                            {
                                errorMessage = "Cannot get time";
                            }

                            if (!DBNull.Value.Equals(reader["StartTime"]) && DBNull.Value.Equals(reader["StopTime"]))
                            {
                                errorMessage = "You are already Clocked in and have not clocked out";
                                return false;
                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(MethodInfo.GetCurrentMethod().DeclaringType.Name + " " +
                                  MethodInfo.GetCurrentMethod().Name + " " + ex.Message);
            }
            return true;
        }

        public static bool CheckIfAlreadyClockedOut(int UserId, int ProjectId, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                int timeId = 0;

                if (connection != null)
                {
                    string selectIdSQL = $"SELECT MAX(TimeSheetId) FROM TimeSheet WHERE UserId = '{UserId}' AND ProjectId = '{ProjectId}'";

                    SqlCommand selectIdCommand = new SqlCommand(selectIdSQL, connection);

                    using (SqlDataReader reader = selectIdCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (!reader.HasRows)
                            {
                                errorMessage = "Cannot find entry";
                            }

                            timeId = reader.GetInt32(0);
                        }
                    }

                    string selectTimeSQL = $"SELECT StartTime, StopTime FROM TimeSheet WHERE TimeSheetId = '{timeId}'";

                    SqlCommand selectTimeCommand = new SqlCommand(selectTimeSQL, connection);

                    using (SqlDataReader reader = selectTimeCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (!reader.HasRows)
                            {
                                errorMessage = "Cannot get time";
                            }

                            if (!DBNull.Value.Equals(reader["StartTime"]) && !DBNull.Value.Equals(reader["StopTime"]))
                            {
                                errorMessage = "You have not Clocked in and cannot clock out";
                                return false;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(MethodInfo.GetCurrentMethod().DeclaringType.Name + " " +
                                  MethodInfo.GetCurrentMethod().Name + " " + ex.Message);
            }
            return true;
        }

        public static bool CheckIfTimesAreBlank(int UserId, int ProjectId, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                int timeId = 0;

                if (connection != null)
                {
                    string selectBlanksSQL = $"SELECT MAX(TimeSheetId) FROM TimeSheet WHERE UserId = {UserId} AND ProjectId = {ProjectId}";

                    SqlCommand selectBlankCommand = new SqlCommand(selectBlanksSQL, connection);

                    using (SqlDataReader reader = selectBlankCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (!reader.HasRows)
                            {
                                return false;
                            }

                            if (!reader.IsDBNull(0))
                            {
                                timeId = reader.GetInt32(0);

                            }
                        }
                    }

                    string selectTimeSQL = $"SELECT StartTime, StopTime FROM TimeSheet WHERE TimeSheetId = '{timeId}'";

                    SqlCommand selectTimeCommand = new SqlCommand(selectTimeSQL, connection);

                    using (SqlDataReader reader = selectTimeCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (!reader.HasRows)
                            {
                                errorMessage = "Cannot get time";
                            }

                            if (DBNull.Value.Equals(reader["StartTime"]) && DBNull.Value.Equals(reader["StopTime"]))
                            {
                                errorMessage = "Something went wrong";
                                return true;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(MethodInfo.GetCurrentMethod().DeclaringType.Name + " " +
                                  MethodInfo.GetCurrentMethod().Name + " " + ex.Message);
            }
            return false;
        }

        public static bool InsertTimeForBlankRow(int UserId, int ProjectId, DateTime startTime, out string errorMessage)
        {
            int timeId = 0;
            errorMessage = "";
            string selectMaxSQL = $"SELECT MAX(TimeSheetId) FROM TimeSheet WHERE UserId = '{UserId}' AND ProjectId = '{ProjectId}'";

            SqlCommand selectMaxCommand = new SqlCommand(selectMaxSQL, connection);

            using (SqlDataReader reader = selectMaxCommand.ExecuteReader())
            {
                if (reader.Read())
                {
                    if (!reader.HasRows)
                    {
                        errorMessage = "Something went wrong";
                        return false;
                    }

                    timeId = reader.GetInt32(0);
                }


            }

            string insertSQL = $"UPDATE TimeSheet SET StartTime = '{startTime}' WHERE TimeSheetId = '{timeId}'";

            SqlCommand updateCommand = new SqlCommand(insertSQL, connection);
            using (SqlDataReader evenNewerReader = updateCommand.ExecuteReader())
            {
                if (evenNewerReader.Read())
                {
                    if (!evenNewerReader.HasRows)
                    {
                        errorMessage = "something went wrong";
                        return false;
                    }
                }
            }

            return true;
        }
    }
}

