using UnityEngine;

public class GeneralGameManager : MonoBehaviour {

    #region Start/Update

    void Start () {
		
	}
	
	void Update () {
		
	}

    #endregion

    #region Exit Game

    /// <summary>
    /// Exit the game.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    #endregion

}
