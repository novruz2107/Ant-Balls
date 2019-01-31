using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSpawner : MonoBehaviour {

    public static CircleSpawner instance;

    public GameObject circle;
    public float degreesPerSecond = 30.0f;
    public Transform center;
    public Vector3[] offset = new Vector3[10];
    public GameObject[] instCircles = new GameObject[10];
    public int[] colorCodes = new int[5];
    public float radius = 2.25f;

    public Sprite[] sprites = new Sprite[5];
    public float angle;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
for(int i=0; i<5; i++)
        {
            colorCodes[i] = 0;
        }
        startSpawning();
    }

    // Use this for initialization
    void Start () {
        angle = (360 / 10) * Mathf.PI / 180;


    }
	
	// Update is called once per frame
	void Update () {
       

        for(int i=0; i<10; i++)
        {
            
            offset[i] = Quaternion.AngleAxis(degreesPerSecond * Time.deltaTime, Vector3.forward) * offset[i];
            instCircles[i].transform.position = center.position + offset[i];
        }
    }

    void startSpawning()
    {
        float angle = (360 / 10) * Mathf.PI / 180;

        for (int i=0; i<10; i++)
        {
            int rand = Random.Range(0, 5);
            instCircles[i] = Instantiate(circle, new Vector3(Mathf.Sin(i*angle), Mathf.Cos(i*angle), 0)*radius, Quaternion.identity);
            offset[i] = new Vector3(Mathf.Sin(i * angle), Mathf.Cos(i * angle), 0) * radius - center.position;
            colorCodes[rand] = colorCodes[rand]+1;

            instCircles[i].GetComponent<SpriteRenderer>().sprite = sprites[rand];
            

        }
    }


    private float convert(float num)
    {
        return (num / 255.0f);
    }

    
}
