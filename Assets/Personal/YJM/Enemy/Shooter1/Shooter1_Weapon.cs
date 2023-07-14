using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Shooter1_Weapon : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Shooter1 owner;
    [SerializeField] Transform firePos;

    private void Update()
    {
        Vector3 dir = owner.target.transform.position - this.transform.position;
        dir.Normalize();

        this.transform.rotation = Quaternion.LookRotation(this.transform.forward, dir);
    }

    public void ShootBullet()
    {
        Vector3 dir = owner.target.transform.position - firePos.transform.position;
        dir.Normalize();

        float rnd = Random.Range(-owner.spreadValue * 0.5f, owner.spreadValue * 0.5f);
        Quaternion rndRot = Quaternion.Euler(0f, 0f, rnd);

        //Debug.Log(Quaternion.LookRotation(Vector3.forward, dir));
        GameObject go = Instantiate(bulletPrefab, firePos.transform.position, Quaternion.LookRotation(Vector3.forward, dir) * rndRot);
        go.GetComponent<Bullet_Enemy>().dmg = owner.dmg;
    }
}
