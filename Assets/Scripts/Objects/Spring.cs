using UnityEngine;
using System.Collections;

public class Spring : MonoBehaviour
{
    public float springPower; //the power of the spring
    Animation anim;
    public Transform target; //posiiton of the target the spring should launch you towards.

    void Start()
    {
        if(transform.tag == "floatSpring")
        {
            anim = GetComponent<Animation>();
            anim["floatSpringAnim"].speed = 1.8f;
            anim.Play();
        }
        if(target)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
