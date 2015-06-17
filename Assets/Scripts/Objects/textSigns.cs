using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

public class textSigns : MonoBehaviour
{
    Player playerRef; //reference to the player object
    Material signMaterial;

    void Start()
    {
        playerRef = FindObjectOfType<Player>();
        signMaterial = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        //makes the sign dissapear steadily if the player is over 2 units away from it until they are 4 units away.
        if (playerRef)
        {
            if (Vector3.Distance(playerRef.transform.position, transform.position) > 2.0f && Vector3.Distance(playerRef.transform.position, transform.position) < 4.0f)
                signMaterial.color = new Color(1, 1, 1, (4 / Vector3.Distance(playerRef.transform.position, transform.position)) - 1);
            else if (Vector3.Distance(playerRef.transform.position, transform.position) < 2.0f)
                signMaterial.color = new Color(1, 1, 1, 1);
            else if (Vector3.Distance(playerRef.transform.position, transform.position) > 4.0f)
                signMaterial.color = new Color(1, 1, 1, 0);
        }
    }
}
