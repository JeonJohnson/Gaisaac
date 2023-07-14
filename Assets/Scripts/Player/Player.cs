
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
    public float consumeRange;

    public int bulletCnt;

    public float atkSpd;
}



public class Player : MonoBehaviour
{
    public PlayerStat stat;

    public Material fovMat;
    public Transform fovSprite;

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
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveDir += Vector2.down ;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveDir += Vector2.right ;
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

		Vector2 dir = (cursorWorldPos - transform.position).normalized;

        //Debug.Log(dir);

        fovSprite.up = dir;


	}

	public void Fire()
    { 
        


    }


    public void Consume()
    { 
        
    }


	private void Awake()
	{
        fovMat = fovSprite.GetComponent<SpriteRenderer>().material;
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        fovSprite.localScale = new Vector2(stat.consumeRange, stat.consumeRange);
        fovMat.SetFloat("_FovAngle", stat.consumeAngle);


        Aim();


    }

	private void LateUpdate()
	{
        PlayerMove();
    }

	private void FixedUpdate()
	{
        
    }
}
