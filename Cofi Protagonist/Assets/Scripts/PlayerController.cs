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
    public int maxCrossingsWhileJumping = 1;
    private int crossingsWhileJumping = 0;

    [Space(10)]
    [Header("Jump/Crouch")]
    public float lowAltitude = -1;
    public float middleAltitude = 0;
    public float highAltitude = 1.5f;
    private float jumpStartTime;
    public float jumpTime = 1;
    public float crouchTime = 1;
    public float jumpFloatingTime = 1;
    [HideInInspector]
    public float crouchFloatingTime = 1;
    public float noClickZone = 100f;
    public float controlMovementVerticalAngle = 0.3f;
    public float controlMovementHorizontalAngle = 0.3f;
        
    [HideInInspector]
    public float invulnerableEndTime;

    private bool crossing = false;
    private bool jumping = false;
    private bool crouching = false;
    private bool crouchingUp = false;
    private int lane;
    private int targetLane;
    private float crossStartTime;

    [Space(10)]
    [Header("Input")]
    [Range(0,1)]
    public float screenPercentageToClick = .3f;

    [Space(10)]
    [Header("Sounds")]
    [HideInInspector]
    public AudioSource audioSource;
    public AudioClip audioHurt;
    public AudioClip audioJump;
    public AudioClip audioCrouch;
    public AudioClip audioMov;


    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
        lane = LevelController.instance.numLanes / 2;
        transform.localPosition = new Vector3(LevelController.instance.lanes[lane],transform.localPosition.y, transform.localPosition.z);
        //needed to avoid keyboard input leaking when changing scenes (might also affect touch, no idea)
        Input.ResetInputAxes();
    }

    private void Update()
    {
        float angle = 0;
        if (Input.touchCount > 0)
        {
            angle = Mathf.Atan2(Input.touches[0].deltaPosition.x, Input.touches[0].deltaPosition.y);
        }

        bool inputUpSwipe = Input.touchCount > 0 && Input.touches[0].deltaPosition.y > 10f && angle > -Mathf.PI * controlMovementVerticalAngle && angle < Mathf.PI * controlMovementVerticalAngle;
        bool inputDownSwipe = Input.touchCount > 0 && Input.touches[0].deltaPosition.y < -10f && (angle > Mathf.PI * (1f - controlMovementVerticalAngle) || angle < -Mathf.PI * (1f - controlMovementVerticalAngle));
        //maybe better swipe detection later
        float swipeHor = Input.touchCount > 0 ? Input.touches[0].deltaPosition.x : 0;
        bool inputLeftSwipe = swipeHor < -10f && -angle > Mathf.PI * (0.5f - controlMovementHorizontalAngle) && -angle < Mathf.PI * (0.5f + controlMovementHorizontalAngle);
        bool inputRightSwipe = (swipeHor > 10f && angle > Mathf.PI * (0.5f - controlMovementHorizontalAngle)) && angle < Mathf.PI * (0.5 + controlMovementHorizontalAngle);


        if (!crossing && (!jumping || crossingsWhileJumping < maxCrossingsWhileJumping) && laneCrossingCoolDownTime + laneCrossingCoolDown < Time.time)
        {
            
            bool possibleClick = Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended && Input.touches[0].deltaPosition.sqrMagnitude < 1f && Input.touches[0].position.y > noClickZone;
            //right
            if ((inputRightSwipe || (possibleClick && Input.touches[0].position.x > (1- screenPercentageToClick) * Screen.width) || Input.GetAxis("Horizontal") > 0.1f) && lane < LevelController.instance.numLanes - 1)
            {
                crossing = true;
                targetLane = lane + 1;
                crossStartTime = Time.time;
                audioSource.PlayOneShot(audioMov);
                if (jumping)
                    crossingsWhileJumping++;
            }
            //left
            else if ((inputLeftSwipe || (possibleClick && Input.touches[0].position.x < screenPercentageToClick * Screen.width) || Input.GetAxis("Horizontal") < -0.1f) && lane > 0)
            {
                crossing = true;
                targetLane = lane - 1;
                crossStartTime = Time.time;
                audioSource.PlayOneShot(audioMov);
                if (jumping)
                    crossingsWhileJumping++;

            }
        }
        if (crossing)
        {
            if((Time.time - crossStartTime) * LevelController.instance.currentRound.gameSpeed < laneCrossingTime )
            {
                transform.localPosition = new Vector3(Mathf.Lerp(LevelController.instance.lanes[lane], LevelController.instance.lanes[targetLane], (Time.time - crossStartTime ) * LevelController.instance.currentRound.gameSpeed / laneCrossingTime),transform.localPosition.y ,transform.localPosition.z );
            }
            else
            {
                transform.localPosition = new Vector3(LevelController.instance.lanes[targetLane], transform.localPosition.y, transform.localPosition.z);
                lane = targetLane;
                crossing = false;
                laneCrossingCoolDownTime = Time.time;
            }
        }
        
        if(!jumping && !crouching)
        {
            if(inputUpSwipe || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow))
            {
                jumping = true;
                jumpStartTime = Time.time;
                audioSource.PlayOneShot(audioJump);
                if (crossing)
                    crossingsWhileJumping = 1;
            }
        }
        if(jumping)
        {
            Vector3 pos = transform.localPosition;
            //going up
            if( (Time.time - jumpStartTime) * LevelController.instance.currentRound.gameSpeed < jumpTime )
            {
                pos.y = Mathf.Lerp(middleAltitude, highAltitude, (Time.time - jumpStartTime) * LevelController.instance.currentRound.gameSpeed / jumpTime);
            }
            //floating
            else if((Time.time - jumpStartTime) * LevelController.instance.currentRound.gameSpeed < (jumpTime + jumpFloatingTime))
            {
                if(Input.GetKey(KeyCode.DownArrow))
                {
                    jumpStartTime = Time.time - (jumpTime + jumpFloatingTime) / LevelController.instance.currentRound.gameSpeed;
                }
                pos.y = highAltitude;
            }
            //falling
            else if((Time.time - jumpStartTime) * LevelController.instance.currentRound.gameSpeed - (jumpTime + jumpFloatingTime) <  jumpTime)
            {
                pos.y = Mathf.Lerp(highAltitude, middleAltitude, ((Time.time - jumpStartTime) * LevelController.instance.currentRound.gameSpeed - (jumpTime + jumpFloatingTime ))/ jumpTime);
            }
            else
            {
                pos.y = middleAltitude;
                jumping = false;
                crossingsWhileJumping = 0;
            }
            transform.localPosition = pos;
        }
        if(!crouching && !jumping)
        {
            if (inputDownSwipe || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.DownArrow))
            {
                crouching = true;
                jumpStartTime = Time.time;
                audioSource.PlayOneShot(audioCrouch);
            }
        }
        if(crouching)
        {
            Vector3 pos = transform.localPosition;
            //going up
            if ((Time.time - jumpStartTime) * LevelController.instance.currentRound.gameSpeed < crouchTime)
            {
                pos.y = Mathf.Lerp(middleAltitude, lowAltitude, (Time.time - jumpStartTime) * LevelController.instance.currentRound.gameSpeed / crouchTime);
            }
            else if(!crouchingUp)
            {
                if ((SystemInfo.deviceType == DeviceType.Desktop &&
                    #if UNITY_EDITOR
                             !UnityEditor.EditorApplication.isRemoteConnected &&
                    #endif
                    !Input.GetKey(KeyCode.DownArrow))

                    ||
                        
                    ((SystemInfo.deviceType == DeviceType.Handheld
                    #if UNITY_EDITOR 
                        || UnityEditor.EditorApplication.isRemoteConnected 
                    #endif
                    ) && (Input.touchCount == 0 || inputUpSwipe)))

                {
                    jumpStartTime = Time.time - (crouchTime + crouchFloatingTime) / LevelController.instance.currentRound.gameSpeed;
                    crouchingUp = true;
                }
                pos.y = lowAltitude;
            }
            else if (crouchingUp && (Time.time - jumpStartTime) * LevelController.instance.currentRound.gameSpeed - (crouchTime + crouchFloatingTime) < crouchTime)
            {
                pos.y = Mathf.Lerp(lowAltitude, middleAltitude, ((Time.time - jumpStartTime) * LevelController.instance.currentRound.gameSpeed - (crouchTime + crouchFloatingTime)) / crouchTime);
            }
            else
            {
                pos.y = middleAltitude;
                crouching = false;
                crouchingUp = false;
            }
            transform.localPosition = pos;
        }
        if(Input.GetKeyDown(KeyCode.Keypad1) ||Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameController.instance.bonusDisplay.ActivateBonus(GameController.instance.bonusDisplay.numOfBonuses - 3);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameController.instance.bonusDisplay.ActivateBonus(GameController.instance.bonusDisplay.numOfBonuses - 2);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            GameController.instance.bonusDisplay.ActivateBonus(GameController.instance.bonusDisplay.numOfBonuses - 1);
        }
    }

    

    public void LooseLive()
    {
        if (Time.time > invulnerableEndTime)
        {
            audioSource.PlayOneShot(audioHurt);
            LevelController.instance.looseLive();
        }
    }

    public void AddLive()
    {
        LevelController.instance.addLive();
    }


    private void SwipeDetection()
    {
        
    }

    

}
