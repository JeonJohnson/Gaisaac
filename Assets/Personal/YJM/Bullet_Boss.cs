using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet_Boss : MonoBehaviour
{
    public float destoryTime;

    public float force;

    public int dmg;

    public Rigidbody2D rd;

    public int collisionCount = 2;


    public void Fire(Vector2 dir, int _dmg)
    {
        //transform.position = pos;
        dmg = _dmg;
        rd.AddForce(dir * force);
    }

    public IEnumerator DestoryCor()
    {
        yield return new WaitForSeconds(destoryTime);

        Destroy(this.gameObject);
    }


    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        dmg = 1;
        rd.AddForce(transform.up * force);
        StartCoroutine(DestoryCor());
    }


	private void Update()
	{
        Vector3 pos = transform.position;
        pos.z = 0;
        transform.position = pos;
	}

	private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("트리거 충돌");
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            //Debug.Log("적과 충돌"); 
            enemy.Hit(dmg);
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("콜리전 충돌");
        collisionCount--;

        Player player = collision.gameObject.GetComponent<Player>();

        if (player)
        {
            player.Hit(dmg);
            Destroy(this.gameObject);
        }

        if(collisionCount <= 0) Destroy(this.gameObject);
    }

}
