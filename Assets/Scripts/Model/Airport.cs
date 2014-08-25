using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Airport : MonoBehaviour
{
    //public int id;
    static int MAX_CAPACITY = 1000;

    [SerializeField]
    private float CIRCLE_RADIUS = 0.3f;


    public List<Plane> PlaneList;

    public Dictionary<Airport, int> AirportPassengerCountDictionary;

    public int passengers = 0;
	public int capacity = MAX_CAPACITY;

    private GameObject pin, pinSelected;

    void Start()
    {
        pin = transform.FindChild("Pin").gameObject;
        pinSelected = transform.FindChild("Pin Select").gameObject;
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
