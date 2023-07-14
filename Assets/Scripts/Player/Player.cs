
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct PlayerStat
{
    public int maxHp;
    public int curHp;

    public float moveSpd;

    public float curConsumeRatio;
    public float consumeTime;
    //흡수 게이지는 normalize 된 0 ~ 1 사이이고
    //관리하는건 쭉 눌렀을 때 다 소모되는 시간
    //한번 눌르면 1/consumeTime 만큼 없애주면됨

    public float consumeAngle;
    public float consumeRange;//지름

    public int bulletCnt;

    public float atkSpd;
}



public class Player : MonoBehaviour
{
    public PlayerStat stat;

    public Transform spriteTr;

    public Material fovMat;
    public Transform fovSpriteTr;

    public GameObject bulletPrefab;

    public Vector2 leftDir;
    public Vector2 lookDir;
    public Vector2 rightDir;

    public void Hit(int dmg)
	{
        stat.curHp -= dmg;
	}


	public void PlayerMove()
    {

        Vector2 moveDir = Vector2.zero;


        if (Input.GetKey(KeyCode.W))
        {
            moveDir += Vector2.up ;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveDir += Vector2.left ;
            spriteTr.localScale = new Vector3(-1f, 1f, 1f);
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveDir += Vector2.down ;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveDir += Vector2.right ;

            spriteTr.localScale = new Vector3(1f, 1f, 1f);
        }

        moveDir = moveDir.normalized * Time.deltaTime * stat.moveSpd;

        transform.position += new Vector3(moveDir.x, moveDir.y, 0f);
	}

	public void Aim()
	{
		//FOV 스프라이트 회전 하고 하믄 댐
		//UIManager.Instance.crossHair.rectTransform

		//Vector2 pos = Input.mousePosition;

		UIManager.Instance.crossHairHolder.anchoredPosition = Input.mousePosition;


		Vector3 cursorWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //fovSprite.LookAt(cursorWorldPos);

        lookDir = (cursorWorldPos - transform.position).normalized;


        fovSpriteTr.up = lookDir;
	}

	public void Fire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject bulletObj = Instantiate(bulletPrefab);
            bulletObj.transform.position = transform.position;
            Bullet_Player script = bulletObj.GetComponent<Bullet_Player>();

            script.Fire(transform.position, lookDir);
        }
    }

    


    public void Consume()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //stat.curConsumeRatio = stat.curConsumeRatio - (1f / stat.consumeTime);
            
            //float halfFovAngle = stat.consumeAngle * 0.5f;
            //leftDir = DegreeAngle2Dir(fovSpriteTr.eulerAngles.z - halfFovAngle);
            //rightDir = DegreeAngle2Dir(fovSpriteTr.eulerAngles.z + halfFovAngle);

            var cols = Physics2D.OverlapCircleAll(transform.position, stat.consumeRange/2f,LayerMask.GetMask("Bullet_Enemy"));

            foreach (var col in cols)
            {
                Vector3 targetPos = col.transform.position;
                Vector2 targetDir = (targetPos - transform.position).normalized;

                var tempLookDir = DegreeAngle2Dir(-fovSpriteTr.eulerAngles.z);
                //lookDir랑 값다른데 이거로 적용됨 일단 나중에 ㄱ
                float angleToTarget = Mathf.Acos(Vector2.Dot(targetDir, tempLookDir)) * Mathf.Rad2Deg;

                //내적해주고 나온 라디안 각도를 역코사인걸어주고 오일러각도로 변환.
                if (angleToTarget <= (stat.consumeAngle * 0.5f))
                {
                    ++stat.bulletCnt;
                }
            }
        }

    }


    public Vector3 DegreeAngle2Dir(float degreeAngle)
    {
        //각도를 벡터로 바꿔주는 거

        //ex)회전되지 않은 오브젝트인 경우
        //rotation의 y값 euler값 넣으면 forward Dir나옴.

        //조금 더 자세한 내용은 벡터 내적, 외적 봐보셈

        float radAngle = degreeAngle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radAngle), Mathf.Cos(radAngle));
    }


    private void Awake()
	{
        fovMat = fovSpriteTr.GetComponent<SpriteRenderer>().material;
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        fovSpriteTr.localScale = new Vector2(stat.consumeRange, stat.consumeRange);
        fovMat.SetFloat("_FovAngle", stat.consumeAngle);


        Aim();

        Fire();

        Consume();
    }

	private void LateUpdate()
	{
        PlayerMove();
    }

	private void FixedUpdate()
	{
        
    }

    public void OnDrawGizmos()
    {

		float halfFovAngle = stat.consumeAngle * 0.5f;
        float angle = -fovSpriteTr.eulerAngles.z;
        leftDir = DegreeAngle2Dir(angle - halfFovAngle);
		rightDir = DegreeAngle2Dir(angle + halfFovAngle);

		Gizmos.DrawWireSphere(transform.position, halfFovAngle);

		Debug.DrawRay(transform.position, leftDir.normalized * halfFovAngle, Color.black);
		Debug.DrawRay(transform.position, lookDir.normalized * halfFovAngle, Color.green);
		Debug.DrawRay(transform.position, rightDir.normalized * halfFovAngle, Color.cyan);

	}

}
