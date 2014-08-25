using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Plane : MonoBehaviour {

	private int capacity;
    public Airport from;
    public Airport to;
    public Vector3 position;

    private List<Plane> avaiablePlanes;

    private bool Active
    {
        get { return gameObject.activeSelf; }
    }
    

    // Use this for initialization
	void Start () 
    {
        var planes = GameObject.FindObjectOfType<PlaneList>();
        if (planes != null)
        {
            avaiablePlanes = planes.planes;
        }
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    Arrive();
	    Translate();

	}

    private void Arrive()
    {
        if (!Active) return;
        if (transform.position == to.transform.position)
        {
            avaiablePlanes.Remove(this);
            gameObject.GetComponent<SelfPoolScript>().PoolObject();
            
        }
    }

    private void Translate()
    {
        if (!Active) return;
        var direction = from.transform.position - to.transform.position;
        direction.z = 0.0f;

        transform.Translate(direction);
    }
}
