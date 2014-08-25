using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AirportList : MonoBehaviour
{
    public int initialAirportsQuantity = 5;

    public Airport[] airports;
    public List<Airport> available;
    public List<Airport> hidden;

    public float timer = 30f;

    void Start()
    {
        timer = Constants.instance.newAirportTimer;

        airports = GetComponentsInChildren<Airport>();
        available.Clear();

        foreach (Airport a in airports)
        {
            a.gameObject.SetActive(false);
            hidden.Add(a);
        }

        for (int i = 0; i < initialAirportsQuantity; ++i)
        {
            int rand = Random.Range(0, hidden.Count);
            hidden[rand].gameObject.SetActive(true);
            available.Add(hidden[rand]);
            hidden.RemoveAt(rand);
        }
    }

    void FixedUpdate()
    {
        timer -= Time.fixedDeltaTime;
        if (timer <= 0f)
        {
            NewAirport();
            timer = Constants.instance.newAirportTimer;
        }
    }

    private void NewAirport()
    {
        int rand = Random.Range(0, hidden.Count);
        hidden[rand].gameObject.SetActive(true);
        available.Add(hidden[rand]);
        hidden.RemoveAt(rand);
    }
}
