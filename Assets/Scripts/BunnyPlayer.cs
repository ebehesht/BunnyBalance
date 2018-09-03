using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyPlayer : MonoBehaviour {

    private Rigidbody2D bunnyBody;
    public GameObject mySeat = null;
    public GameObject oldSeat = null;
    public int weight;
    public float initX = 0.0f , initY = 0.0f;
    public string seesawSide = "None";
    public AudioClip bunnySound;
    public AudioSource soundSource;
    // these are baselines for bunny1, for other bunnies need to change them.
    public Vector3 onSeatPosBalanced = new Vector3(-4.55f, 2.3f, 0.0f);
    public Vector3 onSeatPosDown = new Vector3(-4.63f, 2.3f, 0.0f);
    public Vector3 onSeatPosUp = new Vector3(-4.45f, 2.3f, 0.0f);

    // Use this for initialization
    void Start () {
        // the rigid body of the current bunny
        bunnyBody = GetComponent<Rigidbody2D>();
        mySeat = null;
        this.gameObject.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        switch (this.name)
        {
            case "Bunny1":
                this.weight = 1;
                this.initX = -5.25f;
                this.initY = -3.0f;
                break;
            case "Bunny2":
                this.weight = 2;
                this.initX = -3.19f;
                this.initY = -2.9f;
                break;
            case "Bunny3":
                this.weight = 3;
                this.initX = -0.79f;
                this.initY = -2.8f;
                this.onSeatPosBalanced += new Vector3(0.0f, 0.2f, 0.0f);
                this.onSeatPosDown += new Vector3(0.0f, 0.2f, 0.0f);
                this.onSeatPosUp += new Vector3(0.0f, 0.2f, 0.0f);
                break;
            case "Bunny4":
                this.weight = 4;
                this.initX = 1.93f;
                this.initY = -2.75f;
                break;
            case "Bunny5":
                this.weight = 5;
                this.initX = 4.85f;
                this.initY = -2.65f;
                this.onSeatPosBalanced += new Vector3(0.0f, 0.4f, 0.0f);
                this.onSeatPosDown += new Vector3(0.0f, 0.4f, 0.0f);
                this.onSeatPosUp += new Vector3(0.0f, 0.4f, 0.0f);
                break;

        }


        // place the bunny on its initial position
        this.gameObject.transform.position += new Vector3(initX, initY, 0);
         
        

        soundSource.clip = bunnySound;

        
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void TouchUp()
    {
        GameObject seesaw = GameObject.FindWithTag("Seesaw");

        if (mySeat == null && oldSeat == null) // it's just a touch-up event
        {
            Debug.Log("just a touch up event");
            this.gameObject.transform.position = new Vector3(initX, initY, 0.0f);
            //this.GetComponent<HingeJoint2D>().enabled = false;
        }
        else if (mySeat == null && oldSeat != null) //bunny is removed from the seesaw
        {
            Debug.Log("recalculate the weight on seesaw");
            RemoveBunny();
            seesaw.GetComponent<Seesaw>().Move();

        }
        else //mySeat is not null! so put bunny on the seat if unoccupied
        {
            Collider2D seatCollider = mySeat.GetComponent<Collider2D>();
            bool seatIsOccupied = seatCollider.GetComponent<SeesawSeat>().occupied;
            if (seatIsOccupied)
            {
                this.gameObject.transform.position = new Vector3(initX, initY, 0.0f);
                return;
            }
            else { // seat is not occupied
                soundSource.Play();
                seatCollider.GetComponent<SeesawSeat>().occupied = true;

                if (oldSeat == null) //if the bunny is on a seat for the first time, put the bunny on the seesaw
                {
                    SeatBunny(seatCollider);
                    seesaw.GetComponent<Seesaw>().Move();
                }

                else if (oldSeat.transform.tag != mySeat.transform.tag)
                { // if the bunny is moved from one side to the other
                    Debug.Log("Hey! The bunny is moved to another seat");
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
                    this.oldSeat = mySeat;

                    seesaw.GetComponent<Seesaw>().Move();

                }

            }

        }

        //if (touchedBunny.GetComponent<Collider2D>().IsTouching(seatCollider))

        //Debug.Log("left weight: " + GlobalVariables.leftWeight);
        //Debug.Log("right weight: " + GlobalVariables.rightWeight);

    }

    public void SeatBunny(Collider2D seatCollider)
    {
        Vector3 scaleV; 
        // duplicate a bunny in the initial position on grass
        Instantiate(this.gameObject);
        //Debug.Log("bunny is touching the seat: " + seatCollider.transform.name);

        // calculate the weight and determine the seesaw new status
        if (seatCollider.transform.tag == "LeftSeat")
        {
            GlobalVariables.leftWeight += this.weight;
            scaleV = new Vector3(1, 1, 0);
        }

        else
        {
            GlobalVariables.rightWeight += this.weight;
            scaleV = new Vector3(-1, 1, 0);
       }

        // reposition the bunny

        if (GlobalVariables.leftWeight < GlobalVariables.rightWeight) // right tilted
        {
            if (seatCollider.transform.tag == "LeftSeat")
            {
                this.gameObject.transform.position = Vector3.Scale(this.onSeatPosUp, scaleV);
            }
            else
            {
                this.gameObject.transform.position = Vector3.Scale(this.onSeatPosDown, scaleV);
            }                

        }
        else if (GlobalVariables.leftWeight > GlobalVariables.rightWeight)
        {
            if (seatCollider.transform.tag == "LeftSeat")
            {
                this.gameObject.transform.position = Vector3.Scale(this.onSeatPosDown, scaleV);
            }
            else
            {
                this.gameObject.transform.position = Vector3.Scale(this.onSeatPosUp, scaleV);
            }


        }
        else //left and weight are equal
        {
            this.gameObject.transform.position = Vector3.Scale(this.onSeatPosBalanced, scaleV);

        }

        Vector3 seatPos = mySeat.transform.position;
        Debug.Log("seat is:" + mySeat.name + " and position is:" + this.transform.position);
        //this.gameObject.transform.position = this.mySeat.transform.position;
        this.GetComponent<HingeJoint2D>().enabled = true;



        this.oldSeat = mySeat;

    }

    public void RemoveBunny()
    {
        // make the seat tagged as unoccupied
        // mySeat is already null, so update the oldSeat, which is not null yet.
        Collider2D seatCollider = oldSeat.GetComponent<Collider2D>();
        seatCollider.GetComponent<SeesawSeat>().occupied = false;

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

        this.gameObject.transform.position = new Vector3(initX, initY, 0.0f);


    }

    public void ResetBunny()
    {
        // place the bunny on its initial position
        this.gameObject.transform.position = new Vector3(initX, initY, 0);
        this.mySeat = null;
        this.oldSeat = null;
        this.seesawSide = "None";

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
