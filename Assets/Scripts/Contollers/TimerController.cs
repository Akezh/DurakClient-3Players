using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    public static TimerController instance;
    public TextMeshProUGUI timer;

    private void Awake()
    {
        instance = this;
    }

    private int countOfTimer;
    private string username;

    public void SetTimer(int countOfTimer, string username)
    {
        this.countOfTimer = countOfTimer;
        this.username = username;

        if (DurakController.instance.humanPlayer.username.text.Equals(username))
        {
            if (countOfTimer == 35)
            {
                FindObjectOfType<AudioManager>().Play("BellRing1");
            } else if (countOfTimer == 15)
            {
                FindObjectOfType<AudioManager>().Play("BellRing2");
            }
        }
    }

    private void Update()
    {
        RectTransform timerRect = timer.GetComponent<RectTransform>();

        if (DurakController.instance.humanPlayer.username.text.Equals(this.username))
        {
            timerRect.localPosition = new Vector3(1130, -600);
        }

        if (DurakController.instance.enemyPlayers[0].username.text.Equals(this.username))
        {
            timerRect.localPosition = new Vector3(50, -325);
        }

        if (DurakController.instance.enemyPlayers[1].username.text.Equals(this.username))
        {
            timerRect.localPosition = new Vector3(670, -60);
        }

        timer.text = this.countOfTimer.ToString();
    }
}
