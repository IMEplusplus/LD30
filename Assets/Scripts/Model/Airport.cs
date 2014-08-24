using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Airport : MonoBehaviour {

	public int Id { get; set; }
	
	public List<Plane> PlaneList { get; set; }
	
	public Dictionary<Airport,int> AirportPassengerCountDictionary { get; set; } 
	
	public Vector2 Position { get; set; }
	
	public readonly int Capacity;
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
