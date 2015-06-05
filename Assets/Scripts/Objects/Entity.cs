using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour
{
    public GameController gameMaster; //reference to the gameController.

    public float moveSpeed; //the amount of force applied to 
    public float maxSpeed; //the max velocity of the entity.
    public float health; //the current health of the entity.
    protected float maxHealth; //the entity's max health.
    public float baseDamage; //the base damage the entity does
    public float damagePushBack; //the force that the entity is pushed back by when damage is dealt.
}
