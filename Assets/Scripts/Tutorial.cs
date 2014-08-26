using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tutorial : MonoBehaviour
{
    int step = 0;

    public List<GameObject> pages;

    private Player player;

    void OnEnable()
    {
        step = 0;
        pages[0].SetActive(true);
    }

	void Start ()
    {
        player = GameObject.FindObjectOfType<Player>();

        foreach (var page in pages)
            page.SetActive(false);

        step = 0;
        pages[0].SetActive(true);
	}
	
	void Update ()
    {
        if (player.state != Player.GameState.Tutorial)
            return;

        if (Input.anyKeyDown)
        {
            NextPage();
        }
	}

    private void NextPage()
    {
        pages[step].SetActive(false);

        step++;
        if (step >= pages.Count)
        {
            step = 0;
            player.ChangeState();
            return;
        }

        pages[step].SetActive(true);
    }
}
