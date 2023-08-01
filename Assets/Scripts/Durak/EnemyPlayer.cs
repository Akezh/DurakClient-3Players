using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayer : DurakPlayer
{
    public bool HasLeftPosition = false;
    private void Start()
    {
        playerHand.isEnemy = true;
    }

}
