using UnityEngine;
using System.Collections;

public class LockCursor : MonoBehaviour
{

    void Update()
    {
        if (Application.loadedLevel != 0)
        {
            if (!GameController.gameMaster.isPaused)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (GameController.gameMaster.isPaused)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        else if(!Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
