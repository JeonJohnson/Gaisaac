using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Enemy : MonoBehaviour
{
    public float speed = 1f;
    [HideInInspector] public float dmg;

    void Update()
    {
        this.transform.position += transform.up * Time.deltaTime * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Player player = other.gameObject.GetComponent<Player>();
        //if (player != null)
        //    player.Hit(dmg);
    }
}
