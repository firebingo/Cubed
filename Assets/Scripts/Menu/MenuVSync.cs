using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuVSync : MonoBehaviour
{
    Toggle vSyncToggle;

    // Use this for initialization
    void Start()
    {
        vSyncToggle = GetComponent<Toggle>();
        vSyncToggle.isOn = GameController.gameMaster.vSync;
    }

    //Toggle VSync in gameMaster
    public void updateValue()
    {
        if (vSyncToggle.isOn)
            GameController.gameMaster.vSync = true;
        else
            GameController.gameMaster.vSync = false;
        
        GameController.gameMaster.applyQuality();
    }
}
