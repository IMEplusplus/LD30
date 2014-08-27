using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Audio : MonoBehaviour
{
    public bool playing = false;

    private AudioSource bellSource;
    public AudioSource planesSource;
    public AudioSource voicesSource;

    public AudioClip bell;
    public AudioClip newAirport;
    public AudioClip overcrowd;
    public List<AudioClip> planes;
    public List<AudioClip> instructions;

    private Player player;

    private void RingBell()
    {
        bellSource.Play();
    }

    public void PlayNewAirport()
    {
        if (player.state != Player.GameState.Play)
            return;

        if (voicesSource.isPlaying)
            voicesSource.Stop();

        RingBell();
        Invoke("NewAirport", bell.length);
    }

    private void NewAirport()
    {
        if (player.state != Player.GameState.Play)
            return;

        voicesSource.clip = newAirport;
        voicesSource.Play();
    }

    public void PlayOvercrowded()
    {
        if (player.state != Player.GameState.Play)
            return;

        if (voicesSource.isPlaying)
            voicesSource.Stop();

        RingBell();
        Invoke("Overcrowded", bell.length);
    }

    private void Overcrowded()
    {
        if (player.state != Player.GameState.Play)
            return;
        voicesSource.clip = overcrowd;
        voicesSource.Play();
    }

    public void PlayPlane()
    {
        if (player.state != Player.GameState.Play)
            return;

        if (planesSource.isPlaying)
            return;

        if (Random.Range(0f, 1f) > Constants.instance.audioPlaneChance)
            return;

        var rand = Random.Range(0, planes.Count);
        planesSource.clip = planes[rand];
        planesSource.Play();
    }

    public void PlayInstructions()
    {
        if (player.state != Player.GameState.Play)
            return;

        if (bellSource.isPlaying || voicesSource.isPlaying)
        {
            playing = false;
            return;
        }

        RingBell();
        Invoke("Instructions", bell.length);

        Invoke("PlayInstructions", Random.Range(Constants.instance.audioRandomMin, Constants.instance.audioRandomMax));
    }

    private void Instructions()
    {
        if (player.state != Player.GameState.Play)
            return;

        var rand = Random.Range(0, instructions.Count);
        voicesSource.clip = instructions[rand];
        voicesSource.Play();
    }

    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        bellSource = GetComponent<AudioSource>();
        bellSource.clip = bell;
    }

    void Update()
    {
        if (player.state != Player.GameState.Play)
        {
            playing = false;
            return;
        }

        if (!playing)
        {
            if (player.state == Player.GameState.Play)
            {
                Invoke("PlayInstructions", Random.Range(Constants.instance.audioRandomMin, Constants.instance.audioRandomMax));
                playing = true;
            }
        }
    }
}
