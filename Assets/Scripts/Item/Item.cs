using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public PlayerItemState ItemEffect;

	

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			PlayerItem script = other.GetComponent<PlayerItem>();

			script.RootItem(ItemEffect);

			gameObject.SetActive(false);
			Destroy(gameObject);
		}
	}

	

	
}
