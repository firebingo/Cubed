using UnityEngine;
using System.Collections;

public class MovingPlatform : Entity
{
    Vector3 start;
    [SerializeField]
    Vector3 end;

    bool isMoving = true;

    // Use this for initialization
    void Start ()
    {
        start = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!GameController.gameMaster.isPaused)
        {
            //if the game has unpaused since last check call the onUnpause() function and change hasPaused to false
            if (hasPaused)
            {
                onUnpause();
                hasPaused = false;
            }

            // Move the platform
            if(isMoving)
                transform.position = Vector3.MoveTowards(transform.position, end, moveSpeed * Time.deltaTime);

            //Reverse the platforms direction
            if (transform.position == end && isMoving)
            {
                StartCoroutine(Wait());
                Vector3 s = start;
                start = end;
                end = s;
            }
        }
        else
        {
            //if the game has paused since the last check call the onPause() function and change hasPaused to true
            if (!hasPaused)
            {
                onPause();
                hasPaused = true;
            }
        }
    }

    void OnEnable()
    {
        isMoving = true;
    }

    IEnumerator Wait()
    {
        isMoving = false;
        yield return new WaitForSeconds(2); 
        isMoving = true;
    }
}
