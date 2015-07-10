using UnityEngine;
using System.Collections;

public class RisingLava : MonoBehaviour
{
    public Collider lavaStart;
    public bool isMoving;
    public float moveSpeed;

    // Update is called once per frame
    void Update()
    {
        if(isMoving)
        {
            transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
        }
    }
}
