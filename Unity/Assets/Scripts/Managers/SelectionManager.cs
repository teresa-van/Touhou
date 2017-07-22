using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    #region Singleton instance

    public static SelectionManager Instance { private set; get; }

    #endregion

    #region Variables

    public GameObject PlayerSelectionPrefab;
    public GameObject ViewArea;
    public int playersReady = 1;
    public bool ready = false;
    //public Dictionary<int, GameObject> players = new Dictionary<int, GameObject>();
    public Button ReadyOrStart;

    public GameObject playerReady;
    private PhotonView myPhotonView;

    #endregion

    #region Initialization

    void Start()
    {
        //Reference this instance as singleton instance
        Instance = this;
        float spacing = (ViewArea.transform.localScale.y * 768) / PhotonNetwork.playerList.Length;
        int count = 0;
        //foreach(PhotonPlayer player in PhotonNetwork.playerList)
        //{
        //    GameObject playerReady = PhotonNetwork.Instantiate("Player(Selection)", Vector3.zero, Quaternion.identity, 0);
        //    players.Add(player.ID, playerReady);
        //    playerReady.transform.SetParent(ViewArea.transform);
        //    playerReady.transform.localPosition = Vector3.zero; 
        //    playerReady.transform.localScale = Vector3.one;
        //    playerReady.transform.localPosition = new Vector3(-385, spacing * count, 0);
        //    Text readyText = playerReady.transform.Find("Ready").GetComponent<Text>();
        //    Text nicknameText = playerReady.transform.Find("Nickname").GetComponent<Text>();
        //    nicknameText.text = player.NickName;
        //    if (player.IsMasterClient) readyText.text = "MASTER";
        //    else readyText.text = "";
        //    count++;
        //}
        playerReady = PhotonNetwork.Instantiate("Player(Selection)", Vector3.zero, Quaternion.identity, 0);
        //players.Add(player.ID, playerReady);
        playerReady.transform.SetParent(ViewArea.transform);
        playerReady.transform.localPosition = Vector3.zero;
        playerReady.transform.localScale = Vector3.one;
        playerReady.transform.localPosition = new Vector3(-385, spacing * count, 0);
        Text readyText = playerReady.transform.Find("Ready").GetComponent<Text>();
        Text nicknameText = playerReady.transform.Find("Nickname").GetComponent<Text>();
        nicknameText.text = PhotonNetwork.player.NickName;
        if (PhotonNetwork.player.IsMasterClient) readyText.text = "MASTER";
        else readyText.text = "";
        myPhotonView = playerReady.GetComponent<PhotonView>();

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
        myPhotonView.RPC("UpdateReadyText", PhotonTargets.All);
        if (playersReady == PhotonNetwork.playerList.Length && PhotonNetwork.player.IsMasterClient) ReadyOrStart.interactable = true; 
    }

    public void ReadyUp()
    {
        if (!PhotonNetwork.player.IsMasterClient)
        {
            if (ready)
                ready = false;
            else
                ready = true;
        }
        else
        {
            print("START GAME BUTTON CLICKED. INSERT FUNCTIONALITY HERE..");
        }
    }

}
