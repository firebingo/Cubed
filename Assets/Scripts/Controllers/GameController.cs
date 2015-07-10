using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameController : MonoBehaviour
{
    public static GameController gameMaster;

    //Player Variables
    public Player gamePlayer;
    public int playerMaxJumpCount;
    public bool playerCanUseIceShield;
    public bool playerCanUseFireShield;
    public float playerMaxHealth;
    public float playerMaxIceShieldTime;
    public float playerMaxFireShieldTime;
    public bool playerCanAirDash;
    public bool playerCanDash;
    public Vector3 playerPosition;
    public float playerDefense;

    //Options
    public int SMAAQuality; //0 = off, 1 = low, 2 = medium, 3 = high, 4 = ultra 
    public int SSAOQuality; //0 = off, 1 = low, 2 = medium, 3 = high
    public int postProcessingQuality; //0 = no bloom or sun shafts, 1 = bloom, 2 = sun shafts
    public int reflectionQuality; //the resolution of cubemaps. 0 = 128, 1 = 256, 2 = 512, 3 = 1024
    public int reflectionUpdateFrequency; //frequnecy of cubemap updates, 0 = on awake, 1 = 0.8s, 2 = 0.5s, 3 = 0.25s, 4 = 0.1s, 5 = 0.05s
    public int shadowQuality; //0 = low res, 2 cascades, 1 = medium, two cascades, 2 = high, four cascades, 3 = very high
    public int waterQuality; //0 = simple, 1 = 128, 2 = 256, 3 = 512, 4 = 1024 
    public int frameTarget; //frame rate target, 0 = 30, 1 = 60, 2 = 75, 3 = 120, 4 = 144, 5 = 300
    public bool vSync; // false = off, true = on
    public bool Fullscreen; // true = fullscreen, false = windowed

    public bool refreshQuality; //whether or not objects with quality settings whould refresh

    //camera options
    public float cameraSensitivity; //sensitivity of camera movement
    public short cameraXInvert; //camera invert on the horizontal.
    public short cameraYInvert; //camera invert on the vertical.


    // Unity Awake() method
    void Awake()
    {
        if (gameMaster == null)
        {
            DontDestroyOnLoad(this.gameObject);
            gameMaster = this;
        }
        else if(gameMaster != this)
        {
            Destroy(this.gameObject);
        }

        playerMaxJumpCount = 2;
        playerMaxFireShieldTime = 20;
        playerMaxIceShieldTime = 20;
        playerMaxHealth = 100;
        playerDefense = 1.0f;
        playerCanAirDash = true;
        playerCanDash = true;
        playerCanUseFireShield = true;
        playerCanUseIceShield = true;
    }

    // Unity Start() method
    void Start()
    {
        if (loadOptions())
            refreshQuality = true;
        else
        {
            SMAAQuality = 2;
            SSAOQuality = 2;
            postProcessingQuality = 2;
            reflectionQuality = 2;
            reflectionUpdateFrequency = 3;
            shadowQuality = 2;
            waterQuality = 2;
            frameTarget = 5;
            vSync = false;
            Fullscreen = true;
            refreshQuality = true;
            cameraSensitivity = 1;
            cameraXInvert = 1;
            cameraYInvert = 1;
        }

        saveOptions();
    }

    // Unity Update() method
    void Update()
    {
        //if refresh quality settings is true, refresh quality settings.
        if(refreshQuality)
            setQuality();

        if (Input.GetKeyDown(KeyCode.Escape))
            loadScene(0);
    }

    public void loadScene(int iIndex)
    {
        Application.LoadLevelAsync(iIndex);
    }

    //set the quality settings important for this object
    private void setQuality()
    {
        // **  Set the quality settings  ** //
        //Shadows, 0 = hard shadows, low res, 2 cascades, 1 = hard and soft, medium, two cascades, 2 = high, four cascades, 3 = very high
        if (shadowQuality == 0)
            QualitySettings.SetQualityLevel(0);
        else if (shadowQuality == 1)
            QualitySettings.SetQualityLevel(1);
        else if (shadowQuality == 2)
            QualitySettings.SetQualityLevel(2);
        else if (shadowQuality == 3)
            QualitySettings.SetQualityLevel(3);

        //vsync
        if (vSync)
            QualitySettings.vSyncCount = 1;
        else
            QualitySettings.vSyncCount = 0;

        //fullscreen
        if (Fullscreen)
            Screen.fullScreen = true;
        else
            Screen.fullScreen = false;

        //frame rate target
        if (frameTarget == 0)
            Application.targetFrameRate = 30;
        if (frameTarget == 1)
            Application.targetFrameRate = 60;
        if (frameTarget == 2)
            Application.targetFrameRate = 75;
        if (frameTarget == 3)
            Application.targetFrameRate = 120;
        if (frameTarget == 4)
            Application.targetFrameRate = 144;
        if (frameTarget == 5)
            Application.targetFrameRate = 300;

        StartCoroutine("waitForQualityRefresh");
    }

    //applies quality settings.
    public void applyQuality()
    {
        saveOptions();
        refreshQuality = true;
        setQuality();
    }

    //saves the game options to a file.
    private void saveOptions()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.dataPath + "/gameOptions.wort", FileMode.OpenOrCreate);

        GameOptions options = new GameOptions(SMAAQuality, SSAOQuality, postProcessingQuality, reflectionQuality, reflectionUpdateFrequency, shadowQuality, vSync, Fullscreen, waterQuality, frameTarget, cameraSensitivity, cameraXInvert, cameraYInvert);

        bf.Serialize(file, options);
        file.Close();
    }

    //loads the game options from a file if it exists.
    private bool loadOptions()
    {
        if (File.Exists(Application.dataPath + "/gameOptions.wort"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.dataPath + "/gameOptions.wort", FileMode.Open);
            GameOptions options = bf.Deserialize(file) as GameOptions;
            file.Close();

            SMAAQuality = options.SMAAQuality;
            SSAOQuality = options.SSAOQuality;
            postProcessingQuality = options.postProcessingQuality;
            reflectionQuality = options.reflectionQuality;
            reflectionUpdateFrequency = options.reflectionUpdateFrequency;
            shadowQuality = options.shadowQuality;
            vSync = options.vSync;
            Fullscreen = options.Fullscreen;
            waterQuality = options.waterQuality;
            frameTarget = options.frameTarget;
            cameraSensitivity = options.cameraSensitivity;
            cameraXInvert = options.cameraXInvert;
            cameraYInvert = options.cameraYInvert;
            return true;
        }
        else
            return false;
    }

    //waits for the rest of the objects that need to refresh quality before setting it back to false.
    IEnumerator waitForQualityRefresh()
    {
        yield return new WaitForSeconds(0.1f);
        refreshQuality = false;
    }
}

//class for storing the game options.
[Serializable]
class GameOptions
{
    public GameOptions(int iSMAAQuality, int iSSAOQuality, int iPostProcessingQuality, int iReflectionQuality, int iReflectionUpdateFrequency, int iShadowQuality, bool iVsync, bool iFullScreen, int iWaterQuality, int iFrameTarget, float iCameraSensitivity, short iCameraXInvert, short iCameraYInvert)
    {
        SMAAQuality = iSMAAQuality;
        SSAOQuality = iSSAOQuality;
        postProcessingQuality = iPostProcessingQuality;
        reflectionQuality = iReflectionQuality;
        reflectionUpdateFrequency = iReflectionUpdateFrequency;
        shadowQuality = iShadowQuality;
        waterQuality = iWaterQuality;
        vSync = iVsync;
        Fullscreen = iFullScreen;
        frameTarget = iFrameTarget;
        cameraSensitivity = iCameraSensitivity;
        cameraXInvert = iCameraXInvert;
        cameraYInvert = iCameraYInvert;
    }

    //Options
    public int SMAAQuality; //0 = off, 1 = low, 2 = medium, 3 = high, 4 = ultra
    public int SSAOQuality; //0 = off, 1 = low, 2 = medium, 3 = high
    public int postProcessingQuality; //0 = no bloom or sun shafts, 1 = bloom, 2 = sun shafts
    public int reflectionQuality; //the resolution of cubemaps. 0 = 128, 1 = 256, 2 = 512, 3 = 1024
    public int reflectionUpdateFrequency; //frequnecy of cubemap updates, 0 = on awake, 1 = 0.8s, 2 = 0.5s, 3 = 0.25s, 4 = 0.1s, 5 = 0.05s
    public int shadowQuality; //0 = low res, 2 cascades, 1 = medium, two cascades, 2 = high, four cascades, 3 = very high
    public int waterQuality; //0 = simple, 1 = 128, 2 = 256, 3 = 512, 4 = 1024 
    public int frameTarget; //frame rate target, 0 = 30, 1 = 60, 2 = 75, 3 = 120, 4 = 144, 5 = 300
    public bool vSync; // false = off, true = on
    public bool Fullscreen; // true = fullscreen, false = windowed

    //camera options
    public float cameraSensitivity; //sensitivity of camera movement
    public short cameraXInvert; //camera invert on the horizontal.
    public short cameraYInvert; //camera invert on the vertical.
}
