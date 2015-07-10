using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuCamYInv : MonoBehaviour
{
    Toggle camYInvToggle;

    // Use this for initialization
    void Start()
    {
        camYInvToggle = GetComponent<Toggle>();
        if (GameController.gameMaster.cameraYInvert == -1)
            camYInvToggle.isOn = true;
        else
            camYInvToggle.isOn = false;
    }

    public void updateValue()
    {
        if (camYInvToggle.isOn)
            GameController.gameMaster.cameraYInvert = -1;
        else
            GameController.gameMaster.cameraYInvert = 1;

        GameController.gameMaster.applyQuality();
    }
}
