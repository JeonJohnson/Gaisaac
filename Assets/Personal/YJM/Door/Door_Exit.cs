using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Exit : MonoBehaviour
{
    [SerializeField] GameObject Model;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Model.SetActive(true);
        }
    }
}
