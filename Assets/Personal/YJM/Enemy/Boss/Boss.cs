using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Enemy
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
    [SerializeField] int teleportTicket = 3;
    private int curTeleportTicket;

    [SerializeField] Boss_Weapon weapon;

    public Player target;

    [Header("NavAI")]
    [SerializeField] NavMeshAgent agent;

    public eBossStatus status;

    public override void Start()
    {
        base.Start();
        if (target == null)
            target = ObjectManager.Instance.player;

        status = eBossStatus.Idle;
        curBulletCount = bulletCount;
        curTeleportTicket = teleportTicket;

        timer = idleTime;
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
    }

    public override void Update()
    {
        Vector3 dir = new Vector3(0f, 0f, 0f);
        transform.eulerAngles = new Vector3(0f, 0f, 0f);

        base.Update();
        switch (status)
        {
            case eBossStatus.Idle:
                Idle();
                break;
            case eBossStatus.Attack:
                Attack();
                break;
            case eBossStatus.Trace:
                Trace();
                break;
            case eBossStatus.Teleport:
                Teleport();
                break;
            case eBossStatus.Death:
                Death();
                break;
        }

        if(InGameController.Instance.bossHpBar != null)
        {
            InGameController.Instance.bossHpBar.UpdateHpBar((float)hp / (float)maxHp);
        }
    }

    private void Idle()
    {

        timer -= Time.deltaTime;
        if (timer < 0f)
        {
                status = eBossStatus.Attack;
                timer = 4f;
                weapon.ShootBullet();
        }

        if (Vector3.Distance(this.transform.position, target.transform.position) > shootingDistance)
        {
            status = eBossStatus.Trace;
        }
    }

    private void Trace()
    {
        Vector3 dir = target.transform.position - transform.position;
        dir.Normalize();
        Vector3 destinationPos = (target.transform.position + dir * 5f);
        agent.SetDestination(destinationPos);
        agent.isStopped = false;

        // 추적
        if (Vector3.Distance(this.transform.position, target.transform.position) < traceDistance)
        {
            status = eBossStatus.Idle;
            agent.isStopped = true;
        }
    }

    private void Attack()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            status = eBossStatus.Idle;
            curBulletCount = bulletCount;
            timer = idleTime;
        }
    }

    private SpriteRenderer sRenderer;

    private void Teleport()
    {
        timer -= Time.deltaTime;

        if(timer < 0f)
        {
            agent.enabled = false;
            Vector3 rndRange = new Vector3(Random.Range(2, 4f), Random.Range(2, 4f), 0f);
            this.transform.position += rndRange;
            this.transform.position = new Vector3(transform.position.x,transform.position.y,0f);
            agent.enabled = true;
            StartCoroutine(FadeCoro(true));
            status = eBossStatus.Idle;
        }
    }

    IEnumerator FadeCoro(bool isShow)
    {
        sRenderer = GetComponent<SpriteRenderer>();
        float timer = 1f;
        while(timer > 0f)
        {
            if (isShow)
            {
                sRenderer.color = new Color(1, 1, 1, timer);
            }
            else
            {
                sRenderer.color = new Color(1, 1, 1, 1f - timer);
            }
            yield return null;
        }
        yield break;
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
            status = eBossStatus.Death;
        }
    }
}
