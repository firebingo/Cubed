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
        setQuality();
    }

    // Unity Update() method
    void Update()
    {
        if (Target)
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
        }

        //if refresh quality settings is true, refresh quality settings.
        if (gameMaster.refreshQuality)
            setQuality();
    }

    //set the quality settings important for this object
    public void setQuality()
    {
        if (GetComponent<SMAA>())
        {
            // **  Set the quality settings for the camera scripts  ** //
            //SMAA, 0 = off, 1 = low, 2 = medium, 3 = high, 4 = ultra 
            if (gameMaster.SMAAQuality == 0)
                GetComponent<SMAA>().enabled = false;
            else if (gameMaster.SMAAQuality == 1)
            {
                GetComponent<SMAA>().enabled = true;
                GetComponent<SMAA>().Quality = QualityPreset.Low;
            }
            else if (gameMaster.SMAAQuality == 2)
            {
                GetComponent<SMAA>().enabled = true;
                GetComponent<SMAA>().Quality = QualityPreset.Medium;
            }
            else if (gameMaster.SMAAQuality == 3)
            {
                GetComponent<SMAA>().enabled = true;
                GetComponent<SMAA>().Quality = QualityPreset.High;
            }
            else if (gameMaster.SMAAQuality == 4)
            {
                GetComponent<SMAA>().enabled = true;
                GetComponent<SMAA>().Quality = QualityPreset.Ultra;
            }
        }

        if (GetComponent<ScreenSpaceAmbientOcclusion>())
        {
            //SSAO, 0 = off, 1 = low, 2 = medium, 3 = high
            if (gameMaster.SSAOQuality == 0)
                GetComponent<ScreenSpaceAmbientOcclusion>().enabled = false;
            else if (gameMaster.SSAOQuality == 1)
            {
                GetComponent<ScreenSpaceAmbientOcclusion>().gameObject.SetActive(true);
                GetComponent<ScreenSpaceAmbientOcclusion>().enabled = true;
            }
            else if (gameMaster.SSAOQuality == 2)
            {
                GetComponent<ScreenSpaceAmbientOcclusion>().gameObject.SetActive(true);
                GetComponent<ScreenSpaceAmbientOcclusion>().enabled = true;
            }
            else if (gameMaster.SSAOQuality == 3)
            {
                GetComponent<ScreenSpaceAmbientOcclusion>().gameObject.SetActive(true);
                GetComponent<ScreenSpaceAmbientOcclusion>().enabled = true;
            }
        }

        if (GetComponent<BloomOptimized>() && GetComponent<SunShafts>())
        {
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
}
