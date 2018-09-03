using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seesaw : MonoBehaviour {

    public string status = "balanced";
    public AudioClip seesawMoveSound;
    //public AudioClip madebuleheavy;
    //public AudioClip maderedheavy;
    //public AudioClip youbalanced;
    public AudioSource seesawSource;

    // Use this for initialization
    void Start () {
        seesawSource = GetComponent<AudioSource>();
        //seesawSource.clip = seesawMoveSound; //the audio clip for moving seesaw

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Move()
    {
        
        //HingeJoint2D seesawJoint = mySeat.transform.parent.GetComponent<HingeJoint2D>();
        //HingeJoint2D seesawJoint = GameObject.FindWithTag("Seesaw").GetComponent<HingeJoint2D>();
        HingeJoint2D seesawJoint = this.GetComponent<HingeJoint2D>();
        JointMotor2D thisMotor = seesawJoint.motor;
        JointAngleLimits2D limits = seesawJoint.limits;

        float speed = 0.0f;
        float seesawAngleMin = -5.0f;
        float seesawAngleMax = 5.0f;
        if (GlobalVariables.leftWeight > GlobalVariables.rightWeight)
        {// left side is heavier, but check if this has changed from previous status
            if (this.status != "leftTilted")
            {
                speed = -50.0f;
                seesawAngleMin = -5.0f;
                seesawAngleMax = 5.0f;
                this.status = "leftTilted";
            }

        }
        else if (GlobalVariables.leftWeight < GlobalVariables.rightWeight)
        { // right side is heavier, but check if this has changed from previous status
            if (this.status != "rightTilted")
            {
                speed = 50.0f;
                seesawAngleMin = -5.0f;
                seesawAngleMax = 5.0f;
                this.status = "rightTilted";
            }
        }
        else
        {// seesaw needs to be balanced
            if (this.status == "leftTilted")
            {
                speed = 50.0f;
                seesawAngleMin = -5.0f;
                seesawAngleMax = 0.0f;
                this.status = "balanced";

            }
            else
            { // it is tilted ot the right
                speed = -50.0f;
                seesawAngleMin = 0.0f;
                seesawAngleMax = 5.0f;
                this.status = "balanced";
            }

        }

        limits.min = seesawAngleMin;
        limits.max = seesawAngleMax;
        thisMotor.motorSpeed = speed;

        // play a sound if the seesaw is moving
        if (speed != 0.0)
        {
            seesawSource.PlayOneShot(seesawMoveSound, 1F);
            //seesawSource.Play();
            /*
            // play prompt sounds that says the seesaw is balances, left tilted, or right tilted
            AudioClip thisPrompt = null;
            switch (this.status)
            {
                case "balanced":
                    thisPrompt = youbalanced;
                    break;
                case "leftTilted":
                    thisPrompt = maderedheavy;
                    break;
                case "rightTilted":
                    thisPrompt = madebuleheavy;
                    break;
            }
            //seesawSource.Play();
            seesawSource.PlayOneShot(thisPrompt, 1F);
            */

        }

        seesawJoint.limits = limits;
        seesawJoint.motor = thisMotor;

        Debug.Log("speed: " + speed);

    }

    public void ResetSeesaw()
    {
        GlobalVariables.leftWeight = 0;
        GlobalVariables.rightWeight = 0;
        if (this.status != "balanced") this.Move();
    }
}
