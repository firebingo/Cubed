using UnityEngine;
using System.Collections;

public class PauseUnpause : MonoBehaviour
{
    public void UnPause()
    {
        GameController.gameMaster.isPaused = false;
    }

    //resets the player to their last checkpoint
    public void resetPlayer()
    {
        GameController.gameMaster.gamePlayer.health = -1;
    }
}
