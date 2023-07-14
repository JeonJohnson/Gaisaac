using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Player : MonoBehaviour
{

    public float destoryTime;

	public float force;

	public int dmg;

    public Rigidbody2D rd;


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
		StartCoroutine(DestoryCor());
	}

	//  private void OnTriggerEnter2D(Collider2D collision)
	//  {
	//Debug.Log("충돌");
	//      Enemy enemy = collision.gameObject.GetComponent<Enemy>();
	//if(enemy != null) { enemy.Hit(1); Destroy(this.gameObject); }
	//  }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Enemy enemy = collision.gameObject.GetComponent<Enemy>();
		if (enemy != null)
		{
			Debug.Log("적과 충돌");
			enemy.Hit(dmg);
			Destroy(this.gameObject);
		}
	}


}
