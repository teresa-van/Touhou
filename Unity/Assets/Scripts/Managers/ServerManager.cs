using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerManager : MonoBehaviour { 

    #region Singleton instance

    public static ServerManager Instance { private set; get; }

    #endregion

    #region Variables

    /// <summary>
    /// This client's version number. Users are separated from each other by gameversion (which allows you to make breaking changes).
    /// </summary>
    private string GameVersion = "1.0";
    ClientState ClientStateCache;

    public Text ConnectionStatusText;
    public GameObject StatusMenu;
    public GameObject LobbyMenu;
    public GameObject Buttons;
    #endregion

    #region CallBacks

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// </summary>
    void Awake()
    {
        PhotonNetwork.autoJoinLobby = true;
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.automaticallySyncScene = true;
    }

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    /// </summary>
    void Start()
    {
        Instance = this;
    }

    void Update()
    {
        if (ClientStateCache != PhotonNetwork.connectionStateDetailed)
        {
            ClientStateCache = PhotonNetwork.connectionStateDetailed;
            ConnectionStatusText.text = ClientStateCache.ToString();
        }
    }

    //public virtual void OnConnectedToMaster()
    //{
    //    Debug.Log("DemoAnimator/Launcher: OnConnectedToMaster() was called by PUN");
    //}


    //public virtual void OnDisconnectedFromPhoton()
    //{
    //    Debug.LogWarning("DemoAnimator/Launcher: OnDisconnectedFromPhoton() was called by PUN");
    //}

    #endregion

    #region Public Methods

    ///// <summary>
    ///// Start the connection process. 
    ///// - If already connected, we attempt joining a random room
    ///// - if not yet connected, Connect this application instance to Photon Cloud Network
    ///// </summary>
    //public void Connect()
    //{
    //    // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
    //    if (PhotonNetwork.connected)
    //    {
    //        // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnPhotonRandomJoinFailed() and we'll create one.
    //        PhotonNetwork.JoinRandomRoom();
    //    }
    //    else
    //    {
    //        // #Critical, we must first and foremost connect to Photon Online Server.
    //        PhotonNetwork.ConnectUsingSettings(GameVersion);
    //    }
    //}

    public void JoinLobby()
    {
        PhotonNetwork.ConnectUsingSettings(GameVersion);
        StatusMenu.SetActive(true);
        Buttons.SetActive(false);
    }

    public virtual void OnJoinedLobby()
    {
        StatusMenu.SetActive(false);
        LobbyMenu.SetActive(true);
    }

    public void LeaveLobby()
    {
        StatusMenu.SetActive(true);
        PhotonNetwork.Disconnect();
    }

    public virtual void OnLeftLobby()
    {
        Buttons.SetActive(true);
        StatusMenu.SetActive(false);
        LobbyMenu.SetActive(false);
    }
    #endregion
}