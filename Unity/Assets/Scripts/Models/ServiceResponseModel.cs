using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Scripts.Models
{
    [DataContract]
    public class ServiceResponseModel<T>
    {

        #region Constructors

        public ServiceResponseModel()
        {

        }

        public ServiceResponseModel(bool success, string message,
            T data)
        {
            this.Success = success;
            this.Message = message;
            this.Data = data;
        }

        #endregion

        #region Properties

        /// <summary>
        /// State whether the response represents success or failure 
        /// </summary>
        [DataMember]
        public bool Success { set; get; }

        /// <summary>
        /// Message explaining this response
        /// </summary>
        [DataMember]
        public string Message { set; get; }

        /// <summary>
        /// The data that this response is returning
        /// </summary>
        [DataMember]
        public T Data { set; get; }

        #endregion

    }
}
