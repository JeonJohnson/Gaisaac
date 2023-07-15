using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletState
{ 
    Fire,
    ConsumeWait,
    ToGoal,
    End
}

public class Bullet_Enemy : MonoBehaviour
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

    [Header("For General")]
    public float speed = 1f;
    [HideInInspector] public int dmg;


    void Start()
    {
        curState = BulletState.Fire;
    }

    void Update()
    {

		switch (curState)
		{
			case BulletState.Fire:
                {
                    this.transform.position += transform.up * Time.deltaTime * speed;
                }
				break;
			case BulletState.ConsumeWait:
                { 
                    
                }
				break;
			case BulletState.ToGoal:
                {
                    //Vector2 dir = player.jarMouthTr.position - transform.position;
                    //float dist = Vector2.Distance(player.jarMouthTr.position, transform.position);

                    accSpd += Time.deltaTime  / moveTime; 

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("ÃÑ¾Ë Ãæµ¹µÊ");
         Player player = other.transform.root.GetComponent<Player>();
        if(player != null ) { player.Hit(dmg); Destroy(this.gameObject); }
    }

	public void Consumed(Player p)
	{
        GetComponent<SpriteRenderer>().color = Color.black;

        isConsumed = true;

        curState = BulletState.ConsumeWait;
        //jarMouthTr = goalTr;
        player = p;

        col.enabled = false;

        waitTime = Random.Range(0.1f, 0.25f);


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
