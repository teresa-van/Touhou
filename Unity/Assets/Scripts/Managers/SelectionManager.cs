using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Scripts.Models;

public class SelectionManager : MonoBehaviour
{
    #region Singleton instance

    public static SelectionManager Instance { private set; get; }

    #endregion

    #region Variables

    public int playersReady;
    public Button ReadyOrStart;
    public int yPos;
    private PhotonView myPhotonView;
    public GameObject playerReady;

    public Image RoleCard;

    public Image Choice1;
    public Image Choice2;
    public Text Choice1Name;
    public Text Choice2Name;

    public GameObject Details1View;
    public GameObject Details2View;
    public Image Details1;
    public Image Details2;

    public Text SelectedName;
    public Image Selected;

    public PlayerModel player;

    #endregion

    #region Initialization

    void Awake()
    {
        playersReady = 1;
        PhotonNetwork.isMessageQueueRunning = true;
    }

    void Start()
    {
        //Reference this instance as singleton instance
        Instance = this;
        player = new PlayerModel(PhotonNetwork.player.ID, PhotonNetwork.player.NickName, "Character", "Role", 4, 1, 1, 4);

        playerReady = PhotonNetwork.Instantiate("Player(Selection)", Vector3.zero, Quaternion.identity, 0);
        myPhotonView = playerReady.GetComponent<PhotonView>();
        yPos = (325 - (85 * (PhotonNetwork.player.ID - 1)));

        if (PhotonNetwork.player.IsMasterClient)
        {
            List<string> tempRoles = RolesManager.Instance.ShuffleRoles();
            List<List<string>> tempCharacters = CharacterManager.Instance.ShuffleCharacters();

            string roles = JsonConvert.SerializeObject(tempRoles);
            myPhotonView.RPC("SetRoles", PhotonTargets.All, roles);
            string characters = JsonConvert.SerializeObject(tempCharacters);
            myPhotonView.RPC("SetCharacters", PhotonTargets.All, characters);
        }

        SetButton();
    }

    public void Uh()
    {
        myPhotonView.RPC("InstantiateText", PhotonTargets.All, myPhotonView.owner, yPos);
        player.Role = myPhotonView.GetComponent<PlayerSelectMethods>().role;
        if (player.Role.Equals("Heroine"))
        {
            player.Health = 5;
            player.MaxHandSize = 16;
        }

        Sprite role = Resources.Load<Sprite>("Role Cards/" + myPhotonView.GetComponent<PlayerSelectMethods>().role);
        RoleCard.sprite = role;
        RoleCard.preserveAspect = true;

        Sprite choice1 = Resources.Load<Sprite>("Characters(UI)/" + myPhotonView.GetComponent<PlayerSelectMethods>().choices[0]);
        Sprite details1 = Resources.Load<Sprite>("Character Cards/" + myPhotonView.GetComponent<PlayerSelectMethods>().choices[0]);

        Choice1.sprite = choice1;
        Details1.sprite = details1;
        Choice1.preserveAspect = true;
        Details1.preserveAspect = true;
        Choice1Name.text = myPhotonView.GetComponent<PlayerSelectMethods>().choices[0];

        Sprite choice2 = Resources.Load<Sprite>("Characters(UI)/" + myPhotonView.GetComponent<PlayerSelectMethods>().choices[1]);
        Sprite details2 = Resources.Load<Sprite>("Character Cards/" + myPhotonView.GetComponent<PlayerSelectMethods>().choices[1]);

        Choice2.sprite = choice2;
        Details2.sprite = details2;
        Choice2.preserveAspect = true;
        Details2.preserveAspect = true;
        Choice2Name.text = myPhotonView.GetComponent<PlayerSelectMethods>().choices[1];
    }

    void SetButton()
    {
        if (PhotonNetwork.player.IsMasterClient)
        {
            ReadyOrStart.transform.Find("Text").GetComponent<Text>().text = "START";
            ReadyOrStart.interactable = false;
        }
        else
        {
            ReadyOrStart.transform.Find("Text").GetComponent<Text>().text = "READY";
        }
    }

    #endregion

    void Update()
    {
        //if (playersReady >= PhotonNetwork.playerList.Length && PhotonNetwork.player.IsMasterClient) ReadyOrStart.interactable = true;
        if (playersReady < PhotonNetwork.playerList.Length && PhotonNetwork.player.IsMasterClient) ReadyOrStart.interactable = false;
        else if (Selected.sprite.name == "Unchosen") ReadyOrStart.interactable = false;
        else ReadyOrStart.interactable = true;
    }

    #region Character Details

    public void DisplayDetails1()
    {
        Details1View.SetActive(true);
    }

    public void DisplayDetails2()
    {
        Details2View.SetActive(true);
    }

    public void CloseDetails()
    {
        Details1View.SetActive(false);
        Details2View.SetActive(false);
    }

    #endregion

    #region Select/Readying Methods

    public void SelectCharacter(Button buttonPressed)
    {
        Selected.preserveAspect = true;
        if (buttonPressed.name.Equals("Choose1"))
        {
            Sprite choice1 = Resources.Load<Sprite>("Characters(UI)/" + myPhotonView.GetComponent<PlayerSelectMethods>().choices[0]);
            Selected.sprite = choice1;
            SelectedName.text = myPhotonView.GetComponent<PlayerSelectMethods>().choices[0];
            player.Character = myPhotonView.GetComponent<PlayerSelectMethods>().choices[0];
        }
        else if (buttonPressed.name.Equals("Choose2"))
        {
            Sprite choice2 = Resources.Load<Sprite>("Characters(UI)/" + myPhotonView.GetComponent<PlayerSelectMethods>().choices[1]);
            Selected.sprite = choice2;
            SelectedName.text = myPhotonView.GetComponent<PlayerSelectMethods>().choices[1];
            player.Character = myPhotonView.GetComponent<PlayerSelectMethods>().choices[1];
        }
    }

    public void PlayerReady()
    {
        if (!PhotonNetwork.player.IsMasterClient)
        {
            myPhotonView.RPC("UpdateText", PhotonTargets.All, myPhotonView.owner);
        }
        else
        {
            myPhotonView.RPC("StartGame", PhotonTargets.All);
        }
    }

    public void StartGame()
    {
        string playerString = JsonConvert.SerializeObject(player);
        myPhotonView.RPC("InstantiatePlayer", PhotonTargets.All, playerString);
    }
    #endregion

    #region Photon Callbacks

    public virtual void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        if (newMasterClient == PhotonNetwork.player)
        {
            SetButton();
            myPhotonView.RPC("ChangeToMaster", PhotonTargets.All, myPhotonView.owner);
        }
    }

    public virtual void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        if (playerReady.GetComponent<PlayerSelectMethods>().ready)
            playersReady--;
    }

    #endregion
}
