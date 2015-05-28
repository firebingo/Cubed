using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameBarController : MonoBehaviour
{
    Image uiImage; //reference to the Image for the UI Bar

    float pastIceTime; //the last ice time
    float pastFireTime; //the last fire time
    float pastHP; //the last HP amount
    Vector3 baseIcePos;
    Vector3 baseFirePos;
    Vector3 baseBarPos;

    void Start()
    {
        pastIceTime = 0.0f;
        pastFireTime = 0.0f;
        pastHP = 0.0f;
        uiImage = GetComponent<Image>();

        baseBarPos = transform.parent.position;
    }

    // Update is called once per frame
    void Update()
    {
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
                transform.parent.position = baseBarPos;
            }
            else
                transform.parent.position = baseBarPos + new Vector3(0.0f, 100.0f, 0.0f);
        }

        if (this.gameObject.name == "IceBar")
        {
            transform.localScale = new Vector3(GameController.gameMaster.gamePlayer.iceShieldTime / GameController.gameMaster.playerMaxIceShieldTime, 1.0f, 1.0f);
            uiImage.color = new Color(0.0f, transform.localScale.x, transform.localScale.x);

            if (pastIceTime != GameController.gameMaster.gamePlayer.iceShieldTime)
            {
                pastIceTime = GameController.gameMaster.gamePlayer.iceShieldTime;
                transform.parent.position = baseBarPos;
            }
            else
                transform.parent.position = baseBarPos + new Vector3(0.0f, 100.0f, 0.0f);
        }
        if (this.gameObject.name == "FireBar")
        {
            transform.localScale = new Vector3(GameController.gameMaster.gamePlayer.fireShieldTime / GameController.gameMaster.playerMaxFireShieldTime, 1.0f, 1.0f);
            uiImage.color = new Color(transform.localScale.x, transform.localScale.x * 0.5f, 0.0f);

            if (pastFireTime != GameController.gameMaster.gamePlayer.fireShieldTime)
            {
                pastFireTime = GameController.gameMaster.gamePlayer.fireShieldTime;
                transform.parent.position = baseBarPos;
            }
            else
                transform.parent.position = baseBarPos + new Vector3(0.0f, 100.0f, 0.0f);
        }
    }
}
