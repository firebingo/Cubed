using UnityEngine;
using System.Collections;

public class Spring : MonoBehaviour
{
    public float springPower;
    Animation anim;

    void Start()
    {
        if(transform.name == "floatSpring")
        {
            anim = GetComponent<Animation>();
            anim["floatSpringAnim"].speed = 1.8f;
            anim.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
