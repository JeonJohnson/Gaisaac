﻿using System.Collections;
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


	public void RootItem(PlayerItemState effect)
	{
		if (curState != effect)
		{
			ReturnNormal(curState);

			curState = effect;
		}
		

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
				
				}
				break;

			case PlayerItemState.Dash:
				{ 
				
				}
				break;
			case PlayerItemState.end:
				{ 
				
				}
				break;
			default:
				break;
		}



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
					
				}
				break;
			case PlayerItemState.Dash:
				{

				}
				break;
			case PlayerItemState.end:
				{

				}
				break;
			default:
				break;

				
		}

		curState = PlayerItemState.None;
	}
	
	





}