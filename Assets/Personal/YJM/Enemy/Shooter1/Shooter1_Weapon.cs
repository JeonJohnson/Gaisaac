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
        Vector3 dir = owner.target.transform.position - firePos.transform.position;
        this.transform.eulerAngles = Vector3.Lerp(firePos.transform.eulerAngles, Quaternion.LookRotation(Vector3.forward, dir).eulerAngles, 5f * Time.deltaTime);
    }

    public void ShootBullet()
    {
        Vector3 dir = owner.target.transform.position - firePos.transform.position;
        dir.Normalize();

        float rnd = Random.Range(-32.5f, 32.5f);
        Quaternion rndRot = Quaternion.Euler(0f, 0f, rnd);

        Debug.Log(Quaternion.LookRotation(Vector3.forward, dir));
        GameObject go = Instantiate(bulletPrefab, firePos.transform.position, Quaternion.LookRotation(Vector3.forward, dir) * rndRot);
        go.GetComponent<Bullet_Enemy>().dmg = owner.dmg;
    }
}
