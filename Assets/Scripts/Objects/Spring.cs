using UnityEngine;
using System.Collections;

public class Spring : MonoBehaviour
{
    public Vector3 springPower; //the power of the spring
    Animation anim;
    public Transform target; //posiiton of the target the spring should launch you towards.
    public float inputDTime; //how long the spring should disable the players input.
    public float maxHeight; //the max height for the trajectory above the targets y coord.
    public float powerMultiplier; //power multiplier for the spring. 

    void Start()
    {
        //powerMultipler should have no reason to be 0. but it's default for the prefab is 0.
        //this is here so the prefab can just be placed down and no change to the multiplier has to be made unless needed.
        if(powerMultiplier == 0) 
            powerMultiplier = 1;

        if (target)
        {
            Vector3 direction = target.position - transform.position; //direction vector
            float distance = direction.magnitude; // magnitude of direction

            float height = direction.y; //store vertical component
            direction.y = 0; //remove vertical component from direction to make it just horizontal

            float vertChange = maxHeight + height; //the y coordinate to hit in the arc.
            if(vertChange < 0) //if the target is below the spring vertchange will be negative so clamp it to 0.
                vertChange = 0;
            float vertDelta = vertChange - height; //the change in y for the arc.

            float timeUp = Mathf.Sqrt(vertChange / (-0.5f * -Physics.gravity.magnitude)); //how long it takes for the player to hit the peak of the arc
            float timeTotal = timeUp + Mathf.Sqrt(vertDelta / (-0.5f * -Physics.gravity.magnitude)); //how long it will take for the player to hit the ground.

            float yVel = Physics.gravity.magnitude * timeUp; //the vertical velocity needed
            float xVel = distance / timeTotal; //the horizontal velocity needed

            springPower = new Vector3(direction.normalized.x * xVel, yVel, direction.normalized.z * xVel) * powerMultiplier; //the final power to apply.
        }

        //if the spring is  afloating spring play it's animation.
        if (transform.tag == "floatSpring")
        {
            anim = GetComponent<Animation>();
            anim["floatSpringAnim"].speed = 1.8f;
            anim.Play();
        }
    }
}
