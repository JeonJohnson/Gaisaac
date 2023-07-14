using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFall : MonoBehaviour
{

    public Player player;

    public float dampingDist;

	public void FixedUpdate()
	{
		Vector3 vel = player.rd.velocity;

        if (vel != Vector3.zero)
        {
            float dist = Vector2.Distance(transform.position, player.spriteTr.position);

            if (dist <= 0.5f)
            {
                transform.position = player.spriteTr.position - (vel.normalized * Time.deltaTime * dampingDist);
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, player.spriteTr.position, Time.deltaTime * 10f);
        }


	}

	public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Environ_Hole"))
        {
            player.onHole = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Environ_Hole"))
        {
            player.onHole = false;

        }
    }

}
