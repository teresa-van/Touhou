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

    public int playersReady;
    public Button ReadyOrStart;
    public int yPos;
    private PhotonView myPhotonView;
    public Dictionary<PhotonPlayer, int> positions = new Dictionary<PhotonPlayer, int>();
    public bool ready;
    public GameObject playerReady;

    #endregion

    #region Initialization

    void Awake()
    {
        playersReady = 1;
        PhotonNetwork.isMessageQueueRunning = true;
    }

    void Start()
    {
        //Reference this instance as singleton instance
        Instance = this;

        playerReady = PhotonNetwork.Instantiate("Player(Selection)", Vector3.zero, Quaternion.identity, 0);
        myPhotonView = playerReady.GetComponent<PhotonView>();

        yPos = (320 - (80 * (PhotonNetwork.player.ID - 1)));
        myPhotonView.RPC("InstantiateText", PhotonTargets.All, myPhotonView.owner, yPos);

        SetButton();
    }

    void SetButton()
    {
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
        print(playersReady + "<- PLAYERS READY");
        if (playersReady >= PhotonNetwork.playerList.Length && PhotonNetwork.player.IsMasterClient) ReadyOrStart.interactable = true;
        else if (playersReady < PhotonNetwork.playerList.Length && PhotonNetwork.player.IsMasterClient) ReadyOrStart.interactable = false;
    }

    public void ReadyUp()
    {
        if (!PhotonNetwork.player.IsMasterClient)
        {
            myPhotonView.RPC("UpdateText", PhotonTargets.All, myPhotonView.owner);
        }
        else
        {
            print("START GAME BUTTON CLICKED. INSERT FUNCTIONALITY HERE..");
        }
    }

    public virtual void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        if (newMasterClient == PhotonNetwork.player)
        {
            SetButton();
            myPhotonView.RPC("ChangeToMaster", PhotonTargets.All, myPhotonView.owner);
        }
    }

    public virtual void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        if (playerReady.GetComponent<UpdateReadyText>().ready)
            playersReady--;
    }
}
