using Website.Helpers;
using Website.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace Website.Webservices
{
    /// <summary>
    /// Webservices for project Touhou
    /// </summary>
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class UserWebservices : System.Web.Services.WebService
    {
        #region Add/Delete Methods

        /// <summary>
        /// Add a new user to the system
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string AddUser(string username, string password, string nickname)
        {
            StringBuilder JSON = new StringBuilder();

            string message = null;
            string id = null;

            bool valid = UserHelper.AddUser(username, password, nickname, out id, out message);
            if (valid)
            {
                JSON.Append("{");
                JSON.Append("\"success\":true,");
                JSON.Append("\"id\"");
                JSON.Append(":");
                JSON.Append(JsonConvert.SerializeObject(id));
                JSON.Append("}");
            }
            else
            {
                JSON.Append("{");
                JSON.Append("\"success\":false,");
                JSON.Append("\"message\"");
                JSON.Append(":");
                JSON.Append(JsonConvert.SerializeObject(message));
                JSON.Append("}");
            }

            return JSON.ToString();
        }

        /// <summary>
        /// Delete an user from the system
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string DeleteUser(string id, string password)
        {
            StringBuilder JSON = new StringBuilder();

            string message = null;

            bool valid = UserHelper.DeleteUser(id, password, out message);
            if (valid)
            {
                JSON.Append("{");
                JSON.Append("\"success\":true,");
                JSON.Append("\"message\"");
                JSON.Append(":");
                JSON.Append(JsonConvert.SerializeObject(message));
                JSON.Append("}");
            }
            else
            {
                JSON.Append("{");
                JSON.Append("\"success\":false,");
                JSON.Append("\"message\"");
                JSON.Append(":");
                JSON.Append(JsonConvert.SerializeObject(message));
                JSON.Append("}");
            }

            return JSON.ToString();
        }
        #endregion

        #region Get/Update Methods


        /// <summary>
        /// Get the information about a specific user
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void GetUser(string id)
        {
            StringBuilder JSON = new StringBuilder();
            string message;

            UserModel user = UserHelper.GetUser(id, out message);
            if (user != null)
            {
                JSON.Append("{");
                JSON.Append("\"success\":true,");
                JSON.Append("\"data\":");
                JSON.Append("{");
                JSON.Append("\"user\":" + JsonConvert.SerializeObject(user));
                JSON.Append("},");
                JSON.Append("\"message\"");
                JSON.Append(":");
                JSON.Append(JsonConvert.SerializeObject(message));
                JSON.Append("}");
            }
            else
            {
                JSON.Append("{");
                JSON.Append("\"success\":false,");
                JSON.Append("\"message\"");
                JSON.Append(":");
                JSON.Append(JsonConvert.SerializeObject(message));
                JSON.Append("}");
            }

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(JSON.ToString());
        }

        /// <summary>
        /// Get information about all users
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void GetAllUsers()
        {
            StringBuilder JSON = new StringBuilder();
            string message;

            List<UserModel> users = UserHelper.GetAllUsers(out message);
            if (users != null)
            {
                JSON.Append("{");
                JSON.Append("\"success\":true,");
                JSON.Append("\"data\":");
                JSON.Append("[");
                foreach (UserModel user in users)
                {
                    if (user != null)
                    {
                        JSON.Append("{");
                        JSON.Append("\"user\":" + JsonConvert.SerializeObject(user));
                        JSON.Append("}");
                    }
                    else
                    {
                        JSON.Append("{");
                        JSON.Append("\"message\"");
                        JSON.Append(":");
                        JSON.Append(JsonConvert.SerializeObject(message));
                        JSON.Append("}");
                    }

                    if (users.Last() != user)
                        JSON.Append(",");
                }
                JSON.Append("],");
                JSON.Append("\"message\"");
                JSON.Append(":");
                JSON.Append(JsonConvert.SerializeObject(message));
                JSON.Append("}");
            }
            else
            {
                JSON.Append("{");
                JSON.Append("\"success\":false,");
                JSON.Append("\"message\"");
                JSON.Append(":");
                JSON.Append(JsonConvert.SerializeObject(message));
                JSON.Append("}");
            }

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(JSON.ToString());
        }

        #endregion

        #region Edit Methods

        /// <summary>
        /// Update the nickname of an existing user
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string EditNickname(string id, string nickname)
        {
            StringBuilder JSON = new StringBuilder();

            string message;

            bool valid = UserHelper.EditNickname(id, nickname, out message);
            if (valid)
            {
                JSON.Append("{");
                JSON.Append("\"success\":true,");
                JSON.Append("\"id\"");
                JSON.Append(":");
                JSON.Append(JsonConvert.SerializeObject(id));
                JSON.Append("}");
            }
            else
            {
                JSON.Append("{");
                JSON.Append("\"success\":false,");
                JSON.Append("\"message\"");
                JSON.Append(":");
                JSON.Append(JsonConvert.SerializeObject(message));
                JSON.Append("}");
            }

            return JSON.ToString();
        }

        /// <summary>
        /// Update the password of an existing user
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string UpdatePassword(string id, string currentPassword, string newPassword)
        {
            StringBuilder JSON = new StringBuilder();

            string message;

            bool valid = UserHelper.UpdatePassword(id, currentPassword, newPassword, out message);
            if (valid)
            {
                JSON.Append("{");
                JSON.Append("\"success\":true,");
                JSON.Append("\"message\"");
                JSON.Append(":");
                JSON.Append(JsonConvert.SerializeObject(message));
                JSON.Append("}");
            }
            else
            {
                JSON.Append("{");
                JSON.Append("\"success\":false,");
                JSON.Append("\"message\"");
                JSON.Append(":");
                JSON.Append(JsonConvert.SerializeObject(message));
                JSON.Append("}");
            }

            return JSON.ToString();
        }

        #endregion

        #region Authenticate

        /// <summary>
        /// Get the information about a specific user
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void AuthenticateUser(string username, string password)
        {
            StringBuilder JSON = new StringBuilder();
            string message;

            string id = UserHelper.AuthenticateUser(username, password, out message);
            if (id != null)
            {
                JSON.Append("{");
                JSON.Append("\"success\":true,");
                JSON.Append("\"data\":");
                JSON.Append(JsonConvert.SerializeObject(id));
                JSON.Append(",");
                JSON.Append("\"message\"");
                JSON.Append(":");
                JSON.Append(JsonConvert.SerializeObject(message));
                JSON.Append("}");
            }
            else
            {
                JSON.Append("{");
                JSON.Append("\"success\":false,");
                JSON.Append("\"message\"");
                JSON.Append(":");
                JSON.Append(JsonConvert.SerializeObject(message));
                JSON.Append("}");
            }

            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(JSON.ToString());
        }

        #endregion

    }
}
