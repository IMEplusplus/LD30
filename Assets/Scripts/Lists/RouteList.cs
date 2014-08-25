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
            SetRoute(Route.RouteHeight.Low);
	    }
        else if (Input.GetMouseButtonDown(1))
        {
            SetRoute(Route.RouteHeight.High);
        }
	}

    private void SetRoute(Route.RouteHeight height)
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        SetAirport(airports.available, mousePosition, height);        
    }

    private void SetAirport(List<Airport> airportList, Vector3 mousePosition, Route.RouteHeight height)
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
                CreateNewRoute(from, to, height);

            from.ChangeAnimation(false);
            to.ChangeAnimation(false);
            from = null;
            to = null;
        }
    }

    private void CreateNewRoute(Airport from, Airport to, Route.RouteHeight height)
    {
        Route newRoute = ObjectPoolScript.instance.GetObjectForType("Route", false).GetComponent<Route>();
        newRoute.from = from;
        newRoute.to = to;
        newRoute.height = height;
        newRoute.transform.parent = transform;
        routes.Add(newRoute);
    }
}
