using UnityEngine;
using System.Collections;

public class CameraTracker : MonoBehaviour
{
    public GameObject cameraFollow;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position = cameraFollow.transform.position;
    }
}
