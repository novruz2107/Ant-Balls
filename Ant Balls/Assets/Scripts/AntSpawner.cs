using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntSpawner : MonoBehaviour {

    public GameObject ant;
    public Transform center;
    public Vector3[] offset = new Vector3[12];
    private GameObject[] ants = new GameObject[12];

    private void Awake()
    {
        startSpawning();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < 10; i++)
        {
            offset[i] = Quaternion.AngleAxis(CircleSpawner.instance.degreesPerSecond * Time.deltaTime, Vector3.forward) * CircleSpawner.instance.offset[i];
            
            ants[i].transform.position = center.position + CircleSpawner.instance.offset[i];
            ants[i].transform.Rotate(0, 0, CircleSpawner.instance.degreesPerSecond * Time.deltaTime);
        }
    }

    private void startSpawning()
    {
        float angle = (360 / 10) * Mathf.PI / 180;
        for(int i=0; i<10; i++)
        {
            ants[i] = Instantiate(ant, new Vector3(Mathf.Sin(i * angle), Mathf.Cos(i * angle), 0) * CircleSpawner.instance.radius, Quaternion.identity);
            offset[i] = new Vector3(Mathf.Sin(i * angle), Mathf.Cos(i * angle), 0) * CircleSpawner.instance.radius - center.position;
            ants[i].transform.Rotate(0, 0, (i*(-36.0f))-90);
        }
    }
}
