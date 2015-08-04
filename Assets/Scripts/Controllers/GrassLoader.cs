using UnityEngine;
using System.Collections;

public class GrassLoader : MonoBehaviour
{
    public GameObject[] toLoad;
    public GameObject grassParent;

    // Use this for initialization
    void Start()
    {
        if (toLoad.Length < 1 || !grassParent)
        {
#if UNITY_EDITOR
            Debug.Log(gameObject + "Load/Parent not set");
#endif
            GameObject.Destroy(this);
        }
    }

    public void loadFunction()
    {
        for (int i = 0; i < grassParent.transform.childCount; ++i)
            grassParent.transform.GetChild(i).gameObject.SetActive(false);

        for (int i = 0; i < toLoad.Length; ++i)
            toLoad[i].gameObject.SetActive(true);
    }
}
