using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuSSAO : MonoBehaviour
{

    Slider SSAOSlider; //reference to the ui slider

    // Use this for initialization
    void Start()
    {
        SSAOSlider = GetComponent<Slider>();
        SSAOSlider.value = GameController.gameMaster.SSAOQuality;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.gameMaster.SSAOQuality == 0)
            transform.GetChild(3).GetComponent<Text>().text = "SSAO: Off";
        if (GameController.gameMaster.SSAOQuality == 1)        
            transform.GetChild(3).GetComponent<Text>().text = "SSAO: Low";
        if (GameController.gameMaster.SSAOQuality == 2)        
            transform.GetChild(3).GetComponent<Text>().text = "SSAO: Medium";
        if (GameController.gameMaster.SSAOQuality == 3)        
            transform.GetChild(3).GetComponent<Text>().text = "SSAO: High";
    }

    //update the value of the SSAO in the gameMaster
    public void updateValue()
    {
        GameController.gameMaster.SSAOQuality = (int)SSAOSlider.value;
        GameController.gameMaster.applyQuality();
    }
}
