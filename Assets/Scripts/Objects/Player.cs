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
    bool canIceShield; //whether or not the player can use the ice shield.
    bool canFireShield; //whether or not the player can use the fire shield.
    bool canDash; //whether or not the player can dash
    public bool canDashTimer; //whether the timer allows the player to dash
    bool canAirDash; //whether or not the player can dash in the air
    float dashTimer; //timer to reset the dash
    public bool isDashing; //whether or not the player is currently dashing.
    public float collisionTimer; //timer for checking collision to reset the jumps.
    public float iceShieldTime; //how much time is left for the ice shield in seconds.
    float maxIceShieldTime; //the max time the ice shield can be used.
    public float fireShieldTime; //how much time is left for the fire shield in seconds.
    float maxFireShieldTime; //the max time the fire shield can be used.
    

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

        isDashing = false;
        playerPhysics = GetComponent<Rigidbody>();
        moveSpeed = 11.0f;
        maxSpeed = 3.0f;
        jumpForce = 4.0f;
        jumpCount = maxJumps;
        gCamera = Camera.main;
    }

    //Unity Update() method
    void Update()
    {
        gameMaster.playerPosition = transform.position;
        jumpTimer += Time.deltaTime;
        dashTimer += Time.deltaTime;
        //if the Jump button is pressed, the player has aviliable jumps, and isin't multi-jumping too quickly,
        // have the player jump, decrement the jump count, and reset the timers.
        if (Input.GetButtonDown("A") && jumpCount > 0 && jumpTimer > 0.2f)
        {
            playerPhysics.AddForce(new Vector3(0, 1, 0) * jumpForce * Time.fixedDeltaTime, ForceMode.Impulse);
            jumpCount--;
            jumpTimer = 0;
            collisionTimer = 0;
        }

        if(dashTimer > 0.75f)
            isDashing = false;
        //if the dash timer is over the cooldown time, reset it and allow to dash
        if (dashTimer > 1.5f)
        {
            dashTimer = 0;
            canDashTimer = true;
        }
        //if the dash button is pressed and the player can currently dash, make the player dash.
        if (Input.GetButtonDown("Y") && canDash && canDashTimer)
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
            isDashing = true;
        }

        //if the Ice Shield button is hit, the Fire Shield Button isin't and the player can use the Ice Shield,
        // set the Ice Shield to be active and disable the cube's hit box.
        //Otherwise, disable the Ice Shield and enable the cube's collider.
        if (Input.GetButton("X") && !Input.GetButton("B") && canIceShield && iceShieldTime > 0)
        {
            //if (Input.GetButtonDown("X") && jumpCount > 0)
            //    playerPhysics.AddForce(new Vector3(0, 1, 0) * jumpForce / 4 * Time.fixedDeltaTime, ForceMode.Impulse);
            iceShieldTime -= Time.deltaTime;
            if (!transform.GetChild(0).gameObject.activeSelf)
            {
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
        if (Input.GetButton("B") && !Input.GetButton("X") && canFireShield && fireShieldTime > 0)
        {
            //if (Input.GetButtonDown("B") && jumpCount > 0)
            //    playerPhysics.AddForce(new Vector3(0, 1, 0) * jumpForce / 4 * Time.fixedDeltaTime, ForceMode.Impulse);
            fireShieldTime -= Time.deltaTime;
            if (!transform.GetChild(1).gameObject.activeSelf)
            {
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
    }

    //Unity FixedUpdate() method.
    void FixedUpdate()
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

    //used for placing trakers of where the cube hits after jumping
    //void OnCollisionEnter(Collision iOther)
    //{
    //    if(iOther.gameObject.name == "Cube")
    //    {
    //        Instantiate(Resources.Load("Objects/HitPositionTracker"), iOther.contacts[0].point, Quaternion.EulerAngles(0,0,0));
    //    }
    //}

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
}
