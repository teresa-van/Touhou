using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHoverScript : MonoBehaviour {

    public AudioSource AudioSource;
    public AudioClip ClickSound;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ButtonHoverSound()
    {
        AudioSource.PlayOneShot(ClickSound);
    }

}
