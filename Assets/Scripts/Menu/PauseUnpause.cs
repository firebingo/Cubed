using UnityEngine;
using System.Collections;

public class PauseUnpause : MonoBehaviour
{
    public void UnPause()
    {
        GameController.gameMaster.isPaused = false;
    }
}
