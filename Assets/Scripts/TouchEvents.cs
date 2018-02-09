using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchEvents : MonoBehaviour {
    bool bunnyIsTapped;
    GameObject touchedBunny;
    Vector3 touchPosWorld;
    Vector3 offset;

	// Use this for initialization
	void Start () {
        bunnyIsTapped = false;
	}

    // Update is called once per frame
    void Update() {

        // TAP BUNNY //

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            //transform the touch position into world space from screen space and store it.
            touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

            //raycast with this information. If we have hit something we can process it.
            RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

            if (hitInformation.collider != null)
            {
                //We should have hit something with a 2D Physics collider!
                //touchedBunny should be the object someone touched.
                if (hitInformation.transform.gameObject.tag == "Bunny")
                {
                    touchedBunny = hitInformation.transform.gameObject;
                    bunnyIsTapped = true;
                    offset = touchPosWorld - touchedBunny.transform.position;
                    //Debug.Log("Touched " + touchedBunny.name);

                }
            }
        }

        // MOVE BUNNY //

        if (Input.touchCount > 0 && bunnyIsTapped && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            // Get movement of the finger since last frame
            touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Vector3 newBunnyPosition = touchPosWorld - offset;
            touchedBunny.transform.position = new Vector3(newBunnyPosition.x, newBunnyPosition.y, newBunnyPosition.z);
            //Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            //float speed = 0.1f;

            // Move object across XY plane
            //touchedBunny.transform.Translate(touchDeltaPosition.x * speed, touchDeltaPosition.y * speed, 0);

        }
    }

    void LateUpdate()
    {
        // END MOVE BUNNY & DETECT IF SAT ON A SEAT//
        if (Input.touchCount > 0 && bunnyIsTapped && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            bunnyIsTapped = false;

            //if (GlobalVariables.collidedSeat != null)
            if (touchedBunny.GetComponent<BunnyPlayer>().mySeat != null)
            { //put the bunny on the seesaw
                GameObject touchedSeat = touchedBunny.GetComponent<BunnyPlayer>().mySeat;
                Collider2D seatCollider = touchedSeat.GetComponent<Collider2D>();
                Debug.Log("bunny is touching the seat: " + seatCollider.transform.name);

                // calculate the weight and determine the seesaw new status
                if (seatCollider.transform.tag == "LeftSeat")
                {
                    GlobalVariables.leftWeight += touchedBunny.GetComponent<BunnyPlayer>().weight;
                }
                else
                {
                    GlobalVariables.rightWeight += touchedBunny.GetComponent<BunnyPlayer>().weight;
                }

                moveSeesaw();

                //seesawJoint.useMotor = true;
                touchedBunny.GetComponent<HingeJoint2D>().enabled = true;
                touchedBunny.GetComponent<BunnyPlayer>().isInAir = false;
            }
            else //seatcollider is null
            {
                if (!touchedBunny.GetComponent<BunnyPlayer>().isInAir)
                {
                    Debug.Log("recalculate the weight on seesaw");

                    if (touchedBunny.GetComponent<BunnyPlayer>().seesawSide == "Left")
                    {
                        GlobalVariables.leftWeight -= touchedBunny.GetComponent<BunnyPlayer>().weight;
                    }
                    else
                    {
                        GlobalVariables.rightWeight -= touchedBunny.GetComponent<BunnyPlayer>().weight;
                    }


                    //GameObject seesaw = GameObject.FindWithTag("Seesaw");
                    //HingeJoint2D seesawJoint = seesaw.GetComponent<HingeJoint2D>();
                    //JointMotor2D thisMotor = seesawJoint.motor;
                    //thisMotor.motorSpeed = thisMotor.motorSpeed * -1.0f;
                    //JointAngleLimits2D limits = seesawJoint.limits;
                    //limits.min = -20;
                    //seesawJoint.limits = limits;
                    //seesawJoint.motor = thisMotor;

                    touchedBunny.GetComponent<HingeJoint2D>().enabled = false;
                    touchedBunny.GetComponent<BunnyPlayer>().isInAir = true;
                    touchedBunny.GetComponent<BunnyPlayer>().seesawSide = "None";

                    moveSeesaw();

                }
                else
                {
                    Debug.Log("just a touch up event");
                }
            }
            //if (touchedBunny.GetComponent<Collider2D>().IsTouching(seatCollider))


        }
    }




 

    void moveSeesaw ()
    {
        //HingeJoint2D seesawJoint = touchedSeat.transform.parent.GetComponent<HingeJoint2D>();
        HingeJoint2D seesawJoint = GameObject.FindWithTag("Seesaw").GetComponent<HingeJoint2D>();
        JointMotor2D thisMotor = seesawJoint.motor;
        JointAngleLimits2D limits = seesawJoint.limits;

        float speed = 0.0f;
        float seesawAngle = 10.0f;
        if (GlobalVariables.leftWeight > GlobalVariables.rightWeight)
        {// left side is heavier, but check if this has changed from previous status
            if (GlobalVariables.seesawStatus != "leftTilted")
            {
                speed = -50.0f;
                GlobalVariables.seesawStatus = "leftTilted";
            }

        }
        else if (GlobalVariables.leftWeight < GlobalVariables.rightWeight)
        { // right side is heavier, but check if this has changed from previous status
            if (GlobalVariables.seesawStatus != "rightTilted")
            {
                speed = 50.0f;
                GlobalVariables.seesawStatus = "rightTilted";
            }
        }
        else
        {// seesaw needs to be balanced
            if (GlobalVariables.seesawStatus == "leftTilted")
            {
                speed =50.0f;
                seesawAngle = 0.0f;
                GlobalVariables.seesawStatus = "balanced";

            }
            else
            { // it is tilted ot the right
                speed = -50.0f;
                seesawAngle = 0.0f;
                GlobalVariables.seesawStatus = "balanced";
            }

        }

        limits.min = -1.0f * seesawAngle;
        limits.max = seesawAngle;
        thisMotor.motorSpeed = speed;
        seesawJoint.limits = limits;
        seesawJoint.motor = thisMotor;

        Debug.Log("speed: " + speed);
        Debug.Log("left weight: " + GlobalVariables.leftWeight);
        Debug.Log("right weight: " + GlobalVariables.rightWeight);
    }
}
