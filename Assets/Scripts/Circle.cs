using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Circle : MonoBehaviour
{
    public Airport airport;
    public double size;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPos.z = 0;
        transform.position = newPos;
	}
}
