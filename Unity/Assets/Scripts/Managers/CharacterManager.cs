using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    #region Singleton instance
    public static CharacterManager Instance { private set; get; }
    #endregion

    #region Characters 

    public List<string> AllCharacters = new List<string>
        {
            "Alice Margatroid",
            "Cirno",
            "Fujiwara no Mokou",
            "Hakurei Reimu",
            "Hijiri Byakuren",

            "Hinanawi Tenshi",
            "Hong Meiling",
            "Hoshiguma Yuugi",
            "Houjuu Nue",
            "Ibuki Suika",

            "Izayoi Sakuya",
            "Kamishirasawa Keine",
            "Kawashiro Nitori",
            "Kazami Yuuka",
            "Kirisame Marisa",

            "Kochiya Sanae",
            "Komeiji Satori",
            "Konpaku Youmu",
            "Mononobe no Futo",
            "Patchouli Knowledge",

            "Player 2",
            "Reisen Udongein Inaba",
            "Reiuji Utsuho",
            "Remilia Scarlet",
            "Shameimaru Aya",

            "Toramaru Shou",
            "Toyosatomimi no Miko",
            "Yagokoro Eirin",
            "Yakumo Yukari"
        };

    #endregion

    public static Dictionary<int, List<string>> characters = new Dictionary<int, List<string>>();

    void Awake()
    {
        //Reference this instance as singleton instance
        Instance = this;
    }

    public List<List<string>> ShuffleCharacters()
    {
        List<List<string>> tempCharacters = new List<List<string>>();
        List<string> temp = GeneralManager.Instance.Shuffle(AllCharacters);
        int j = 0;
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            tempCharacters.Add(new List<string> { temp[j], temp[j+1] });
            j += 2;
        }

        return tempCharacters;
    }
}
