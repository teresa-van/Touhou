using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerManager : Photon.PunBehaviour {

    #region Singleton instance

    public static ServerManager Instance { private set; get; }

    #endregion

    #region Variables

    /// <summary>
    /// This client's version number. Users are separated from each other by gameversion (which allows you to make breaking changes).
    /// </summary>
    private string GameVersion = "1";
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
        // #Critical
        // we don't join the lobby. There is no need to join a lobby to get the list of rooms.
        PhotonNetwork.autoJoinLobby = false;

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
            if (ConnectionStatusText.text.Equals("ConnectedToMaster"))
            {
                StatusMenu.SetActive(false);
                LobbyMenu.SetActive(true);
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("DemoAnimator/Launcher: OnConnectedToMaster() was called by PUN");
    }


    public override void OnDisconnectedFromPhoton()
    {
        Debug.LogWarning("DemoAnimator/Launcher: OnDisconnectedFromPhoton() was called by PUN");
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Start the connection process. 
    /// - If already connected, we attempt joining a random room
    /// - if not yet connected, Connect this application instance to Photon Cloud Network
    /// </summary>
    public void Connect()
    {
        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.connected)
        {
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnPhotonRandomJoinFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.ConnectUsingSettings(GameVersion);
        }
    }

    public void JoinLobby()
    {
        PhotonNetwork.ConnectUsingSettings("1.0");
        StatusMenu.SetActive(true);
        Buttons.SetActive(false);
    }

    public void LeaveLobby()
    {
        PhotonNetwork.Disconnect();
        Buttons.SetActive(true);
        LobbyMenu.SetActive(false);
    }
    #endregion
}