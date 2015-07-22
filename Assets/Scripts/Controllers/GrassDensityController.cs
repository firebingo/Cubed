using UnityEngine;
using System.Collections;

public class GrassDensityController : MonoBehaviour
{
    int lastQuality;
    public GameObject lowGrass;
    public GameObject medGrass;
    public GameObject highGrass;

    // Use this for initialization
    void Start()
    {
        if(!lowGrass || !medGrass || !highGrass)
        {
#if UNITY_EDITOR
            Debug.Log(this.gameObject + "Does not have Grass Qualities Set, Destroying");
#endif
            GameObject.Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.gameMaster.grassQuality != lastQuality)
        {
            if (GameController.gameMaster.grassQuality == 1 || GameController.gameMaster.grassQuality == 2)
            {
                lowGrass.SetActive(true);
                medGrass.SetActive(false);
                highGrass.SetActive(false);
            }
            if (GameController.gameMaster.grassQuality == 3 || GameController.gameMaster.grassQuality == 4)
            {
                lowGrass.SetActive(false);
                medGrass.SetActive(true);
                highGrass.SetActive(false);
            }
            if (GameController.gameMaster.grassQuality == 5)
            {
                lowGrass.SetActive(false);
                medGrass.SetActive(false);
                highGrass.SetActive(true);
            }
        }
        lastQuality = GameController.gameMaster.grassQuality;
    }
}
