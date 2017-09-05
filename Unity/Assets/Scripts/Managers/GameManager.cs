using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Scripts.Models;
using System.Linq;
using System;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    #region Singleton instance

    public static GameManager Instance { private set; get; }

    #endregion

    #region Variables
    private List<int> turnOrder;
    private int currentTurn;

    private List<GameObject> HandVisuals;
    public GameObject TurnIndicator;
    public Button EndTurnButton;

    public List<Card> Deck;
    public Text DeckSize;
    public GameObject CardPrefab;
    public GameObject HandParent;

    private PhotonView myPhotonView;

    public GameObject playerUI;
    public GameObject playerSprite;

    private int yPos;
    private double spriteX;
    private double spriteY;

    public Text RoleText;
    public Text CharacterText;
    public Text Range;
    public Text Distance;
    public Image Icon;
    public Image Health;

    private PlayerModel playerModel;
    private PlayerModel heroine;
    public bool dragging = false;
    public bool draggable = false;
    #endregion

    #region Initialization

    void Awake()
    {
        turnOrder = new List<int>();
        int count = 1;

        Sequence s = DOTween.Sequence();
        s.Append(TurnIndicator.transform.DOLocalMoveX(-265, 0.5f));
        s.Insert(0, TurnIndicator.transform.DOScaleX(0.85f, 0.5f));
        s.Append(TurnIndicator.transform.DOLocalMoveX(-270, 0.5f));
        s.Insert(0.5f, TurnIndicator.transform.DOScaleX(1, 0.5f));
        s.SetLoops(-1);

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
        HandVisuals = new List<GameObject>();
        currentTurn = 0;

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

            foreach (PlayerModel player in PlayerManager.Instance.players)
            {
                string pm = JsonConvert.SerializeObject(player);
                myPhotonView.RPC("DrawToMaxHand", PhotonTargets.All, player.MaxHandSize, pm);
                myPhotonView.RPC("UpdateDeck", PhotonTargets.All, player.MaxHandSize);
            }

            myPhotonView.RPC("UpdateHandVisuals", PhotonTargets.All);
        }

        TurnIndicator.SetActive(true);
    }

    public void Uh()
    {
        myPhotonView.RPC("InstantiateUI", PhotonTargets.All, myPhotonView.owner, yPos, spriteX, spriteY);
    }

    public void Fuck(int max)
    {
        myPhotonView.RPC("Fuck", PhotonTargets.All, max);
    }

    public void UpdateHandVisuals()
    {
        print("HERE!!");
        print(myPhotonView.gameObject.GetComponent<PlayerMethods>().Nickname.text + " " + 
            myPhotonView.gameObject.GetComponent<PlayerMethods>().Hand[0].Name + " " + 
            myPhotonView.gameObject.GetComponent<PlayerMethods>().Hand[1].Name + " " +
            myPhotonView.gameObject.GetComponent<PlayerMethods>().Hand[2].Name + " " +
            myPhotonView.gameObject.GetComponent<PlayerMethods>().Hand[3].Name);

        int count = 0;
        int y = 0;
        int x = 0;
        int x2 = 0;
        if (myPhotonView.gameObject.GetComponent<PlayerMethods>().Hand.Count < 8)
        {
            if (myPhotonView.gameObject.GetComponent<PlayerMethods>().Hand.Count % 2 == 0) x = -45 * (myPhotonView.gameObject.GetComponent<PlayerMethods>().Hand.Count - 1);
            else x = -90 * ((myPhotonView.gameObject.GetComponent<PlayerMethods>().Hand.Count - 1) / 2);
        }
        else
        {
            int overflow = myPhotonView.gameObject.GetComponent<PlayerMethods>().Hand.Count - 8;
            if (overflow % 2 == 0) x2 = -45 * (overflow - 1);
            else x2 = -90 * ((overflow - 1) / 2);
            x = -315;
        }

        foreach (Card card in myPhotonView.gameObject.GetComponent<PlayerMethods>().Hand)
        {
            print(card.Name);
            GameObject cardPrefab = Instantiate(CardPrefab);
            cardPrefab.name = card.Name + "(" + card.Season + ")";
            Sprite cardType = Resources.Load<Sprite>("Cards/" + cardPrefab.name);
            cardPrefab.GetComponent<Image>().sprite = cardType;
            HandVisuals.Add(cardPrefab);

            if (count >= 8)
            {
                count = 0; y = -115; x = x2;
            }
            cardPrefab.transform.SetParent(HandParent.transform);
            cardPrefab.transform.localPosition = new Vector3(x + (90 * count), y, 0);
            count++; 
        }
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
        Deck.Add(new Card("Bomb", "Spring", new List<string> { "Invocation", "Reaction" }));
        Deck.Add(new Card("Bomb", "Summer", new List<string> { "Invocation", "Reaction" }));
        Deck.Add(new Card("Bomb", "Winter", new List<string> { "Invocation", "Reaction" }));
        Deck.Add(new Card("Bomb", "Fall", new List<string> { "Invocation", "Reaction" }));

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

        DeckSize.text = Deck.Count.ToString();
        print(Deck.Count + " <- Initial");
    }

    #endregion

    void Update()
    {
        if (turnOrder[currentTurn] == PhotonNetwork.player.ID)
        {
            myPhotonView.RPC("MoveTurnIndicator", PhotonTargets.All);
            draggable = true;
            EndTurnButton.interactable = true;
        }
        else
        {
            draggable = false;
            EndTurnButton.interactable = false;
        }
    }

    public void UpdateDeck(int drawn)
    {
        Deck.RemoveRange(0, drawn);
        print(Deck.Count);
        DeckSize.text = Deck.Count.ToString();
        Debug.Log("AFTER UPDATE: " + GameManager.Instance.Deck[0].Name + " " + GameManager.Instance.Deck[1].Name + " " + GameManager.Instance.Deck[2].Name + " " + GameManager.Instance.Deck[3].Name + " " + GameManager.Instance.Deck[4].Name);
    }

    public void EndTurnPressed()
    {
        myPhotonView.RPC("NextPlayer", PhotonTargets.All);
    }

    public void NextPlayer()
    {
        if (currentTurn == turnOrder.Count-1) currentTurn = 0;
        else currentTurn++;
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
