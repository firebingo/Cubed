using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour
{
    public GameController gameMaster; //reference to the gameController.

    public float moveSpeed; //the amount of force applied to 
    public float defMoveSpeed; // the entity's default move speed.
    public float maxSpeed; //the max velocity of the entity.
    public float health; //the current health of the entity.
    protected float maxHealth; //the entity's max health.
    public float baseDamage; //the base damage the entity does
    public float damagePushBack; //the force that the entity is pushed back by when damage is dealt.
    protected Vector3 pastVelocity; //the past velocity of the object, used for pausing.
    protected bool hasPaused;

    protected void onPause()
    {
        if(GetComponent<Rigidbody>())
        {
            pastVelocity = GetComponent<Rigidbody>().velocity;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().detectCollisions = false;
        }
    }
    protected void onUnpause()
    {
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().detectCollisions = true;
            GetComponent<Rigidbody>().velocity = pastVelocity;
        }
    }
}
