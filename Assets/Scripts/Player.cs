using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public enum GameState
    {
        Title,
        Tutorial,
        Play,
        GameOver
    }

    public GameState state = GameState.Title;

    public int passengers = 0;
    public float timer = 0f;

    AirportList airports;
    RouteList routes;
    PlaneList planes;

    GameManager gm;

    void Start()
    {
        airports = GameObject.FindObjectOfType<AirportList>();
        routes = GameObject.FindObjectOfType<RouteList>();
        planes = GameObject.FindObjectOfType<PlaneList>();
        gm = GameObject.FindObjectOfType<GameManager>();

        passengers = 0;
        timer = 0f;

        gm.StateChanged();
    }

    private void Reset()
    {
        passengers = 0;
        timer = 0f;

        airports.Reset();
        routes.Reset();
        planes.Reset();
    }

    void FixedUpdate()
    {
        if (state == GameState.Play)
        {
            timer += Time.deltaTime;
        }
    }

    public void ChangeState()
    {
        switch (state)
        {
            case GameState.Title:
                state = GameState.Tutorial;
                break;
            case GameState.Tutorial:
                Reset();
                state = GameState.Play;
                break;
            case GameState.Play:
                state = GameState.GameOver;
                break;
            case GameState.GameOver:
                state = GameState.Tutorial;
                break;
        }

        gm.StateChanged();
    }

    public void AddPassengers(int count)
    {
        passengers += count;
    }
}
