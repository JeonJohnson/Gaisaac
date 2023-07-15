using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPopUoWindow : MonoBehaviour
{
    PlayerItem item;
    Player player;

    [SerializeField] Image ItemImg;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI InfoText;

    private void Awake()
    {
        player = ObjectManager.Instance.player;
        item = player.itemSystem;
    }

    public void InitItem()
    {
        Time.timeScale = 0f;
        ItemImg.sprite = player.itemIconSprite.sprite;
        nameText.text = player.itemName.text;
        InfoText.text = player.itemExplain.text;
    }

    public void PressCloseButton()
    {
        Time.timeScale = 1f;
        this.gameObject.SetActive(false);
    }
}
