using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadTestScene : MonoBehaviour
{
    public void loadScene(int iIndex)
    {
        GameController.gameMaster.loadScene(iIndex);
    }
}
