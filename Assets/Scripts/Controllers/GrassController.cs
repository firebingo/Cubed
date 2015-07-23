using UnityEngine;
using System.Collections;

public class GrassController : MonoBehaviour
{
    MeshFilter thisMesh;
    MeshRenderer thisRender;
    public Mesh baseMesh;
    public Mesh LOD1;
    public Mesh LOD2;
    Camera mainCamera;
    public float disFromCam;

    // Use this for initialization
    void Start()
    {
        thisMesh = GetComponent<MeshFilter>();
        thisRender = GetComponent<MeshRenderer>();
        mainCamera = Camera.main;

        if (!thisMesh || !baseMesh || !LOD1 || !LOD2)
        {
#if UNITY_EDITOR
            Debug.Log("Grass object does not have LODs/Mesh Filter set, destroying");
#endif
            GameObject.Destroy(this.transform.parent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (thisRender.isVisible)
        {
            if (GameController.gameMaster.grassQuality == 0)
                thisMesh.mesh = null;
            else
            {
                disFromCam = (mainCamera.transform.position - transform.position).magnitude;
                if (GameController.gameMaster.grassQuality == 1)
                {
                    if (disFromCam > 8.5f)
                        thisMesh.mesh = null;
                    else if (disFromCam > 5)
                        thisMesh.mesh = LOD2;
                    else
                        thisMesh.mesh = LOD1;
                }
                else if (GameController.gameMaster.grassQuality == 2 || GameController.gameMaster.grassQuality == 3)
                {
                    if (disFromCam > 10.5f)
                        thisMesh.mesh = null;
                    else if (disFromCam > 6.5f)
                        thisMesh.mesh = LOD2;
                    else if (disFromCam > 4.5f)
                        thisMesh.mesh = LOD1;
                    else
                        thisMesh.mesh = baseMesh;
                }
                else if (GameController.gameMaster.grassQuality == 4)
                {
                    if (disFromCam > 12)
                        thisMesh.mesh = null;
                    else if (disFromCam > 8)
                        thisMesh.mesh = LOD2;
                    else if (disFromCam > 5)
                        thisMesh.mesh = LOD1;
                    else
                        thisMesh.mesh = baseMesh;
                }
                else
                {
                    if (disFromCam > 16)
                        thisMesh.mesh = null;
                    else if (disFromCam > 10)
                        thisMesh.mesh = LOD2;
                    else if (disFromCam > 7)
                        thisMesh.mesh = LOD1;
                    else
                        thisMesh.mesh = baseMesh;
                }
            }
        }
    }
}
