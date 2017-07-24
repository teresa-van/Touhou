using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class PlayerSelectMethods : MonoBehaviour {

    public GameObject ViewArea;
    public GameObject playerReady;
    public Text readyText;
    public bool ready = false;

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
        playerReady.transform.localPosition = new Vector3(-385, yPos, 0);
        Text readyText = playerReady.transform.Find("Ready").GetComponent<Text>();
        Text nicknameText = playerReady.transform.Find("Nickname").GetComponent<Text>();
        Text roleText = playerReady.transform.Find("Role").GetComponent<Text>();
        nicknameText.text = player.NickName;

        print(RolesManager.roles[1] + ", " + RolesManager.roles[2] + ", " + RolesManager.roles[3] + ", " + RolesManager.roles[4]);

        roleText.text = RolesManager.roles[player.ID];
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

    #region Roles Methods

    [PunRPC]
    public void SetRoles(string roles)
    {
        List<string> tempRoles = JsonConvert.DeserializeObject<List<string>>(roles);
        int count = 0;
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            RolesManager.roles.Add(player.ID, tempRoles[count]);
            count++;
        }
        print(RolesManager.roles[1] + ", " + RolesManager.roles[2] + ", " + RolesManager.roles[3] + ", " + RolesManager.roles[4]);
    }
     
    #endregion
}
