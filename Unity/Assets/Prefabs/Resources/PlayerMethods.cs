using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Scripts.Models;
using System.Linq;

public class PlayerMethods : MonoBehaviour
{
    public PlayerModel playerModel;

    public GameObject Menu;
    public GameObject playerUI;

    public Image Icon;
    public Text Nickname;
    public Text Character;
    public Text Role;
    public Text Range;
    public Text Distance;
    public Image Health;

    public Text Hand;

    #region Initialization

    void Awake()
    {
        Menu = GameObject.Find("Menu").gameObject;
        playerUI = this.gameObject;
    }

    [PunRPC]
    public void InstantiateUI(PhotonPlayer player, int yPos, double spriteX, double spriteY)
    {
        foreach (PlayerModel model in PlayerManager.Instance.players)
            if (model.ID == player.ID) playerModel = model;

        playerUI.transform.SetParent(Menu.transform);
        playerUI.transform.localPosition = Vector3.zero;
        playerUI.transform.localScale = Vector3.one;
        playerUI.transform.localPosition = new Vector3(-465, yPos, 0);

        if (playerModel.Role.Equals("Heroine"))
        {
            Role.text = playerModel.Role;
        }
        else Role.text = "";

        Hand.text = GameManager.Instance.Hand.Count.ToString();
        Nickname.text = playerModel.Nickname;
        Character.text = playerModel.Character;
        Range.text = "RANGE: " + playerModel.Range;
        Distance.text = "DISTANCE: " + playerModel.Distance;

        Sprite icon = Resources.Load<Sprite>("Characters(Icons)/" + playerModel.Character);
        Sprite health = Resources.Load<Sprite>("Icons/health" + playerModel.Health);

        Icon.sprite = icon;
        Health.sprite = health;
        Icon.preserveAspect = true;
        Health.preserveAspect = true;

        Image playerSprite = playerUI.transform.Find("PlayerSprite").GetComponent<Image>();
        playerSprite.transform.SetParent(Menu.transform);
        playerSprite.transform.localPosition = new Vector2((float)spriteX, (float)spriteY);
        Sprite chibi = Resources.Load<Sprite>("Characters(Chibi)/" + playerModel.Character);
        playerSprite.sprite = chibi;
    }

    #endregion

    #region Update Methods

    #endregion

    #region Card Methods

    [PunRPC]
    public void DealCards(string deck)
    {
        List<Card> Deck = JsonConvert.DeserializeObject<List<Card>>(deck);
        GameManager.Instance.Deck = Deck;
        Debug.Log("FROM CREATION: " + GameManager.Instance.Deck[0].Name + " " + GameManager.Instance.Deck[1].Name + " " + GameManager.Instance.Deck[2].Name + " " + GameManager.Instance.Deck[3].Name + " " + GameManager.Instance.Deck[4].Name);
        GameManager.Instance.Uh();
    }

    [PunRPC]
    public void DrawToMaxHand(int max, string player)
    {
        GameManager.Instance.Fuck(player);
        GameManager.Instance.UpdateDeck(max);
    }

    [PunRPC]
    public void Fuck(string player)
    {
        PlayerModel pm = JsonConvert.DeserializeObject<PlayerModel>(player);
        print(pm.Nickname + " = PLAYER PASSED IN");
        print(this.Nickname.text + " = THE ACTUAL PLAYER");
        if (pm == this.playerModel)
        {
            var toDraw = GameManager.Instance.Deck.Where(card => GameManager.Instance.Deck.IndexOf(card) < this.playerModel.MaxHandSize);
            GameManager.Instance.Hand.AddRange(toDraw);
            Hand.text = GameManager.Instance.Hand.Count.ToString();
            print("STARTING HAND: " + GameManager.Instance.Hand[0].Name + " " + GameManager.Instance.Hand[1].Name + " " + GameManager.Instance.Hand[2].Name + " " + GameManager.Instance.Hand[3].Name);
        }
    }

    #endregion
}
