﻿using UnityEngine;
using System.Collections;

/*
 * IMPORTANT NOTE:
 * This file should be used as an example of the coding standards of this project laid out
 *  in the TDD.
 */

public class Player : Entity
{
    public Camera gCamera; //reference to the game's main camera
    Rigidbody playerPhysics; //reference to the players rigidbody
    float jumpForce; //the amount of impluse applied to make a player jump.
    public int jumpCount; //the amount of jumps the player can currently do.
    public int maxJumps; //the max amount of jumps the player can do.
    float jumpTimer; //timer used for preventing multi-jumping too quickly.
    float defJumpForce; // the players default jump force
    bool canIceShield; //whether or not the player can use the ice shield.
    bool canFireShield; //whether or not the player can use the fire shield.
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

    //Unity Start() method
    void Start()
    {
        gameMaster = GameController.gameMaster;

        gameMaster.gamePlayer = this;
        maxJumps = gameMaster.playerMaxJumpCount;
        maxIceShieldTime = gameMaster.playerMaxIceShieldTime;
        maxFireShieldTime = gameMaster.playerMaxFireShieldTime;
        maxHealth = gameMaster.playerMaxHealth;
        health = maxHealth;
        iceShieldTime = maxIceShieldTime;
        fireShieldTime = maxFireShieldTime;
        canAirDash = gameMaster.playerCanAirDash;
        canDash = gameMaster.playerCanDash;
        Defense = gameMaster.playerDefense;
        inWater = false;
        gravityMul = 1.0f;
        inputDTimer = 0;

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

        hasPaused = false;
    }

    //Unity Update() method
    void Update()
    {
        if (!gameMaster.isPaused)
        {
            if (inputDTimer > 0)
            {
                inputDTimer -= Time.deltaTime;
            }
            if (invincTimer > 0)
            {
                invincTimer -= Time.deltaTime;
            }
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
            }

            //if the dash timer is over the cooldown time, reset it and allow to dash
            if (dashTimer > 1.5f)
            {
                dashTimer = 0;
                canDashTimer = true;
            }
            if (dashTimer > 0.75f)
                isDashing = false;

            //if the dash button is pressed and the player can currently dash, make the player dash.
            if (Input.GetButtonDown("Y") && canDash && canDashTimer && inputDTimer < 0.1f)
            {
                ParticleSystem part = transform.GetChild(2).GetComponent<ParticleSystem>();
                ParticleSystem partIce = transform.GetChild(3).GetComponent<ParticleSystem>();
                ParticleSystem partFire = transform.GetChild(4).GetComponent<ParticleSystem>();
                //if cant air dash and is on the ground.
                if (!canAirDash && jumpCount > 0)
                {
                    playerPhysics.AddForce(Vector3.Scale(playerPhysics.velocity.normalized, new Vector3(1, 0, 1)) * jumpForce / 1.2f * Time.fixedDeltaTime, ForceMode.Impulse);
                    canDashTimer = false;
                    if (!transform.GetChild(0).gameObject.activeSelf && !transform.GetChild(1).gameObject.activeSelf)
                        part.Play();
                    if (transform.GetChild(0).gameObject.activeSelf)
                        partIce.Play();
                    if (transform.GetChild(1).gameObject.activeSelf)
                        partFire.Play();
                }
                //if canairdash and is on the ground.
                else if (canAirDash && jumpCount > 0)
                {
                    playerPhysics.AddForce(Vector3.Scale(playerPhysics.velocity.normalized, new Vector3(1, 0, 1)) * jumpForce / 1.2f * Time.fixedDeltaTime, ForceMode.Impulse);
                    canDashTimer = false;
                    if (!transform.GetChild(0).gameObject.activeSelf && !transform.GetChild(1).gameObject.activeSelf)
                        part.Play();
                    if (transform.GetChild(0).gameObject.activeSelf)
                        partIce.Play();
                    if (transform.GetChild(1).gameObject.activeSelf)
                        partFire.Play();
                }
                //if can air dash and is not on the ground
                else if (canAirDash)
                {
                    playerPhysics.AddForce(Vector3.Scale(playerPhysics.velocity.normalized, new Vector3(1, 0, 1)) * jumpForce / 1.5f * Time.fixedDeltaTime, ForceMode.Impulse);
                    canDashTimer = false;
                    if (!transform.GetChild(0).gameObject.activeSelf && !transform.GetChild(1).gameObject.activeSelf)
                        part.Play();
                    if (transform.GetChild(0).gameObject.activeSelf)
                        partIce.Play();
                    if (transform.GetChild(1).gameObject.activeSelf)
                        partFire.Play();
                }
                dashTimer = 0;
                isDashing = true;
            }

            //if the Ice Shield button is hit, the Fire Shield Button isin't and the player can use the Ice Shield,
            // set the Ice Shield to be active and disable the cube's hit box.
            //Otherwise, disable the Ice Shield and enable the cube's collider.
            if (Input.GetButton("X") && !Input.GetButton("B") && canIceShield && iceShieldTime > 0 && inputDTimer < 0.1f)
            {
                iceShieldTime -= Time.deltaTime;
                if (!transform.GetChild(0).gameObject.activeSelf)
                {
                    //playerPhysics.AddForce(new Vector3(0, 1, 0) * 1.0f * Time.fixedDeltaTime, ForceMode.Impulse);
                    transform.GetChild(0).gameObject.SetActive(true);
                    GetComponent<BoxCollider>().enabled = false;
                }
            }
            else
            {
                if (transform.GetChild(0).gameObject.activeSelf)
                {
                    transform.GetChild(0).gameObject.SetActive(false);
                    GetComponent<BoxCollider>().enabled = true;
                }
            }

            //if the Fire Shield button is hit, the Fire Shield Button isin't and the player can use the Fire Shield,
            // set the Fire Shield to be active and disable the cube's hit box.
            //Otherwise, disable the Fire Shield and enable the cube's collider.
            if (Input.GetButton("B") && !Input.GetButton("X") && canFireShield && fireShieldTime > 0 && inputDTimer < 0.1f)
            {
                fireShieldTime -= Time.deltaTime;
                if (!transform.GetChild(1).gameObject.activeSelf)
                {
                    //playerPhysics.AddForce(new Vector3(0, 1, 0) * 1.0f * Time.fixedDeltaTime, ForceMode.Impulse);
                    transform.GetChild(1).gameObject.SetActive(true);
                    GetComponent<BoxCollider>().enabled = false;
                }
            }
            else
            {
                if (transform.GetChild(1).gameObject.activeSelf)
                {
                    transform.GetChild(1).gameObject.SetActive(false);
                    GetComponent<BoxCollider>().enabled = true;
                }
            }

            //check if the sheild use is allowed according to the global settings.
            if (GameController.gameMaster.playerCanUseIceShield)
                canIceShield = true;
            else if (canIceShield)
                canIceShield = false;

            if (GameController.gameMaster.playerCanUseFireShield)
                canFireShield = true;
            else if (canFireShield)
                canFireShield = false;

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
                gCamera.GetComponent<CameraMovement>().waterCorrect.enabled = false;
                gCamera.GetComponent<CameraMovement>().lavaCorrect.enabled = false;
                playerPhysics.velocity = new Vector3(0, 0, 0);
                iceShieldTime = maxIceShieldTime;
                fireShieldTime = maxFireShieldTime;
            }

            //do damage if in lava
            if (inLava && !transform.GetChild(1).gameObject.activeSelf)
            {
                health -= 16.5f * Time.deltaTime;
            }
        }

        if(Input.GetButtonDown("Start"))
        {
            if( gameMaster.isPaused == true)
                gameMaster.isPaused = false;
            else if (gameMaster.isPaused == false)
                gameMaster.isPaused = true;
        }
    }

    //Unity FixedUpdate() method.
    void FixedUpdate()
    {
        if (!gameMaster.isPaused)
        {
            if (hasPaused)
            {
                onUnpause();
                hasPaused = false;
            }
            //Gravity, be sure to disable the rigidbody's gravity so it just uses this.
            playerPhysics.AddForce(Physics.gravity * playerPhysics.mass * gravityMul);

            if (inputDTimer < 0.1f)
            {
                //this is done in a if else statement because most of the time if the player stops moving the raycast that prevents the player from applying force
                // passes and prevents movement unless the player jumps or activates a shield.
                if (playerPhysics.velocity.magnitude > 0.02f)
                {
                    //if the player isin't too close to a environment object.
                    if (!Physics.Raycast(transform.position, playerPhysics.velocity, 0.25f, 1 << 8))
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
                else
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

        if (iOther.gameObject.tag == "Enemy")
        {
            if (invincTimer <= 0)
            {
                float damage = 0.0f; //tracks the damage that needs to be done to the player
                float enemyDamage = 0.0f; //tracks the damage that needs to be done to the enemy
                Entity collisionObject = iOther.gameObject.GetComponent<Entity>();
                //if the velocity is lower than 1 do reduced damage to the enemy based on the velocity, otherwise increase the damage.
                if (playerPhysics.velocity.magnitude < 1.0f)
                {
                    damage = collisionObject.baseDamage;
                    enemyDamage = baseDamage * playerPhysics.velocity.magnitude;
                }
                else
                {
                    //reduce the damage by 1/3 if the player is dashing
                    //if the player is dashing increase the damage based on the velocity.
                    if (isDashing)
                    {
                        damage = (collisionObject.baseDamage * 0.66f);
                        if (playerPhysics.velocity.magnitude > 2)
                            enemyDamage = baseDamage * 2;
                        else
                            enemyDamage = baseDamage * playerPhysics.velocity.magnitude;
                    }
                    else
                    {
                        damage = collisionObject.baseDamage;
                        enemyDamage = baseDamage;
                    }
                }
                //reduces the damage taken by half if the ice shield is active
                if (transform.GetChild(0).gameObject.activeSelf)
                    damage /= 2;
                //multiplies the damage by 2 if the fire shield is active
                if (transform.GetChild(1).gameObject.activeSelf)
                {
                    enemyDamage *= 1.5f;
                    if (isDashing)
                        enemyDamage *= 2;
                }
                //deals the final damage
                collisionObject.health -= enemyDamage;
                health -= damage * Defense;
                invincTimer = invincTime;
            }
            playerPhysics.AddForce(((iOther.contacts[0].normal).normalized + new Vector3(0, 0.25f, 0)) * Time.fixedDeltaTime * damagePushBack, ForceMode.Impulse);
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
            if (Physics.Raycast(ray, 0.2f, 1 << 8))
            {
                jumpCount = maxJumps;
            }
        }
    }

    void OnTriggerEnter(Collider iOther)
    {
        if (iOther.gameObject.tag == "Checkpoint")
        {
            if (iOther.GetComponent<Checkpoint>().hasCheckpoint == false)
            {
                checkpointPos = iOther.gameObject.transform.position;
                iOther.GetComponent<Checkpoint>().hasCheckpoint = true;
            }
        }
        if (iOther.gameObject.tag == "Spring")
        {
            Spring springScript = iOther.transform.parent.GetComponent<Spring>();
            inputDTimer = springScript.inputDTime;
            if (springScript.target)
            {
                playerPhysics.velocity = new Vector3(0, 0, 0);
                if (springScript.gameObject.tag == "floatSpring")
                    this.transform.position = springScript.gameObject.transform.position + (springScript.gameObject.transform.up.normalized * 1.0f);
                else
                    this.transform.position = springScript.gameObject.transform.position + (springScript.gameObject.transform.forward.normalized * 0.85f);

                playerPhysics.AddForce(springScript.springPower * Time.fixedDeltaTime, ForceMode.Impulse);
            }
            else
                playerPhysics.AddForce(springScript.springPower * Time.fixedDeltaTime, ForceMode.Impulse);
        }
        if (iOther.gameObject.tag == "Lava")
        {
            if (iOther == iOther.gameObject.GetComponent<RisingLava>().lavaStart)
                iOther.gameObject.GetComponent<RisingLava>().isMoving = true;
        }
    }

    void OnTriggerExit(Collider iOther)
    {
        if (iOther.gameObject.tag == "Water")
        {
            if (transform.position.y > iOther.transform.position.y)
            {
                inWater = false;
                gravityMul = 1.0f;
                moveSpeed = defMoveSpeed;
                jumpForce = defJumpForce;

            }
            else if (transform.position.y < iOther.transform.position.y)
            {
                inWater = true;
                gravityMul = 0.5f;
                moveSpeed = defMoveSpeed*0.5f;
                jumpForce = defJumpForce * 0.71f;
            }
        }
        if (iOther.gameObject.tag == "Lava")
        {
            if (transform.position.y > iOther.transform.position.y)
            {
                inLava = false;
                gravityMul = 1.0f;
                moveSpeed = defMoveSpeed;
                jumpForce = defJumpForce;

            }
            else if (transform.position.y < iOther.transform.position.y)
            {
                inLava = true;
                gravityMul = 0.5f;
                moveSpeed = defMoveSpeed * 0.4f;
                jumpForce = defJumpForce * 0.61f;
            }
        }
    }
}
