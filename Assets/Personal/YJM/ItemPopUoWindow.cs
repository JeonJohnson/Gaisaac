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
    [SerializeField] TextMeshProUGUI ThinkText;


    private void Awake()
    {
        player = ObjectManager.Instance.player;
        item = player.itemSystem;
    }

    public void InitItem()
    {
        Time.timeScale = 0f;

        Cursor.visible = true;
        ItemImg.sprite = player.itemIconSprite.sprite;
        nameText.text = player.itemName.text;
        InfoText.text = player.itemExplain.text;
        ThinkText.text = player.itemThink;
    }

    public void PressCloseButton()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        this.gameObject.SetActive(false);
    }
}
