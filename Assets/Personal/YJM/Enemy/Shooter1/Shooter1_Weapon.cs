using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter1_Weapon : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Shooter1 owner;

    private void Update()
    {
       
    }

    private void ShootBullet()
    {
        Vector3 rot = new Vector3(0f, 0f, Random.Range(0, 360f));
        GameObject go = Instantiate(bulletPrefab, this.transform.position, Quaternion.Euler(rot));
        //go.GetComponent<Bullet_Enemy>().dmg = this.dmg;
    }
}
