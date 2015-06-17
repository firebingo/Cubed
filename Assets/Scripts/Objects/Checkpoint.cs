using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    public bool hasCheckpoint; //whteher or not the checkpoint has been used, prevents using a checkpoint multiple times.

    // Use this for initialization
    void Start()
    {
        hasCheckpoint = false;
    }
}
