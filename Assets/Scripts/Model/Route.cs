using UnityEngine;
using System.Collections;

public class Route : MonoBehaviour {

	public enum RouteHeight
	{
		Low, High
	}

    public Airport from;
    public Airport to;
    public RouteHeight height;

}
