using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour
{
    private Player player;

    private GUIText passengers;
    private GUIText time;

    void OnEnable()
    {
        if (passengers)
        {
            passengers.text = player.passengers.ToString();
            passengers.text += " P";
        }

        if (time)
        {
            time.text = player.timer.ToString("0.00");
            time.text += " s";
        }
    }

    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        passengers = transform.FindChild("Passengers").GetComponent<GUIText>();
        time = transform.FindChild("Time").GetComponent<GUIText>();
    }

    void Update()
    {
        if (player.state != Player.GameState.GameOver)
            return;

        if (Input.anyKeyDown)
        {
            player.ChangeState();
        }
    }
}
