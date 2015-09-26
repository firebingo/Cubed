using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

    Vector3 start;
    [SerializeField]
    Vector3 end;

    // Use this for initialization
    void Start ()
    {
        start = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = Vector3.MoveTowards(transform.position, end, 0.01f);
        if(transform.position == end)
        {
            Vector3 s = start;
            start = end;
            end = s;
        }
	}
}
