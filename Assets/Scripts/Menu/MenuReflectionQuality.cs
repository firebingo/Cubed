using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuReflectionQuality : MonoBehaviour {

    Slider reflectionQualitySlider; //reference to the ui slider

    // Use this for initialization
    void Start()
    {
        reflectionQualitySlider = GetComponent<Slider>();
        reflectionQualitySlider.value = GameController.gameMaster.reflectionQuality;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.gameMaster.reflectionQuality == 0)
            transform.GetChild(3).GetComponent<Text>().text = "Reflection Quality: Low";
        if (GameController.gameMaster.reflectionQuality == 1)
            transform.GetChild(3).GetComponent<Text>().text = "Reflection Quality: Medium";
        if (GameController.gameMaster.reflectionQuality == 2)
            transform.GetChild(3).GetComponent<Text>().text = "Reflection Quality: High";
        if (GameController.gameMaster.reflectionQuality == 3)
            transform.GetChild(3).GetComponent<Text>().text = "Reflection Quality: Ultra";
    }

    //update the value of the Reflection Quality in the gameMaster
    public void updateValue()
    {
        GameController.gameMaster.reflectionQuality = (int)reflectionQualitySlider.value;
        GameController.gameMaster.applyQuality();
    }
}
