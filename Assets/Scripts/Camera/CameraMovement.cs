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
    private enum cameraStates { autoFollow, freeCam } //the states the camera can be in.
    private int cameraState; //the current state the camera is in

    float cameraYSpeed;
    float cameraXSpeed; //the speed of the camera's movement.
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
        cameraXSpeed = 200.0f;
        cameraYSpeed = 3.5f;
        inWater = false;
        if (waterCorrect)
            waterCorrect.enabled = false;
        if (lavaCorrect)
            lavaCorrect.enabled = false;
        if (waterFog)
            waterFog.enabled = false;
        setQuality();
        cameraState = 0;
    }

    void LateUpdate()
    {
        if (!gameMaster.isPaused)
        {

            float cameraVertical = Input.GetAxis("CameraVertical");
            float cameraHorizontal = Input.GetAxis("CameraHorizontal");

            if(Input.GetAxis("DPadVertical") > 0)
            {
                cameraState = (int)cameraStates.autoFollow;
            }
            if(Input.GetAxis("DPadVertical") < 0)
            {
                cameraState = (int)cameraStates.freeCam;
            }

            switch (cameraState)
            {
                //auto follow state
                case (int)cameraStates.autoFollow:

                    //find the vector between the target and the camera's position, remove it's y component, then normalize it.
                    lookDirection = Vector3.Normalize(Target.position - transform.position);
                    lookDirection.y = 0;

                    //set the target position to be properly offset from the character.
                    targetPosition = Target.position + Vector3.up * distanceUp - lookDirection * distanceAway;

                    //camera horizontal movement
                    if (cameraHorizontal > 0 || cameraHorizontal < 0)
                        transform.RotateAround(Target.position, Vector3.up, cameraXSpeed * cameraHorizontal * gameMaster.cameraXInvert * gameMaster.cameraSensitivity * Time.deltaTime);

                    //camera horizontal movement
                    if (cameraVertical > 0 || cameraVertical < 0)
                    {
                        //if the angle is above 75 degrees push it back before letting it move further
                        if (transform.rotation.eulerAngles.x > 75 && transform.rotation.eulerAngles.x < 95)
                            transform.rotation = Quaternion.Euler(75, transform.rotation.y, transform.rotation.z);
                        else
                            transform.RotateAround(Target.position, transform.right, cameraXSpeed * cameraVertical * gameMaster.cameraYInvert * gameMaster.cameraSensitivity * Time.deltaTime);
                    }

                    //Debug.DrawRay(Target.position, Vector3.up * distanceUp, Color.red);
                    //Debug.DrawRay(Target.position, -1.0f * Target.forward * distanceAway, Color.blue);
                    //Debug.DrawLine(Target.position, targetPosition, Color.magenta);

                    //prevent camera from clipping into environment objects.
                    wallBlocking(Target.position, ref targetPosition);

                    //smooth the camera's position to it's new target position then look at the target.
                    smoothCamera(transform.position, targetPosition);

                    break;
                case (int)cameraStates.freeCam:

                    lookDirection = Vector3.Normalize(Target.position - transform.position);
                    lookDirection.y = 0;

                    targetPosition = Target.position + Vector3.up * (targetPosition.y - Target.position.y) - (lookDirection * distanceAway);

                    transform.position = targetPosition;

                    //camera horizontal movement
                    if (cameraHorizontal > 0 || cameraHorizontal < 0)
                        transform.RotateAround(Target.position, Vector3.up, cameraXSpeed * cameraHorizontal * gameMaster.cameraXInvert * gameMaster.cameraSensitivity * Time.deltaTime);

                    //camera vertical movement
                    if (cameraVertical > 0 || cameraVertical < 0)
                        transform.RotateAround(Target.position, transform.right, cameraXSpeed * cameraVertical * gameMaster.cameraYInvert * gameMaster.cameraSensitivity * Time.deltaTime);

                    //makes sure the camera is never to directly above the player
                    if (transform.rotation.eulerAngles.x > 80 && transform.rotation.eulerAngles.x < 95)
                        {
                            transform.rotation = Quaternion.Euler(79, transform.rotation.y, transform.rotation.z);
                            transform.position = new Vector3(transform.position.x, Target.position.y + 1.75f, transform.position.z);
                        }

                    //prevent camera from clipping into environment objects.
                    RaycastHit wallHit = new RaycastHit();
                    if (Physics.Linecast(Target.position, transform.position, out wallHit, 1 << 8))
                    {
                        transform.position = new Vector3(wallHit.point.x, wallHit.point.y, wallHit.point.z);
                    }

                    targetPosition = transform.position;

                    break;
            }
        }

        //make the camera look at the Target
        transform.LookAt(Target);        
    }

    // Unity Update() method
    void Update()
    {
        //if refresh quality settings is true, refresh quality settings.
        if (gameMaster.refreshQuality)
            setQuality();
    }

    //Camera smoothing method.
    //Sets its new position to be a interpolated value between it's current position (from) and it's target (to) based of a time.
    private void smoothCamera(Vector3 from, Vector3 to)
    {
        transform.position = Vector3.SmoothDamp(from, to, ref velocityCamSmooth, smoothTime);
    }

    //Camera clipping prevention method.
    //if a linecast between the camera's position and target position hits an environment object, 
    // change the target position to be where the linecast hit plus it's forward to prevent it from
    // clipping into the wall.
    private void wallBlocking(Vector3 from, ref Vector3 to)
    {
        RaycastHit wallHit = new RaycastHit();
        if (Physics.Linecast(from, to, out wallHit, 1 << 8))
        {
            Debug.DrawLine(to, wallHit.point, Color.red);
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
