using UnityEngine;
using System.Collections;

public class ObjectBoyancy : MonoBehaviour
{
    public GameObject waterObject;
    Rigidbody objectPhysics;
    float gravityMul;
    bool hasPaused;
    protected Vector3 pastVelocity; //the past velocity of the object, used for pausing.

    // Use this for initialization
    void Start()
    {
        hasPaused = false;
        objectPhysics = GetComponent<Rigidbody>();
        gravityMul = 1.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameController.gameMaster.isPaused)
        {
            if (hasPaused)
            {
                onUnpause();
                hasPaused = false;
            }

            if (transform.position.y > waterObject.transform.position.y)
                gravityMul = 1.0f;
            else if (transform.position.y < waterObject.transform.position.y)
                gravityMul = -1.0f;

            if (transform.position.y > waterObject.transform.position.y - 0.1f && transform.position.y < waterObject.transform.position.y + 0.1f)
                gravityMul = 0;

            //Gravity, be sure to disable the rigidbody's gravity so it just uses this.
            objectPhysics.AddForce(Physics.gravity * objectPhysics.mass * gravityMul);
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

    protected void onPause()
    {
        if (objectPhysics)
        {
            pastVelocity = objectPhysics.velocity;
            objectPhysics.isKinematic = true;
            objectPhysics.detectCollisions = false;
        }
    }

    protected void onUnpause()
    {
        if (objectPhysics)
        {
            objectPhysics.isKinematic = false;
            objectPhysics.detectCollisions = true;
            objectPhysics.velocity = pastVelocity;
        }
    }
}
