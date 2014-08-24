using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AirportList : MonoBehaviour
{
    public Airport[] airports;

    void Start()
    {
        airports = GetComponentsInChildren<Airport>();
    }
}
