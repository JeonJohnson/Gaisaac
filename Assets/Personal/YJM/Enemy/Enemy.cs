using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int hp;
    public int speed;

    public virtual void Update()
    {

    }

    public virtual void Hit(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            InGameController.Instance.rounds[InGameController.Instance.curRound].remainEnemyCount--;
        }
    }
}
