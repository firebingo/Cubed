using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameBarController : MonoBehaviour
{
    Image uiImage; //reference to the Image for the UI Bar

    float pastIceTime; //the last ice time
    float pastFireTime; //the last fire time
    float pastHP; //the last HP amount
    Vector3 baseBarPos; //the starting position of the bar
    float barTimer; //timer for how long the bar is on the screen.
    float barShowTime; //how long the bars should show on screen.

    void Start()
    {
        pastIceTime = 0.0f;
        pastFireTime = 0.0f;
        pastHP = 0.0f;
        uiImage = GetComponent<Image>();
        barTimer = -1.0f;
        barShowTime = 2.5f;

        baseBarPos = transform.parent.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(barTimer > 0)
        {
            barTimer -= Time.deltaTime;
            transform.parent.position = baseBarPos;
        }
        else
            transform.parent.position = baseBarPos + new Vector3(0.0f, 2000.0f, 0.0f);

        if (this.gameObject.name == "HpBar")
        {
            transform.localScale = new Vector3(GameController.gameMaster.gamePlayer.health / GameController.gameMaster.playerMaxHealth, 1.0f, 1.0f);
            if (transform.localScale.x > 0.5f)
                uiImage.color = new Color(2.0f - (transform.localScale.x / 0.5f), 1.0f, 0.0f);
            else if (transform.localScale.x < 0.5f)
                uiImage.color = new Color(1.0f, transform.localScale.x / 0.5f, 0.0f);

            if (pastHP != GameController.gameMaster.gamePlayer.health)
            {
                pastHP = GameController.gameMaster.gamePlayer.health;
                barTimer = barShowTime;
            }
        }

        if(Input.GetButton("Select"))
        {
            barTimer = barShowTime;
        }
    }
}
