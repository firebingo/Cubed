using UnityEngine;
using System.Collections;
using UnityStandardAssets.Water;

public class WaterQualityController : MonoBehaviour
{
    Water objectWater; //reference to the objects water script

    // Use this for initialization
    void Start()
    {
        objectWater = GetComponent<Water>();
        setQuality();
    }

    // Update is called once per frame
    void Update()
    {
        //if refresh quality settings is true, refresh quality settings.
        if (GameController.gameMaster.refreshQuality)
            setQuality();
    }

    public void setQuality()
    {
        // **  Set the quality settings for water  ** //
        //waterQuality //0 = simple, 1 = 128, 2 = 256, 3 = 512, 4 = 1024 
        if (GameController.gameMaster.waterQuality == 0)
        {
            objectWater.textureSize = 256;
            objectWater.waterMode = Water.WaterMode.Simple;
        }
        else if (GameController.gameMaster.waterQuality == 1)
        {
            objectWater.waterMode = Water.WaterMode.Reflective;
            objectWater.textureSize = 128;
        }
        else if (GameController.gameMaster.waterQuality == 2)
        {
            objectWater.waterMode = Water.WaterMode.Refractive;
            objectWater.textureSize = 256;
        }
        else if (GameController.gameMaster.waterQuality == 3)
        {
            objectWater.waterMode = Water.WaterMode.Refractive;
            objectWater.textureSize = 512;
        }
        else if (GameController.gameMaster.waterQuality == 4)
        {
            objectWater.waterMode = Water.WaterMode.Refractive;
            objectWater.textureSize = 1024;
        }
    }
}
