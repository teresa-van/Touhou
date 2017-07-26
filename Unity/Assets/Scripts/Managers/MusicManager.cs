using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour {

    #region Singleton instance

    public static MusicManager Instance { private set; get; }

    #endregion

    public AudioClip login;
    public AudioClip main;
    public AudioClip selection;
    public AudioClip game;

    public bool music = true;

    public void Awake()
    {
        DontDestroyOnLoad(this);

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start ()
    {
        MusicManager.Instance = this;
        SceneManager.sceneLoaded += SceneLoadComplete;
    }
	
    public void SceneLoadComplete(Scene scene, LoadSceneMode mode)
    {
        AudioListener.volume = GeneralGameManager.Instance.defaultVolume;
        if (scene.name.Equals("Main"))
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = main;
            audio.Play();
        }
        else if (scene.name.Equals("Login"))
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = login;
            audio.Play();
            if (AudioListener.volume == 0) music = false;
            GeneralGameManager.Instance.SyncMusicVolume();
        }
        else if (scene.name.Equals("Selection"))
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = selection;
            audio.Play();
        }
        else if (scene.name.Equals("Game"))
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = game;
            audio.Play();
        }
        else
        {
            print("nulling audio clip");
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = null;
            audio.Play();
        }
    }

}
