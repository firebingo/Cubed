using UnityEngine;
using System.Collections;

public class PauseUnpause : MonoBehaviour
{
    public void UnPause()
    {
        GameController.gameMaster.isPaused = false;
    }

    public void loadLevel()
    {
        GameController.gameMaster.loadScene(Application.loadedLevel);
    }

    //resets the player to their last checkpoint
    public void resetPlayer()
    {
        GameController.gameMaster.gamePlayer.health = -1;
    }
}
