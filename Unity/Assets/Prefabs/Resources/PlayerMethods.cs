using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Scripts.Models;

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
            Hand.text = playerModel.MaxHandSize.ToString();
        }
        else Role.text = "";
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
        GameManager.Instance.Uh();
    }
    #endregion
}
