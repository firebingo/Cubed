using UnityEngine;
using System.Collections;

public class ObjectLOD : MonoBehaviour
{

    //allows up to 4 lod levels with 0 being full quality.
    public Mesh LOD0;
    public Mesh LOD1;
    public Mesh LOD2;
    public Mesh LOD3;
    public float LODScale; //can be used to scale the LOD distances if needed.

    Material[] baseMaterial;
    public Material[] changeMaterial; //material to cahnge object to if set.

    MeshFilter thisMesh;
    MeshRenderer thisRender;

    Camera mainCamera;
    public float disFromCam;

    // Use this for initialization
    void Start()
    {
        thisMesh = GetComponent<MeshFilter>();
        thisRender = GetComponent<MeshRenderer>();
        baseMaterial = thisRender.materials;
        mainCamera = Camera.main;

        //if the object doesnt have a mesh filter or a base mesh destroy it
        if (!thisMesh || !LOD0)
        {
#if UNITY_EDITOR
            Debug.Log(this.gameObject + "does not have LODs/Mesh Filter set, destroying");
#endif
            GameObject.Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //only do the lod checking if the object is actually being rendered.
        if (thisRender.isVisible)
        {
            disFromCam = (mainCamera.transform.position - transform.position).magnitude;
            if (LOD3 && disFromCam > 90 * LODScale)
            {
                thisMesh.mesh = LOD3;
                if (changeMaterial.Length > 0)
                    thisRender.materials = changeMaterial;
            }
            else if (LOD2 && disFromCam > 60 * LODScale)
            {
                thisMesh.mesh = LOD2;
                if (changeMaterial.Length > 0)
                    thisRender.materials = changeMaterial;
            }
            else if (LOD1 && disFromCam > 30 * LODScale)
            {
                thisMesh.mesh = LOD1;
                if (changeMaterial.Length > 0)
                    thisRender.materials = changeMaterial;
            }
            else
            {
                thisMesh.mesh = LOD0;
                if (changeMaterial.Length > 0)
                    thisRender.materials = baseMaterial;
            }
        }
    }
}
