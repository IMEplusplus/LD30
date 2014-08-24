using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Airport : MonoBehaviour
{
    //public int id;
    static int MAX_CAPACITY = 1000;

    public List<Plane> PlaneList;

    public Dictionary<Airport, int> AirportPassengerCountDictionary;

    public int passengers = 0;
	public int capacity = MAX_CAPACITY;
}
