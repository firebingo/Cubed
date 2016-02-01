using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * IMPORTANT NOTE:
 * This file should be used as an example of the coding standards of this project laid out
 *  in the TDD.
 */

public class Player : Entity
{
    enum shapes { Cube, Sphere, Pyramid };
    enum colors { pure, red, green, blue };

    public Camera gCamera; //reference to the game's main camera
    public Rigidbody playerPhysics; //reference to the players rigidbody
    float jumpForce; //the amount of impluse applied to make a player jump.
    public int jumpCount; //the amount of jumps the player can currently do.
    public int maxJumps; //the max amount of jumps the player can do.
    float jumpTimer; //timer used for preventing multi-jumping too quickly.
    bool isGrounded; //whether or not the player is on the ground
    float jumpResetThreshold; //the threshold for the hit normal for ressetting jumps
    float defJumpForce; // the players default jump force
    bool canDash; //whether or not the player can dash
    public bool canDashTimer; //whether the timer allows the player to dash
    bool canAirDash; //whether or not the player can dash in the air
    public float dashTimer; //timer to reset the dash
    public bool isDashing; //whether or not the player is currently dashing.
    public float collisionTimer; //timer for checking collision to reset the jumps.
    public float iceShieldTime; //how much time is left for the ice shield in seconds.
    float maxIceShieldTime; //the max time the ice shield can be used.
    public float fireShieldTime; //how much time is left for the fire shield in seconds.
    float maxFireShieldTime; //the max time the fire shield can be used.
    float Defense; //the defense multiplier
    public float invincTime; //how long your invincible after getting hit.
    float invincTimer; //timer for invicibility
    bool inWater; //whether or not the player is in the water.
    bool inLava; //whether or not the player is in lava.
    float gravityMul; //gravity multiplier
    Vector3 checkpointPos; //the position of the checkpoint the player will respawn at if they die.
    float inputDTimer; // timer for disabling the player input temporairly. 
    public bool isActive; // whether or not the player object is currently active and receiving controls.
    public bool isAbsorbed; // whether or not the object has been absored/absorbed another object 
    public List<Player> absorbedList; // the list of objects that have been absorbed.
    [SerializeField]
    int pColor; //the color of the player object.
    [SerializeField]
    float wallStickCompValue; //value to compensate so player doesn't stick to walls. values below .02 have little to no effect, values above .027 are a bit too noticable.


    //Unity Start() method
    void Start()
    {
        gameMaster = GameController.gameMaster;

        gameMaster.gamePlayer = this;
        maxJumps = gameMaster.playerMaxJumpCount;
        maxHealth = gameMaster.playerMaxHealth;
        health = maxHealth;
        inWater = false;
        gravityMul = 1.0f;
        inputDTimer = 0;
        isGrounded = true;
        isDashing = false;
        playerPhysics = GetComponent<Rigidbody>();
        //moveSpeed = 19.5f;
        //jumpForce = 6.1f;
        defMoveSpeed = 13.0f;
        moveSpeed = defMoveSpeed;
        defJumpForce = 4.1f;
        jumpForce = defJumpForce;
        maxSpeed = 3.0f;
        jumpCount = maxJumps;
        gCamera = Camera.main;
        checkpointPos = transform.position;
        absorbedList = new List<Player>();
        absorbedList.Add(this);
        jumpResetThreshold = 0.85f; //.85 is around 45 degrees

        hasPaused = false;
    }

    //Unity Update() method
    void Update()
    {
        if (!gameMaster.isPaused)
        {
            if (isActive)
            {
                if (inputDTimer > 0)
                    inputDTimer -= Time.deltaTime;

                if (invincTimer > 0)
                    invincTimer -= Time.deltaTime;

                gameMaster.playerPosition = transform.position;
                jumpTimer += Time.deltaTime;
                dashTimer += Time.deltaTime;
                //if the Jump button is pressed, the player has aviliable jumps, and isin't multi-jumping too quickly,
                // have the player jump, decrement the jump count, and reset the timers.
                if (Input.GetButtonDown("A") && jumpCount > 0 && jumpTimer > 0.2f && inputDTimer < 0.1f)
                {
                    playerPhysics.AddForce(new Vector3(0, 1, 0) * jumpForce * Time.fixedDeltaTime, ForceMode.Impulse);
                    jumpCount--;
                    jumpTimer = 0;
                    collisionTimer = 0;
                    isGrounded = false;
                }

                if ((Input.GetButtonDown("X") || Input.GetButtonDown("B")) && inputDTimer < 0.1f)
                {
                    //find the index of this object in the playerObjects array
                    int index = 0;
                    for (int i = 0; i < gameMaster.playerObjects.Count; ++i)
                    {
                        if (gameMaster.playerObjects[i])
                        {
                            if (gameMaster.playerObjects[i].Equals(this))
                            {
                                if (Input.GetButtonDown("X"))
                                    index = (int)Mathf.Repeat((i + 1), gameMaster.playerObjects.Count);
                                else if (Input.GetButtonDown("B"))
                                    index = (int)Mathf.Repeat((i - 1), gameMaster.playerObjects.Count);
                                break;
                            }
                        }
                    }

                    if (gameMaster.playerObjects[index])    // Make sure something exists at this index
                    {
                        if (gameMaster.playerObjects[index].isAbsorbed)     // If the object that we are switching to is absorbed
                        {
                            // Search through the parent's children for the only active object, that's the one we're trading places with
                            for (int i = 0; i < gameMaster.playerObjects[index].transform.parent.childCount; i++)
                            {
                                if (gameMaster.playerObjects[index].transform.parent.GetChild(i).gameObject.activeSelf)
                                {
                                    gameMaster.playerObjects[index].transform.parent.GetChild(i).gameObject.SetActive(false);

                                    gameMaster.playerObjects[index].gameObject.SetActive(true);
                                    gameMaster.playerObjects[index].transform.position = gameMaster.playerObjects[index].transform.parent.GetChild(i).transform.position;
                                    gameMaster.playerObjects[index].playerPhysics.velocity = gameMaster.playerObjects[index].transform.parent.GetChild(i).GetComponent<Player>().playerPhysics.velocity;
                                    gameMaster.playerObjects[index].jumpCount = gameMaster.playerObjects[index].transform.parent.GetChild(i).GetComponent<Player>().jumpCount;
                                    break;
                                }
                            }
                        }
                        isActive = false;
                        gameMaster.playerObjects[index].inputDTimer = 0.2f;
                        gameMaster.playerObjects[index].isActive = true;
                        gameMaster.cameraTracker.cameraFollow = gameMaster.playerObjects[index].transform;
                    }
                }
            }

            //if the player is out of health
            if (health < 0)
            {
                health = maxHealth;
                transform.position = checkpointPos;
                inWater = false;
                inLava = false;
                gravityMul = 1.0f;
                moveSpeed = defMoveSpeed;
                jumpForce = defJumpForce;
                gCamera.GetComponent<CameraMovement>().inWater = false;

                if (gCamera.GetComponent<CameraMovement>().waterCorrect)
                    gCamera.GetComponent<CameraMovement>().waterCorrect.enabled = false;
                if (gCamera.GetComponent<CameraMovement>().lavaCorrect)
                    gCamera.GetComponent<CameraMovement>().lavaCorrect.enabled = false;
                if (gCamera.GetComponent<CameraMovement>().waterFog)
                    gCamera.GetComponent<CameraMovement>().waterFog.enabled = false;

                playerPhysics.velocity = Vector3.zero;
                iceShieldTime = maxIceShieldTime;
                fireShieldTime = maxFireShieldTime;
            }

            //do damage if in lava
            if (inLava && !transform.GetChild(1).gameObject.activeSelf)
            {
                health -= 16.5f * Time.deltaTime;
            }
        }

        if (isActive)
        {
            //if the start button has been pressed, pause the game
            if (Input.GetButtonDown("Start"))
            {
                if (gameMaster.isPaused == true)
                    gameMaster.isPaused = false;
                else if (gameMaster.isPaused == false)
                    gameMaster.isPaused = true;
            }
        }
    }

    //Unity FixedUpdate() method.
    void FixedUpdate()
    {
        //if the game isint paused
        if (!gameMaster.isPaused)
        {
            //if the game has unpaused since last check call the onUnpause() function and change hasPaused to false
            if (hasPaused)
            {
                onUnpause();
                hasPaused = false;
            }

            //Gravity, be sure to disable the rigidbody's gravity so it just uses this.
            playerPhysics.AddForce(Physics.gravity * playerPhysics.mass * gravityMul);
            if (isActive)
            {
                if (inputDTimer < 0.1f)
                {
                    //Forward and reverse movement controls.
                    if (Input.GetAxis("Vertical") > 0 && playerPhysics.velocity.magnitude < maxSpeed)
                    {
                        playerPhysics.AddForceAtPosition(new Vector3(gCamera.transform.forward.x, 0, gCamera.transform.forward.z).normalized * moveSpeed * Time.fixedDeltaTime * Input.GetAxis("Vertical"), new Vector3(transform.position.x, transform.position.y + (0.35f * transform.localScale.y), transform.position.z - (0.5f * transform.localScale.z)), ForceMode.Force);
                    }
                    else if (Input.GetAxis("Vertical") < 0 && playerPhysics.velocity.magnitude < maxSpeed)
                    {
                        playerPhysics.AddForceAtPosition(new Vector3(gCamera.transform.forward.x, 0, gCamera.transform.forward.z).normalized * moveSpeed * Time.fixedDeltaTime * Input.GetAxis("Vertical"), new Vector3(transform.position.x, transform.position.y + (0.35f * transform.localScale.y), transform.position.z + (0.5f * transform.localScale.z)), ForceMode.Force);
                    }

                    //Sideways movement controls.
                    if (Input.GetAxis("Horizontal") > 0 && playerPhysics.velocity.magnitude < maxSpeed)
                    {
                        playerPhysics.AddForceAtPosition(gCamera.transform.right * moveSpeed * Time.fixedDeltaTime * Input.GetAxis("Horizontal"), new Vector3(transform.position.x - (0.5f * transform.localScale.x), transform.position.y + (0.35f * transform.localScale.y), transform.position.z), ForceMode.Force);
                    }
                    else if (Input.GetAxis("Horizontal") < 0 && playerPhysics.velocity.magnitude < maxSpeed)
                    {
                        playerPhysics.AddForceAtPosition(gCamera.transform.right * moveSpeed * Time.fixedDeltaTime * Input.GetAxis("Horizontal"), new Vector3(transform.position.x + (0.5f * transform.localScale.x), transform.position.y + (0.35f * transform.localScale.y), transform.position.z), ForceMode.Force);
                    }
                }
            }
        }
        else
        {
            //if the game has paused since the last check call the onPause() function and change hasPaused to true
            if (!hasPaused)
            {
                onPause();
                hasPaused = true;
            }
        }
    }

    void OnCollisionEnter(Collision iOther)
    {
        //used for placing trakers of where the cube hits after jumping
        //if(iOther.gameObject.name == "Cube")
        //{
        //    Instantiate(Resources.Load("Objects/HitPositionTracker"), iOther.contacts[0].point, Quaternion.EulerAngles(0,0,0));
        //}

        if (iOther.gameObject.tag == "Player")
        {
            // Take all the children of the collided object's parent, and child it with the parent of this object
            if (!(transform.parent == iOther.transform.parent) && !iOther.gameObject.GetComponent<Player>().isActive)
            {
                Transform par = iOther.transform.parent;

                while (par.childCount > 0)
                {
                    par.GetChild(0).SetParent(transform.parent);
                }

                Destroy(par.gameObject);

                isAbsorbed = true;
                iOther.gameObject.GetComponent<Player>().isAbsorbed = true;
                iOther.gameObject.SetActive(false);
            }
        }

        //if the player hits a kill plane kill them.
        if (iOther.gameObject.tag == "killPlane")
        {
            //is less than zero because health check for death only checks for less than 0.
            health = -1;
        }

        if (iOther.gameObject.tag == "movPlat")
        {
            transform.parent.SetParent(iOther.transform);
        }
    }

    void OnCollisionExit(Collision iOther)
    {
        if (iOther.gameObject.tag == "movPlat")
        {
            transform.parent.parent = null;
        }
    }

    //Unity OnCollisionStay() Method
    void OnCollisionStay(Collision iOther)
    {
        //timer doesn't need to count higher than this.
        if (collisionTimer < 1.0f)
            collisionTimer += Time.deltaTime;

        //if the collision timer is over its threshold, raycast to see if the player is on a environment object to reset the jump count.
        //This is to prevent the jump count from resetting when the player hits jump but is still close to the ground.
        //Frame perfect jumping may still possibly reset the jump count allowing for a extra jump. Not practical to test.
        if (collisionTimer > 0.1f)
        {
            Ray ray = new Ray(transform.position, -(Vector3.up));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 0.2f, 1 << 8))
            {
                // Debug.Log(hit.normal.y);
                if (hit.normal.y > jumpResetThreshold)
                {
                    jumpCount = maxJumps;
                    isGrounded = true;
                }
            }
        }
        //if the player hits a environemnt object and isin't grounded shortly disable input.
        //this is to prevent wall sticking and theoretically shouldnt cause other problems.

        if (iOther.gameObject.layer == 8 && !isGrounded)
            inputDTimer += wallStickCompValue;
    }

    //Unity OnTriggerEnter function.
    void OnTriggerEnter(Collider iOther)
    {
        //if the player hits a checkpoint that hasn't been used set the checkpointPos to the new checkpoint
        // and tell the new checkpoint it's been used.
        if (iOther.gameObject.tag == "Checkpoint")
        {
            if (iOther.GetComponent<Checkpoint>().hasCheckpoint == false)
            {
                checkpointPos = iOther.gameObject.transform.position;
                iOther.GetComponent<Checkpoint>().hasCheckpoint = true;
            }
        }
        //if the player hits a spring, disable the player's input for the springs set time.
        if (iOther.gameObject.tag == "Spring")
        {
            Spring springScript = iOther.transform.parent.GetComponent<Spring>();
            inputDTimer = springScript.inputDTime;
            //if the spring has a target
            if (springScript.target)
            {
                //Set the player's velocity to zero and place it above the spring's position, this should prevent errors in where the player lands.
                playerPhysics.velocity = new Vector3(0, 0, 0);
                if (springScript.gameObject.tag == "floatSpring")
                    this.transform.position = springScript.gameObject.transform.position + (springScript.gameObject.transform.up.normalized * 1.0f);
                else
                    this.transform.position = springScript.gameObject.transform.position + (springScript.gameObject.transform.forward.normalized * 0.85f);

                //set the player's velocity to the spring's power.
                playerPhysics.velocity = springScript.springPower;
            }
            //if the spring doesn't have a target just launch the player up.
            else
                playerPhysics.velocity = springScript.springPower;
        }
        //if the player hits a rising lava trigger.
        if (iOther.gameObject.tag == "Lava")
        {
            if (iOther == iOther.gameObject.GetComponent<RisingLava>().lavaStart)
                iOther.gameObject.GetComponent<RisingLava>().isMoving = true;
        }

        //if the player hits a grass loader.
        if (iOther.gameObject.tag == "grassLoader")
            iOther.GetComponent<GrassLoader>().loadFunction();
    }

    //Unity OnTriggerExit function.
    void OnTriggerExit(Collider iOther)
    {
        if (iOther.gameObject.tag == "Water")
        {
            //if the player passes a water plane and is above it change it's physics to accomodate
            if (transform.position.y > iOther.transform.position.y)
            {
                inWater = false;
                gravityMul = 1.0f;
                moveSpeed = defMoveSpeed;
                jumpForce = defJumpForce;

            }
            //if the player passes a water plane and is under it change it's physics to accomodate
            else if (transform.position.y < iOther.transform.position.y)
            {
                inWater = true;
                gravityMul = 0.5f;
                moveSpeed = defMoveSpeed * 0.5f;
                jumpForce = defJumpForce * 0.71f;
            }
        }
        if (iOther.gameObject.tag == "Lava")
        {
            //if the player passes a lava plane and is above it change it's physics to accomodate
            if (transform.position.y > iOther.transform.position.y)
            {
                inLava = false;
                gravityMul = 1.0f;
                moveSpeed = defMoveSpeed;
                jumpForce = defJumpForce;

            }
            //if the player passes a lava plane and is under it change it's physics to accomodate
            else if (transform.position.y < iOther.transform.position.y)
            {
                inLava = true;
                gravityMul = 0.5f;
                moveSpeed = defMoveSpeed * 0.4f;
                jumpForce = defJumpForce * 0.61f;
            }
        }
    }

    //getters and setters
    public int getColor()
    {
        return pColor;
    }
}

