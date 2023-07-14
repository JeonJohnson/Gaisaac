using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RdMoveTest : MonoBehaviour
{

    public Rigidbody2D rd2D;

    public float moveSpd;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void FixedUpdate()
	{
        Vector2 moveDir = Vector2.zero;


        if (Input.GetKey(KeyCode.W))
        {
            moveDir += Vector2.up;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveDir += Vector2.left;
            //spriteTr.localScale = new Vector3(-1f, 1f, 1f);
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveDir += Vector2.down;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveDir += Vector2.right;

            //spriteTr.localScale = new Vector3(1f, 1f, 1f);
        }

        moveDir = moveDir.normalized * Time.deltaTime * moveSpd;

        //transform.position += new Vector3(moveDir.x, moveDir.y, 0f);
        rd2D.velocity = moveDir;
    }
}
