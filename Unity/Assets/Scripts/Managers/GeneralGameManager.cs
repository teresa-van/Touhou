using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GeneralGameManager : MonoBehaviour {

    #region Singleton instance

    public static GeneralGameManager Instance { private set; get; }

    #endregion

    #region Variables
    public SpriteRenderer BlackScreen;
    public bool music = true;
    public float defaultVolume = 1;

    public Text nickname;
    #endregion

    #region Initialization

    void Start () {
        //Reference this instance as singleton instance
        GeneralGameManager.Instance = this;
        if (SceneManager.GetActiveScene().name.Equals("Main"))
        {
            nickname = GameObject.Find("Menu/UserUI/Text").GetComponent<Text>();
            nickname.text = AuthenticationManager.Instance.User.NickName;
        }
    }

    #endregion

    #region Main Menu

    public void ToggleRulesOn(GameObject menu)
    {
        GameObject rules = menu.transform.Find("Canvas/Scroll View").gameObject;
        GameObject buttons = menu.transform.Find("Buttons").gameObject;
        rules.SetActive(true);
        buttons.SetActive(false);
    }

    public void ToggleRulesOff(GameObject menu)
    {
        GameObject rules = menu.transform.Find("Canvas/Scroll View").gameObject;
        GameObject buttons = menu.transform.Find("Buttons").gameObject;
        rules.SetActive(false);
        buttons.SetActive(true);
    }

    #endregion

    #region Load Scenes

    public void LoadScene(string level)
    {
        StartCoroutine(StartLoadScene(level));
    }

    IEnumerator StartLoadScene(string level)
    {
        float elapsedTime = 0;

        while (elapsedTime < 0.5f)
        {
            elapsedTime += Time.deltaTime;
            AudioListener.volume = Mathf.Lerp(defaultVolume, 0, elapsedTime / 0.5f);
            //BlackScreen.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(0.0f, 1f, elapsedTime/0.3f));
            yield return null;
        }
        SceneManager.LoadScene(level);
    }

    public void EnterScene()
    {
        StartCoroutine(StartEnterScene());
    }

    IEnumerator StartEnterScene()
    {
        float elapsedTime = 0;

        while (elapsedTime < 0.5f)
        {
            elapsedTime += Time.deltaTime;
            BlackScreen.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1f, 0.0f, elapsedTime / 0.5f));
            yield return null;
        }
    }

    #endregion

    #region Music/Volume Control

    public void ToggleMusic()
    {
        if (music)
        {
            AudioListener.volume = 0;
            music = false;
        }
        else
        {
            AudioListener.volume = defaultVolume;
            music = true;
        }
    }

    #endregion

    #region Exit Game/Logout

    /// <summary>
    /// Exit the game.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    public void Logout()
    {
        PhotonNetwork.Disconnect();
        AuthenticationManager.Instance.User = null;
        LoadScene("Scenes/Login");
    }

    #endregion
}
