﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Scripts.Models;
using System.Linq;
using System.Collections;
using DG.Tweening;

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

    public Text HandText;
    public List<string> test;
    public List<Card> Hand;

    private GameObject info;
    #region Initialization

    void Awake()
    {
        Menu = GameObject.Find("Menu").gameObject;
        playerUI = this.gameObject;
        Hand = new List<Card>();
        test = new List<string>();
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

        HandText.text = Hand.Count.ToString();
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
        playerSprite.material.SetFloat("_GlowScale", 0);
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
    public void UpdateDeck(int drawn)
    {
        GameManager.Instance.UpdateDeck(drawn);
    }

    [PunRPC]
    public void DrawToMaxHand(int max, string player)
    {
        //GameManager.Instance.Fuck(player, max);
        PlayerModel pm = JsonConvert.DeserializeObject<PlayerModel>(player);

        print(pm.Nickname + " = PLAYER PASSED IN");
        print(PhotonNetwork.player.NickName + " = THE ACTUAL PLAYER");
        print(pm.Nickname.Equals(PhotonNetwork.player.NickName));

        if (pm.Nickname.Equals(PhotonNetwork.player.NickName))
        {
            print(PhotonNetwork.player.NickName + " draws starting hand.");
            GameManager.Instance.Fuck(max);
        }
    }

    [PunRPC]
    public void Fuck(int max)
    {
        print(playerModel.Nickname + " INSIDE RPC CALL");
        var toDraw = GameManager.Instance.Deck.Take(playerModel.MaxHandSize);                                               
        Hand.AddRange(toDraw);
        foreach (Card c in toDraw) test.Add(c.Name);
        HandText.text = Hand.Count.ToString();
        print("STARTING HAND: " + Hand[0].Name + " " + Hand[1].Name + " " + Hand[2].Name + " " + Hand[3].Name);
    }

    [PunRPC]
    public void UpdateHandVisuals()
    {
        GameManager.Instance.UpdateHandVisuals();
    }
    #endregion

    #region Turn Methods
    [PunRPC]
    public void MoveTurnIndicator()
    {
        Sequence s = DOTween.Sequence();
        s.Append(GameManager.Instance.TurnIndicator.transform.DOLocalMoveY(playerUI.transform.localPosition.y, 0.75f));
    }

    [PunRPC]
    public void NextPlayer()
    {
        GameManager.Instance.NextPlayer();
    }
    #endregion

    #region Non-RPC Methods
    public void ShowCharacterInfo(GameObject icon)
    {
        info = new GameObject();
        info.transform.SetParent(Menu.transform);
        info.AddComponent<Image>();
        info.GetComponent<Image>().preserveAspect = true;
        Sprite infoCard = Resources.Load<Sprite>("Character Cards/" + icon.GetComponent<Image>().sprite.name);
        info.GetComponent<Image>().sprite = infoCard;
        info.AddComponent<Outline>();
        info.GetComponent<Outline>().effectDistance = new Vector2(0.5f, 0.5f);

        info.transform.position = icon.transform.position;
        info.transform.position += new Vector3(5.5f, 0, 0);
        info.transform.localScale = new Vector3(4, 4, 4);
    }

    public void HideCharacterInfo()
    {
        Destroy(info);
    }
    #endregion
}
