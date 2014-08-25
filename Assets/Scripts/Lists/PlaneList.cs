using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PlaneList : MonoBehaviour
{

    public List<Plane> planes;
    private RouteList routes;
    // Use this for initialization
    void Start() 
    {
	
        planes = new List<Plane>();
        routes = GameObject.FindObjectOfType<RouteList>();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
