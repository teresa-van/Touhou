using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonHelper
{
    /// <summary>
    /// The webservice REST API URL (readonly)
    /// </summary>
    public static string WEBSERVICE_URL { get { return "http://localhost:50478/Webservices/UserWebservices.asmx/"; } }

    /// <summary>
    /// Parse the json text into a C# object
    /// </summary>
    public static T ParseJSON<T>(string jsonResponse)
    {
        T obj = JsonConvert.DeserializeObject<T>(jsonResponse);
        return obj;
    }
}
