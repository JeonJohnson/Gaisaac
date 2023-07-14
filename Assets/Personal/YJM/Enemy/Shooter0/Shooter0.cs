using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System.Threading;

public class Shooter0 : Enemy
{
    [Header("Stat")]
    public float idleTime = 2f;
    public float dps = 0.1f;
    public int bulletCount = 5;
    public int dmg = 1;
    bool isDead = false;

    [SerializeField] float timer = 0f;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Player target;


    public eShooter0Status status;

    private void Start()
    {
        status = eShooter0Status.Idle;
        timer = idleTime;
    }

    public override void Update()
    {
        base.Update();
        switch(status)
        {
            case eShooter0Status.Idle :
            {
                    Idle();
            }
            break;
            case eShooter0Status.Attack:
            {
                    Attack();
            }
            break;
            case eShooter0Status.Death:
            {
                    Death();
            }
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
                ShootBullet();
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
        if(!isDead)
        {
            isDead = true;
            Destroy(this.gameObject);
        }
    }

    private void ShootBullet()
    {
        Vector3 dir = target.transform.position - this.transform.position;
        dir.Normalize();

        float rnd = Random.Range(-32.5f, 32.5f);
        Quaternion rndRot = Quaternion.Euler(0f, 0f, rnd);

        Debug.Log(Quaternion.LookRotation(Vector3.forward, dir));
        GameObject go = Instantiate(bulletPrefab, this.transform.position, Quaternion.LookRotation(Vector3.forward,dir) * rndRot);
        go.GetComponent<Bullet_Enemy>().dmg = this.dmg;
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
        if(hp <= 0)
        {
            status = eShooter0Status.Death;
        }
    }
}
