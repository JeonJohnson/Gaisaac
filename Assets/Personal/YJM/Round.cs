using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Round : MonoBehaviour
{
    public int remainEnemyCount;
    [SerializeField] Light2D[] lights;

    public void Init()
    {
        for (int i =0; i < lights.Length; i++)
        {
            lights[i].intensity = 0;
        }
        StartCoroutine(LightOnCoro());
    }
    
    IEnumerator LightOnCoro()
    {
        yield return new WaitForSeconds(0.2f);
        SoundManager.Instance.PlayTempSound("explosion", ObjectManager.Instance.player.transform.position, 0.7f, 0.8f, 1f);
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].intensity = 0;
        }
        yield return new WaitForSeconds(0.6f);

        SoundManager.Instance.PlayTempSound("explosion", ObjectManager.Instance.player.transform.position, 0.85f, 0.8f, 1f);
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].intensity = 0.5f;
        }
        yield return new WaitForSeconds(0.6f);

        SoundManager.Instance.PlayTempSound("explosion", ObjectManager.Instance.player.transform.position, 1f, 0.8f, 1f);
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].intensity = 1.58f;
        }
    }
}
