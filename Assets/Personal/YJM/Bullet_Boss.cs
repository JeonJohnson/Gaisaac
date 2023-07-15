using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet_Boss : MonoBehaviour
{
    [Header("For Consume")]
    public bool isConsumed = false;
    public float waitTime;
    public Player player;
    public BulletState curState;
    private Vector2 startPos;
    public float moveTime;
    private float accSpd = 0f;
    public Collider2D col;
    public SpriteRenderer sRDR;


    [Header("For Origin")]
    public Coroutine destroyCor;
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

        curState = BulletState.Fire;
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
        destroyCor = StartCoroutine(DestoryCor());
    }


	private void Update()
	{
        switch (curState)
        {
            case BulletState.Fire:
                {
                    Vector3 pos = transform.position;
                    pos.z = 0;
                    transform.position = pos;
                }
                break;
            case BulletState.ConsumeWait:
                {

                }
                break;
            case BulletState.ToGoal:
                {
                    accSpd += Time.deltaTime / moveTime;

                    transform.position = Vector2.Lerp(startPos, player.jarMouthTr.position, accSpd);

                    if (accSpd >= 1f)
                    {
                        ++player.stat.bulletCnt;
                        Destroy(this.gameObject);
                        curState = BulletState.End;
                    }
                }
                break;
            case BulletState.End:
                break;
            default:
                break;
        }



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

    public void Consumed(Player p)
    {
        sRDR.color = Color.black;

        isConsumed = true;

        curState = BulletState.ConsumeWait;
        player = p;

        rd.velocity = Vector3.zero;
        col.enabled = false;

        waitTime = Random.Range(0.1f, 0.25f);

        StopCoroutine(destroyCor);
        StartCoroutine(Wait());
    }

    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitTime);
        moveTime = Random.Range(0.15f, 0.35f);
        startPos = transform.position;
        curState = BulletState.ToGoal;
    }
}
