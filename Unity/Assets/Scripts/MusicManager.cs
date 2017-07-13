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
        OnLevelWasLoaded(0);
    }
	
    public void OnLevelWasLoaded(int level)
    {
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = main;
            audio.Play();
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = login;
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
