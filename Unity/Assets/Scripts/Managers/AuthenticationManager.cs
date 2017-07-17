using BestHTTP;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Models.Auth;
using Scripts.Models;
using UnityEngine.SceneManagement;

public class AuthenticationManager : MonoBehaviour {

    #region Singleton instance

    public static AuthenticationManager Instance { private set; get; }

    #endregion

    #region Variables

    /// <summary>
    /// User information
    /// </summary>
    public UserModel User;

    /// <summary>
    /// State whether the user is logged in or not
    /// </summary>
    public bool IsLoggedIn
    {
        get { return this.User != null; }
    }

    /// <summary>
    /// Username input field
    /// </summary>
    public InputField usernameField;

    /// <summary>
    /// Password input field
    /// </summary>
    public InputField passwordField;

    /// <summary>
    /// Path of file for saving user information 
    /// </summary>
    public string userInfoPath;

    public GameObject IncorrectPassword;
    public GameObject IncPCanvas;
    public GameObject LoginMenu;

    #endregion

    #region Initialization

    /// <summary>
    /// Execute once when the scene starts
    /// </summary>
    void Start()
    {
        //Reference this instance as singleton instance
        AuthenticationManager.Instance = this;

        //Setting the full path to the file where we will save and load user information
        userInfoPath = Path.Combine(Application.persistentDataPath, "UserInformation.txt");

        IncorrectPassword = GameObject.Find("IncorrectPassword").gameObject;
        IncPCanvas = IncorrectPassword.transform.Find("Canvas").gameObject;
        LoginMenu = GameObject.Find("LoginMenu/Panel").gameObject;
    }

    #endregion

    #region Authentication

    /// <summary>
    /// Login when the user clicks the login button
    /// </summary>
    public void Login()
    {
        string username = usernameField.text;
        string password = passwordField.text;

        AuthenticateUser(username, password);
    }

    /// <summary>
    /// Logout when the user clicks the logout button
    /// </summary>
    public void Logout()
    {
        SaveUserInfoFile(null);
    }

    /// <summary>
    /// Authenticate user through webservice
    /// </summary>
    private void AuthenticateUser(string username, string password)
    {
        HTTPRequest request = UserHelper.GetRequest_AuthenticateUser(username, password);
        StartCoroutine(ProcessAuthenticateUser(request));
    }

    /// <summary>
    /// Execute the request and process the resposnse
    /// </summary>
    private IEnumerator ProcessAuthenticateUser(HTTPRequest request)
    {

        //Execute the request
        request.Send();

        //Wait for reply
        yield return StartCoroutine(request);

        //Parse the response
        ServiceResponseModel<string> response = UserHelper.ParseResponse_AuthenticateUser(request.Response.DataAsText);

        //If authenticated, get user information
        if (response.Success)
        {
            request = UserHelper.GetRequest_GetUserInformation(response.Data);

            //Execute the request
            request.Send();

            //Wait for reply
            yield return StartCoroutine(request);

            //Parse the response
            ServiceResponseModel<UserModel> userInformationResponse = UserHelper.ParseResponse_GetUserInformation(request.Response.DataAsText);

            //Information retrieved successfully
            if (userInformationResponse.Success)
            {
                GeneralGameManager.Instance.LoadScene("Scenes/Main");
                Debug.Log("User information retrieved successfully");
                this.User = userInformationResponse.Data;
            }
            //Error while getting user information
            else
            {
                Debug.Log("Failed to retrieve user information");
            }
        }
        //Not authenticated
        else
        {
            IncPCanvas.SetActive(true);
            LoginMenu.SetActive(false);

            Debug.Log("Authentication failed.");
        }
    }

    public void CloseIncorrectPassword()
    {
        IncPCanvas.SetActive(false);
        LoginMenu.SetActive(true);
    }

    #endregion

    #region Save/Load Methods

    /// <summary>
    /// Saves user info as a JSON string  
    /// </summary>
    public void SaveUserInfoFile(UserModel user)
    {
        //If user is null, delete the file (if exists)
        if (user == null)
        {
            File.Delete(userInfoPath);
        }
        //Save the file with user information
        else
        {
            //Check if file doesnt exist, then create it
            if (!(File.Exists(userInfoPath)))
                File.Create(userInfoPath).Dispose();

            //Then save it
            string userJson = JsonConvert.SerializeObject(user);
            File.WriteAllText(userInfoPath, userJson);

            //Reference it
            this.User = user;
        }
    }

    /// <summary>
    /// Reads user info from a file
    /// </summary>
    public bool LoadUserInfoFile()
    {
        //Check if file exists
        if (!(File.Exists(userInfoPath)))
        {
            this.User = null;
            return false;
        }

        //If it does, load the data from the file
        string userJson = File.ReadAllText(userInfoPath);

        //Decode and reference
        UserModel user = JsonConvert.DeserializeObject<UserModel>(userJson);
        this.User = user;
        return true;
    }

    #endregion
}
