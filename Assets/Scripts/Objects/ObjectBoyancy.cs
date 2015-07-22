using UnityEngine;
using System.Collections;

public class ObjectBoyancy : MonoBehaviour
{
    public GameObject waterObject;
    Rigidbody objectPhysics;
    float gravityMul;

    // Use this for initialization
    void Start()
    {
        objectPhysics = GetComponent<Rigidbody>();
        gravityMul = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (transform.position.y > waterObject.transform.position.y)
            gravityMul = 1.0f;
        else if (transform.position.y < waterObject.transform.position.y)
            gravityMul = -1.0f;

        if (transform.position.y > waterObject.transform.position.y - 0.1f && transform.position.y < waterObject.transform.position.y + 0.1f)
            gravityMul = 0;

        //Gravity, be sure to disable the rigidbody's gravity so it just uses this.
        objectPhysics.AddForce(Physics.gravity * objectPhysics.mass * gravityMul);
    }
}
