using UnityEngine;
using System.Collections;

public class CameraTracker : MonoBehaviour
{
    public Transform cameraFollow; //reference to the gameobject the script should follow, set in editor.

    //Unity Update() method
    void Update()
    {
        //set the object's position 
        transform.position = cameraFollow.transform.position; //set this object's position to the position of the follow object.
    }
}
