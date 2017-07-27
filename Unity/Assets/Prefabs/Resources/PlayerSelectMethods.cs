using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Scripts.Models;

public class PlayerSelectMethods : MonoBehaviour {

    public GameObject ViewArea;
    public GameObject playerReady;
    public Text readyText;
    public bool ready = false;
    public string role;
    public List<string> choices;
    public string character;

    #region Initialization

    void Awake()
    {
        ViewArea = GameObject.Find("Menu/ViewArea").gameObject;
        playerReady = this.gameObject;
    }

    [PunRPC]
    public void InstantiateText(PhotonPlayer player, int yPos)
    {
        playerReady.transform.SetParent(ViewArea.transform);
        playerReady.transform.localPosition = Vector3.zero;
        playerReady.transform.localScale = Vector3.one;
        playerReady.transform.localPosition = new Vector3(-410, yPos, 0);
        Text readyText = playerReady.transform.Find("Ready").GetComponent<Text>();
        Text nicknameText = playerReady.transform.Find("Nickname").GetComponent<Text>();
        nicknameText.text = player.NickName;

        role = RolesManager.roles[player.ID];
        choices = CharacterManager.characters[player.ID];

        if (player.IsMasterClient) readyText.text = "MASTER";
        else readyText.text = "";
    }

    #endregion

    #region Update Methods

    [PunRPC]
    public void ChangeToMaster(PhotonPlayer player)
    {
        if (player.IsMasterClient) readyText.text = "MASTER";
    }

    [PunRPC]
    public void UpdateText(PhotonPlayer player)
    {
        if (!player.IsMasterClient)
        {
            if (!ready)
            {
                readyText.text = "READY"; SelectionManager.Instance.playersReady++; 
                ready = true;
            }
            else
            {
                readyText.text = ""; SelectionManager.Instance.playersReady--; 
                ready = false;
            }
        }
    }

    #endregion

    #region Roles/Character Methods

    [PunRPC]
    public void SetRoles(string roles)
    {
        List<string> tempRoles = JsonConvert.DeserializeObject<List<string>>(roles);
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            RolesManager.roles.Add(i+1, tempRoles[i]);
        }
    }

    [PunRPC]
    public void SetCharacters(string characters)
    {
        List<List<string>> tempCharacters = JsonConvert.DeserializeObject<List<List<string>>>(characters);
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            CharacterManager.characters.Add(i + 1, tempCharacters[i]);
        }
        SelectionManager.Instance.Uh();
    }
    #endregion

    #region Start Game Methods

    [PunRPC]
    public void StartGame()
    {
        SelectionManager.Instance.StartGame();
    }

    [PunRPC]
    public void InstantiatePlayer(string playerString)
    {
        PlayerModel player = JsonConvert.DeserializeObject<PlayerModel>(playerString);
        PlayerManager.Instance.players.Add(player);
        GeneralManager.Instance.LoadScene("Scenes/Game", true);
    }
    #endregion
}