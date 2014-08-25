using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RouteList : MonoBehaviour {

	public List<Route> routes;
    private Airport from;
    private Airport to;

    private AirportList airports;

    private Route.RouteHeight clickHeight = Route.RouteHeight.Low;

    private SpriteRenderer line;

    void Start()
    {
        routes = new List<Route>();
        airports = GameObject.FindObjectOfType<AirportList>();
        if (airports == null)
            Debug.Log("AirportList not found!");

        line = transform.Find("Line").GetComponent<SpriteRenderer>();
    }

	void Update () 
	{
        // Line to mouse
        if (from != null)
        {
            if (!line.gameObject.activeSelf)
            {
                line.gameObject.SetActive(true);
                line.transform.position = from.transform.position;
            }

            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var direction = line.transform.position - mousePosition;
            direction.z = 0.0f;
            var dist = direction.magnitude;

            var width = Constants.instance.lineSize.x;
            Debug.Log(width);
            line.transform.localScale = new Vector3(dist / width, 0.5f, 1.0f);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            line.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            line.color = clickHeight == Route.RouteHeight.Low ? Constants.instance.routeLowColor :
                                                                Constants.instance.routeHighColor;
        }
        else
        {
            line.gameObject.SetActive(false);
        }

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
        SetAirport(airports.available, height);        
    }

    private void SetAirport(List<Airport> airportList, Route.RouteHeight height)
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (airportList.All(airport => !airport.MouseOver(mousePosition))) return;

        var selectedAirport = airportList.First(airport => airport.MouseOver(mousePosition));

        if (from == null)
        {
            from = selectedAirport;
            from.ChangeAnimation(true);
            clickHeight = height;
        }
        else if (from == selectedAirport)
        {
            from.ChangeAnimation(false);
            from = null;
        }
        else
        {
            to = selectedAirport;
            if (!routes.Any(route => route.from == from && route.to == to) &&
                !routes.Any(route => route.from == to && route.to == from))
                CreateNewRoute(from, to, clickHeight);

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
        newRoute.transform.position = from.transform.position;

        var direction = from.transform.position - to.transform.position;
        direction.z = 0.0f;
        var dist = direction.magnitude;

        var line = newRoute.transform.FindChild("Line");
        //var width = line.GetComponent<SpriteRenderer>().bounds.size.x;
        var width = Constants.instance.lineSize.x;
        line.transform.localScale = new Vector3(dist / width, 0.5f, 1.0f);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        line.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        line.GetComponent<SpriteRenderer>().color = height == Route.RouteHeight.Low ? Constants.instance.routeLowColor :
                                                                                      Constants.instance.routeHighColor;
        routes.Add(newRoute);
    }
}
