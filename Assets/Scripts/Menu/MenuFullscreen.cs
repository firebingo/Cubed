using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuFullscreen : MonoBehaviour
{
    Toggle fullToggle;

    // Use this for initialization
    void Start()
    {
        fullToggle = GetComponent<Toggle>();
        fullToggle.isOn = GameController.gameMaster.Fullscreen;
    }

    //Toggle VSync in gameMaster
    public void updateValue()
    {
        if (fullToggle.isOn)
            GameController.gameMaster.Fullscreen = true;
        else
            GameController.gameMaster.Fullscreen = false;

        GameController.gameMaster.applyQuality();
    }
}
