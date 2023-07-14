using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter1 : Enemy
{
    [Header("Stat")]
    public float idleTime = 2f;
    public float dps = 0.1f;
    public int bulletCount = 5;
    public int dmg = 1;
    bool isDead = false;

    [SerializeField] float timer = 0f;

    [SerializeField] Shooter1_Weapon weapon;
    [SerializeField] GameObject bulletPrefab;

    [SerializeField] Transform targetTr;


    public eShooter0Status status;

    private void Start()
    {
        status = eShooter0Status.Idle;
        timer = idleTime;
    }

    public override void Update()
    {
        base.Update();
        switch (status)
        {
            case eShooter0Status.Idle:
                    Idle();
                break;
            case eShooter0Status.Attack:
                    Attack();
                break;
            case eShooter0Status.Death:
                    Death();
                break;
        }
    }

    private void Idle()
    {
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            status = eShooter0Status.Attack;
            timer = dps;
        }
    }

    private void Attack()
    {
        timer -= Time.deltaTime;

        if (bulletCount > 0)
        {
            if (timer <= 0f)
            {
                //ShootBullet();
                bulletCount--;
                timer = dps;
            }
        }
        else
        {
            status = eShooter0Status.Idle;
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
    //        Debug.Log("ÃÑ¾Ë ¹ß»ç");
    //        bulletCount--;
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //}    


    public override void Hit(int dmg)
    {
        base.Hit(dmg);
        if (hp <= 0)
        {
            status = eShooter0Status.Death;
        }
    }
}
