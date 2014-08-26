﻿using System.Linq;
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
    private List<Airport> availableAirports;
    private List<Route> availableRoutes;
    private List<Plane> availablePlanes;
    private PlaneList planeList;

    private Circle circle;

    private Player player;
    private Audio audioPlayer;

    private bool Active
    {
        get { return gameObject.activeSelf; }
    }

    float timer = 1.0f;

    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        audioPlayer = GameObject.FindObjectOfType<Audio>();

        circle = transform.FindChild("Circle").GetComponent<Circle>();

        capacity = Constants.instance.airportCapacity;

        pin = transform.FindChild("Pin").gameObject;
        pinSelected = transform.FindChild("Pin Select").gameObject;

        var airports = GameObject.FindObjectOfType<AirportList>();
        if (airports != null)
        {
            availableAirports = airports.available;
        }

        var routes = GameObject.FindObjectOfType<RouteList>();
        if (routes != null)
        {
            availableRoutes = routes.routes;
        }

        planeList = GameObject.FindObjectOfType<PlaneList>();
        if (planeList != null)
        {
            availablePlanes = planeList.planes;
        }

        Reset();
    }

    public void Reset()
    {
        AirportPassengerCountDictionary = new Dictionary<Airport, int>();
        timer = Constants.instance.airportNewPassengersTimer;
    }

    void Update()
    {
        if (player.state != Player.GameState.Play)
            return;

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
        //if (!Active) return;
        var newPassengers = new Random().Next(Constants.instance.passengersMin, Constants.instance.passengersMax);

        if (passengers + newPassengers >= capacity)
        {
            player.ChangeState();
            return;
        }

        var airportIndex = new Random().Next(0, availableAirports.Count);

        if (airportIndex == availableAirports.IndexOf(this))
        {
            airportIndex = (airportIndex + 1) % availableAirports.Count;
        }
        var airportTo = availableAirports[airportIndex];

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
        //if (!Active) return;

        var airportsWithRoutesAndPassengersToGo = availableRoutes
            .Where(route => route.from == this || route.to == this)
            .Select(route =>
            {
                if (route.from == this) return route.to;
                if (route.to == this) return route.from;
                return null;
            }).Where(airport => AirportPassengerCountDictionary.Where(kvp => kvp.Value >= Constants.instance.passengersPerFlight)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                    .ContainsKey(airport)
            ).ToList();
        
        if (airportsWithRoutesAndPassengersToGo.Count == 0) return;

        var airportIndex = new Random().Next(0, airportsWithRoutesAndPassengersToGo.Count);
        var selectedAirport = airportsWithRoutesAndPassengersToGo[airportIndex];

        AirportPassengerCountDictionary[selectedAirport] -= Constants.instance.passengersPerFlight;

        if (AirportPassengerCountDictionary[selectedAirport] <= 0)
        {
            AirportPassengerCountDictionary.Remove(selectedAirport);
        }

        CreatePlane(selectedAirport);

        //Debug.Log("VOOU " + this.gameObject.name + " : " + selectedAirport.gameObject.name);
    }

    private void CreatePlane(Airport to)
    {
        var plane = ObjectPoolScript.instance.GetObjectForType("Plane", false).GetComponent<Plane>();

        plane.transform.parent = planeList.transform;
        plane.transform.position = transform.position;
        plane.from = this;
        plane.to = to;

        var direction = transform.position - to.transform.position;
        direction.z = 0.0f;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
        plane.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        availablePlanes.Add(plane);

        audioPlayer.PlayPlane();
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
