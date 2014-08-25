using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AirportInfo : MonoBehaviour
{
    private AirportList airports;

    [SerializeField]
    private GUIText airportName;

    [SerializeField]
    private GUITexture airportBG;

    private List<GUITexture> labels = new List<GUITexture>();
    private List<GUIText> names = new List<GUIText>();
    private List<GUIText> passengers = new List<GUIText>();

    void Start()
    {
        airports = GameObject.FindObjectOfType<AirportList>();
    }

	void Update () 
    {
        ShowAirportInfo();
	}

    void ShowAirportInfo()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (airports.available.All(airport => !airport.MouseOver(mousePosition)))
        {
            Visible(false);
            return;
        }

        Visible(true);

        var selectedAirport = airports.available.First(airport => airport.MouseOver(mousePosition));

        var newPos = Camera.main.WorldToViewportPoint(selectedAirport.transform.position);
        newPos.z = 0f;
        transform.position = newPos;

        airportName.text = selectedAirport.name.ToUpper();

        var destinations = selectedAirport.AirportPassengerCountDictionary
            .OrderByDescending(kvp => kvp.Value);

        int i = 0;
        foreach (var dest in destinations)
        {
            // If dont have enough objects, use the pool
            if (i > labels.Count - 1)
            {
                labels.Add(ObjectPoolScript.instance.GetObjectForType("Label", false).GetComponent<GUITexture>());
                names.Add(ObjectPoolScript.instance.GetObjectForType("Label Name", false).GetComponent<GUIText>());
                passengers.Add(ObjectPoolScript.instance.GetObjectForType("Label Passengers", false).GetComponent<GUIText>());

                labels[i].transform.parent = transform;
                newPos = Constants.instance.labelPosition;
                newPos.y += Constants.instance.labelDeltaY * i;
                labels[i].transform.localPosition = newPos;

                names[i].transform.parent = transform;
                newPos = Constants.instance.namePosition;
                newPos.y += Constants.instance.labelDeltaY * i;
                names[i].transform.localPosition = newPos;

                passengers[i].transform.parent = transform;
                newPos = Constants.instance.passengersPosition;
                newPos.y += Constants.instance.labelDeltaY * i;
                passengers[i].transform.localPosition = newPos;
            }

            names[i].text = dest.Key.name;
            passengers[i].text = dest.Value.ToString();
            passengers[i].text += " P";

            labels[i].gameObject.SetActive(true);
            names[i].gameObject.SetActive(true);
            passengers[i].gameObject.SetActive(true);

            i++;
        }
    }

    private void Visible(bool visible)
    {
        airportName.gameObject.SetActive(visible);
        airportBG.gameObject.SetActive(visible);

        foreach (var label in labels)
            label.gameObject.SetActive(false);
        foreach (var text in names)
            text.gameObject.SetActive(false);
        foreach (var text in passengers)
            text.gameObject.SetActive(false);
    }
}
