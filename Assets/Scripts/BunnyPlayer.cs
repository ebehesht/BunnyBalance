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
                this.initX = -5.66f;
                this.initY = -2.18f;
                break;
            case "Bunny2":
                this.weight = 2;
                this.initX = -3.29f;
                this.initY = -2.19f;
                break;
            case "Bunny3":
                this.weight = 3;
                this.initX = -0.59f;
                this.initY = -2.16f;
                break;
            case "Bunny4":
                this.weight = 4;
                this.initX = 2.02f;
                this.initY = -2.18f;
                break;
            case "Bunny5":
                this.weight = 5;
                this.initX = 4.67f;
                this.initY = -2.13f;
                break;

        }


        // place the bunny on its initial position
        this.gameObject.transform.position += new Vector3(initX, initY, 0);
         
        

        soundSource.clip = bunnySound;
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void touchUp()
    {
        GameObject seesaw = GameObject.FindWithTag("Seesaw");

        if (mySeat == null && oldSeat == null) // it's just a touch-up event
        {
            Debug.Log("just a touch up event");
            this.gameObject.transform.position = new Vector3(initX, initY, 0.0f);
            //this.GetComponent<HingeJoint2D>().enabled = false;
        }
        else if (mySeat == null && oldSeat != null)
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

            this.gameObject.transform.position = new Vector3(initX, initY, 0.0f);
            seesaw.GetComponent<Seesaw>().Move();

        }
        else //mySeat is not null!
        {
            Collider2D seatCollider = mySeat.GetComponent<Collider2D>();
            bool seatIsOccupied = seatCollider.GetComponent<SeesawSeat>().occupied;
            if (seatIsOccupied)
            {
                seatIsOccupied = false;
                this.gameObject.transform.position = new Vector3(initX, initY, 0.0f);
                return;
            }
            else {
                soundSource.Play();
                seatIsOccupied = true;

                if (oldSeat == null) //if the bunny is on a seat for the first time, put the bunny on the seesaw
                {

                    // duplicate a bunny in the initial position on grass
                    Instantiate(this.gameObject);
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
