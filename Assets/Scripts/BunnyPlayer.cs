using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyPlayer : MonoBehaviour {

    private Rigidbody2D bunnyBody;
    public GameObject mySeat;
    public bool isInAir = true;
    public int weight;
    public string seesawSide = "None";
    //public bool bunnyHasToMove = false; //not using this variable!

	// Use this for initialization
	void Start () {
        // the rigid body of the current bunny
        bunnyBody = GetComponent<Rigidbody2D>();
        mySeat = null;

        switch (this.name)
        {
            case "Bunny1":
                this.weight = 1;
                break;
            case "Bunny2":
                this.weight = 2;
                break;
            case "Bunny3":
                this.weight = 3;
                break;
            case "Bunny4":
                this.weight = 4;
                break;
            case "Bunny5":
                this.weight = 5;
                break;

        }
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
         * //instead of this, I used layer-based collision detection
        if (collision.gameObject.tag == "Bunny")
        {
            var colliderToIgnore = collision.gameObject.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(colliderToIgnore, this.GetComponent<Collider2D>());
        }
        */
    }

    public void printFunction()
    {
        //print("Touch down" + bunnyBody.tag);
    }
}
