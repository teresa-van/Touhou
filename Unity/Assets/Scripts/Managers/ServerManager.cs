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

    public Text RoomTitle;
    public Text ConnectionStatusText;
    public GameObject StatusMenu;
    public GameObject LobbyMenu;
    public GameObject CreateRoomMenu;
    public GameObject RoomMenu;
    public GameObject Buttons;
    public GameObject ErrorMenu;

    public Button StartGameButton;
    public InputField RoomName;
    #endregion

    #region Etc

    public void HideAllMenus()
    {
        StatusMenu.SetActive(false);
        LobbyMenu.SetActive(false);
        CreateRoomMenu.SetActive(false);
        RoomMenu.SetActive(false);
        Buttons.SetActive(false);
        ErrorMenu.SetActive(false);
    }

    public void OKErrorButtonClicked()
    {
        HideAllMenus();
        RoomMenu.SetActive(true);
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
        PhotonNetwork.playerName = AuthenticationManager.Instance.User.NickName;
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
        HideAllMenus();
        StatusMenu.SetActive(true);
    }

    public virtual void OnJoinedLobby()
    {
        HideAllMenus();
        LobbyMenu.SetActive(true);
    }

    public void LeaveLobby()
    {
        HideAllMenus();
        StatusMenu.SetActive(true);
        Buttons.SetActive(true);
        PhotonNetwork.Disconnect();
    }

    public virtual void OnLeftLobby()
    {
        StatusMenu.SetActive(false);
    }
    #endregion

    #region Room Methods

    public void JoinRoom(RoomInfo room)
    {
        RoomTitle.text = room.Name;
        PhotonNetwork.JoinRoom(room.Name);
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void OpenCreateRoomMenu()
    {
        HideAllMenus();
        CreateRoomMenu.SetActive(true);
    }

    public void CreateRoom()
    {
        HideAllMenus();
        StatusMenu.SetActive(true);
        RoomTitle.text = RoomName.text;
        PhotonNetwork.CreateRoom(RoomName.text, new RoomOptions() { MaxPlayers = 8 }, null);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public virtual void OnLeftRoom()
    {
        HideAllMenus();
    }

    public virtual void OnJoinedRoom()
    {
        HideAllMenus();
        RoomMenu.SetActive(true);
    }
    #endregion

    #region Start Game Methods

    public void StartGameClicked()
    {
        if (PhotonNetwork.playerList.Length <= 0)
        {
            HideAllMenus();
            ErrorMenu.SetActive(true);
        }
        else
        {
            Debug.Log("START GAME CLICKED. INSERT FUNCTIONALITY HERE.");
            PhotonNetwork.LoadLevel("Scenes/Selection");
        }
    }

    #endregion
}