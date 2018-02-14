using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyPlayer : MonoBehaviour {

    private Rigidbody2D bunnyBody;
    public GameObject mySeat;
    public bool isInAir = true;
    public int weight;
    public string seesawSide = "None";

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

    public void touchUp()
    {
        GameObject seesaw = GameObject.FindWithTag("Seesaw");

        if (mySeat != null)
        { //if the bunny is on a seat, put the bunny on the seesaw
            Collider2D seatCollider = mySeat.GetComponent<Collider2D>();
            Debug.Log("bunny is touching the seat: " + seatCollider.transform.name);

            // calculate the weight and determine the seesaw new status
            if (seatCollider.transform.tag == "LeftSeat")
            {
                GlobalVariables.leftWeight += this.weight;
            }
            else
            {
                GlobalVariables.rightWeight += this.weight;
            }

            seesaw.GetComponent<Seesaw>().Move();

            //seesawJoint.useMotor = true;
            this.enabled = true;
            this.isInAir = false;
        }
        else //seatcollider is null
        {
            if (!this.isInAir)
            {
                Debug.Log("recalculate the weight on seesaw");

                if (this.seesawSide == "Left")
                {
                    GlobalVariables.leftWeight -= this.weight;
                }
                else
                {
                    GlobalVariables.rightWeight -= this.weight;
                }

                this.GetComponent<HingeJoint2D>().enabled = false;
                this.isInAir = true;
                this.seesawSide = "None";

                seesaw.GetComponent<Seesaw>().Move();

            }
            else
            {
                Debug.Log("just a touch up event");
            }
        }
        //if (touchedBunny.GetComponent<Collider2D>().IsTouching(seatCollider))

    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
         * //instead of this, I used layer-based collision detection
        if (collision.gameObject.tag == "Bunny")
        {
            var colliderToIgnore = collision.gameObject.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(colliderToIgnore, this.GetComponent<Collider2D>());
        }
        
    }
    */
}
