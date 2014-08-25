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
    public float airportMinCircle = 0.2f;

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
