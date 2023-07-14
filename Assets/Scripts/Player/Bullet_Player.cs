using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Player : MonoBehaviour
{

    public float destoryTime;

	public float force;


    public Rigidbody2D rd;


	public void Fire(Vector2 pos, Vector2 dir)
	{
		//transform.position = pos;

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

}
