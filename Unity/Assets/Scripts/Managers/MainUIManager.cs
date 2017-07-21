using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIManager : MonoBehaviour {

    #region Singleton instance

    public static MainUIManager Instance { private set; get; }

    #endregion

    #region Variables 

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

    // Use this for initialization
    void Start ()
    {
        Instance = this;
	}

}
