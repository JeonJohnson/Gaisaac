using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shooter1 : Enemy
{
    [Header("Stat")]
    public float idleTime = 2f;
    public float dps = 0.1f;
    public int bulletCount = 5;
    public int dmg = 1;
    bool isDead = false;

    [Header("RangeSetting")]
    [SerializeField] float traceDistance = 12f; // 어느 거리까지 추적할것인가
    [SerializeField] float shootingDistance = 15f; // 어느범위까지 멀어지면 추적 시작할것인가

    [SerializeField] float timer = 0f;

    [SerializeField] Shooter1_Weapon weapon;

    public Player target;

    [Header("NavAI")]
    [SerializeField] NavMeshAgent agent;

    public eShooter1Status status;

    private void Start()
    {
        status = eShooter1Status.Idle;
        timer = idleTime;
        if (agent == null) agent = GetComponent<NavMeshAgent>();
    }

    public override void Update()
    {
        base.Update();
        switch (status)
        {
            case eShooter1Status.Idle:
                Idle();
                break;
            case eShooter1Status.Trace:
                Trace();
                break;
            case eShooter1Status.Attack:
                Attack();
                break;
            case eShooter1Status.Death:
                Death();
                break;
        }
    }

    private void Idle()
    {
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            status = eShooter1Status.Attack;
            timer = dps;
        }

        if (Vector3.Distance(this.transform.position, target.transform.position) > shootingDistance)
        {
            status = eShooter1Status.Trace;
        }
    }

    private void Trace()
    {
        Vector3 dir = target.transform.position - transform.position;
        dir.Normalize();
        Vector3 destinationPos = (target.transform.position + dir * 5f);
        agent.SetDestination(destinationPos);

        // 추적
        if (Vector3.Distance(this.transform.position, target.transform.position) < traceDistance)
        {
            status = eShooter1Status.Idle;
            agent.isStopped = true;
        }
    }

    private void Attack()
    {
        timer -= Time.deltaTime;

        if (bulletCount > 0)
        {
            if (timer <= 0f)
            {
                weapon.ShootBullet();
                bulletCount--;
                timer = dps;
            }
        }
        else
        {
            status = eShooter1Status.Idle;
            bulletCount = 5;
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



    //IEnumerator ShooterCoro()
    //{
    //    yield return new WaitForSeconds(idleTime);

    //    int bulletCount = 5;
    //    while (bulletCount > 0) 
    //    {
    //        Debug.Log("총알 발사");
    //        bulletCount--;
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //}    


    public override void Hit(int dmg)
    {
        base.Hit(dmg);
        if (hp <= 0)
        {
            status = eShooter1Status.Death;
        }
    }
}
