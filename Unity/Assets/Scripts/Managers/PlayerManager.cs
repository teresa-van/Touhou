using UnityEngine;
using Scripts.Models;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    #region Singleton instance

    public static PlayerManager Instance { private set; get; }

    #endregion

    #region Variables

    public List<PlayerModel> players;

    #endregion

    #region Initialization
    void Awake()
    {
        DontDestroyOnLoad(this);

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Instance = this;
        players = new List<PlayerModel>();
    }
    #endregion
}
