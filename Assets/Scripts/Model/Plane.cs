using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Plane : MonoBehaviour {

	//private int capacity;
    public Airport from;
    public Airport to;
    public Vector3 position;

    private List<Plane> availablePlanes;

    private Player player;

    private bool Active
    {
        get { return gameObject.activeSelf; }
    }
    
	void Start () 
    {
        player = GameObject.FindObjectOfType<Player>();

        var planes = GameObject.FindObjectOfType<PlaneList>();
        if (planes != null)
        {
            availablePlanes = planes.planes;
        }
	}
	
	void Update ()
	{
        if (player.state != Player.GameState.Play)
            return;

	    Translate();
	}

    private void Translate()
    {
        var direction = to.transform.position - from.transform.position;
        direction.z = 0.0f;

        var arriveDist = Constants.instance.planeArrivalDistance;

        if ((transform.position - to.transform.position).sqrMagnitude <= arriveDist * arriveDist)
        {
            player.AddPassengers(Constants.instance.passengersPerFlight);

            availablePlanes.Remove(this);
            gameObject.GetComponent<SelfPoolScript>().PoolObject();
            return;
        }

        var newPos = transform.position;
        newPos += direction.normalized * Constants.instance.planeVelocity * Time.deltaTime;
        transform.position = newPos;
    }
}
