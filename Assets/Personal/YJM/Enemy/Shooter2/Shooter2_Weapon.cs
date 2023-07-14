using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter2_Weapon : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Shooter2 owner;
    [SerializeField] Transform firePos;

    private void Update()
    {
        Vector3 dir = owner.target.transform.position - this.transform.position;
        dir.Normalize();

        this.transform.rotation = Quaternion.LookRotation(this.transform.forward, dir);
    }

    public void ShootBullet()
    {
        SoundManager.Instance.PlayTempSound("shoot", this.transform.position, 1f, 0.75f, 1f);

        Vector3 dir = owner.target.transform.position - firePos.transform.position;
        dir.Normalize();

        float rnd = Random.Range(-owner.spreadValue * 0.5f, owner.spreadValue * 0.5f);
        Quaternion rndRot = Quaternion.Euler(0f, 0f, rnd);

        GameObject go = Instantiate(bulletPrefab, firePos.transform.position, Quaternion.LookRotation(Vector3.forward, dir) * rndRot);
        go.GetComponent<Bullet_Enemy>().dmg = owner.dmg;
    }
}
