using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkEffector : MonoBehaviour
{
    [SerializeField] SpriteRenderer sRenderer;
    [SerializeField] float blinkPower = 2f;
    [SerializeField] int blinkCount = 5;
    [SerializeField] float blinkCycle = 0.1f;

    private Color defColor;

    private void Start()
    {
        if(sRenderer == null) sRenderer = GetComponent<SpriteRenderer>();
        defColor = sRenderer.material.color;
    }

    public void SetDuringTime(float time)
    {
        blinkCount = (int)(time / blinkCycle);
    }

    public void StopBlink()
    {
        StopAllCoroutines();
    }

    public void StartBlink()
    {
        StopAllCoroutines();
        StartCoroutine(BlinkCoro());
    }

    IEnumerator BlinkCoro()
    {
        int count = blinkCount;
        while (true)
        {
            count--;
            
            yield return new WaitForSeconds(blinkCycle / 2f);
            sRenderer.material.color = defColor * blinkPower;
            
            yield return new WaitForSeconds(blinkCycle / 2f);
            sRenderer.material.color = defColor;

            if (count <= 0)
            {
                yield break;
            }
        }
    }
}
