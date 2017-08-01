using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Scripts.Models;
using System.Linq;
using System;

public class GameManager : MonoBehaviour
{
    #region Singleton instance

    public static GameManager Instance { private set; get; }

    #endregion

    #region Variables
    public List<int> turnOrder;

    private PhotonView myPhotonView;

    public GameObject playerUI;
    public GameObject playerSprite;

    public int yPos;
    public double spriteX;
    public double spriteY;

    public Text RoleText;
    public Text CharacterText;
    public Text Range;
    public Text Distance;
    public Image Icon;
    public Image Health;

    public PlayerModel playerModel;
    public PlayerModel heroine;
    #endregion

    #region Initialization

    void Awake()
    {
        turnOrder = new List<int>();
        int count = 1;
        foreach (PlayerModel player in PlayerManager.Instance.players)
        {
            if (player.ID == PhotonNetwork.player.ID) playerModel = player;
            if (player.Role.Equals("Heroine"))
            {
                heroine = player;
                continue;
            }
            else turnOrder.Add(player.ID);
            count++;
        }
        turnOrder.Sort(); turnOrder.Insert(0, heroine.ID);
    }

    void Start()
    {
        //Reference this instance as singleton instance
        Instance = this;

        foreach (int turn in turnOrder) print(turn);

        print(playerModel);

        RoleText.text = playerModel.Role;
        CharacterText.text = playerModel.Character;
        Range.text = "RANGE: " + playerModel.Range;
        Distance.text = "DISTANCE: " + playerModel.Distance;

        Sprite icon = Resources.Load<Sprite>("Characters(Icons)/" + playerModel.Character);
        Sprite health = Resources.Load<Sprite>("Icons/health" + playerModel.Health);

        Icon.sprite = icon;
        Health.sprite = health;
        Icon.preserveAspect = true;
        Health.preserveAspect = true;

        playerUI = PhotonNetwork.Instantiate("PlayerUI", Vector3.zero, Quaternion.identity, 0);

        //playerUI.SetActive(false);
        myPhotonView = playerUI.GetComponent<PhotonView>();

        yPos = (200 - (85 * (turnOrder.IndexOf(PhotonNetwork.player.ID))));

        spriteX = (250 * Math.Cos( (turnOrder.IndexOf(PhotonNetwork.player.ID) * (360/PhotonNetwork.playerList.Length)) * (Math.PI / 180) )) + 80;
        spriteY = (110 * Math.Sin( (turnOrder.IndexOf(PhotonNetwork.player.ID) * (360 / PhotonNetwork.playerList.Length)) * (Math.PI / 180) )) + 125;

        if (PhotonNetwork.player.IsMasterClient)
        {
            myPhotonView.RPC("DealCards", PhotonTargets.All);
        }
    }

    public void Uh()
    {
        myPhotonView.RPC("InstantiateUI", PhotonTargets.All, myPhotonView.owner, yPos, spriteX, spriteY);
    }

    #endregion

    void Update()
    {
    }


    #region Photon Callbacks

    public virtual void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {

    }

    public virtual void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {

    }

    #endregion
}
