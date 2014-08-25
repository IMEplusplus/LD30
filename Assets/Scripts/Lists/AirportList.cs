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

    private Player player;

    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();

        airports = GetComponentsInChildren<Airport>();
    }

    public void Reset()
    {
        timer = Constants.instance.newAirportTimer;

        available.Clear();
        hidden.Clear();

        foreach (Airport a in airports)
        {
            a.Reset();
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
        if (player.state != Player.GameState.Play)
            return;

        timer -= Time.fixedDeltaTime;
        if (timer <= 0f)
        {
            NewAirport();
            timer = Constants.instance.newAirportTimer;
        }
    }

    private void NewAirport()
    {
        if (hidden.Count <= 0) return;

        int rand = Random.Range(0, hidden.Count);
        hidden[rand].gameObject.SetActive(true);
        available.Add(hidden[rand]);
        hidden.RemoveAt(rand);
    }
}
