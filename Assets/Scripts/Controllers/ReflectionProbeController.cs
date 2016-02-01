using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class ReflectionProbeController : MonoBehaviour
{
    float updateTimer; //timer for updating probe
    public float updateFrequency; //how many seconds there should be between updates.
    public ReflectionProbe rProbe; //reference to the ReflectionProbe component.
    public GameController gameMaster; //reference to the game controller

    //Unity Start() method
    void Start()
    {
        rProbe = GetComponent<ReflectionProbe>();
        gameMaster = GameController.gameMaster;
        setQuality();
    }

    //Unity Update() method
    void Update()
    {
        if (rProbe.refreshMode != 0)
        {
            updateTimer += Time.deltaTime;
            //if the updatetimer is over the frequency time, update the probe.
            if (updateTimer > updateFrequency)
            {
                rProbe.RenderProbe();
                updateTimer = 0;
            }
        }

        //if refresh quality settings is true, refresh quality settings.
        if (gameMaster.refreshQuality)
            setQuality();
    }

    //set the quality settings important for this object
    public void setQuality()
    {
        // **  Set the quality settings for reflections  ** //
        //reflection quality, 0 = 128, 1 = 256, 2 = 512, 3 = 1024
        if (gameMaster.reflectionQuality == 0)
            rProbe.resolution = 128;
        else if (gameMaster.reflectionQuality == 1)
            rProbe.resolution = 256;
        else if (gameMaster.reflectionQuality == 2)
            rProbe.resolution = 512;
        else if (gameMaster.reflectionQuality == 3)
            rProbe.resolution = 1024;

        //reflection update frequency, 0 = on awake, 1 = 0.8s, 2 = 0.5s, 3 = 0.25s, 4 = 0.1s, 5 = 0.05s
        if (gameMaster.reflectionUpdateFrequency == 0)
            rProbe.refreshMode = 0;
        else if (gameMaster.reflectionUpdateFrequency == 1)
        {
            rProbe.refreshMode = ReflectionProbeRefreshMode.ViaScripting;
            updateFrequency = 0.8f;
        }
        else if (gameMaster.reflectionUpdateFrequency == 2)
        {
            rProbe.refreshMode = ReflectionProbeRefreshMode.ViaScripting;
            updateFrequency = 0.5f;
        }
        else if (gameMaster.reflectionUpdateFrequency == 3)
        {
            rProbe.refreshMode = ReflectionProbeRefreshMode.ViaScripting;
            updateFrequency = 0.25f;
        }
        else if (gameMaster.reflectionUpdateFrequency == 4)
        {
            rProbe.refreshMode = ReflectionProbeRefreshMode.ViaScripting;
            updateFrequency = 0.1f;
        }
        else if (gameMaster.reflectionUpdateFrequency == 5)
        {
            rProbe.refreshMode = ReflectionProbeRefreshMode.ViaScripting;
            updateFrequency = 0.05f;
        }
    }
}
