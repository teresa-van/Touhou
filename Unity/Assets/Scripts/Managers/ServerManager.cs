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

    public string PlayerName
    {
        get
        {
            return PhotonNetwork.playerName;
        }
        set
        {
            PhotonNetwork.playerName = value;
        }
    }

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

    public virtual void OnConnectedToMaster()
    {
        Debug.Log("DemoAnimator/Launcher: OnConnectedToMaster() was called by PUN");
    }


    public virtual void OnDisconnectedFromPhoton()
    {
        Debug.LogWarning("DemoAnimator/Launcher: OnDisconnectedFromPhoton() was called by PUN");
    }

    #endregion

    #region Lobby Methods

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

    #region Room Methods

    public void JoinRoom(RoomInfo room)
    {
        PhotonNetwork.JoinRoom(room.Name);
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(PhotonNetwork.playerName + "'s Room", new RoomOptions() { MaxPlayers = 8 }, null);
        Debug.Log(PhotonNetwork.playerName);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public virtual void OnLeftRoom()
    {
        Debug.Log("Left Room.");
    }

    public virtual void OnJoinedRoom()
    {
        Debug.Log("Joined Room.");
    }
    #endregion
}