using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuGrass : MonoBehaviour
{
    Slider grassQualitySlider; //reference to the ui slider

    // Use this for initialization
    void Start()
    {
        grassQualitySlider = GetComponent<Slider>();
        grassQualitySlider.value = GameController.gameMaster.grassQuality;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.gameMaster.grassQuality == 0)
            transform.GetChild(3).GetComponent<Text>().text = "Grass Quality: Off";
        if (GameController.gameMaster.grassQuality == 1)
            transform.GetChild(3).GetComponent<Text>().text = "Grass Quality: Low";
        if (GameController.gameMaster.grassQuality == 2)
            transform.GetChild(3).GetComponent<Text>().text = "Grass Quality: Semi-Medium";
        if (GameController.gameMaster.grassQuality == 3)
            transform.GetChild(3).GetComponent<Text>().text = "Grass Quality: Medium";
        if (GameController.gameMaster.grassQuality == 4)
            transform.GetChild(3).GetComponent<Text>().text = "Grass Quality: High";
        if (GameController.gameMaster.grassQuality == 5)
            transform.GetChild(3).GetComponent<Text>().text = "Grass Quality: Ultra (Good Luck)";
    }

    //update the value of the Grass Quality in the gameMaster
    public void updateValue()
    {
        GameController.gameMaster.grassQuality = (int)grassQualitySlider.value;
        GameController.gameMaster.applyQuality();
    }
}
