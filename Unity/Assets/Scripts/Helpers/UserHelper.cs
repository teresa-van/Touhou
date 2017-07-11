using BestHTTP;
using Scripts.Models.Auth;
using Scripts.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserHelper
{

    /// <summary>
    /// Get HTTPRequest for authenticating user
    /// </summary>
    public static HTTPRequest GetRequest_AuthenticateUser(string username, string password)
    {
        //Create request
        HTTPRequest request = new HTTPRequest(new Uri(CommonHelper.WEBSERVICE_URL + "AuthenticateUser"), HTTPMethods.Post);
        request.AddField("username", username);
        request.AddField("password", password);
        return request;
    }

    /// <summary>
    /// Parse the service response json text for authenticating user
    /// </summary>
    public static ServiceResponseModel<string> ParseResponse_AuthenticateUser(string serviceResponseJson)
    {
        try
        {
            return CommonHelper.ParseJSON<ServiceResponseModel<string>>(serviceResponseJson);
        }
        catch (Exception exception)
        {
            return new ServiceResponseModel<string>(false, exception.Message, null);
        }
    }

    /// <summary>
    /// Get HTTPRequest for getting user information
    /// </summary>
    public static HTTPRequest GetRequest_GetUserInformation(string id)
    {
        //Create request
        HTTPRequest request = new HTTPRequest(new Uri(CommonHelper.WEBSERVICE_URL + "GetUser"), HTTPMethods.Post);
        request.AddField("id", id);
        return request;
    }

    /// <summary>
    /// Parse the service response json text for getting user information
    /// </summary>
    public static ServiceResponseModel<UserModel> ParseResponse_GetUserInformation(string serviceResponseJson)
    {
        try
        {
            return CommonHelper.ParseJSON<ServiceResponseModel<UserModel>>(serviceResponseJson);
        }
        catch (Exception exception)
        {
            return new ServiceResponseModel<UserModel>(false, exception.Message, null);
        }
    }

}
