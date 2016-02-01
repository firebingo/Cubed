using UnityEngine;
using System.Collections;

public class deathParts : MonoBehaviour
{
    bool isDying;
    float deathTime;
    // Use this for initialization
    void Start()
    {
        isDying = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDying)
            deathTime += Time.deltaTime;
        if(deathTime > 3.0f)
            Destroy(this.gameObject);
    }

    public void startDying()
    {
        isDying = true;
    }
}
