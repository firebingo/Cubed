using UnityEngine;
using System.Collections;
using Smaa;
using UnityStandardAssets.ImageEffects;

public class CameraMovement : MonoBehaviour
{
    public GameController gameMaster; // reference to the game controller
    public GameObject Target; //the object the camera should rotate around, set in editor.
    public Vector3 targetPosition; //the target position for the camera to move to.
    float cameraSpeed; //the speed of the camera's movement.
    public bool inWater; //whether or not the camera is in water.
    public ColorCorrectionLookup waterCorrect; //reference to the color correction for the water.
    public ColorCorrectionLookup lavaCorrect;
    public GlobalFog waterFog;

    // Unity Start() method
    void Start()
    {
        targetPosition = new Vector3(0.0f, 1.5f, -1.5f);
        transform.localPosition = targetPosition;
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

    // Unity Update() method
    void FixedUpdate()
    {
        if (!gameMaster.isPaused)
        {
            if (Target)
            {
                //face the player
                transform.LookAt(Target.transform);

                // Controller Input
                Vector2 inputCVector = new Vector2(Input.GetAxis("CameraHorizontal"), Input.GetAxis("CameraVertical"));

                //Horizontal controller camera movement
                if (inputCVector.x > 0)
                {
                    targetPosition += ((transform.right - (transform.forward * 0.5f)).normalized * -inputCVector.x * cameraSpeed * gameMaster.cameraSensitivity * Time.deltaTime) * gameMaster.cameraXInvert;
                }
                else if (inputCVector.x < 0)
                {
                    targetPosition += ((transform.right + (transform.forward * 0.5f)).normalized * -inputCVector.x * cameraSpeed * gameMaster.cameraSensitivity * Time.deltaTime) * gameMaster.cameraXInvert;
                }

                //Vertical controller camera movement
                if (inputCVector.y > 0 && (transform.localEulerAngles.x < 80 || transform.localEulerAngles.x > 110))
                {
                    targetPosition += ((transform.up + (transform.forward * 0.5f)).normalized * inputCVector.y * cameraSpeed * gameMaster.cameraSensitivity * Time.deltaTime) * gameMaster.cameraYInvert;
                }
                else if (inputCVector.y < 0)
                {
                    targetPosition += ((transform.up - (transform.forward * 0.5f)).normalized * inputCVector.y * cameraSpeed * gameMaster.cameraSensitivity * Time.deltaTime) * gameMaster.cameraYInvert;
                }

                // Mouse Input
                Vector2 inputMVector = new Vector2(Input.GetAxis("CameraHorizontalM"), Input.GetAxis("CameraVerticalM"));
                inputMVector = new Vector2(inputMVector.x / (Screen.width* 0.01f), inputMVector.y / (Screen.height * 0.01f));
                

                //Horizontal mouse camera movement
                if (inputMVector.x > 0)
                {
                    targetPosition += ((transform.right - (transform.forward * 0.75f)).normalized * -inputMVector.x * cameraSpeed * gameMaster.cameraSensitivity * Time.deltaTime) * gameMaster.cameraXInvert;
                }
                else if (inputMVector.x < 0)
                {
                    targetPosition += ((transform.right + (transform.forward * 0.75f)).normalized * -inputMVector.x * cameraSpeed * gameMaster.cameraSensitivity * Time.deltaTime) * gameMaster.cameraXInvert;
                }

                //Vertical mouse camera movement
                if (inputMVector.y > 0 && (transform.localEulerAngles.x < 80 || transform.localEulerAngles.x > 110))
                {
                    targetPosition += ((transform.up + (transform.forward * 0.75f)).normalized * inputMVector.y * cameraSpeed * gameMaster.cameraSensitivity * Time.deltaTime) * gameMaster.cameraYInvert;
                }
                else if (inputMVector.y < 0)
                {
                    targetPosition += ((transform.up - (transform.forward * 0.75f)).normalized * inputMVector.y * cameraSpeed * gameMaster.cameraSensitivity * Time.deltaTime) * gameMaster.cameraYInvert;
                }

                //Camera positional checks
                //if the camera is too far away from the player, move it closer.
                if (Vector3.Distance(transform.position, Target.transform.position) > 2.2)
                {
                    targetPosition += transform.forward * 0.1f;
                }
                //if the camera is too close to the player, move it further.
                if (Vector3.Distance(transform.position, Target.transform.position) < 2.0)
                {
                    targetPosition -= transform.forward * 0.05f;
                }
                RaycastHit hit;
                //check if the camera is moving towards a wall
                if (Physics.Raycast(transform.position, (targetPosition - transform.localPosition).normalized, out hit, 0.45f, 1 << 8))
                {
                    targetPosition += hit.normal * 0.1f;
                }
                //check if the camera is too close to the floor and move it up if it is.
                if (Physics.Raycast(transform.position, -transform.forward, out hit, 0.45f, 1 << 8))
                {
                    targetPosition += transform.forward * 0.3f;
                }
                //check if the camera has anything behind it or in front of it, and move it up if there is.
                if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.45f, 1 << 8) || Physics.Raycast(transform.position, transform.forward, out hit, 0.35f, 1 << 8))
                {
                    targetPosition += Vector3.up * 0.05f;
                }
                //move the camera
                transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, 0.2f);
            }
        }
        //if refresh quality settings is true, refresh quality settings.
        if (gameMaster.refreshQuality)
            setQuality();
    }

    //void LateUpdate()
    //{
    //    if (!gameMaster.isPaused)
    //    {
    //        if (Target)
    //        {
    //            //if the camera is too far away from the player, move it closer.
    //            if (Vector3.Distance(transform.position, Target.transform.position) > 2.2)
    //            {
    //                targetPosition += transform.forward * 0.1f;
    //            }
    //            //if the camera is too close to the player, move it further.
    //            if (Vector3.Distance(transform.position, Target.transform.position) < 2.0)
    //            {
    //                targetPosition -= transform.forward * 0.05f;
    //            }
    //            RaycastHit hit;
    //            //check if the camera is moving towards a wall
    //            if (Physics.Raycast(transform.position, (targetPosition - transform.localPosition).normalized, out hit, 0.45f, 1 << 8))
    //            {
    //                targetPosition += hit.normal * 0.1f;
    //            }
    //            //check if the camera is too close to the floor and move it up if it is.
    //            if (Physics.Raycast(transform.position, -transform.forward, out hit, 0.45f, 1 << 8))
    //            {
    //                targetPosition += transform.forward * 0.3f;
    //            }
    //            //check if the camera has anything behind it or in front of it, and move it up if there is.
    //            if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.45f, 1 << 8) || Physics.Raycast(transform.position, transform.forward, out hit, 0.35f, 1 << 8))
    //            {
    //                targetPosition += Vector3.up * 0.05f;
    //            }
    //            //move the camera
    //            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, 0.9f);
    //        }
    //    }
    //}

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
