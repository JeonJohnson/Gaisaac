using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool isEnter = false;

    [SerializeField] GameObject enterModel;
    [SerializeField] GameObject exitModel;

    public void Open()
    {
        enterModel.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if(!isEnter)
            {
                isEnter = true;
                exitModel.SetActive(true);
                InGameController.Instance.rounds[InGameController.Instance.curRound].gameObject.SetActive(true);
                InGameController.Instance.rounds[InGameController.Instance.curRound].Init();
                InGameController.Instance.MiniMap.UpdateMiniMap(InGameController.Instance.curRound);
            }
        }
    }
}
