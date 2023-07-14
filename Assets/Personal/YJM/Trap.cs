using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("�ݸ�����!");
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            int rnd = Random.Range(0, 100);
            if(rnd <= 10)
            {
                Debug.Log("��÷!");
                collision.gameObject.GetComponent<Player>().stat.curHp -= 5;
            }
        }
    }
}
