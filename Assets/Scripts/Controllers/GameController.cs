using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameController : MonoBehaviour
{
    public static GameController gameMaster;

    //Player Variables
    public int playerMaxJumpCount;
    public bool playerCanUseIceShield;
    public bool playerCanUseFireShield;

    //Options
    public int SMAAQulity; //0 = low, 1 = medium, 2 = high, 3 = Ultra 
    public int SSAOQuality; //0 = low, 1 = medium, 2 = high
    public int postProcessingQuality; //0 = no bloom or sun shafts, 1 = bloom, 2 = sun shafts
    public int reflectionQuality; //the resolution of cubemaps. 0 = 128, 1 = 256, 2 = 512, 3 = 1024
    public int reflectionUpdateFrequency; //frequnecy of cubemap updates, 0 = on awake, 1 = 0.8s, 2 = 0.5s, 3 = 0.25s, 4 = 0.1s, 5 = 0.05s
    public int shadowQuality; //0 = low res, 2 cascades, 1 = medium, two cascades, 2 = high, four cascades, 3 = very high
    public bool vSync; // false = off, true = on
    public bool Fullscreen; // true = fullscreen, false = windowed

    public bool refreshQuality; //whether or not objects with quality settings whould refresh

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
        
    }

    // Unity Start() method
    void Start()
    {
        if (loadOptions())
            refreshQuality = true;
        else
        {
            SMAAQulity = 2;
            SSAOQuality = 2;
            postProcessingQuality = 2;
            reflectionQuality = 2;
            reflectionUpdateFrequency = 3;
            shadowQuality = 2;
            vSync = false;
            Fullscreen = true;
            refreshQuality = true;
        }

        saveOptions();
    }

    // Unity Update() method
    void Update()
    {
        //if refresh quality settings is true, refresh quality settings.
        if(refreshQuality)
            setQuality();
    }

    //set the quality settings important for this object
    public void setQuality()
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
        if (!vSync)
            QualitySettings.vSyncCount = 0;
        else
            QualitySettings.vSyncCount = 1;

        //fullscreen
        if (Fullscreen)
            Screen.fullScreen = true;
        else
            Screen.fullScreen = false;

        StartCoroutine("waitForQualityRefresh");
        refreshQuality = true;
    }

    //saves the game options to a file.
    public void saveOptions()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.dataPath + "/gameOptions.wort", FileMode.OpenOrCreate);

        GameOptions options = new GameOptions(SMAAQulity, SSAOQuality, postProcessingQuality, reflectionQuality, reflectionUpdateFrequency, shadowQuality, vSync, Fullscreen);

        bf.Serialize(file, options);
        file.Close();
    }

    //loads the game options from a file if it exists.
    public bool loadOptions()
    {
        if (File.Exists(Application.dataPath + "/gameOptions.wort"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.dataPath + "/gameOptions.wort", FileMode.Open);
            GameOptions options = bf.Deserialize(file) as GameOptions;
            file.Close();

            SMAAQulity = options.SMAAQulity;
            SSAOQuality = options.SSAOQuality;
            postProcessingQuality = options.postProcessingQuality;
            reflectionQuality = options.reflectionQuality;
            reflectionUpdateFrequency = options.reflectionUpdateFrequency;
            shadowQuality = options.shadowQuality;
            vSync = options.vSync;
            Fullscreen = options.Fullscreen;
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
    public GameOptions(int iSMAAQuality, int iSSAOQuality, int iPostProcessingQuality, int iReflectionQuality, int iReflectionUpdateFrequency, int iShadowQuality, bool iVsync, bool iFullScreen)
    {
        SMAAQulity = iSMAAQuality;
        SSAOQuality = iSSAOQuality;
        postProcessingQuality = iPostProcessingQuality;
        reflectionQuality = iReflectionQuality;
        reflectionUpdateFrequency = iReflectionUpdateFrequency;
        shadowQuality = iShadowQuality;
        vSync = iVsync;
        Fullscreen = iFullScreen;
    }

    //Options
    public int SMAAQulity; //0 = low, 1 = medium, 2 = high, 3 = Ultra 
    public int SSAOQuality; //0 = low, 1 = medium, 2 = high
    public int postProcessingQuality; //0 = no bloom or sun shafts, 1 = bloom, 2 = sun shafts
    public int reflectionQuality; //the resolution of cubemaps. 0 = 128, 1 = 256, 2 = 512, 3 = 1024
    public int reflectionUpdateFrequency; //frequnecy of cubemap updates, 0 = on awake, 1 = 0.8s, 2 = 0.5s, 3 = 0.25s, 4 = 0.1s, 5 = 0.05s
    public int shadowQuality; //0 = low res, 2 cascades, 1 = medium, two cascades, 2 = high, four cascades, 3 = very high
    public bool vSync; // false = off, true = on
    public bool Fullscreen; // true = fullscreen, false = windowed
}
