using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager2 : MonoBehaviour {

    public AudioManager am;
    bool direction = true;
    bool increase = false;

	// Use this for initialization
	void Start () {

        InvokeRepeating("getMoreHard", 5f, 10f);
        InvokeRepeating("changeDirection", 15f, 15f);
        InvokeRepeating("changeIncrease", 9f, 9f);

        if (PlayerPrefs.GetInt("musicOn") == 1)
            GameObject.FindGameObjectWithTag("music").GetComponent<AudioSource>().Stop();
    }
	
	// Update is called once per frame
	void Update () {

        incAndDecRadius();

        if (CenterCircleController.instance.gameOver)
        {
            if (PlayerPrefs.HasKey("highScore"))
            {
                if (CenterCircleController.instance.score > PlayerPrefs.GetInt("highScore"))
                {
                    PlayerPrefs.SetInt("highScore", CenterCircleController.instance.score);
                }
            }
            else
            {
                PlayerPrefs.SetInt("highScore", CenterCircleController.instance.score);
            }
        }
	}

    public void back()
    {
        GameObject.FindGameObjectWithTag("music").GetComponent<AudioSource>().volume = 0.5f;
        if (PlayerPrefs.GetInt("soundOn") == 1)
            am.click.Play();
        SceneManager.LoadScene(0);
    }

    public void replay()
    {
        if (PlayerPrefs.GetInt("soundOn") == 1)
            am.click.Play();
        GameObject.FindGameObjectWithTag("music").GetComponent<AudioSource>().volume = 0.5f;
        SceneManager.LoadScene("level1");

    }

    void getMoreHard()
    {
        if (direction)
            CenterCircleController.instance.RotateSpeed = CenterCircleController.instance.RotateSpeed - 0.1f;
        else
            CenterCircleController.instance.RotateSpeed = CenterCircleController.instance.RotateSpeed + 0.1f;
    }

    void changeDirection()
    {
        CircleSpawner.instance.degreesPerSecond = CircleSpawner.instance.degreesPerSecond * (-1);
        CenterCircleController.instance.RotateSpeed = CenterCircleController.instance.RotateSpeed * (-1);
        if (direction)
        {
            direction = false;
        }
        else
        {
            direction = true;
        }
    }

    void incAndDecRadius()
    {

        if (!increase)
        {
            for (int i = 0; i < 10; i++)
            {
                CircleSpawner.instance.offset[i] = CircleSpawner.instance.offset[i] * 0.9995f;
            }
        }
        else
            for (int i = 0; i < 10; i++)
            {
                CircleSpawner.instance.offset[i] = CircleSpawner.instance.offset[i] * 1.0005f;
            }

    }
    

    void changeIncrease()
    {
        if (increase)
        {
            increase = false;
        }
        else
        {
            increase = true;
        }
    }
    
}
