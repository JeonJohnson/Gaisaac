using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public enum SCENE_NAME
{ 
	Intro,
	Title,
	InGame,
	End
}


public class GameManager : Singleton<GameManager>
{

	public float BgmOffset = 1f;
    public float SeOffset = 1f;


    private void InitApp()
	{ 
	
	}

	private void InitGame()
	{ 
	
	
	}

	private void InitScene(SCENE_NAME sceneName)
	{
		switch (sceneName)
		{
			case SCENE_NAME.Intro:
				break;
			case SCENE_NAME.Title:
				break;
			case SCENE_NAME.InGame:
				break;
			case SCENE_NAME.End:
				break;
			default:
				break;
		}

	}


	public void LoadScene(int buildIndex)
	{ 
	

	}

	public void LoadScene(SCENE_NAME sceneName)
	{


	}

	private void InitIntroScene()
	{ 
	
	}

	private void InitTitleScene()
	{ 
	
	}

	private void InitGameScene()
	{
	
	}



	private void Awake()
	{
		
	}


	private void Start()
	{
		
	}

	private void Update()
	{
		
	}


	public override void OnSceneChanged(Scene scene, LoadSceneMode mode)
	{
		base.OnSceneChanged(scene, mode);

	}


	public void OnApplicationFocus(bool focus)
	{
		
	}

}
