﻿
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;



[Serializable]
public struct PlayerStat
{
    public int maxHp;
    public int curHp;

    public int maxArmor;
    public int curArmor;



    public int dmg;

    public float moveSpd;

    public float curConsumeRatio;
    public float consumeTime;
    //흡수 게이지는 normalize 된 0 ~ 1 사이이고
    //관리하는건 쭉 눌렀을 때 다 소모되는 시간
    //한번 눌르면 1/consumeTime 만큼 없애주면됨
    public float chargingTime;

    public float consumeAngle;
    public float consumeRange;//지름

    public int bulletCnt;

    public float atkSpd;
}



public class Player : MonoBehaviour
{
    
    public PlayerStat stat;
    public bool isDead = false;

    public bool onHole = false;
    
    public Coroutine fallCor = null;
    public Transform fallCol;

    public PlayerItem itemSystem;

    public Rigidbody2D rd;

    public float curAtkTime = 0f;

    public Transform spriteTr;

    public bool GodMode = false;
    public Coroutine godCor;
    public BlinkEffector bilnker;

    private bool isWalking = false;
    [SerializeField] SpriteRenderer playerRenderer;
    [SerializeField] Sprite[] playerImages; //0 : idle, // 1 : idle, walk0 // 2 : walk1

    public void GodModeOn(float time)
    {
        if (godCor != null)
        {
            StopCoroutine(godCor);
            bilnker.StartBlink();
        }

        godCor = StartCoroutine(GodCoroutine(time));


    }

    public void GodModeOff()
    {
        if (godCor != null)
        {
            StopCoroutine(godCor);
            bilnker.StopBlink();
        }

        if (itemSystem.curState == PlayerItemState.God)
        {
            itemSystem.ReturnNormal(PlayerItemState.God);
        }

        GodMode = false;
        godCor = null;
    }

    private IEnumerator GodCoroutine(float time)
    {
        GodMode = true;
        bilnker.SetDuringTime(time);
        bilnker.StartBlink();

        yield return new WaitForSeconds(time);

        GodMode = false;

        godCor = null;
    }



    [Header("Consume_FOV")]
    public Transform consumeHolder;

    [Space(5f)]
    //public Transform fovTr;
    public SpriteRenderer fovSRDR;
    public Material fovMat;
    public Vector2 leftDir;
    public Vector2 lookDir;
    public Vector2 rightDir;
    [Space(5f)]
    public Transform jarMouthTr;

    [Space(5f)]
    public Transform bulletHolder;

    //public List<GameObject> consumeList = new List<GameObject>();


    [Header("Bullet")]
    public GameObject bulletPrefab;

    
    [Header("UI")]
    public TextMeshProUGUI bulletCnt;
    public Image consumeGauge;
    public TextMeshProUGUI godModeTxt;


    [Header("Item")]
    public GameObject itemIconHolder;
    public Image itemIconSprite;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemExplain;
    public string itemThink;


    [Header("HP")]
    public Transform hpHolder;
    public List<Image> hpImgList;
    public Sprite fullHp;
    public Sprite emptyHp;

    public TextMeshProUGUI lifeCnt;

    [Header("Armor")]
    public Transform armorHolder;
    public List<Image> armorImgList;
    public Sprite armorSprite;




	public RectTransform crossHairHolder;


    public void Revive()
    {
        stat.curHp = stat.maxHp;

        //깜빡이고 잠시 몇초간 무적
    }

    public void Hit(int dmg)
	{
        //if (itemSystem.curState == PlayerItemState.God)
        //{
        //    return;
        //}

        if (GodMode || isDead)
        {
            return;
        }


        GodModeOn(0.25f);
        //stat.curHp -= dmg;
        Debug.Log($"{dmg}만큼 딜 받음");

        if (stat.curArmor > 0)
        {
            --stat.curArmor;

            if (stat.curArmor == 0)
            {
                itemSystem.ReturnNormal(PlayerItemState.EquipArmor);
            }
        }
        else
        {
            stat.curHp = Math.Clamp(stat.curHp - dmg, 0, stat.maxHp);
            
            UpdateHpUI();

            if (stat.curHp <= 0)
            {
                if (itemSystem.curState == PlayerItemState.OneMoreChance)
                {
                    stat.curHp = stat.maxHp;

                   

                    itemSystem.ReturnNormal(PlayerItemState.OneMoreChance);
                }
                else
                {
                    isDead = true;
                }
            }

            UpdateHpUI();
        }
	}

    public void FallingDeath()
    {
        fallCor = StartCoroutine(FallAnim());
    }


    public IEnumerator FallAnim()
    {
        rd.velocity = Vector3.zero;

        yield return new WaitForSeconds(0.5f);

        

        while (true)
        {
            Vector2 scale = spriteTr.localScale;

            if (scale.y <= 0f)
            {
                break;
            }
            scale.x = scale.x < 0 ? scale.x + Time.deltaTime : scale.x - Time.deltaTime;
            scale.y -= Time.deltaTime;
            spriteTr.localScale = scale;

            spriteTr.Rotate(new Vector3(0f, 0f, 10f));

            yield return null;
        }

        isDead = true;
    }


    private float walkTime = 0.25f;
    private float walkTimer = 0f;

    private float breathTime = 0.2f;
    private float breathTimer = 0f;

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

        if(moveDir != Vector2.zero)
        {
            isWalking = true;
            walkTimer -= Time.deltaTime;
            if (walkTimer >= 0f)
            {
                playerRenderer.sprite = playerImages[2];
            }
            else
            {
                playerRenderer.sprite = playerImages[0];
                if(walkTimer <= -walkTime)
                {
                    walkTimer = walkTime;
                }
            }
        }
        else
        {
            isWalking = false;
            breathTimer -= Time.deltaTime;
            if (breathTimer >= 0f)
            {
                playerRenderer.sprite = playerImages[1];
            }
            else
            {
                playerRenderer.sprite = playerImages[0];
                if (breathTimer <= -breathTime)
                {
                    breathTimer = breathTime;
                }
            }
        }

        if (onHole && moveDir != Vector2.zero)
        {
            FallingDeath();
            return;
        }


        moveDir = moveDir.normalized * Time.deltaTime * stat.moveSpd * Dash();
        

        rd.velocity = moveDir;
	}




	public void Aim()
	{
		//FOV 스프라이트 회전 하고 하믄 댐

		crossHairHolder.anchoredPosition = Input.mousePosition;

		Vector3 cursorWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        lookDir = (cursorWorldPos - transform.position);
        lookDir.Normalize();

        //Debug.Log(lookDir);

        consumeHolder.up = lookDir;
	}

	public void Fire()
    {
		if (Input.GetMouseButton(0))
		{
			if (stat.bulletCnt > 0 && curAtkTime >= stat.atkSpd)
			{
				GameObject bulletObj = Instantiate(bulletPrefab);
				bulletObj.transform.position = transform.position + (new Vector3(lookDir.x, lookDir.y, 0f) * 2f);
				Bullet_Player script = bulletObj.GetComponent<Bullet_Player>();

				script.Fire(lookDir, stat.dmg);

				--stat.bulletCnt;

				curAtkTime = 0f;
			}
		}
	}


    public float Dash()
    {
        if (itemSystem.curState != PlayerItemState.Dash)
        {
            return 1f;
        }

        if (itemSystem.count <= 0)
        {
            return 1f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            --itemSystem.count;

            if (itemSystem.count == 0)
            {
                itemSystem.ReturnNormal(PlayerItemState.Dash);
            }

            return 10f;
        }

        return 1f;
    }

    public void Consume()
    {
        if (stat.curConsumeRatio <= 0f)
        {
            if (!Input.GetMouseButton(1))
            {
                Charging();

				Color temp = Color.grey;
				temp.a = 0.3f;
                fovSRDR.color = temp;
			}
        }
        else
        {
            if (Input.GetMouseButton(1))
            {
                float amount = (1f / stat.consumeTime) * Time.deltaTime;

                if (stat.curConsumeRatio < amount)
                {
                    return;
                }
				
                Color temp = Color.green;
				temp.a = 0.5f;
                fovSRDR.color = temp;

				stat.curConsumeRatio -= amount;
                stat.curConsumeRatio  = Mathf.Clamp(stat.curConsumeRatio, 0f, 1f);

                var cols = Physics2D.OverlapCircleAll(transform.position, stat.consumeRange / 2f, LayerMask.GetMask("Bullet_Enemy", "Bullet_Boss"));

                foreach (var col in cols)
                {
                    Vector3 targetPos = col.transform.position;
                    Vector2 targetDir = (targetPos - transform.position).normalized;

                    var tempLookDir = DegreeAngle2Dir(-consumeHolder.eulerAngles.z);
                    //lookDir랑 값다른데 이거로 적용됨 일단 나중에 ㄱ
                    float angleToTarget = Mathf.Acos(Vector2.Dot(targetDir, tempLookDir)) * Mathf.Rad2Deg;

					//내적해주고 나온 라디안 각도를 역코사인걸어주고 오일러각도로 변환.
					if (angleToTarget <= (stat.consumeAngle * 0.5f))
					{
                        if (col.gameObject.GetComponent<Bullet_Enemy>())
                        {
                            var script = col.gameObject.GetComponent<Bullet_Enemy>();

                            if (!script.isConsumed)
                            { 
                                script.Consumed(this);
                            }

                        }
						else if (col.gameObject.GetComponent<Bullet_Boss>())
						{
							var script = col.gameObject.GetComponent<Bullet_Boss>();

							if (!script.isConsumed)
							{ script.Consumed(this); }
						}
					}
				}
            }
            else
            {
                Charging();

                Color temp = Color.grey;
                temp.a = 0.3f;
                fovSRDR.color = temp;
            }
        }

    }

    public void Charging()
    {
        stat.curConsumeRatio = Mathf.Clamp(stat.curConsumeRatio + (1/stat.chargingTime * Time.deltaTime) , 0f, 1f);
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


    public void UpdateHpUI()
    {
        for (int i = 0; i < hpImgList.Count; ++i)
        {
            if (i < stat.maxHp)
            {
                hpImgList[i].gameObject.SetActive(true);

                if (i < stat.curHp)
                {
                    hpImgList[i].sprite = fullHp;
                }
                else
                {
                    hpImgList[i].sprite = emptyHp;
                }

            }
            else
            {
                hpImgList[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < armorImgList.Count; ++i)
        {
            if (i < stat.curArmor)
            {
                armorImgList[i].gameObject.SetActive(true);
            }
            else
            {
                armorImgList[i].gameObject.SetActive(false);
            }
        }
    }

    private void Awake()
	{
        fovMat = fovSRDR.GetComponent<SpriteRenderer>().material;

        stat.curConsumeRatio = 1f;


        hpImgList = new List<Image>();

        for (int i = 0; i < hpHolder.childCount; ++i)
        {
            hpImgList.Add(hpHolder.GetChild(i).GetComponent<Image>());
        }

        armorImgList = new List<Image>();
        for (int i = 0; i < armorHolder.childCount; ++i)
        {
            armorImgList.Add(armorHolder.GetChild(i).GetComponent<Image>());
        }

        lifeCnt.text = "";
    }


    void Update()
    {

        if (isDead || fallCor != null)
        {
            return;
        }

        fovSRDR.transform.localScale = new Vector2(stat.consumeRange, stat.consumeRange);
        fovMat.SetFloat("_FovAngle", stat.consumeAngle);


        Aim();


        if (curAtkTime < stat.atkSpd)
        {
            curAtkTime += Time.deltaTime;
        }

        Fire();

        Consume();

        consumeGauge.fillAmount = stat.curConsumeRatio;
        bulletCnt.text = $"총알 : {stat.bulletCnt}";


        UpdateHpUI();



        if (Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            GodMode = !GodMode;
            godModeTxt.gameObject.SetActive(GodMode);
        }

    }

	private void LateUpdate()
	{
    }

	private void FixedUpdate()
	{

        if (isDead || fallCor != null)
        {
            return;
        }
        PlayerMove();
        
    }



	public void OnDrawGizmos()
    {

#if UNITY_EDITOR
        float halfFovAngle = stat.consumeAngle * 0.5f;
        float angle = -consumeHolder.eulerAngles.z;
        leftDir = DegreeAngle2Dir(angle - halfFovAngle);
		rightDir = DegreeAngle2Dir(angle + halfFovAngle);

		Gizmos.DrawWireSphere(transform.position, halfFovAngle);

		Debug.DrawRay(transform.position, leftDir.normalized * halfFovAngle, Color.black);
		Debug.DrawRay(transform.position, lookDir.normalized * halfFovAngle, Color.green);
		Debug.DrawRay(transform.position, rightDir.normalized * halfFovAngle, Color.cyan);
#endif

    }



}
