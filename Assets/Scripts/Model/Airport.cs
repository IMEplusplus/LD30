using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = System.Random;

public class Airport : MonoBehaviour
{
    static int MAX_CAPACITY = 1000;

    [SerializeField]
    private float CIRCLE_RADIUS = 0.3f;


    public List<Plane> PlaneList;

    public Dictionary<Airport, int> AirportPassengerCountDictionary;

    public int passengers
    {
        get { return AirportPassengerCountDictionary.Any() ? AirportPassengerCountDictionary.Values.Sum() : 0; }
    }
    public int capacity = MAX_CAPACITY;

    private GameObject pin, pinSelected;
    private List<Airport> avaiableAirports;
    private List<Route> avaiableRoutes;

    private bool active
    {
        get { return gameObject.activeSelf; }
    }

    float timer = 1.0f;

    void Start()
    {
        pin = transform.FindChild("Pin").gameObject;
        pinSelected = transform.FindChild("Pin Select").gameObject;

        AirportPassengerCountDictionary = new Dictionary<Airport, int>();
        var airports = GameObject.FindObjectOfType<AirportList>();
        if (airports != null)
        {
            avaiableAirports = airports.available;
        }
        var routes = GameObject.FindObjectOfType<RouteList>();
        if (routes != null)
        {
            avaiableRoutes = routes.routes;
        }

    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0.0f)
        {
            AddPassengers();
            Fly();
            timer = 1.0f;
        }



    }

    void AddPassengers()
    {
        if (!active) return;
        var newPassengers = new Random().Next(5, 10);

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
        Debug.Log("Airport: " + gameObject.name + " destination: " + airportTo.gameObject.name + " passengers: " + passengers + " newPassengers: " + newPassengers + " capacity: " + capacity);
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
        int PASSENGERS_PER_FLIGHT = 50;
        if (!active) return;
        var airportsWithRoutesAndPassengersToGo = avaiableRoutes
            .Where(route => route.from == this || route.to == this)
            .Select(route =>
            {
                if (route.from == this) return route.to;
                if (route.to == this) return route.from;
                return null;
            }).Where(airport =>
            {
                var result = AirportPassengerCountDictionary.Where(kvp => kvp.Value > PASSENGERS_PER_FLIGHT)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                    .ContainsKey(airport);
                Debug.Log(result);
                return result;
            }).ToList();
        if (airportsWithRoutesAndPassengersToGo.Count == 0) return;

        var airportIndex = new Random().Next(0, airportsWithRoutesAndPassengersToGo.Count);
        var selectedAirport = airportsWithRoutesAndPassengersToGo[airportIndex];
        AirportPassengerCountDictionary[selectedAirport] -= PASSENGERS_PER_FLIGHT;
        if (AirportPassengerCountDictionary[selectedAirport] == 0)
        {
            AirportPassengerCountDictionary.Remove(selectedAirport);
        }
        Debug.Log("VOOU " + this.gameObject.name + " : " + selectedAirport.gameObject.name);
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

    public bool HaveClicked(Vector3 clickPosition)
    {
        var circleCenter = transform.position;
        var vectorDiff = circleCenter - clickPosition;
        vectorDiff.z = 0;

        return vectorDiff.sqrMagnitude < CIRCLE_RADIUS * CIRCLE_RADIUS;
    }

}
