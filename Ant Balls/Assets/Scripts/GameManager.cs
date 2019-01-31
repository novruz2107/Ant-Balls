using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    PlayBackgroundMusic am;
    public Text highScoreText;
    public GameObject audioManager;

    //MusicController
    public Image btImage;
    public Sprite musicOn;
    public Sprite musicOff;
    //
    //SoundController
    public Image btImage2;
    public Sprite soundOn;
    public Sprite soundOff;
    //

	// Use this for initialization
	void Start () {
        audioManager = GameObject.FindGameObjectWithTag("music");
        am = audioManager.GetComponent<PlayBackgroundMusic>();
        if (PlayerPrefs.HasKey("highScore"))
        {
            highScoreText.text = "Highest Score: " + PlayerPrefs.GetInt("highScore");
        }
        
	}
	
	// Update is called once per frame
	void Update () {
        MusicController();
        SoundController();
	}

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void handleMusicButton()
    {
        if (PlayerPrefs.GetInt("musicOn") == 1)
        {
            btImage.sprite = musicOff;
            PlayerPrefs.SetInt("musicOn", 0);
        }
        else
        {
            btImage.sprite = musicOn;
            PlayerPrefs.SetInt("musicOn", 1);
        }
    }

    public void handleSoundButton()
    {
        if (PlayerPrefs.GetInt("soundOn") == 1)
        {
            btImage2.sprite = soundOff;
            PlayerPrefs.SetInt("soundOn", 0);
        }
        else
        {
            btImage2.sprite = soundOn;
            PlayerPrefs.SetInt("soundOn", 1);
        }
    }

    public void handleReviewButton()
    {
        Application.OpenURL("market://details?id=com.antech.antballs");
    }

    void MusicController()
    {
        if (PlayerPrefs.HasKey("musicOn"))
        {
            if (PlayerPrefs.GetInt("musicOn") == 1)
            {
                btImage.sprite = musicOn;
                am.backMusic.Play();
            }
            else
            {
                btImage.sprite = musicOff;
            }

        }
        else
        {
            btImage.sprite = musicOn;
            am.backMusic.Play();
        }
    }

    void SoundController()
    {
        if (PlayerPrefs.HasKey("soundOn"))
        {
            if (PlayerPrefs.GetInt("soundOn") == 0)
            {
                btImage2.sprite = soundOn;
            }
            else
            {
                btImage2.sprite = soundOff;
            }

        }
        else
        {
            btImage2.sprite = soundOn;
        }
    }
}
