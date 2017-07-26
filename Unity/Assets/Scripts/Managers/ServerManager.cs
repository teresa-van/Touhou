using UnityEngine;
using UnityEngine.SceneManagement;

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

    #endregion

    #region Etc

    public void HideAllMenus()
    {
        MainUIManager.Instance.StatusMenu.SetActive(false);
        MainUIManager.Instance.LobbyMenu.SetActive(false);
        MainUIManager.Instance.CreateRoomMenu.SetActive(false);
        MainUIManager.Instance.RoomMenu.SetActive(false);
        MainUIManager.Instance.Buttons.SetActive(false);
        MainUIManager.Instance.ErrorMenu.SetActive(false);
    }

    public void OKErrorButtonClicked()
    {
        HideAllMenus();
        MainUIManager.Instance.RoomMenu.SetActive(true);
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
            if (SceneManager.GetActiveScene().name.Equals("Main"))
                MainUIManager.Instance.ConnectionStatusText.text = ClientStateCache.ToString();
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
        if (SceneManager.GetActiveScene().name.Equals("Main"))
        {
            HideAllMenus();
            MainUIManager.Instance.StatusMenu.SetActive(true);
        }
    }

    public virtual void OnJoinedLobby()
    {
        if (SceneManager.GetActiveScene().name.Equals("Main"))
        {
            HideAllMenus();
            MainUIManager.Instance.LobbyMenu.SetActive(true);
        }
    }

    public void LeaveLobby()
    {
        if (SceneManager.GetActiveScene().name.Equals("Main"))
        {
            HideAllMenus();
            MainUIManager.Instance.StatusMenu.SetActive(true);
            MainUIManager.Instance.Buttons.SetActive(true);
        }
        PhotonNetwork.Disconnect();
    }

    public virtual void OnLeftLobby()
    {
        if (SceneManager.GetActiveScene().name.Equals("Main"))
            MainUIManager.Instance.StatusMenu.SetActive(false);
    }
    #endregion

    #region Room Methods

    public void JoinRoom(RoomInfo room)
    {
        if (SceneManager.GetActiveScene().name.Equals("Main"))
            MainUIManager.Instance.RoomTitle.text = room.Name;
        PhotonNetwork.JoinRoom(room.Name);
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void OpenCreateRoomMenu()
    {
        if (SceneManager.GetActiveScene().name.Equals("Main"))
        {
            HideAllMenus();
            MainUIManager.Instance.CreateRoomMenu.SetActive(true);
            MainUIManager.Instance.RoomName.text = "Touhou Game";
        }
    }

    public void CreateRoom()
    {
        if (SceneManager.GetActiveScene().name.Equals("Main"))
        {
            HideAllMenus();
            MainUIManager.Instance.StatusMenu.SetActive(true);
            MainUIManager.Instance.RoomTitle.text = MainUIManager.Instance.RoomName.text;
            PhotonNetwork.CreateRoom(MainUIManager.Instance.RoomName.text, new RoomOptions() { MaxPlayers = 8 }, null);
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public virtual void OnLeftRoom()
    {
        if (SceneManager.GetActiveScene().name.Equals("Main"))
            HideAllMenus();
    }

    public virtual void OnJoinedRoom()
    {
        if (SceneManager.GetActiveScene().name.Equals("Main"))
        {
            HideAllMenus();
            MainUIManager.Instance.RoomMenu.SetActive(true);
        }
    }
    #endregion

    #region Start Game Methods

    public void StartGameClicked()
    {
        if (PhotonNetwork.playerList.Length <= 0)
        {
            HideAllMenus();
            MainUIManager.Instance.ErrorMenu.SetActive(true);
        }
        else
        {
            Debug.Log("START GAME CLICKED. INSERT FUNCTIONALITY HERE.");
            GeneralGameManager.Instance.LoadScene("Scenes/Selection", true);
            PhotonNetwork.isMessageQueueRunning = false;
        }
    }

    #endregion
}