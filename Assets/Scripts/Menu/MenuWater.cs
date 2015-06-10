using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuWater : MonoBehaviour
{

    Slider waterSlider; //reference to the ui slider

    // Use this for initialization
    void Start()
    {
        waterSlider = GetComponent<Slider>();
        waterSlider.value = GameController.gameMaster.waterQuality;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.gameMaster.waterQuality == 0)
            transform.GetChild(3).GetComponent<Text>().text = "Water Quality: Simple";
        if (GameController.gameMaster.waterQuality == 1)       
            transform.GetChild(3).GetComponent<Text>().text = "Water Quality: Low";
        if (GameController.gameMaster.waterQuality == 2)       
            transform.GetChild(3).GetComponent<Text>().text = "Water Quality: Medium";
        if (GameController.gameMaster.waterQuality == 3)       
            transform.GetChild(3).GetComponent<Text>().text = "Water Quality: High";
        if (GameController.gameMaster.waterQuality == 4)
            transform.GetChild(3).GetComponent<Text>().text = "Water Quality: Ultra";
    }

    //update the value of the Shdaows in the gameMaster
    public void updateValue()
    {
        GameController.gameMaster.waterQuality = (int)waterSlider.value;
        GameController.gameMaster.applyQuality();
    }
}
