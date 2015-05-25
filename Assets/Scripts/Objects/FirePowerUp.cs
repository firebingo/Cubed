using UnityEngine;
using System.Collections;

public class FirePowerUp : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        //if the player hits the powerup, enable the global powerup use.
        if (other.gameObject.name == "Player" || other.gameObject.name == "Player(Clone)")
        {
            GameController.gameMaster.playerCanUseFireShield = true;
            Destroy(this.gameObject);
        }
    }
}
