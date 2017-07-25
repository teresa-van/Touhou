using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class RolesManager : MonoBehaviour
{
    #region Singleton instance
    public static RolesManager Instance { private set; get; }
    #endregion

    #region Roles 

    public string Heroine = "Heroine";

    public string Rival = "Rival";

    public List<string> StageBosses = new List<string>
        {
            //4
            "Stage Boss",
            "Stage Boss",
            "Stage Boss",
            //5
            "Final Boss",
            "Challenger",
            "Anti-Heroine"
        };

    public List<string> Partners = new List<string>
        {
            //54
            "Partner",
            "Partner",
            "Ex Midboss",
            //7
            "One True Partner"
        };

    public List<string> ExBosses = new List<string>
        {
            //4
            "Ex Boss",
            "Phantom Boss"
        };

    #endregion

    public static Dictionary<int, string> roles = new Dictionary<int, string>();

    void Awake()
    {
        //Reference this instance as singleton instance
        Instance = this;
    }

    public List<string> ShuffleRoles()
    {
        List<string> tempRoles = new List<string>();
        List<string> tempSB = Shuffle(StageBosses);
        List<string> tempXB = Shuffle(ExBosses);
        if (PhotonNetwork.playerList.Length == 4)
        {
            tempRoles.Add("Heroine");
            tempRoles.Add("Stage Boss");
            tempRoles.Add("Stage Boss");
            tempRoles.Add(tempXB[0]);
        }
        else if (PhotonNetwork.playerList.Length == 5)
        {
            List<string> tempP = Shuffle(Partners.GetRange(0, 3));
            tempRoles.Add("Heroine");
            tempRoles.Add(tempSB[0]);
            tempRoles.Add(tempSB[1]);
            tempRoles.Add(tempXB[0]);
            tempRoles.Add(tempP[0]);
        }
        else if (PhotonNetwork.playerList.Length == 6)
        {
            List<string> tempP = Shuffle(Partners.GetRange(0, 3));
            tempRoles.Add("Heroine");
            tempRoles.Add(tempSB[0]);
            tempRoles.Add(tempSB[1]);
            tempRoles.Add(tempSB[2]);
            tempRoles.Add(tempXB[0]);
            tempRoles.Add(tempP[0]);
        }
        else if (PhotonNetwork.playerList.Length == 7)
        {
            List<string> tempP = Shuffle(Partners);
            tempRoles.Add("Heroine");
            tempRoles.Add(tempSB[0]);
            tempRoles.Add(tempSB[1]);
            tempRoles.Add(tempSB[2]);
            tempRoles.Add(tempXB[0]);
            tempRoles.Add(tempP[0]);
            tempRoles.Add(tempP[1]);
        }
        else if (PhotonNetwork.playerList.Length == 8)
        {
            List<string> tempP = Shuffle(Partners);
            tempRoles.Add("Heroine");
            tempRoles.Add(tempSB[0]);
            tempRoles.Add(tempSB[1]);
            tempRoles.Add(tempSB[2]);
            tempRoles.Add(tempXB[0]);
            tempRoles.Add(tempP[0]);
            tempRoles.Add(tempP[1]);
            tempRoles.Add("Rival");
        }

        tempRoles = Shuffle(tempRoles);
        return tempRoles;
    }

    public List<string> Shuffle(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            string temp = list[i];
            int randomIndex = UnityEngine.Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }
}
