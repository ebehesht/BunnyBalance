using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyPlayer : MonoBehaviour {

    private Rigidbody2D bunnyBody;
    public GameObject mySeat = null;
    public GameObject oldSeat = null;
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

        if (mySeat != null)  //the bunny is on one seat
        {
            
            if (oldSeat == null) //if the bunny is on a seat, put the bunny on the seesaw
            {
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

                this.GetComponent<HingeJoint2D>().enabled = true;

                this.oldSeat = mySeat;

                seesaw.GetComponent<Seesaw>().Move();

                //seesawJoint.useMotor = true;
 
                //Debug.Log("my seat tag: " + mySeat.transform.tag);

            }
            else if (oldSeat.transform.tag != mySeat.transform.tag)
            { // if the bunny is moved from one side to the other
                Collider2D seatCollider = mySeat.GetComponent<Collider2D>();
                if (seatCollider.transform.tag == "LeftSeat")
                {
                    GlobalVariables.leftWeight += this.weight;
                    GlobalVariables.rightWeight -= this.weight;
                }
                else
                {
                    GlobalVariables.rightWeight += this.weight;
                    GlobalVariables.leftWeight -= this.weight;
                }


                //seesawJoint.useMotor = true;
                //this.GetComponent<HingeJoint2D>().enabled = true;
                this.oldSeat = mySeat;

                seesaw.GetComponent<Seesaw>().Move();

            }

        }
        else if (mySeat == null) //seatcollider is null
        {
            if (this.oldSeat != null)
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
                this.oldSeat = null;
                this.seesawSide = "None";

                seesaw.GetComponent<Seesaw>().Move();

            }
            else
            {
                Debug.Log("just a touch up event");
                //this.GetComponent<HingeJoint2D>().enabled = false;
            }
        }
        //if (touchedBunny.GetComponent<Collider2D>().IsTouching(seatCollider))

        Debug.Log("left weight: " + GlobalVariables.leftWeight);
        Debug.Log("right weight: " + GlobalVariables.rightWeight);

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
