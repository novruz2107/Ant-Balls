using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource click;
    public AudioSource wrongShoot;
    public AudioSource rightShoot;
    public AudioSource missedShoot;
    public AudioSource comboShoot;
    GameObject song;

	// Use this for initialization
	void Start () {
            song = GameObject.FindGameObjectWithTag("music");
	}
	
	// Update is called once per frame
	void Update () {
        if (CenterCircleController.instance.gameOver)
        {
            StartCoroutine("stopMusic");
        }
		
	}

    IEnumerator stopMusic()
    {
        while(song.GetComponent<AudioSource>().volume > 0.15f)
        {
            song.GetComponent<AudioSource>().volume = song.GetComponent<AudioSource>().volume - 0.01f;
            yield return new WaitForSeconds(1f);
        }

        if(song.GetComponent<AudioSource>().volume == 0.0f)
        {
            song.GetComponent<AudioSource>().Stop();
        }     

        
    }
}
