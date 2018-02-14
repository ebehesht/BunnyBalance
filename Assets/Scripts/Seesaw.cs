﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seesaw : MonoBehaviour {

    public string status = "balanced";

    // Use this for initialization
    void Start () {
		
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
        float seesawAngleMin = -10.0f;
        float seesawAngleMax = 10.0f;
        if (GlobalVariables.leftWeight > GlobalVariables.rightWeight)
        {// left side is heavier, but check if this has changed from previous status
            if (this.status != "leftTilted")
            {
                speed = -50.0f;
                seesawAngleMin = -10.0f;
                seesawAngleMax = 10.0f;
                this.status = "leftTilted";
            }

        }
        else if (GlobalVariables.leftWeight < GlobalVariables.rightWeight)
        { // right side is heavier, but check if this has changed from previous status
            if (this.status != "rightTilted")
            {
                speed = 50.0f;
                seesawAngleMin = -10.0f;
                seesawAngleMax = 10.0f;
                this.status = "rightTilted";
            }
        }
        else
        {// seesaw needs to be balanced
            if (this.status == "leftTilted")
            {
                speed = 50.0f;
                seesawAngleMin = -10.0f;
                seesawAngleMax = 0.0f;
                this.status = "balanced";

            }
            else
            { // it is tilted ot the right
                speed = -50.0f;
                seesawAngleMin = 0.0f;
                seesawAngleMax = 10.0f;
                this.status = "balanced";
            }

        }

        limits.min = seesawAngleMin;
        limits.max = seesawAngleMax;
        thisMotor.motorSpeed = speed;
        seesawJoint.limits = limits;
        seesawJoint.motor = thisMotor;

        Debug.Log("speed: " + speed);
        Debug.Log("left weight: " + GlobalVariables.leftWeight);
        Debug.Log("right weight: " + GlobalVariables.rightWeight);
    }
}
