using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int maxHp;
    public int hp;
    public int speed;

    [SerializeField] BlinkEffector effector;

    public virtual void Start()
    {
        maxHp = hp;
    }

    public virtual void Update()
    {

    }

    public virtual void Hit(int dmg)
    {
        hp -= dmg;

        if (effector != null) effector.StartBlink();

        if (hp <= 0)
        {
            InGameController.Instance.rounds[InGameController.Instance.curRound].remainEnemyCount--;
        }
    }
}
