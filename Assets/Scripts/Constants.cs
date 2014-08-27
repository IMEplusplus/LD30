using UnityEngine;
using System.Collections;

public class Constants : MonoBehaviour
{
    public static Constants instance { get; private set; }

    [SerializeField]
    SpriteRenderer circle, line;

    [HideInInspector]
    public Vector3 circleSize;
    [HideInInspector]
    public Vector3 lineSize;

    public int airportQntInitial = 5;
    public int airportCapacity = 1000;

    public float airportMinCircle = 0.3f;
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

    public float planeArrivalDistance = 0.3f;
    // Plane velocity has to be less than planeArrivalDistance!!!
    public float planeVelocity = 0.3f;

    public Vector3 labelPosition = new Vector3(0.08f, 0.048f, 0f);
    public Vector3 namePosition = new Vector3(0.06f, 0.048f, 1f);
    public Vector3 passengersPosition = new Vector3(0.11f, 0.048f, 1f);
    public float labelDeltaY = -0.028f;

    public float audioRandomMin = 15f;
    public float audioRandomMax = 25f;
    public float audioPlaneChance = .20f;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (circle != null)
            circleSize = circle.bounds.size;
        if (line != null)
            lineSize = line.bounds.size;
    }
}
