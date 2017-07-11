using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Website.Models;

namespace Website.Helpers
{
    public class UserHelper
    {

        #region Add/Delete Methods

        /// <summary>
        /// Add a new user to the system
        /// </summary>
        public static bool AddUser(string username, string password, string nickname, out string id, out string message)
        {
            id = null;
            try
            {
                //Create unique Id 
                id = Guid.NewGuid().ToString();

                //Initialize database connection
                DatabaseHelper dbHelper = new DatabaseHelper();

                //Open database connection
                if (!dbHelper.OpenConnection(out message))
                    return false;

                string commandName = @"CheckIfUserExists";
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@username", username);

                //Execute
                DataTable valid1 = dbHelper.ExecuteDataQueryStoredProcedure(commandName, parameters, out message);
                if (valid1.Rows.Count != 0)
                    throw new Exception(message);

                //Prepare command variables
                commandName = @"AddUser";
                parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@id", id);
                parameters[1] = new SqlParameter("@username", username);
                parameters[2] = new SqlParameter("@password", password);
                parameters[3] = new SqlParameter("@nickname", nickname);

                //Execute
                bool valid = dbHelper.ExecuteNonQueryStoredProcedure(commandName, parameters, out message);
                if (!valid)
                    throw new Exception(message);

                //Close database connection
                dbHelper.CloseConnection(out message);

                //Return
                message = "User added successfully";
                return true;
            }
            catch (Exception exception)
            {
                message = exception.Message;
                return false;
            }
        }

        /// <summary>
        /// Delete user from the system
        /// </summary>
        public static bool DeleteUser(string id, string password, out string message)
        {
            try
            {

                //Initialize database connection
                DatabaseHelper dbHelper = new DatabaseHelper();

                //Open database connection
                if (!dbHelper.OpenConnection(out message))
                    return false;

                string commandName = @"CheckIfIDExists";
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@id", id);

                //Execute
                DataTable valid1 = dbHelper.ExecuteDataQueryStoredProcedure(commandName, parameters, out message);
                if (valid1.Rows.Count == 0)
                    throw new Exception(message);

                commandName = @"GetUser";
                parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@id", id);

                //Execute
                DataTable table = dbHelper.ExecuteDataQueryStoredProcedure(commandName, parameters, out message);
                if (table == null || table.Rows.Count == 0)
                    throw new Exception(message);

                //Parse data received
                DataRow row = table.Rows[0];
                string checkPassword = row["password"] as string;

                if (!password.Equals(checkPassword))
                {
                    message = "Password incorrect.";
                    return false;
                }

                //Prepare command variables
                commandName = @"DeleteUser";
                parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@id", id);
                parameters[1] = new SqlParameter("@password", password);

                //Execute
                bool valid = dbHelper.ExecuteNonQueryStoredProcedure(commandName, parameters, out message);
                if (!valid)
                    throw new Exception(message);

                //Close database connection
                dbHelper.CloseConnection(out message);

                //Return
                message = "User deleted successfully.";
                return true;
            }
            catch (Exception exception)
            {
                message = exception.Message;
                return false;
            }
        }

        #endregion

        #region Retrieve Methods

        /// <summary>
        /// Get the information about a specific user
        /// </summary>
        public static UserModel GetUser(string id, out string message)
        {
            try
            {
                //Initialize database connection
                DatabaseHelper dbHelper = new DatabaseHelper();

                //Open database connection
                if (!dbHelper.OpenConnection(out message))
                    return null;

                //Prepare command variables
                string commandName = @"GetUser";
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@id", id);

                //Execute
                DataTable table = dbHelper.ExecuteDataQueryStoredProcedure(commandName, parameters, out message);
                if (table == null || table.Rows.Count == 0)
                    throw new Exception(message);

                //Close database connection
                dbHelper.CloseConnection(out message);

                //Parse data received
                DataRow row = table.Rows[0];
                string username = row["username"] as string;
                string nickname = row["nickname"] as string;
                message = "Information retrieved successfully";
                return new UserModel(id, username, nickname);
            }
            catch (Exception exception)
            {
                message = exception.Message;
                return null;
            }
        }

        /// <summary>
        /// Get information about all alpaca
        /// </summary>
        public static List<UserModel> GetAllUsers(out string message)
        {
            try
            {
                //Initialize database connection
                DatabaseHelper dbHelper = new DatabaseHelper();

                //Open database connection
                if (!dbHelper.OpenConnection(out message))
                    return null;

                //Prepare command variables
                string commandName = @"GetAllUsers";
                SqlParameter[] parameters = new SqlParameter[0];

                //Execute
                DataTable table = dbHelper.ExecuteDataQueryStoredProcedure(commandName, parameters, out message);
                if (table == null)
                    throw new Exception(message);

                //Close database connection
                dbHelper.CloseConnection(out message);

                //Parse data received
                List<UserModel> users = new List<UserModel>();
                foreach (DataRow row in table.Rows)
                {
                    string id = row["id"] as string;
                    string username = row["username"] as string;
                    string nickname = row["nickname"] as string;
                    users.Add(new UserModel(id, username, nickname));
                }

                message = "Information retrieved successfully";
                return users;
            }
            catch (Exception exception)
            {
                message = exception.Message;
                return null;
            }
        }

        #endregion

        #region Edit Methods

        /// <summary>
        /// Update the nickname of an existing user
        /// </summary>
        public static bool EditNickname(string id, string nickname, out string message)
        {
            try
            {
                //Initialize database connection
                DatabaseHelper dbHelper = new DatabaseHelper();

                //Open database connection
                if (!dbHelper.OpenConnection(out message))
                    return false;

                //Prepare command variables
                string commandName = @"EditNickname";
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@id", id);
                parameters[1] = new SqlParameter("@nickname", nickname);

                //Execute
                bool valid = dbHelper.ExecuteNonQueryStoredProcedure(commandName, parameters, out message);
                if (!valid)
                    throw new Exception(message);

                //Close database connection
                dbHelper.CloseConnection(out message);

                //Return
                message = "Executed successfully";
                return true;
            }
            catch (Exception exception)
            {
                message = exception.Message;
                return false;
            }
        }

        /// <summary>
        /// Delete user from the system
        /// </summary>
        public static bool UpdatePassword(string id, string currentPassword, string newPassword, out string message)
        {
            try
            {

                //Initialize database connection
                DatabaseHelper dbHelper = new DatabaseHelper();

                //Open database connection
                if (!dbHelper.OpenConnection(out message))
                    return false;

                string commandName = @"CheckIfIDExists";
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@id", id);

                //Execute
                DataTable valid1 = dbHelper.ExecuteDataQueryStoredProcedure(commandName, parameters, out message);
                if (valid1.Rows.Count == 0)
                    throw new Exception(message);

                commandName = @"GetUser";
                parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@id", id);

                //Execute
                DataTable table = dbHelper.ExecuteDataQueryStoredProcedure(commandName, parameters, out message);
                if (table == null || table.Rows.Count == 0)
                    throw new Exception(message);

                //Parse data received
                DataRow row = table.Rows[0];
                string checkPassword = row["password"] as string;

                if (!currentPassword.Equals(checkPassword))
                {
                    message = "Password incorrect.";
                    return false;
                }

                //Prepare command variables
                commandName = @"UpdatePassword";
                parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@id", id);
                parameters[1] = new SqlParameter("@password", newPassword);

                //Execute
                bool valid = dbHelper.ExecuteNonQueryStoredProcedure(commandName, parameters, out message);
                if (!valid)
                    throw new Exception(message);

                //Close database connection
                dbHelper.CloseConnection(out message);

                //Return
                message = "Password updated successfully.";
                return true;
            }
            catch (Exception exception)
            {
                message = exception.Message;
                return false;
            }
        }

        #endregion

    }
}