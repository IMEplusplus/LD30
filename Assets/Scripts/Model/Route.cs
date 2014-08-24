using UnityEngine;
using System.Collections;

public class Route : MonoBehaviour {

	public enum RouteHeight
	{
		Low, High
	}

	public int Id { get; set; }
	public Airport From { get; set; }
	public Airport To { get; set; }
	public RouteHeight Height { get; set; }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
