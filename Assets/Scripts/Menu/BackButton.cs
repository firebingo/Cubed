using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BackButton : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            if (Input.GetButtonDown("B"))
            {
                GetComponent<Button>().onClick.Invoke();
            }
        }
    }
}
