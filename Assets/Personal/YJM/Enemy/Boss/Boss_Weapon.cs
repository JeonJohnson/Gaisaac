using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Weapon : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Boss owner;
    [SerializeField] Transform firePos;

    public float rotationSpeed = 360f; 
    public float fireInterval = 0.027777f;

    private float rotationAngle = 0f;
    private float fireTimer = 0f;
    private int bulletCount = 0;
    private bool isRotating = false;

    int pastIndex = 0;
    int curIndex = 0;

    private void Update()
    {
    }

    public void ShootBullet()
    {
        StartCoroutine(FireBulletCoro());
    }

    IEnumerator FireBulletCoro()
    {
        rotationAngle = 0;
        bulletCount = 0;
        curIndex = 0;
        isRotating = true;

        while (isRotating)
        {
            Debug.Log("발사 코루틴 진행중");
            yield return null;
            RotateGun();
        }
    }

    private void RotateGun()
    {
        // 회전 각도 계산
        rotationAngle += rotationSpeed * Time.deltaTime;

        // 회전
        transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);

        if(curIndex < rotationAngle)
        {
            bulletCount++;
            curIndex += 10;

            GameObject bullet = Instantiate(bulletPrefab, firePos.position, Quaternion.identity);

            bullet.transform.eulerAngles = firePos.transform.eulerAngles;
            //bullet.GetComponent<Bullet_Boss>().Fire(firePos.transform.eulerAngles, 1);

            if (bulletCount == 37)
            {
                isRotating = false;
            }
        }
    }


    IEnumerator ShootCoro()
    {
        float angle = 0f;
        int bulletCount = 0;

        float fireTimer = 0f;

        while (bulletCount < 36)
        {
            Debug.Log("코루틴실행중");
            yield return null;

            angle += 100f * Time.deltaTime;
            if (angle >= 360f)
            {
                angle -= 360f;
                bulletCount = 0;
            }

            transform.rotation = Quaternion.Euler(0f, 0f, angle);


            fireTimer += Time.deltaTime;

            // 발사 주기마다 총알 발사
            if (fireTimer >= 0.1f && bulletCount < 36)
            {
                // 총알 생성
                GameObject bullet = Instantiate(bulletPrefab, firePos.position, Quaternion.identity);

                // 총알 방향과 속도 설정
                Vector3 bulletDirection = firePos.up;
                bullet.transform.eulerAngles = bulletDirection;

                fireTimer = 0f;
                bulletCount++;
            }
        }
    }
}
