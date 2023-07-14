using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shooter2 : Enemy
{
    [Header("Stat")]
    public float idleTime = 2f;
    public float dps = 0.1f;
    public int bulletCount = 5;
    private int curBulletCount;
    public int dmg = 1;
    bool isDead = false;
    public float spreadValue = 75f;

    [Header("RangeSetting")]
    [SerializeField] float traceDistance = 12f; // 어느 거리까지 추적할것인가
    [SerializeField] float shootingDistance = 15f; // 어느범위까지 멀어지면 추적 시작할것인가

    [SerializeField] float timer = 0f;

    [SerializeField] Shooter2_Weapon weapon;

    public Player target;

    [Header("NavAI")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform[] posList;
    private int i_curPos = 0;
    private bool isArrived = true;
    private enum PatrolType { Loop, YoYo}
    private PatrolType patrolType = PatrolType.Loop;

    public eShooter2Status status;

    private void Start()
    {
        if (target == null)
            target = ObjectManager.Instance.player;

        status = eShooter2Status.Idle;
        curBulletCount = bulletCount;
        timer = idleTime;
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
    }

    public override void Update()
    {
        Vector3 dir = new Vector3(0f, 0f, 0f);
        transform.eulerAngles = new Vector3(0f, 0f, 0f);

        Patrol();

        base.Update();
        switch (status)
        {
            case eShooter2Status.Idle:
                Idle();
                break;
            case eShooter2Status.Attack:
                Attack();
                break;
            case eShooter2Status.Death:
                Death();
                break;
        }
    }

    private void Idle()
    {
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            status = eShooter2Status.Attack;
            timer = dps;
        }

        if (Vector3.Distance(this.transform.position, target.transform.position) > shootingDistance)
        {
            status = eShooter2Status.Patrol;
        }
    }

    private void Patrol()
    {
        if(status != eShooter2Status.Death)
        {
            if(isArrived == true)
            {
                switch (patrolType)
                {
                    case PatrolType.Loop:
                        {
                            i_curPos++;
                            if (i_curPos > posList.Length - 1) i_curPos = 0;
                        }
                        break;
                    case PatrolType.YoYo:
                        {

                        }
                        break;
                }
                agent.SetDestination(posList[i_curPos].position);
                agent.isStopped = false;
                isArrived = false;
            }
        }

        if (Vector3.Distance(this.transform.position, posList[i_curPos].position) <= 0.2f)
        {
            isArrived = true;
        }
    }

    private void Attack()
    {
        timer -= Time.deltaTime;

        if (curBulletCount > 0)
        {
            if (timer <= 0f)
            {
                weapon.ShootBullet();
                curBulletCount--;
                timer = dps;
            }
        }
        else
        {
            status = eShooter2Status.Idle;
            curBulletCount = bulletCount;
            timer = idleTime;
        }
    }

    private void Death()
    {
        if (!isDead)
        {
            isDead = true;
            Destroy(this.gameObject);
        }
    }


    public override void Hit(int dmg)
    {
        base.Hit(dmg);
        if (hp <= 0)
        {
            status = eShooter2Status.Death;
        }
    }
}
