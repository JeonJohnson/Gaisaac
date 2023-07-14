using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveEffector : MonoBehaviour
{
    [SerializeField] SpriteRenderer sRenderer;
    [SerializeField] float blinkPower = 2f;
    private Color defColor;
    [SerializeField] float speed = 0.5f;
    float timer;

    private void Start()
    {
        if (sRenderer == null) sRenderer = GetComponent<SpriteRenderer>();
        defColor = sRenderer.material.color;
    }

    public void StartEffect()
    {
        timer = blinkPower;
        sRenderer.material.color = defColor * blinkPower;
        StartCoroutine(EffectCoro());
    }

    IEnumerator EffectCoro()
    {
        while (true)
        {
            yield return null;
            timer -= Time.deltaTime * speed * blinkPower;
            sRenderer.material.color = defColor * timer;

            if (timer <= 0)
            {
                sRenderer.material.color = defColor;
                yield break;
            }
        }
    }
}
