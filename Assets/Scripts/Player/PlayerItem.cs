using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerItemState
{
	None,
	ConsumeRangeUp,
	OneMoreChance,
	EquipArmor,
	God,
	Dash,
	end
}

public class PlayerItem : MonoBehaviour
{
	public Player player;
	public PlayerItemState curState;

	
	public List<Sprite> iconList;

	public List<string> itemName;
	public List<string> explain;

	public int count;


	public void SetItemIcon(PlayerItemState itemState)
	{
		switch (itemState)
		{
			case PlayerItemState.None:
				{
					player.itemIconHolder.SetActive(false);
				}
				break;
			case PlayerItemState.ConsumeRangeUp:
			case PlayerItemState.OneMoreChance:
			case PlayerItemState.EquipArmor:
			case PlayerItemState.God:
			case PlayerItemState.Dash:
				{
					player.itemIconHolder.SetActive(true);
					player.itemIconSprite.sprite = iconList[(int)itemState - 1];
					player.itemName.text = itemName[(int)itemState - 1];
					player.itemExplain.text = explain[(int)itemState - 1];
				}
				break;


		}
	}

	public void RootItem(PlayerItemState effect)
	{
		if (curState != effect)
		{
			ReturnNormal(curState);

			curState = effect;
		}


		SetItemIcon(effect);

		switch (effect)
		{
			case PlayerItemState.None:
				{
					player.itemIconHolder.SetActive(false);
				}
				break;

			case PlayerItemState.ConsumeRangeUp:
				{
					player.stat.consumeAngle = 90f;
					player.stat.consumeRange = 6f;
				}
				break;

			case PlayerItemState.OneMoreChance:
				{
					//플레이어 측에서 Death 됐을때 처리해주기
					player.lifeCnt.text = "x 1";
				}
				break;

			case PlayerItemState.EquipArmor:
				{
					player.stat.curArmor = 5;
				}
				break;

			case PlayerItemState.God:
				{
					count = 10;
					player.GodModeOn(count);
				}
				break;

			case PlayerItemState.Dash:
				{
					count = 3;
				}
				break;
			default:
				break;
		}

		InGameController.Instance.itemPopUpWindow.gameObject.SetActive(true);
        InGameController.Instance.itemPopUpWindow.InitItem();
    }

	public void ReturnNormal(PlayerItemState preEffect)
	{
		switch (preEffect)
		{
			case PlayerItemState.ConsumeRangeUp:
				{
					player.stat.consumeRange = 4;
					player.stat.consumeAngle = 30;
				}
				break;
			case PlayerItemState.OneMoreChance:
				{
					player.lifeCnt.text = "";
				}
				break;
			case PlayerItemState.EquipArmor:
				{
					player.stat.curArmor = 0;
				}
				break;
			case PlayerItemState.God:
				{
					player.GodModeOff();
				}
				break;
			case PlayerItemState.Dash:
				{
							
				}
				break;
			default:
				break;

				
		}

		curState = PlayerItemState.None;
	}
	
	





}
