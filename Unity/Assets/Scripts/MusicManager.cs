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

    // Use this for initialization
    void Start ()
    {
        DontDestroyOnLoad(this.gameObject);
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

        else
        {
            print("nulling audio clip");
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = null;
            audio.Play();
        }
    }
}
