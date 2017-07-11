using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Website.Helpers
{
    public class DatabaseHelper
    {

        #region Connection String

        /// <summary>
        /// Returns the current active connection string in the web.config
        /// </summary>
        /// <returns></returns>
        private static string GetConnectionString() 
        {
            try
            {
                return SystemConfigurationHelper.Database_ConnectionString;
            }
            catch
            {
                return null;
            }
        }

        #endregion


        
        #region Constructors

        public DatabaseHelper(string ConnectionString)  
        {
            this.connection = new SqlConnection(ConnectionString);
        }
        public DatabaseHelper() 
            : this(GetConnectionString())                   
        {

        }

        #endregion

        #region Variables

        private SqlConnection connection;

        #endregion

        #region Properties

        /// <summary>
        /// Checks if the current connection is opened
        /// </summary>
        public bool IsOpened
        {
            get
            {
                return State == ConnectionState.Open;
            }
        }

        /// <summary>
        /// Checks if the current connection is closed
        /// </summary>
        public bool IsClosed
        {
            get
            {
                return State == ConnectionState.Closed;
            }
        }

        /// <summary>
        /// Gets the connection state
        /// </summary>
        public ConnectionState State
        {
            get
            {
                if (connection == null)
                    return ConnectionState.Closed;
                return connection.State;
            }
        }

        #endregion

        #region Connection Methods

        /// <summary>
        /// This method tries to open a connection with the database
        /// </summary>
        /// <param name="Message"></param>
        /// <returns>boolean whether the connection was opened or not</returns>
        public bool OpenConnection(out string Message)
        {
            try
            {
                if (!IsOpened)
                    connection.Open();

                Message = "Opened successfully";
                return true;
            }
            catch (Exception exception)
            {
                Message = exception.Message;
                return false;
            }
        }

        /// <summary>
        /// This method tries to close the connection with the database
        /// </summary>
        /// <param name="Message"></param>
        /// <returns>boolean whether the connection was closed or not</returns>
        public bool CloseConnection(out string Message)
        {
            try
            {
                if (!IsClosed)
                    connection.Close();

                Message = "Closed successfully";
                return true;
            }
            catch (Exception exception)
            {
                Message = exception.Message;
                return false;
            }
        }

        #endregion

        #region Stored Procedure Methods

        /// <summary>
        /// This method executes a stored procedure and returns a boolean variable stating whether it was successful or not
        /// </summary>
        /// <param name="StoredProcedure"></param>
        /// <param name="Parameters"></param>
        /// <param name="Message">This variable contains the error message; if any; or the successful message</param>
        /// <returns>boolean whether successfully executed or not</returns>
        public bool ExecuteNonQueryStoredProcedure(string StoredProcedure, SqlParameter[] Parameters, out string Message)
        {
            try
            {
                SqlCommand command = new SqlCommand(StoredProcedure, this.connection);
                command.Parameters.AddRange(Parameters);
                command.CommandType = CommandType.StoredProcedure;

                int count = command.ExecuteNonQuery();

                Message = "Executed Successfully: " + count.ToString("N0") + " rows affected";
                return true;
            }
            catch (Exception exception)
            {
                Message = exception.Message;
                return false;
            }
        }

        /// <summary>
        /// This methods exectures a stored procedure and returns a datatable with all the returned rows
        /// </summary>
        /// <param name="StoredProcedure"></param>
        /// <param name="Parameters"></param>
        /// <param name="Message"></param>
        /// <returns>datatabe with all the rows that are returned</returns>
        public DataTable ExecuteDataQueryStoredProcedure(string StoredProcedure, SqlParameter[] Parameters, out string Message)
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(StoredProcedure, this.connection);
                DataTable table = new DataTable();
                adapter.SelectCommand.Parameters.AddRange(Parameters);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.Fill(table);
                adapter.Dispose();

                Message = "Executed successfully";
                return table;
            }
            catch (Exception exception)
            {
                Message = exception.Message;
                return null;
            }
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// This method executes a command and returns a boolean variable stating whether it was successful or not
        /// </summary>
        /// <param name="StoredProcedure"></param>
        /// <param name="Parameters"></param>
        /// <param name="Message">This variable contains the error message; if any; or the successful message</param>
        /// <returns>boolean whether successfully executed or not</returns>
        public bool ExecuteNonQueryCommand(string CommandText, SqlParameter[] Parameters, out string Message)
        {
            try
            {
                SqlCommand command = new SqlCommand(CommandText, this.connection);
                command.Parameters.AddRange(Parameters);

                int count = command.ExecuteNonQuery();

                Message = "Executed Successfully: " + count.ToString("N0") + " rows affected";
                return true;
            }
            catch (Exception exception)
            {
                Message = exception.Message;
                return false;
            }
        }

        /// <summary>
        /// This methods exectures a command and returns a datatable with all the returned rows
        /// </summary>
        /// <param name="StoredProcedure"></param>
        /// <param name="Parameters"></param>
        /// <param name="Message"></param>
        /// <returns>datatabe with all the rows that are returned</returns>
        public DataTable ExecuteDataQueryCommand(string CommandText, SqlParameter[] Parameters, out string Message)
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(CommandText, this.connection);
                adapter.SelectCommand.Parameters.AddRange(Parameters);
                DataTable table = new DataTable();
                adapter.Fill(table);
                adapter.Dispose();

                Message = "Executed successfully";
                return table;
            }
            catch (Exception exception)
            {
                Message = exception.Message;
                return null;
            }
        }

        #endregion



        #region Static Stored Procedure Methods

        /// <summary>
        /// This method executes a stored procedure and returns a boolean variable stating whether it was successful or not
        /// </summary>
        /// <param name="StoredProcedure"></param>
        /// <param name="Parameters"></param>
        /// <param name="Message">This variable contains the error message; if any; or the successful message</param>
        /// <returns>boolean whether successfully executed or not</returns>
        public static bool Static_ExecuteNonQueryStoredProcedure(string StoredProcedure, SqlParameter[] Parameters, out string Message)
        {
            try
            {
                DatabaseHelper controller = new DatabaseHelper();
                if (!controller.OpenConnection(out Message))
                    return false;

                bool valid = controller.ExecuteNonQueryStoredProcedure(StoredProcedure, Parameters, out Message);

                string closeMessage = null;
                controller.CloseConnection(out closeMessage);
                return valid;
            }
            catch (Exception exception)
            {
                Message = exception.Message;
                return false;
            }
        }

        /// <summary>
        /// This methods exectures a stored procedure and returns a datatable with all the returned rows
        /// </summary>
        /// <param name="StoredProcedure"></param>
        /// <param name="Parameters"></param>
        /// <param name="Message"></param>
        /// <returns>datatabe with all the rows that are returned</returns>
        public static DataTable Static_ExecuteDataQueryStoredProcedure(string StoredProcedure, SqlParameter[] Parameters, out string Message)
        {
            DatabaseHelper controller = new DatabaseHelper();
            if (!controller.OpenConnection(out Message))
                return null;

            DataTable table = controller.ExecuteDataQueryStoredProcedure(StoredProcedure, Parameters, out Message);

            string closeMessage = null;
            controller.CloseConnection(out closeMessage);
            return table;
        }

        #endregion

        #region Static Command Methods

        /// <summary>
        /// This method executes a command and returns a boolean variable stating whether it was successful or not
        /// </summary>
        /// <param name="StoredProcedure"></param>
        /// <param name="Parameters"></param>
        /// <param name="Message">This variable contains the error message; if any; or the successful message</param>
        /// <returns>boolean whether successfully executed or not</returns>
        public static bool Static_ExecuteNonQueryCommand(string StoredProcedure, SqlParameter[] Parameters, out string Message)
        {
            try
            {
                DatabaseHelper controller = new DatabaseHelper();
                if (!controller.OpenConnection(out Message))
                    return false;

                bool valid = controller.ExecuteNonQueryCommand(StoredProcedure, Parameters, out Message);

                string closeMessage = null;
                controller.CloseConnection(out closeMessage);
                return valid;
            }
            catch (Exception exception)
            {
                Message = exception.Message;
                return false;
            }
        }

        /// <summary>
        /// This methods exectures a command and returns a datatable with all the returned rows
        /// </summary>
        /// <param name="StoredProcedure"></param>
        /// <param name="Parameters"></param>
        /// <param name="Message"></param>
        /// <returns>datatabe with all the rows that are returned</returns>
        public static DataTable Static_ExecuteDataQueryCommand(string StoredProcedure, SqlParameter[] Parameters, out string Message)
        {
            DatabaseHelper controller = new DatabaseHelper();
            if (!controller.OpenConnection(out Message))
                return null;

            DataTable table = controller.ExecuteDataQueryCommand(StoredProcedure, Parameters, out Message);

            string closeMessage = null;
            controller.CloseConnection(out closeMessage);
            return table;
        }

        #endregion

    }
}