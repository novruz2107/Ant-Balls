using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CenterCircleController : MonoBehaviour {

    public GameObject centerCircle;
    LineRenderer line;
    public float RotateSpeed = -1.2f;
    private float Radius = 1.2f;
    private float _angle = 0;
    private Color[] colors = new Color[5];
    public Sprite[] sprites = new Sprite[5];
    private Vector2 forceDirection;
    public int score = 0;
    public Text scoreText;
    Animator animator;
    public Animator scoreAnimator;
    public GameObject diamondAnim;
    int oldColor, firstColor, currentColor;
    GameObject targetCircle;
    public bool gameOver;
    private bool isCombo = false;
    public ParticleSystem particle;
    Vector3 positionOfEffect;
    public Camera cam;
    public GameObject panel;
    public AudioManager am;
    public static CenterCircleController instance;
    public Text highScore, scoreInPanel, comboText;
    float difference, time1, time2;
    public Sprite colorfulBall, whiteCircle;
    

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("startDiamondAnim", 2f, 2f);
        centerCircle.GetComponent<TrailRenderer>().enabled = false;
        gameOver = false;
        centerCircle = Instantiate(centerCircle, new Vector3(0, 0, 0), Quaternion.identity);
        line = centerCircle.GetComponent<LineRenderer>();
        line.positionCount = 2;

        //Color controller:
        colors[0] = Color.black;
        colors[0].r = convert(21);
        colors[0].g = convert(230);
        colors[0].b = convert(0);
        colors[1] = Color.black;
        colors[1].r = convert(0);
        colors[1].g = convert(100);
        colors[1].b = convert(255);
        colors[2] = Color.black;
        colors[2].r = convert(188);
        colors[2].g = convert(0);
        colors[2].b = convert(178);
        colors[3] = Color.black;
        colors[3].r = convert(255);
        colors[3].g = convert(42);
        colors[3].b = convert(0);
        colors[4] = Color.black;
        colors[4].r = convert(250);
        colors[4].g = convert(255);
        colors[4].b = convert(0);

        centerCircle.GetComponent<SpriteRenderer>().sprite = sprites[changeColor()];
        //////////////////////////////////

        time1 = -3f;

    }

    // Update is called once per frame
    void Update()
    {
       
        //Direction laser controller:
        _angle += RotateSpeed * Time.deltaTime;
        Vector3 pos = new Vector3(Mathf.Sin(_angle), Mathf.Cos(_angle), 0) * Radius;
        line.SetPosition(0, pos);
        //////////////////////////////

        if (Input.GetMouseButtonDown(0) && !gameOver)
        {
            forceDirection = new Vector2(pos.x, pos.y);
            centerCircle.GetComponent<Rigidbody2D>().AddForce(forceDirection * 60, ForceMode2D.Impulse);

            RaycastHit2D hit = Physics2D.Raycast(forceDirection, forceDirection * 20);
            if (hit.collider != null)
            {
                if (centerCircle.GetComponent<SpriteRenderer>().sprite.Equals(hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite) 
                    || centerCircle.GetComponent<SpriteRenderer>().sprite.Equals(colorfulBall)) 
                {
                    time2 = Time.time;
                    positionOfEffect = hit.collider.gameObject.GetComponent<Transform>().position;
                    currentColor = Random.Range(0, 5);
                    //OldColor processing
                    string spriteName = hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite.name;
                    if (spriteName == "greenball")
                        oldColor = 0;
                    else if (spriteName == "blueball")
                        oldColor = 1;
                    else if (spriteName == "purpleball")
                        oldColor = 2;
                    else if (spriteName == "redball")
                        oldColor = 3;
                    else if (spriteName == "yellowball")
                        oldColor = 4;
                    ///////////////////////

                    CircleSpawner.instance.colorCodes[oldColor]--;
                    CircleSpawner.instance.colorCodes[currentColor]++;
                   
                    animator = hit.collider.gameObject.GetComponent<Animator>();
                    targetCircle = hit.collider.gameObject;
                    if (PlayerPrefs.GetInt("soundOn") == 1)
                        am.rightShoot.Play();

                    difference = time2 - time1;
                    time1 = time2;
                    
                    
                    StartCoroutine("effectProcessing");
                    StartCoroutine("colorProcessing");
                }
                else
                {
                    if (PlayerPrefs.GetInt("soundOn") == 1)
                    {
                        am.wrongShoot.Play();
                    }
                    hit.collider.gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
                    gameOver = true;
                    CameraShake.instance.shakecamera();
                }
            }
            else
            {
                if (PlayerPrefs.GetInt("soundOn") == 1)
                {
                    am.missedShoot.Play();
                }
                centerCircle.GetComponent<TrailRenderer>().enabled = true;
                centerCircle.GetComponent<TrailRenderer>().startColor = colors[oldColor];
                gameOver = true;
            }

            if (!gameOver)
            {
                StartCoroutine("destroyAndCreate");
            }
            else
            {
                StartCoroutine("handleGameOver");
            }
        }



    }


    IEnumerator handleGameOver()
    {
        yield return new WaitForSeconds(1f);

        /*if (PlayerPrefs.HasKey("adCount"))
        {
            if (PlayerPrefs.GetInt("adCount") == 1)
            {
                AdsManager.instance.showAd();
                PlayerPrefs.SetInt("adCount", 0);
            }
            else
            {
                PlayerPrefs.SetInt("adCount", PlayerPrefs.GetInt("adCount") + 1);
            }
        }
        else
        {
            PlayerPrefs.SetInt("adCount", -1);
        }*/

        panel.SetActive(true);
        scoreInPanel.text = "Your Score: " + score;
        highScore.text = "Highest Score: " + PlayerPrefs.GetInt("highScore");

    }

    private float convert(float num)
    {
        return (num / 255.0f);
    }

    int changeColor()
    {
        
        int rand = Random.Range(0, 5);
        bool flag = true;
        while (flag)
        {
            rand = Random.Range(0, 5);
            int check = CircleSpawner.instance.colorCodes[rand];
            if(check > 0)
            {
                flag = false;
            }
            
            
        }
        return rand;

    }

    IEnumerator destroyAndCreate()
    {
        yield return new WaitForSeconds(0.25f);
        
            Destroy(centerCircle);
            centerCircle = Instantiate(centerCircle, new Vector3(0, 0, 0), Quaternion.identity);
            centerCircle.GetComponent<SpriteRenderer>().enabled = true;
            centerCircle.GetComponent<Renderer>().material.color = Color.white;

        if (difference < 1f && !isCombo)
        {
            isCombo = true;
            centerCircle.GetComponent<SpriteRenderer>().sprite = colorfulBall;
            comboText.gameObject.SetActive(true);
            score++;
            StartCoroutine("closeCombo");
        }
        else
        {
            isCombo = false; 
            centerCircle.GetComponent<SpriteRenderer>().sprite = sprites[changeColor()];
        }
        line = centerCircle.GetComponent<LineRenderer>();
    }

    IEnumerator colorProcessing()
    {
        animator.SetTrigger("play");
        scoreAnimator.SetTrigger("play");
        yield return new WaitForSeconds(0.35f);

        
        
        score++;
        
        targetCircle.GetComponent<SpriteRenderer>().sprite = sprites[currentColor];
        scoreText.text = "Score: " + score.ToString();

    }

    IEnumerator effectProcessing()
    {
        yield return new WaitForSeconds(0.05f);

        centerCircle.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        centerCircle.GetComponent<SpriteRenderer>().enabled = false;
        particle = Instantiate(particle, positionOfEffect, Quaternion.identity);
#pragma warning disable CS0618 // Type or member is obsolete
        particle.startColor = colors[oldColor];
#pragma warning restore CS0618 // Type or member is obsolete
        particle.Play();
    }

    IEnumerator closeCombo()
    {
        if (PlayerPrefs.GetInt("soundOn") == 1)
        {
            am.comboShoot.Play();
        }
        yield return new WaitForSeconds(2f);

        comboText.gameObject.SetActive(false);
    }

    void startDiamondAnim()
    {
        if (Random.Range(0, 2) == 0)
        {
            diamondAnim.transform.position = new Vector3(Random.Range(-3f, 3f), Random.Range(4.5f, 5.5f), 0);
        }
        else
        {
            diamondAnim.transform.position = new Vector3(Random.Range(-3f, 3f), Random.Range(-6f, -4f), 0);
        }
        diamondAnim.GetComponent<Animator>().SetTrigger("play");
    }

    
}
