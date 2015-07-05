using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuCamXInv : MonoBehaviour
{
    Toggle camXInvToggle;

    // Use this for initialization
    void Start()
    {
        camXInvToggle = GetComponent<Toggle>();
        if(GameController.gameMaster.cameraXInvert == -1)
            camXInvToggle.isOn = true;
        else
            camXInvToggle.isOn = false;
    }

    public void updateValue()
    {
        if (camXInvToggle.isOn)
            GameController.gameMaster.cameraXInvert = -1;
        else
            GameController.gameMaster.cameraXInvert = 1;

        GameController.gameMaster.applyQuality();
    }
}
