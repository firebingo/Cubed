using UnityEngine;
using System.Collections;
using Smaa;
using UnityStandardAssets.ImageEffects;

//Concept and code for camera from https://www.youtube.com/playlist?list=PLKFvhfT4QOqlEReJ2lSZJk_APVq5sxZ-x

public class CameraMovement : MonoBehaviour
{
    public GameController gameMaster; // reference to the game controller
    public Transform Target; //the object the camera should rotate around, set in editor.

    public float distanceAway; //distance camera tries to stay away from player
    public float distanceUp; //
    private Vector3 targetPosition; //the target position for the camera to move to.
    private Vector3 lookDirection;
    private Vector3 velocityCamSmooth = Vector3.zero;
    public float smoothTime;

    float cameraSpeed; //the speed of the camera's movement.
    public bool inWater; //whether or not the camera is in water.
    public ColorCorrectionLookup waterCorrect; //reference to the color correction for the water.
    public ColorCorrectionLookup lavaCorrect;
    public GlobalFog waterFog;

    // Unity Start() method
    void Start()
    {
        //targetPosition = new Vector3(0.0f, 1.5f, -1.5f);
        //transform.localPosition = targetPosition;
        gameMaster = GameController.gameMaster;
        cameraSpeed = 4.0f;
        inWater = false;
        if (waterCorrect)
            waterCorrect.enabled = false;
        if (lavaCorrect)
            lavaCorrect.enabled = false;
        if (waterFog)
            waterFog.enabled = false;
        setQuality();
    }

    void LateUpdate()
    {
        lookDirection = Target.position - transform.position;
        lookDirection.y = 0;
        lookDirection.Normalize();

        //set the target position to be properly offset from the character.
        targetPosition = Target.position + Vector3.up * distanceUp - lookDirection * distanceAway;

        //Debug.DrawRay(Target.position, Vector3.up * distanceUp, Color.red);
        //Debug.DrawRay(Target.position, -1.0f * Target.forward * distanceAway, Color.blue);
        //Debug.DrawLine(Target.position, targetPosition, Color.magenta);

        wallBlocking(Target.position, ref targetPosition);

        smoothCamera(transform.position, targetPosition);
        transform.LookAt(Target);
    }

    // Unity Update() method
    void Update()
    {
        //if refresh quality settings is true, refresh quality settings.
        if (gameMaster.refreshQuality)
            setQuality(); 
    }

    private void smoothCamera(Vector3 from, Vector3 to)
    {
        transform.position = Vector3.SmoothDamp(from, to, ref velocityCamSmooth, smoothTime);
    }

    private void wallBlocking(Vector3 from, ref Vector3 to)
    {
        RaycastHit wallHit = new RaycastHit();
        if(Physics.Linecast(from, to, out wallHit, 1 << 8))
        {
            to = new Vector3(wallHit.point.x + transform.forward.x, to.y, wallHit.point.z + transform.forward.z);
        }
    }

    //set the quality settings important for this object
    private void setQuality()
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
                GetComponent<ScreenSpaceAmbientOcclusion>().m_SampleCount = ScreenSpaceAmbientOcclusion.SSAOSamples.Low;
            }
            else if (gameMaster.SSAOQuality == 2)
            {
                GetComponent<ScreenSpaceAmbientOcclusion>().gameObject.SetActive(true);
                GetComponent<ScreenSpaceAmbientOcclusion>().enabled = true;
                GetComponent<ScreenSpaceAmbientOcclusion>().m_SampleCount = ScreenSpaceAmbientOcclusion.SSAOSamples.Medium;
            }
            else if (gameMaster.SSAOQuality == 3)
            {
                GetComponent<ScreenSpaceAmbientOcclusion>().gameObject.SetActive(true);
                GetComponent<ScreenSpaceAmbientOcclusion>().enabled = true;
                GetComponent<ScreenSpaceAmbientOcclusion>().m_SampleCount = ScreenSpaceAmbientOcclusion.SSAOSamples.High;
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


    void OnTriggerExit(Collider iOther)
    {
        if (iOther.gameObject.tag == "Water")
        {
            if (transform.position.y > iOther.transform.position.y)
            {
                inWater = false;
                waterCorrect.enabled = false;
                if (waterFog)
                    waterFog.enabled = false;
            }
            else if (transform.position.y < iOther.transform.position.y)
            {
                inWater = true;
                waterCorrect.enabled = true;
                if (waterFog)
                    waterFog.enabled = true;
            }
        }
        if (iOther.gameObject.tag == "Lava")
        {
            if (transform.position.y > iOther.transform.position.y)
            {
                inWater = false;
                lavaCorrect.enabled = false;
                if (waterFog)
                    waterFog.enabled = false;
            }
            else if (transform.position.y < iOther.transform.position.y)
            {
                inWater = true;
                lavaCorrect.enabled = true;
                if (waterFog)
                    waterFog.enabled = true;
            }
        }
    }
}
