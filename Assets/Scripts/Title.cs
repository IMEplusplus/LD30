using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour
{
    private Player player;

    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
    }
	
	void Update () 
    {
        if (player.state != Player.GameState.Title)
            return;

        if (Input.anyKeyDown)
        {
            player.ChangeState();
        }
	}
}
