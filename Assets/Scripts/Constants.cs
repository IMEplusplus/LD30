using UnityEngine;
using System.Collections;

public class Constants : MonoBehaviour
{
    public static Constants instance { get; private set; }

    [SerializeField]
    SpriteRenderer circle;

    [HideInInspector]
    public Vector3 circleSize;

    public int airportCapacity = 1000;
    public float airportNewPassengersTimer = 1f;
    public float newAirportTimer = 30f;

    public Color routeLowColor = new Color(0.05f, 0.15f, 0.56f, 1f);
    public Color routeHighColor = new Color(1f, 0.56f, 0f, 1f);

    public Color circleInitialColor = new Color(0f, 0.93f, 1f, 1f);
    public Color circleFinalColor = new Color(1f, 0f, 0f, 1f);
    public float circleMinSize = 0.3f;

    public int passengersMin = 5;
    public int passengersMax = 10;
    public int passengersPerFlight = 50;

    void OnEnable()
    {
        instance = this;
    }

    void Start()
    {
        if (circle != null)
            circleSize = circle.bounds.size;
    }
}
