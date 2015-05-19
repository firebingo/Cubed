using UnityEngine;
using System.Collections;

public class ReflectionProbeController : MonoBehaviour
{
    float updateTimer;
    public float updateFrequency;
    public ReflectionProbe rProbe;
    
    void Start()
    {
        rProbe = GetComponent<ReflectionProbe>() as ReflectionProbe;
    }

    void Update()
    {
        updateTimer += Time.deltaTime;
        if(updateTimer > updateFrequency)
        {
            rProbe.RenderProbe();
            updateTimer = 0;
        }
    }
}
