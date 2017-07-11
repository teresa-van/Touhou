using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Models
{
    public class UserModel
    {

        #region Constructors

        public UserModel(string id,
            string username, string nickname)
        {
            this.ID = id;

            this.Username = username;
            this.Nickname = nickname;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Unique Id of the user
        /// </summary>
        public string ID { set; get; }

        /// <summary>
        /// Username
        /// </summary>
        public string Username { set; get; }

        /// <summary>
        /// User's nickname
        /// </summary>
        public string Nickname { set; get; }

        #endregion

    }
}