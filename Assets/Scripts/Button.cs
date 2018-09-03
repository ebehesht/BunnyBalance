using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

    public AudioClip prompt1;
    public AudioClip prompt2;
    public AudioClip prompt3;
    public AudioClip prompt4;
    public AudioClip prompt5;
    public int counter;

    public AudioSource promptSource;

    // Use this for initialization
    void Start()
    {
        promptSource = GetComponent<AudioSource>();
        counter = 0;
        
    }

        
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Next()
    {
        // reset the scene
        ResetGame();

        // go to next prompt
        counter++;
        if (counter == 6) counter = 1;
        switch (counter)
        {
            case 1:
                promptSource.clip = prompt1;
                break;
            case 2:
                promptSource.clip = prompt2;
                break;
            case 3:
                promptSource.clip = prompt3;
                break;
            case 4:
                promptSource.clip = prompt4;
                break;
            case 5:
                promptSource.clip = prompt5;
                break;
        }

        promptSource.Play();
    }

    public void ResetGame()
    {
        GameObject[] bunnies;
        bool changed = false;

        bunnies = GameObject.FindGameObjectsWithTag("Bunny");
        foreach (GameObject bunny in bunnies)
        {
            if (bunny.GetComponent<BunnyPlayer>().oldSeat != null)
            {
                bunny.GetComponent<BunnyPlayer>().RemoveBunny();
                changed = true;
            }

        }

        if (changed) GameObject.Find("Seesaw").GetComponent<Seesaw>().Move();



        //bunny = GameObject.Find("Bunny1");
        //bunny.GetComponent<BunnyPlayer>().ResetBunny();
        //bunny = GameObject.Find("Bunny2");
        //bunny.GetComponent<BunnyPlayer>().ResetBunny();
        //bunny = GameObject.Find("Bunny3");
        //bunny.GetComponent<BunnyPlayer>().ResetBunny();
        //bunny = GameObject.Find("Bunny4");
        //bunny.GetComponent<BunnyPlayer>().ResetBunny();
        //bunny = GameObject.Find("Bunny5");
        //bunny.GetComponent<BunnyPlayer>().ResetBunny();

        //GameObject.Find("Seesaw").GetComponent<Seesaw>().ResetSeesaw();

        //GameObject seat;

        //seat = GameObject.Find("RedSeatLeft");
        //seat.GetComponent<SeesawSeat>().ResetSeat();
        //seat = GameObject.Find("RedSeatRight");
        //seat.GetComponent<SeesawSeat>().ResetSeat();
        //seat = GameObject.Find("BlueSeatLeft");
        //seat.GetComponent<SeesawSeat>().ResetSeat();
        //seat = GameObject.Find("BlueSeatRight");
        //seat.GetComponent<SeesawSeat>().ResetSeat();

    }
}
