using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RouteList : MonoBehaviour {

	public List<Route> routes;
    private Airport from = new Airport();
    private Airport to = new Airport();
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
        var obj = GameObject.Find("Airports");
        if (obj == null) return;

        var airportList = obj.GetComponent<AirportList>().available;
        
        SetAirport(airportList, mousePosition);        
    }

    private void SetAirport(List<Airport> airportList, Vector3 mousePosition)
    {
        if (airportList.All(airport => airport.transform.position != mousePosition)) return;
        var selectedAirport = airportList.First(airport => airport.transform.position == mousePosition);

        if (from == new Airport())
        {
            from = selectedAirport;
            from.ChangeAnimation(true);
        }
        else if (from == selectedAirport)
        {
            from.ChangeAnimation(false);
            from = new Airport();

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
            from = new Airport();
            to = new Airport();
        }
    }
}
