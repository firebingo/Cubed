using UnityEngine;
using System.Collections;
using Smaa;
using UnityStandardAssets.ImageEffects;

public class CameraMovement : MonoBehaviour
{
    public GameController gameMaster; // reference to the game controller
    public GameObject Target; //the object the camera should rotate around, set in editor.

    // Unity Start() method
    void Start()
    {
        gameMaster = GameController.gameMaster;

    }

    // Unity Update() method
    void Update()
    {
        //Horizontal camera movement
        if (Input.GetAxis("CameraHorizontal") > 0)
        {
            transform.RotateAround(Target.transform.position, Vector3.up, 85 * Input.GetAxis("CameraHorizontal") * Time.deltaTime);
        }
        else if (Input.GetAxis("CameraHorizontal") < 0)
        {
            transform.RotateAround(Target.transform.position, Vector3.up, 85 * Input.GetAxis("CameraHorizontal") * Time.deltaTime);
        }

        //Vertical camera movement
        if (Input.GetAxis("CameraVertical") > 0 && transform.eulerAngles.x < 80)
        {
            transform.RotateAround(Target.transform.position, transform.right, 65 * Input.GetAxis("CameraVertical") * Time.deltaTime);
        }
        else if (Input.GetAxis("CameraVertical") < 0 && transform.eulerAngles.x > 10)
        {
            transform.RotateAround(Target.transform.position, transform.right, 65 * Input.GetAxis("CameraVertical") * Time.deltaTime);
        }

        //if refresh quality settings is true, refresh quality settings.
        if (gameMaster.refreshQuality)
            setQuality();
    }

    //set the quality settings important for this object
    public void setQuality()
    {
        // **  Set the quality settings for the camera scripts  ** //
        //SMAA, 0 = low, 1 = medium, 2 = high, 3 = Ultra 
        if (gameMaster.SMAAQulity == 0)
            GetComponent<SMAA>().Quality = QualityPreset.Low;
        else if (gameMaster.SMAAQulity == 1)
            GetComponent<SMAA>().Quality = QualityPreset.Medium;
        else if (gameMaster.SMAAQulity == 2)
            GetComponent<SMAA>().Quality = QualityPreset.High;
        else if (gameMaster.SMAAQulity == 3)
            GetComponent<SMAA>().Quality = QualityPreset.Ultra;

        //SSAO, 0 = low, 1 = medium, 2 = high
        if (gameMaster.SSAOQuality == 0)
            GetComponent<ScreenSpaceAmbientOcclusion>().m_SampleCount = ScreenSpaceAmbientOcclusion.SSAOSamples.Low;
        else if (gameMaster.SSAOQuality == 1)
            GetComponent<ScreenSpaceAmbientOcclusion>().m_SampleCount = ScreenSpaceAmbientOcclusion.SSAOSamples.Medium;
        else if (gameMaster.SSAOQuality == 2)
            GetComponent<ScreenSpaceAmbientOcclusion>().m_SampleCount = ScreenSpaceAmbientOcclusion.SSAOSamples.High;

        //Post Processing, 0 = no bloom or sun shafts, 1 = bloom, 2 = sun shafts
        if (gameMaster.postProcessingQuality == 0)
        {
            GetComponent<BloomOptimized>().enabled = false;
            GetComponent<SunShafts>().enabled = false;
        }
        else if (gameMaster.postProcessingQuality == 1)
        {
            GetComponent<BloomOptimized>().enabled = true;
            GetComponent<SunShafts>().enabled = false;
        }
        else if (gameMaster.postProcessingQuality == 2)
        {
            GetComponent<BloomOptimized>().enabled = true;
            GetComponent<SunShafts>().enabled = true;
        }
    }
}
