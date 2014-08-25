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
            {
                CreateNewRoute(from, to, clickHeight);
            }
            else
            {
                DestroyRoute(from, to);
            }

            from.ChangeAnimation(false);
            to.ChangeAnimation(false);
            from = null;
            to = null;
        }
    }

    private void CreateNewRoute(Airport from, Airport to, Route.RouteHeight height)
    {
        if (RouteCrosses(from, to, height))
            return;

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
        var width = Constants.instance.lineSize.x;
        line.transform.localScale = new Vector3(dist / width, 0.5f, 1.0f);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        line.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        line.GetComponent<SpriteRenderer>().color = height == Route.RouteHeight.Low ? Constants.instance.routeLowColor :
                                                                                      Constants.instance.routeHighColor;
        routes.Add(newRoute);
    }

    private void DestroyRoute(Airport a1, Airport a2)
    {
        var routeToDestroy = routes.Where(route => route.from == a1 && route.to == a2 ||
                                                   route.from == a2 && route.to == a1)
                                   .First();
        routes.Remove(routeToDestroy);
        routeToDestroy.GetComponent<SelfPoolScript>().PoolObject();
    }

    private bool RouteCrosses(Airport from, Airport to, Route.RouteHeight height)
    {
        var x11 = from.transform.position.x;
        var y11 = from.transform.position.y;

        var x12 = to.transform.position.x;
        var y12 = to.transform.position.y;

        var A1 = y12 - y11;
        var B1 = x11 - x12;
        var C1 = A1 * x11 + B1 * y11;

        foreach (var route in routes)
        {
            if (route.height != height)
                continue;

            if (route.from == from || route.from == to ||
                route.to == from || route.to == to)
                continue;

            var x21 = route.from.transform.position.x;
            var y21 = route.from.transform.position.y;

            var x22 = route.to.transform.position.x;
            var y22 = route.to.transform.position.y;

            var A2 = y22 - y21;
            var B2 = x21 - x22;
            var C2 = A2 * x21 + B2 * y21;

            var det = A1 * B2 - A2 * B1;
            if (det != 0)
            {
                var x = (B2 * C1 - B1 * C2) / det;
                var y = (A1 * C2 - A2 * C1) / det;

                if (x > Mathf.Min(x11, x12) && x < Mathf.Max(x11, x12) &&
                    y > Mathf.Min(y11, y12) && y < Mathf.Max(y11, y12) &&
                    x > Mathf.Min(x21, x22) && x < Mathf.Max(x21, x22) &&
                    y > Mathf.Min(y21, y22) && y < Mathf.Max(y21, y22))
                    return true;
            }
        }

        return false;
    }
}
