using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuPost : MonoBehaviour
{

    Slider PostSlider; //reference to the ui slider

    // Use this for initialization
    void Start()
    {
        PostSlider = GetComponent<Slider>();
        PostSlider.value = GameController.gameMaster.postProcessingQuality;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.gameMaster.postProcessingQuality == 0)
            transform.GetChild(3).GetComponent<Text>().text = "Post Processing: Off";
        if (GameController.gameMaster.postProcessingQuality == 1)
            transform.GetChild(3).GetComponent<Text>().text = "Post Processing: Medium";
        if (GameController.gameMaster.postProcessingQuality == 2)
            transform.GetChild(3).GetComponent<Text>().text = "Post Processing: High";
    }

    //update the value of the Post Processing in the gameMaster
    public void updateValue()
    {
        GameController.gameMaster.postProcessingQuality = (int)PostSlider.value;
        GameController.gameMaster.applyQuality();
    }
}
