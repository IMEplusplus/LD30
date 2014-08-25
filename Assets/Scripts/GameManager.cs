using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private Player player;

    public GameObject title;
    public GameObject tutorial;
    public GameObject gameOver;
	
	void Start ()
    {
        player = GameObject.FindObjectOfType<Player>();
	}
	
	public void StateChanged ()
    {
        switch (player.state)
        {
            case Player.GameState.Title:
                if (title) title.SetActive(true);
                if (tutorial) tutorial.SetActive(false);
                if (gameOver) gameOver.SetActive(false);
                break;
            case Player.GameState.Tutorial:
                if (title) title.SetActive(false);
                if (tutorial) tutorial.SetActive(true);
                if (gameOver) gameOver.SetActive(false);
                break;
            case Player.GameState.Play:
                if (title) title.SetActive(false);
                if (tutorial) tutorial.SetActive(false);
                if (gameOver) gameOver.SetActive(false);
                break;
            case Player.GameState.GameOver:
                if (title) title.SetActive(false);
                if (tutorial) tutorial.SetActive(false);
                if (gameOver) gameOver.SetActive(true);
                break;
        }
	}
}
