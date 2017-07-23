using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateReadyText : MonoBehaviour {

    public GameObject ViewArea;
    public GameObject playerReady;
    public Text readyText;
    public bool ready = false;

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
        nicknameText.text = player.NickName;
        if (player.IsMasterClient) readyText.text = "MASTER";
        else readyText.text = "";
    }

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

}
