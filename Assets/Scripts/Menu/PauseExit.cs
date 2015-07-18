using UnityEngine;
using System.Collections;

public class PauseExit : MonoBehaviour
{
    public void exitToMenu()
    {
        GameController.gameMaster.isPaused = false;
        GameController.gameMaster.loadScene(0);
    }
}
