using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_HpBar : MonoBehaviour
{
    [SerializeField] Image hpBarImg;

    public void UpdateHpBar(float amount)
    {
        Debug.Log(amount);
        hpBarImg.fillAmount = amount;
    }

    private void Update()
    {

    }
}
