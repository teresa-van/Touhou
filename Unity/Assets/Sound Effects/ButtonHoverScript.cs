using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHoverScript : MonoBehaviour {

    public AudioSource AudioSource;
    public AudioClip ClickSound;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ButtonHoverEnter(GameObject button)
    {
        AudioSource.PlayOneShot(ClickSound);
    }

}
