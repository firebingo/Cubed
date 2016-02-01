using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuShadows : MonoBehaviour
{
    Slider shadowSlider; //reference to the ui slider

    // Use this for initialization
    void Start()
    {
        shadowSlider = GetComponent<Slider>();
        shadowSlider.value = GameController.gameMaster.shadowQuality;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.gameMaster.shadowQuality == 0)
            transform.GetChild(3).GetComponent<Text>().text = "Shadow Quality: Low";
        if (GameController.gameMaster.shadowQuality == 1)
            transform.GetChild(3).GetComponent<Text>().text = "Shadow Quality: Medium";
        if (GameController.gameMaster.shadowQuality == 2)
            transform.GetChild(3).GetComponent<Text>().text = "Shadow Quality: High";
        if (GameController.gameMaster.shadowQuality == 3)
            transform.GetChild(3).GetComponent<Text>().text = "Shadow Quality: Ultra";
    }

    //update the value of the Shdaows in the gameMaster
    public void updateValue()
    {
        GameController.gameMaster.shadowQuality = (int)shadowSlider.value;
        GameController.gameMaster.applyQuality();
    }
}
