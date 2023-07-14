using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Enemy : MonoBehaviour
{
    public float speed = 1f;
    [HideInInspector] public int dmg;

    void Update()
    {
        this.transform.position += transform.up * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("ÃÑ¾Ë Ãæµ¹µÊ");
         Player player = other.transform.root.GetComponent<Player>();
        if(player != null ) { player.Hit(dmg); Destroy(this.gameObject); }
    }
}
