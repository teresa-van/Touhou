﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Scripts.Models.Auth
{
    public class UserModel
    {

        #region Constructors

        public UserModel()
            : this("N/A", "Username", "Nickname")
        {

        }

        public UserModel(string id,
            string username, string nickname)
        {
            this.ID = id;

            this.UserName = username;
            this.NickName = nickname;
        }

        #endregion

        #region Properties

        /// <summary>
        /// GUID generated by our system for this user
        /// </summary>
        [DataMember]
        public string ID { set; get; }



        /// <summary>
        /// The username/email address that will be used to login to the system
        /// </summary>
        [DataMember]
        public string UserName { set; get; }

        /// <summary>
        /// The nick name of the user
        /// </summary>
        [DataMember]
        public string NickName { set; get; }

        #endregion

    }
}
