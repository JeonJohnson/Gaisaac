using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_AutoReturn : MonoBehaviour
{
    public float returnTime = 1f;

    private void OnEnable()
    {
        StartCoroutine(CheckIsAlive());
    }

    private IEnumerator CheckIsAlive()
    {
        while(true)
        {
            yield return new WaitForSeconds(returnTime);
            Destroy(this.gameObject);
            break;
        }
    }
}
