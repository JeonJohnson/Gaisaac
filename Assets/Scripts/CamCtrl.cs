using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamCtrl : MonoBehaviour
{
    Vector2 mousePos;
    Player player;
    [SerializeField] float maxDist = 3f;

    private void Start()
    {
        player = ObjectManager.Instance.player;
    }

    private void Update()
    {
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        float distance = Vector2.Distance(player.transform.position, mousePos);

        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);

        if (distance > maxDist)
        {
            Vector2 direction = (playerPos - mousePos).normalized;
            mousePos = playerPos - direction * maxDist;
        }

        this.transform.position = mousePos;
    }
}
