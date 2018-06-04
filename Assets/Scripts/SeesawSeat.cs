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
        //Debug.Log("collision enter");
        //GlobalVariables.collidedSeat = this.gameObject;
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
            
            Debug.Log("collision enter: " + coll.gameObject.GetComponent<BunnyPlayer>().mySeat.transform.name);

        }
        // I don't think this code is necessary!
        else if (bunnySeat.transform.name != this.gameObject.transform.name)
        {
            Debug.Log("this seat is: " + bunnySeat.transform.name);
        }

        
        //GameObject collidedBunny = coll.gameObject;
        //collidedBunny.GetComponent<BunnyPlayer>().isBunnyOnSeat = true;

        //coll.gameObject.transform.parent = this.transform.parent;
        //Rigidbody2D seesawBody = this.transform.parent.GetComponent<Rigidbody2D>();
        //Rigidbody2D collidedBunnyRB = coll.gameObject.GetComponent<Rigidbody2D>();        
        //HingeJoint2D seesawJoint = this.transform.parent.GetComponent<HingeJoint2D>();
        //seesawJoint.useMotor = true;
 
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        //GlobalVariables.collidedSeat = null;
        coll.gameObject.GetComponent<BunnyPlayer>().mySeat = null;
        Debug.Log("collision exit");
        //GameObject collidedBunny = coll.gameObject;
        //collidedBunny.GetComponent<BunnyPlayer>().isBunnyOnSeat = false;
        //HingeJoint2D seesawJoint = this.transform.parent.GetComponent<HingeJoint2D>();
        //seesawJoint.useMotor = false;
    }


}
