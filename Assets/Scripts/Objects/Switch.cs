using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour
{
    enum colors { pure, red, green, blue };
    [SerializeField]
    GameObject[] switchObjects;
    [SerializeField]
    int color; //the color the switch toggles for.
    [SerializeField]
    //since the naming is confusing here, a bit more explination.
    //if activeSwitch is set to true for the switch the switch will toggle it's switchObjects list the first time a player hits it.
    //it then sets switchActive to true to make sure it doesn't toggle the objects again.
    //if activeSwitch is false the switchObjects will toggle when a player gets on the switch, and then toggle again when the player
    // exits the switch.
    bool activeSwitch; //whether or not the switch is used to toggle it's objects only when a player is on it, or only once.
    bool switchActive; //if activeSwitch is true, whether or not the switch has been activated.

    public void toggleObjects()
    {
        if (switchActive)
            return;

        for (int i = 0; i < switchObjects.Length; ++i)
        {
            switchObjects[i].SetActive(!switchObjects[i].activeSelf);
        }
        if (activeSwitch)
            switchActive = true;
    }

    void OnTriggerEnter(Collider col)
    {
        if (!col.transform.parent.IsChildOf(this.transform))
        {
            col.transform.parent.SetParent(this.transform);
            if (this.transform.childCount == 1)
            {
                toggleObjects();
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        col.transform.parent.parent = null;
        if (this.transform.childCount == 0)
        {
            toggleObjects();
        }
    }

    //getters and setters
    public bool getActiveSwitch()
    {
        return activeSwitch;
    }

    public bool getSwitchActive()
    {
        return switchActive;
    }

    public int getColor()
    {
        return color;
    }
}
