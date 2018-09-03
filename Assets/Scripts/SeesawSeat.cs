using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeesawSeat : MonoBehaviour {
    public bool occupied;



    // Use this for initialization
    void Start () {
        this.occupied = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // collision
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider is BoxCollider2D) //bunny has two colliders, the box collider for seating on the seesaw
        {
            Debug.Log("Test: enter collision");
            GameObject bunnySeat = coll.gameObject.GetComponent<BunnyPlayer>().mySeat;
            if (bunnySeat == null)
            {
                coll.gameObject.GetComponent<BunnyPlayer>().mySeat = this.gameObject;
                if (this.tag == "LeftSeat")
                {
                    coll.gameObject.GetComponent<BunnyPlayer>().seesawSide = "Left";
                }
                else
                {
                    coll.gameObject.GetComponent<BunnyPlayer>().seesawSide = "Right";
                }

            }

        }

    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.collider is BoxCollider2D)
        {
            Debug.Log("Test: exit collision");
            coll.gameObject.GetComponent<BunnyPlayer>().mySeat = null;

        }


    }

    public void ResetSeat()
    {
        this.occupied = false;
    }


}
