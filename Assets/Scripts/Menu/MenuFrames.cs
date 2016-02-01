using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuFrames : MonoBehaviour
{
    Slider frameSlider; //reference to the ui slider

    // Use this for initialization
    void Start()
    {
        frameSlider = GetComponent<Slider>();
        frameSlider.value = GameController.gameMaster.frameTarget;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.gameMaster.frameTarget == 0)
            transform.GetChild(3).GetComponent<Text>().text = "Frame Rate Target: 30";
        if (GameController.gameMaster.frameTarget == 1)
            transform.GetChild(3).GetComponent<Text>().text = "Frame Rate Target: 60";
        if (GameController.gameMaster.frameTarget == 2)
            transform.GetChild(3).GetComponent<Text>().text = "Frame Rate Target: 75";
        if (GameController.gameMaster.frameTarget == 3)
            transform.GetChild(3).GetComponent<Text>().text = "Frame Rate Target: 120";
        if (GameController.gameMaster.frameTarget == 4)
            transform.GetChild(3).GetComponent<Text>().text = "Frame Rate Target: 144";
        if (GameController.gameMaster.frameTarget == 5)
            transform.GetChild(3).GetComponent<Text>().text = "Frame Rate Target: 300";
    }

    //update the value of the Shdaows in the gameMaster
    public void updateValue()
    {
        GameController.gameMaster.frameTarget = (int)frameSlider.value;
        GameController.gameMaster.applyQuality();
    }
}
