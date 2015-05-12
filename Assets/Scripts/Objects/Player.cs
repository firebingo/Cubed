using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public Camera gCamera;
    Rigidbody playerPhysics;
    float moveSpeed;
    float maxSpeed;
    float jumpForce;
    int jumpCount;
    int maxJumps;

    // Use this for initialization
    void Start()
    {
        playerPhysics = GetComponent<Rigidbody>();
        moveSpeed = 11.0f;
        maxSpeed = 3.0f;
        jumpForce = 4.0f;
        jumpCount = 1;
        maxJumps = 1;
        gCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && jumpCount > 0) 
        {
            playerPhysics.AddForce(new Vector3(0, 1, 0) * jumpForce * Time.fixedDeltaTime, ForceMode.Impulse);
            jumpCount--;
        }
    }

    void FixedUpdate()
    {
        if (Input.GetAxis("Vertical") > 0 && playerPhysics.velocity.magnitude < maxSpeed)
        {
            playerPhysics.AddForceAtPosition(new Vector3(gCamera.transform.forward.x, 0, gCamera.transform.forward.z).normalized * moveSpeed * Time.fixedDeltaTime * Input.GetAxis("Vertical"), new Vector3(transform.position.x, transform.position.y + (0.35f * transform.localScale.y), transform.position.z - (0.5f * transform.localScale.z)), ForceMode.Force);
        }
        else if (Input.GetAxis("Vertical") < 0 && playerPhysics.velocity.magnitude < maxSpeed)
        {
            playerPhysics.AddForceAtPosition(new Vector3(gCamera.transform.forward.x, 0, gCamera.transform.forward.z).normalized * moveSpeed * Time.fixedDeltaTime * Input.GetAxis("Vertical"), new Vector3(transform.position.x, transform.position.y + (0.35f * transform.localScale.y), transform.position.z + (0.5f * transform.localScale.z)), ForceMode.Force);
        }

        if (Input.GetAxis("Horizontal") > 0 && playerPhysics.velocity.magnitude < maxSpeed)
        {
            playerPhysics.AddForceAtPosition(gCamera.transform.right * moveSpeed * Time.fixedDeltaTime * Input.GetAxis("Horizontal"), new Vector3(transform.position.x - (0.5f * transform.localScale.x), transform.position.y + (0.35f * transform.localScale.y), transform.position.z), ForceMode.Force);
        }
        else if (Input.GetAxis("Horizontal") < 0 && playerPhysics.velocity.magnitude < maxSpeed)
        {
            playerPhysics.AddForceAtPosition(gCamera.transform.right * moveSpeed * Time.fixedDeltaTime * Input.GetAxis("Horizontal"), new Vector3(transform.position.x + (0.5f * transform.localScale.x), transform.position.y + (0.35f * transform.localScale.y), transform.position.z), ForceMode.Force);
        } 
    }

    void OnCollisionEnter(Collision other)
    {
        Ray ray = new Ray(transform.position, -(Vector3.up));
        if(Physics.Raycast(ray, 0.2f, 1 << 8))
        {
            jumpCount = maxJumps;
        }
    }

}
