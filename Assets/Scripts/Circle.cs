using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Circle : MonoBehaviour
{
    Airport airport;
    float size;

	// Use this for initialization
	void Start ()
    {
        airport = GetComponentInParent<Airport>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (airport.capacity != 0)
        {
            size = (float)airport.passengers / airport.capacity;
            transform.localScale = new Vector3(size, size, 1.0f);
        }
        else
        {
            Debug.Log("Capacidade zero!!");
        }
	}
}
