using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Airport : MonoBehaviour
{
    //public int id;
    static int MAX_CAPACITY = 1000;
    private const int CIRCLE_RADIUS = 30;

    public List<Plane> PlaneList;

    public Dictionary<Airport, int> AirportPassengerCountDictionary;

    public int passengers = 0;
	public int capacity = MAX_CAPACITY;


    public void ChangeAnimation(bool isSelected)
    {
        
    }

    public bool HaveClicked(Vector3 clickPosition)
    {
        var circleCenter = transform.position;
        var clickRadius = Vector3.Distance(circleCenter, clickPosition);
        return clickRadius < CIRCLE_RADIUS;
    }

}
