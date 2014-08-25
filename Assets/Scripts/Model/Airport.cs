using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = System.Random;

public class Airport : MonoBehaviour
{
    public List<Plane> PlaneList;
    public Dictionary<Airport, int> AirportPassengerCountDictionary;

    public int passengers
    {
        get { return AirportPassengerCountDictionary.Any() ? AirportPassengerCountDictionary.Values.Sum() : 0; }
    }
    public int capacity = 1000;


    private GameObject pin, pinSelected;
    private List<Airport> avaiableAirports;
    private List<Route> avaiableRoutes;
    private List<Plane> avaiablePlanes;

    private Circle circle;

    private bool Active
    {
        get { return gameObject.activeSelf; }
    }

    float timer = 1.0f;

    void Start()
    {
        circle = transform.FindChild("Circle").GetComponent<Circle>();

        capacity = Constants.instance.airportCapacity;

        pin = transform.FindChild("Pin").gameObject;
        pinSelected = transform.FindChild("Pin Select").gameObject;

        AirportPassengerCountDictionary = new Dictionary<Airport, int>();
        var airports = GameObject.FindObjectOfType<AirportList>();
        if (airports != null)
        {
            avaiableAirports = airports.available;
        }

        timer = Constants.instance.airportNewPassengersTimer;

        var routes = GameObject.FindObjectOfType<RouteList>();
        if (routes != null)
        {
            avaiableRoutes = routes.routes;
        }

        var planes = GameObject.FindObjectOfType<PlaneList>();
        if (planes != null)
        {
            avaiablePlanes = planes.planes;
        }
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0.0f)
        {
            AddPassengers();
            Fly();
            timer = Constants.instance.airportNewPassengersTimer;
        }
    }

    void AddPassengers()
    {
        if (!Active) return;
        var newPassengers = new Random().Next(Constants.instance.passengersMin, Constants.instance.passengersMax);

        if (passengers + newPassengers >= capacity)
        {
            //TODO: O que fazer se estourar a capacidade
            return;
        }
        var airportIndex = new Random().Next(0, avaiableAirports.Count);


        if (airportIndex == avaiableAirports.IndexOf(this))
        {
            airportIndex = (airportIndex + 1) % avaiableAirports.Count;
        }
        var airportTo = avaiableAirports[airportIndex];

        //Debug.Log("Airport: " + gameObject.name + " destination: " + airportTo.gameObject.name + " passengers: " + passengers + " newPassengers: " + newPassengers + " capacity: " + capacity);

        if (AirportPassengerCountDictionary.ContainsKey(airportTo))
        {
            AirportPassengerCountDictionary[airportTo] += newPassengers;
        }
        else
        {
            AirportPassengerCountDictionary.Add(airportTo, newPassengers);
        }

    }

    void Fly()
    {
        if (!Active) return;

        var airportsWithRoutesAndPassengersToGo = avaiableRoutes
            .Where(route => route.from == this || route.to == this)
            .Select(route =>
            {
                if (route.from == this) return route.to;
                if (route.to == this) return route.from;
                return null;
            }).Where(airport => AirportPassengerCountDictionary.Where(kvp => kvp.Value > Constants.instance.passengersPerFlight)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                    .ContainsKey(airport)
            ).ToList();
        if (airportsWithRoutesAndPassengersToGo.Count == 0) return;

        var airportIndex = new Random().Next(0, airportsWithRoutesAndPassengersToGo.Count);
        var selectedAirport = airportsWithRoutesAndPassengersToGo[airportIndex];

        AirportPassengerCountDictionary[selectedAirport] -= Constants.instance.passengersPerFlight;

        if (AirportPassengerCountDictionary[selectedAirport] == 0)
        {
            AirportPassengerCountDictionary.Remove(selectedAirport);
        }
        Debug.Log("VOOU " + this.gameObject.name + " : " + selectedAirport.gameObject.name);
    }

    private void CreatePlane(Airport to)
    {
        var plane = ObjectPoolScript.instance.GetObjectForType("Plane", false).GetComponent<Plane>();
        plane.from = this;
        plane.to = to;

        var direction = transform.position - to.transform.position;
        direction.z = 0.0f;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        plane.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        avaiablePlanes.Add(plane);

    }

    public void ChangeAnimation(bool isSelected)
    {
        if (isSelected)
        {
            pin.SetActive(false);
            pinSelected.SetActive(true);
        }
        else
        {
            pin.SetActive(true);
            pinSelected.SetActive(false);
        }
    }

    public bool MouseOver(Vector3 mousePosition)
    {
        var circleCenter = transform.position;
        var vectorDiff = circleCenter - mousePosition;
        vectorDiff.z = 0;

        var radius = Constants.instance.circleSize.x * circle.size / 2.0f;
        return vectorDiff.sqrMagnitude < radius * radius;
    }

}
