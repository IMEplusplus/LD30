using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PlaneList : MonoBehaviour
{

    public List<Plane> planes;
    //private RouteList routes;
    
    void Start() 
    {
	
        planes = new List<Plane>();
        //routes = GameObject.FindObjectOfType<RouteList>();

    }

    void Update()
    {

    }
}
