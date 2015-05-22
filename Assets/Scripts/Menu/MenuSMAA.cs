using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuSMAA : MonoBehaviour
{
    Slider SMAASlider; //reference to the ui slider

    // Use this for initialization
    void Start()
    {
        SMAASlider = GetComponent<Slider>();
        SMAASlider.value = GameController.gameMaster.SMAAQuality;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.gameMaster.SMAAQuality == 0)
            transform.GetChild(3).GetComponent<Text>().text = "SMAA: Off";
        if (GameController.gameMaster.SMAAQuality == 1)
            transform.GetChild(3).GetComponent<Text>().text = "SMAA: Low";
        if (GameController.gameMaster.SMAAQuality == 2)
            transform.GetChild(3).GetComponent<Text>().text = "SMAA: Medium";
        if (GameController.gameMaster.SMAAQuality == 3)
            transform.GetChild(3).GetComponent<Text>().text = "SMAA: High";
        if (GameController.gameMaster.SMAAQuality == 4)
            transform.GetChild(3).GetComponent<Text>().text = "SMAA: Ultra";
    }

    //update the value of the SMAA in the gameMaster
    public void updateValue()
    {
        GameController.gameMaster.SMAAQuality = (int)SMAASlider.value;
        GameController.gameMaster.applyQuality();
    }
}
