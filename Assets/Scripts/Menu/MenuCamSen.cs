using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuCamSen : MonoBehaviour
{
    Slider camSenSlider;

    // Use this for initialization
    void Start()
    {
        camSenSlider = GetComponent<Slider>();
        camSenSlider.value = GameController.gameMaster.cameraSensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        transform.GetChild(3).GetComponent<Text>().text = "Camera Sensitivty: " + camSenSlider.value;
    }

    //update the value of the Camera Sensitivity in the gameMaster
    public void updateValue()
    {
        GameController.gameMaster.cameraSensitivity = camSenSlider.value;
        GameController.gameMaster.applyQuality();
    }
}
