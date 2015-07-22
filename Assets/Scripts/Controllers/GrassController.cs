using UnityEngine;
using System.Collections;

public class GrassController : MonoBehaviour
{
    MeshFilter thisRender;
    public Mesh baseMesh;
    public Mesh LOD1;
    public Mesh LOD2;
    Camera mainCamera;
    public float disFromCam;

    // Use this for initialization
    void Start()
    {
        thisRender = GetComponent<MeshFilter>();
        mainCamera = Camera.main;

        if (!thisRender || !baseMesh || !LOD1 || !LOD2)
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
        if (GameController.gameMaster.grassQuality == 0)
            thisRender.mesh = null;
        else
        {
            disFromCam = (mainCamera.transform.position - transform.position).magnitude;
            if (GameController.gameMaster.grassQuality == 1)
            {
                if (disFromCam > 7)
                    thisRender.mesh = null;
                else if (disFromCam > 4)
                    thisRender.mesh = LOD2;
                else
                    thisRender.mesh = LOD1;
            }
            else if (GameController.gameMaster.grassQuality == 2 || GameController.gameMaster.grassQuality == 3)
            {
                if (disFromCam > 8.5f)
                    thisRender.mesh = null;
                else if (disFromCam > 5)
                    thisRender.mesh = LOD2;
                else if (disFromCam > 3)
                    thisRender.mesh = LOD1;
                else
                    thisRender.mesh = baseMesh;
            }
            else if (GameController.gameMaster.grassQuality == 4)
            {
                if (disFromCam > 10)
                    thisRender.mesh = null;
                else if (disFromCam > 6)
                    thisRender.mesh = LOD2;
                else if (disFromCam > 4)
                    thisRender.mesh = LOD1;
                else
                    thisRender.mesh = baseMesh;
            }
            else
            {
                if (disFromCam > 13)
                    thisRender.mesh = null;
                else if (disFromCam > 8)
                    thisRender.mesh = LOD2;
                else if (disFromCam > 6)
                    thisRender.mesh = LOD1;
                else
                    thisRender.mesh = baseMesh;
            }
        }
    }
}
