using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Gyroscope gyro;
    public int lives = 3;

    [Header("Crossing")]
    public float laneCrossingTime = 1.0f;
    public float laneCrossingCoolDown = 1.0f;
    private float laneCrossingCoolDownTime = 0f;
    public bool allowMovWhileJumping = false;

    [Space(10)]
    [Header("Jump/Crouch")]
    public float lowAltitude = -1;
    public float middleAltitude = 0;
    public float highAltitude = 1.5f;
    private float jumpStartTime;
    public float jumpTime = 1;
    public float crouchTime = 1;
    public float jumpFloatingTime = 1;
    public float crouchFloatingTime = 1;
    public float noClickZone = 100f;
        
    [HideInInspector]
    public float invulnerableEndTime;

    private bool crossing = false;
    private bool jumping = false;
    private bool crouching = false;
    private int lane;
    private int targetLane;
    private float crossStartTime;

    [Space(10)]
    [Header("Input")]
    [Range(0,1)]
    public float screenPercentageToClick = .3f;
	// Use this for initialization
	void Start () {
        lane = GameController.instance.numLanes / 2 - 1;
        transform.position = new Vector3(GameController.instance.lanes[lane],transform.position.y, transform.position.z);
    }

    private void Update()
    {
        Quaternion lookRotation = new Quaternion();
        if (Input.touchCount > 0)
        {
            lookRotation = Quaternion.LookRotation(Input.touches[0].deltaPosition);
        }
        //Debug.Log(lookRotation);
        if (!crossing && (!jumping || allowMovWhileJumping) && laneCrossingCoolDownTime + laneCrossingCoolDown < Time.time)
        {
            
            //maybe better swipe detection later
            float swipeHor = Input.touchCount > 0 ? Input.touches[0].deltaPosition.x : 0;
            bool possibleClick = Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended && Input.touches[0].deltaPosition.sqrMagnitude < 1f && Input.touches[0].position.y > noClickZone;
            //right
            if ((swipeHor > 10f || (possibleClick && Input.touches[0].position.x > (1- screenPercentageToClick) * Screen.width) || Input.GetAxis("Horizontal") > 0.1f) && lane < GameController.instance.numLanes - 1)
            {
                crossing = true;
                targetLane = lane + 1;
                crossStartTime = Time.time;
            }
            //left
            else if ((swipeHor < -10f || (possibleClick && Input.touches[0].position.x < screenPercentageToClick * Screen.width) || Input.GetAxis("Horizontal") < -0.1f) && lane > 0)
            {
                crossing = true;
                targetLane = lane - 1;
                crossStartTime = Time.time;

            }
        }
        if (crossing)
        {
            if((Time.time - crossStartTime) * GameController.instance.gameSpeed < laneCrossingTime )
            {
                transform.position = new Vector3(Mathf.Lerp(GameController.instance.lanes[lane], GameController.instance.lanes[targetLane], (Time.time - crossStartTime ) * GameController.instance.gameSpeed / laneCrossingTime),transform.position.y ,transform.position.z );
            }
            else
            {
                transform.position = new Vector3(GameController.instance.lanes[targetLane], transform.position.y, transform.position.z);
                lane = targetLane;
                crossing = false;
                laneCrossingCoolDownTime = Time.time;
            }
        }
        if(!jumping && !crouching)
        {
            if((Input.touchCount > 0 && Input.touches[0].deltaPosition.y > 10f) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow))
            {
                jumping = true;
                jumpStartTime = Time.time;
            }
        }
        if(jumping)
        {
            Vector3 pos = transform.position;
            //going up
            if( (Time.time - jumpStartTime) * GameController.instance.gameSpeed < jumpTime )
            {
                pos.z = Mathf.Lerp(middleAltitude, highAltitude, (Time.time - jumpStartTime) * GameController.instance.gameSpeed) / jumpTime;
            }
            //floating
            else if((Time.time - jumpStartTime) * GameController.instance.gameSpeed < (jumpTime + jumpFloatingTime))
            {
                if(Input.GetKey(KeyCode.DownArrow))
                {
                    jumpStartTime = Time.time - (jumpTime + jumpFloatingTime) / GameController.instance.gameSpeed;
                }
                pos.z = highAltitude;
            }
            //falling
            else if((Time.time - jumpStartTime) * GameController.instance.gameSpeed - (jumpTime + jumpFloatingTime) <  jumpTime)
            {
                pos.z = Mathf.Lerp(highAltitude, middleAltitude, ((Time.time - jumpStartTime) * GameController.instance.gameSpeed - (jumpTime + jumpFloatingTime ))/ jumpTime);
            }
            else
            {
                pos.z = middleAltitude;
                jumping = false;
            }
            transform.position = pos;
        }
        if(!crouching && !jumping)
        {
            if ((Input.touchCount > 0 && Input.touches[0].deltaPosition.y < -10f) || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.DownArrow))
            {
                crouching = true;
                jumpStartTime = Time.time;
            }
        }
        if(crouching)
        {
            Vector3 pos = transform.position;
            //going up
            if ((Time.time - jumpStartTime) * GameController.instance.gameSpeed < crouchTime)
            {
                pos.z = Mathf.Lerp(middleAltitude, lowAltitude, (Time.time - jumpStartTime) * GameController.instance.gameSpeed) / crouchTime;
            }
            //floating
            else if ((Time.time - jumpStartTime) * GameController.instance.gameSpeed < (jumpTime + crouchFloatingTime))
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    jumpStartTime = Time.time - (crouchTime + crouchFloatingTime) / GameController.instance.gameSpeed;
                }
                pos.z = lowAltitude;
            }
            //falling
            else if ((Time.time - jumpStartTime) * GameController.instance.gameSpeed - (crouchTime + crouchFloatingTime) < crouchTime)
            {
                pos.z = Mathf.Lerp(lowAltitude, middleAltitude, ((Time.time - jumpStartTime) * GameController.instance.gameSpeed - (crouchTime + crouchFloatingTime)) / crouchTime);
            }
            else
            {
                pos.z = middleAltitude;
                crouching = false;
            }
            transform.position = pos;
        }
    }

    public void LooseLive()
    {
        if (Time.time > invulnerableEndTime)
        {
            GameController.instance.looseLive();
        }
    }

    public void AddLive()
    {
        GameController.instance.addLive();
    }


    private void SwipeDetection()
    {
        
    }
    
}
