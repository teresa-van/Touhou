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
    public float defaultVolume = 1;
    public Toggle music;

    public Text nickname;
    #endregion

    #region Initialization

    void Start () {
        //Reference this instance as singleton instance
        Instance = this;
        if (SceneManager.GetActiveScene().name.Equals("Main") || SceneManager.GetActiveScene().name.Equals("Selection"))
        {
            nickname = GameObject.Find("Menu/UserUI/Text").GetComponent<Text>();
            nickname.text = AuthenticationManager.Instance.User.NickName;
        }
        SyncMusicVolume();
    }

    #endregion

    #region Main Menu

    public void ToggleRulesOn(GameObject menu)
    {
        GameObject rules = menu.transform.Find("RulesMenu/Scroll View").gameObject;
        GameObject buttons = menu.transform.Find("Buttons").gameObject;
        rules.SetActive(true);
        buttons.SetActive(false);
    }

    public void ToggleRulesOff(GameObject menu)
    {
        GameObject rules = menu.transform.Find("RulesMenu/Scroll View").gameObject;
        GameObject buttons = menu.transform.Find("Buttons").gameObject;
        rules.SetActive(false);
        buttons.SetActive(true);
    }

    #endregion

    #region Load Scenes

    public void LoadScene(string level, bool sync)
    {
        StartCoroutine(StartLoadScene(level, sync));
    }

    IEnumerator StartLoadScene(string level, bool sync)
    {
        float elapsedTime = 0;

        while (elapsedTime < 0.75f)
        {
            elapsedTime += Time.deltaTime;
            AudioListener.volume = Mathf.Lerp(AudioListener.volume, 0, elapsedTime / 0.75f);
            //BlackScreen.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(0.0f, 1f, elapsedTime/0.3f));
            yield return null;
        }
        if (sync)
            PhotonNetwork.LoadLevel(level);
        else
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
        if (MusicManager.Instance.music)
        {
            AudioListener.volume = 0;
            MusicManager.Instance.music = false;
        }
        else
        {
            AudioListener.volume = defaultVolume;
            MusicManager.Instance.music = true;
        }
    }

    public void SyncMusicVolume()
    {
        if (!MusicManager.Instance.music)
        {
            music.isOn = true;
            MusicManager.Instance.music = false;
            AudioListener.volume = 0;
        }
    }

    #endregion

    #region Exit Game/Logout/Register

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
        LoadScene("Scenes/Login", false);
    }

    public void Register()
    {
        Application.OpenURL("http://localhost:50478/Home.aspx");
    }

    #endregion
}
