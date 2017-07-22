using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateReadyText : MonoBehaviour {

    public Text readyText;
    public bool ready = false;

    [PunRPC]
    public void UpdateText()
    {
        if (!PhotonNetwork.player.IsMasterClient)
        {
            if (ready)
            {
                readyText.text = "READY"; SelectionManager.Instance.playersReady++;
            }
            else
            {
                readyText.text = ""; SelectionManager.Instance.playersReady++;
            }
        }
    }

}
