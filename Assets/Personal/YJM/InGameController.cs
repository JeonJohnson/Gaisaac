using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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

    public ItemPopUoWindow itemPopUpWindow;

    public PPController ppController;

    private void Start()
    {
        Cursor.visible = false;
    }

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

        Bloom bloom;

        float timer = 0f;
        while (true)
        {
            yield return null;
            timer += Time.unscaledDeltaTime;

            if (ppController.volume.profile.TryGet(out bloom))
                bloom.threshold.value = 0.9f - timer * 2;

            if (timer > 1f)
            {
                break;
            }
        }


        yield return new WaitForSecondsRealtime(0.2f);

        while(true)
        {
            yield return null;

            if(bloom.threshold.value < 0.9f)
            {
                bloom.threshold.value += Time.unscaledDeltaTime * 2;
            }

            Time.timeScale += Time.unscaledDeltaTime * 2f;
            if (Time.timeScale > 1f)
            {
                Time.timeScale = 1f;
                bloom.threshold.value = 0.9f;
                yield break;
            }
        }
    }
}
