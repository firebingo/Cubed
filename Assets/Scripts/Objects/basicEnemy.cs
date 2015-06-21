using UnityEngine;
using System.Collections;

public class basicEnemy : Entity
{
    public float patrolAngle; //changes how wide of a circle the enemy moves in.
    Rigidbody entityPhysics; //refernece to the reigidbody of the enemy
    public Vector3 pushDirection; //the direction in which the enemy tries to move.
    bool xFlip; //used for the vector rotation
    bool zFlip; //used for the vector rotation
    float distanceToPlayer; //the distance away from the player
    int enemyState; //what state the enemy is in, 0 = wandering, 1 = following player, 2 = wait
    public float seekDistance; //the distance that the enemy will seek the player
    public bool waitStill;
    ParticleSystem deathparts;

    // Use this for initialization
    void Start()
    {
        deathparts = transform.GetChild(0).GetComponent<ParticleSystem>();
        entityPhysics = GetComponent<Rigidbody>();
        pushDirection = new Vector3(1, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, GameController.gameMaster.playerPosition);

        if (distanceToPlayer < seekDistance)
            enemyState = 1;
        else if (waitStill)
            enemyState = 2;
        else
            enemyState = 0;

        switch (enemyState)
        {
            case 0:
                //if the vector's x is greater than one, flip the bool so it starts decreasing
                //if it's less than one, flip the bool so it starts increasing.
                //same is then done for z
                if (pushDirection.x > 1)
                    xFlip = true;
                else if (pushDirection.x < -1)
                    xFlip = false;
                if (xFlip)
                    pushDirection.x -= patrolAngle * Time.deltaTime;
                else if (!xFlip)
                    pushDirection.x += patrolAngle * Time.deltaTime;

                if (pushDirection.z > 1)
                    zFlip = true;
                else if (pushDirection.z < -1)
                    zFlip = false;
                if (zFlip)
                    pushDirection.z -= patrolAngle * Time.deltaTime;
                else if (!zFlip)
                    pushDirection.z += patrolAngle * Time.deltaTime;
                break;
            case 1:
                pushDirection = (GameController.gameMaster.playerPosition - transform.position).normalized;
                break;
            case 2:
                pushDirection = new Vector3(0, 0, 0);
                break;
        }

        if (health < 0)
        {
            if (deathparts)
            {
                deathparts.GetComponent<deathParts>().startDying();
                deathparts.transform.parent = null;
                deathparts.Play();
            }
            DestroyObject(this.gameObject);
        }
    }

    //Unity FixedUpdate() method.
    void FixedUpdate()
    {
        entityPhysics.AddForceAtPosition(pushDirection.normalized * moveSpeed * Time.fixedDeltaTime, new Vector3(transform.position.x, transform.position.y + (0.35f * transform.localScale.y), transform.position.z - (0.5f * transform.localScale.z)), ForceMode.Force);
    }

    void OnCollisionEnter(Collision iOther)
    {
        if (iOther.gameObject.tag == "Player")
        {
            entityPhysics.AddForce(((iOther.contacts[0].normal).normalized + new Vector3(0, 0.25f, 0)) * Time.fixedDeltaTime * damagePushBack, ForceMode.Impulse);
        }
    }
}
