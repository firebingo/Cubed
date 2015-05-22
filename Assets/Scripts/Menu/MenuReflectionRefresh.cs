using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuReflectionRefresh : MonoBehaviour
{

    Slider reflectionRefreshSlider; //reference to the ui slider

    // Use this for initialization
    void Start()
    {
        reflectionRefreshSlider = GetComponent<Slider>();
        reflectionRefreshSlider.value = GameController.gameMaster.reflectionUpdateFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.gameMaster.reflectionUpdateFrequency == 0)
            transform.GetChild(3).GetComponent<Text>().text = "Reflection Refresh: Once";
        if (GameController.gameMaster.reflectionUpdateFrequency == 1)
            transform.GetChild(3).GetComponent<Text>().text = "Reflection Refresh: Rare";
        if (GameController.gameMaster.reflectionUpdateFrequency == 2)
            transform.GetChild(3).GetComponent<Text>().text = "Reflection Refresh: Medium";
        if (GameController.gameMaster.reflectionUpdateFrequency == 3)
            transform.GetChild(3).GetComponent<Text>().text = "Reflection Refresh: Often";
        if (GameController.gameMaster.reflectionUpdateFrequency == 4)
            transform.GetChild(3).GetComponent<Text>().text = "Reflection Refresh: Very Often";
        if (GameController.gameMaster.reflectionUpdateFrequency == 5)
            transform.GetChild(3).GetComponent<Text>().text = "Reflection Refresh: Ultra";
    }

    //update the value of the Reflection Refresh Frequnecy in the gameMaster
    public void updateValue()
    {
        GameController.gameMaster.reflectionUpdateFrequency = (int)reflectionRefreshSlider.value;
        GameController.gameMaster.applyQuality();
    }
}
