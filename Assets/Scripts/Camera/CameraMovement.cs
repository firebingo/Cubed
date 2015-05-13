using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    public GameObject target; 

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("CameraHorizontal") > 0)
        {
            transform.RotateAround(target.transform.position, Vector3.up, 85 * Input.GetAxis("CameraHorizontal") * Time.deltaTime);
        }
        else if (Input.GetAxis("CameraHorizontal") < 0)
        {
            transform.RotateAround(target.transform.position, Vector3.up, 85 * Input.GetAxis("CameraHorizontal") * Time.deltaTime);
        }

        if (Input.GetAxis("CameraVertical") > 0 && transform.eulerAngles.x < 80)
        {
            transform.RotateAround(target.transform.position, transform.right, 65 * Input.GetAxis("CameraVertical") * Time.deltaTime);
        }
        else if (Input.GetAxis("CameraVertical") < 0 && transform.eulerAngles.x > 10)
        {
            transform.RotateAround(target.transform.position, transform.right, 65 * Input.GetAxis("CameraVertical") * Time.deltaTime);
        }
    }
}
