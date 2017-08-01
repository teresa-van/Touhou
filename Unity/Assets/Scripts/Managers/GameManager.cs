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
    public List<Card> Deck;
    public List<Card> Hand;

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
        Deck = new List<Card>();

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

        yPos = (210 - (77 * (turnOrder.IndexOf(PhotonNetwork.player.ID))));

        spriteX = (250 * Math.Cos( (turnOrder.IndexOf(PhotonNetwork.player.ID) * (360 / PhotonNetwork.playerList.Length)) * (Math.PI / 180) )) + 80;
        spriteY = (120 * Math.Sin( (turnOrder.IndexOf(PhotonNetwork.player.ID) * (360 / PhotonNetwork.playerList.Length)) * (Math.PI / 180) )) + 125;

        if (PhotonNetwork.player.IsMasterClient)
        {
            CreateDeck();
            Deck = GeneralManager.Instance.Shuffle(Deck);
            string deck = JsonConvert.SerializeObject(Deck);
            myPhotonView.RPC("DealCards", PhotonTargets.All, deck);
        }
    }

    public void Uh()
    {
        myPhotonView.RPC("InstantiateUI", PhotonTargets.All, myPhotonView.owner, yPos, spriteX, spriteY);
    }

    public void CreateDeck()
    {
        //Shoots
        for (int i = 0; i < 24; i++)
        {
            string season = "";
            if (0 <= i && i < 8) season = "Spring";
            else if (8 <= i && i < 13) season = "Summer";
            else if (13 <= i && i < 19) season = "Fall";
            else if (19 <= i && i < 24) season = "Winter";
            Deck.Add(new Card("Shoot", season, new List<string> { "Danmaku", "Action" }));
        }

        //Grazes
        for (int i = 0; i < 12; i++)
        {
            string season = "";
            if (0 <= i && i < 3) season = "Spring";
            else if (3 <= i && i < 6) season = "Summer";
            else if (6 <= i && i < 9) season = "Fall";
            else if (9 <= i && i < 12) season = "Winter";
            Deck.Add(new Card("Graze", season, new List<string> { "Reaction", "Dodge" }));
        }

        //Spiritual Attacks
        for (int i = 0; i < 6; i++)
        {
            string season = "";
            if (0 <= i && i < 2) season = "Summer";
            else if (2 <= i && i < 4) season = "Fall";
            else if (i == 4) season = "Winter";
            else if (i == 5) season = "Spring";
            Deck.Add(new Card("Spiritual Attack", season, new List<string> { "Invocation" }));
        }

        //Powers
        for (int i = 0; i < 6; i++)
        {
            string season = "";
            if (0 <= i && i < 2) season = "Winter";
            else if (2 <= i && i < 4) season = "Fall";
            else if (i == 4) season = "Summer";
            else if (i == 5) season = "Spring";
            Deck.Add(new Card("Power", season, new List<string> { "Item", "Powerup" }));
        }

        //Seal Aways
        Deck.Add(new Card("Seal Away", "Spring", new List<string> { "Danmaku", "Action" }));
        Deck.Add(new Card("Seal Away", "Summer", new List<string> { "Danmaku", "Action" }));
        Deck.Add(new Card("Seal Away", "Winter", new List<string> { "Danmaku", "Action" }));
        Deck.Add(new Card("Seal Away", "Fall", new List<string> { "Danmaku", "Action" }));

        //Bombs
        Deck.Add(new Card("Bombs", "Spring", new List<string> { "Invocation", "Reaction" }));
        Deck.Add(new Card("Bombs", "Summer", new List<string> { "Invocation", "Reaction" }));
        Deck.Add(new Card("Bombs", "Winter", new List<string> { "Invocation", "Reaction" }));
        Deck.Add(new Card("Bombs", "Fall", new List<string> { "Invocation", "Reaction" }));

        //Focus'
        Deck.Add(new Card("Focus", "Fall", new List<string> { "Shield", "Item" }));
        Deck.Add(new Card("Focus", "Summer", new List<string> { "Shield", "Item" }));
        Deck.Add(new Card("Focus", "Winter", new List<string> { "Shield", "Item" }));

        //1UPs
        Deck.Add(new Card("1UP", "Winter", new List<string> { "Action", "Reaction", "Healing" }));
        Deck.Add(new Card("1UP", "Fall", new List<string> { "Action", "Reaction", "Healing" }));

        //Borrows
        Deck.Add(new Card("Borrow", "Spring", new List<string> { "Action" }));
        Deck.Add(new Card("Borrow", "Summer", new List<string> { "Action" }));

        //Supernatural Borders
        Deck.Add(new Card("Supernatural Border", "Spring", new List<string> { "Powerup", "Shield", "Item" }));
        Deck.Add(new Card("Supernatural Border", "Spring", new List<string> { "Powerup", "Shield", "Item" }));

        //Grimoires
        Deck.Add(new Card("Grimoire", "Spring", new List<string> { "Action" }));
        Deck.Add(new Card("Grimoire", "Summer", new List<string> { "Action" }));

        //Kourindous
        Deck.Add(new Card("Kourindou", "Winter", new List<string> { "Action" }));
        Deck.Add(new Card("Kourindou", "Fall", new List<string> { "Action" }));

        //ETC
        Deck.Add(new Card("Capture Spell Card", "Winter", new List<string> { "Invocation" }));
        Deck.Add(new Card("Laser Shot", "Summer", new List<string> { "Danmaku", "Action" }));
        Deck.Add(new Card("Last Word", "Summer", new List<string> { "Danmaku", "Action" }));
        Deck.Add(new Card("Melee", "Winter", new List<string> { "Danmaku", "Action" }));
        Deck.Add(new Card("Master Plan", "Fall", new List<string> { "Action" }));
        Deck.Add(new Card("Tempest", "Summer", new List<string> { "Action" }));
        Deck.Add(new Card("Party", "Spring", new List<string> { "Action" }));
        Deck.Add(new Card("Voile", "Fall", new List<string> { "Action" }));
        Deck.Add(new Card("Mini-Hakkero", "Summer", new List<string> { "Item", "Artifact" }));
        Deck.Add(new Card("Sorcerer's Sutra Scroll", "Fall", new List<string> { "Item", "Artifact" }));
        Deck.Add(new Card("Stopwatch", "Winter", new List<string> { "Item", "Artifact" }));
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
