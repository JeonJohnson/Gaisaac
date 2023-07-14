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
        Debug.Log("�Ѿ� �浹��");
         Player player = other.transform.root.GetComponent<Player>();
        if(player != null ) { player.Hit(dmg); Destroy(this.gameObject); }
    }
}
