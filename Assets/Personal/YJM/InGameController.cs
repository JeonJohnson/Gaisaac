using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class InGameController : MonoBehaviour
{
    public static InGameController Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    public bool isEnd = false;

    public int curRound = 0;

    public Round[] rounds;
    public Door[] doors;
    public MiniMap MiniMap;
    public WinCanvas winCanvas;
    public LoseCanvas loseCanvas;

    public Boss_HpBar bossHpBar;

    private void Update()
    {
        if (!isEnd)
        {
            if (rounds[curRound].remainEnemyCount <= 0)
            {
                curRound++;
                if (curRound >= 3)
                {
                    isEnd = true;
                    winCanvas.Activate();
                    Debug.Log("½Â¸®Ãâ·Â");
                }
                else
                {
                    doors[curRound - 1].Open();
                    StartSlowMotion();
                }
            }


            if (ObjectManager.Instance.player.isDead)
            {
                isEnd = true;
                loseCanvas.Activate();
            }
        }
    }

    public void StartSlowMotion()
    {
        StartCoroutine(SlowMotionCoro());
    }

    IEnumerator SlowMotionCoro()
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(2f);

        while(true)
        {
            yield return null;
            Time.timeScale += Time.unscaledDeltaTime * 2f;
            if(Time.timeScale > 1f)
            {
                Time.timeScale = 1f;
                yield break;
            }
        }
    }
}
