using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RouteList : MonoBehaviour {

	public List<Route> routes;
    private Airport from;
    private Airport to;

    private AirportList airports;

    void Start()
    {
        routes = new List<Route>();
        airports = GameObject.FindObjectOfType<AirportList>();
        if (airports == null)
            Debug.Log("AirportList not found!");
    }

	void Update () 
	{
	    if (Input.GetMouseButtonDown(0))
	    {
            SetRoute();
	    }
	}

    private void SetRoute()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        SetAirport(airports.available, mousePosition);        
    }

    private void SetAirport(List<Airport> airportList, Vector3 mousePosition)
    {
        if (airportList.All(airport => !airport.HaveClicked(mousePosition))) return;
        var selectedAirport = airportList.First(airport => airport.HaveClicked(mousePosition));

        if (from == null)
        {
            from = selectedAirport;
            from.ChangeAnimation(true);
        }
        else if (from == selectedAirport)
        {
            from.ChangeAnimation(false);
            from = null;

        }
        else
        {
            to = selectedAirport;
            if (!routes.Any(route => route.from == from && route.to == to))
            {
                routes.Add(new Route
                {
                    from = from,
                    to = to
                });
            }
            from.ChangeAnimation(false);
            to.ChangeAnimation(false);
            from = null;
            to = null;
        }
    }
}
