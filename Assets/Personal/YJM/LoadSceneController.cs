using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LoadSceneController : MonoBehaviour
{
    #region singletone
    private static LoadSceneController instance;
    public static LoadSceneController Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<LoadSceneController>();
                if (obj != null)
                {
                    instance = obj;

                }
                else
                {
                    instance = Create();
                }
            }
            return instance;
        }
    }

    static LoadSceneController Create()
    {
        return Instantiate(Resources.Load<LoadSceneController>("LoadingUI"));
    }


    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    [SerializeField]
    CanvasGroup canvasGroup;

    [SerializeField]
    GameObject progressCricle;

    [SerializeField]
    Text loadingText;

    [SerializeField]
    Image loadingSlider;

    int loadSceneNumber;

    public void LoadScene(int sceneNumber)
    {
        gameObject.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoaded;
        loadSceneNumber = sceneNumber;
        StartCoroutine(LoadSceneProcess());
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) // arg0 = �ҷ����� �� 
    {
        if (arg0.buildIndex == loadSceneNumber) // �� arg0 �̸��� loadSceneName �̸��� ���ٸ� ���� �ҷ���������
        {
            print(arg0.buildIndex);
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= OnSceneLoaded;
            //GameManager.Instance.SceneCheck(SceneManager.GetActiveScene().buildIndex);
        }
    }

    float loadingTextTimer = 0f;

    void ChangeLoadingText()
    {
        loadingTextTimer += Time.deltaTime;

        if (loadingTextTimer < 0.3f)
        {
            loadingText.text = "Loading.";
        }
        else if (loadingTextTimer < 0.6f)
        {
            loadingText.text = "Loading..";
        }
        else if (loadingTextTimer < 0.9f)
        {
            loadingText.text = "Loading...";
        }
        else
        {
            loadingTextTimer = 0f;
        }
    }
    IEnumerator LoadSceneProcess()
    {
        yield return StartCoroutine(Fade(true)); // �ڷ�ƾ�ȿ��� �ڷ�ƾ �����Ű�鼭 yield return���� �����Ű�� ȣ��� �ڷ�ƾ�� ���������� ��ٸ��� ����� ����. �� Fade timer�ð��� 1�ʸ�ŭ ��ٸ�

        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneNumber); // �񵿱�� ���� �ҷ���
        op.allowSceneActivation = false; // �ε� �����ڸ��� �ڵ����� ����ȯ�� �����ʰ�

        float timer = 0f;
        while (!op.isDone)
        {
            progressCricle.transform.Rotate(new Vector3(0, 0f, -0.5f) * 360 * Time.deltaTime);
            ChangeLoadingText();
            loadingSlider.fillAmount = op.progress;
            yield return null;
            if (op.progress >= 0.9f)
            {
                ChangeLoadingText();
                timer += Time.unscaledDeltaTime;
                loadingSlider.fillAmount = 1;
                if (timer > 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    IEnumerator Fade(bool isFadeIn) // �ε��������� ���̵���/�ƿ� ȿ��
    {
        float timer = 0;
        while (timer <= 1f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime * 4f;
            canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
        }

        if (!isFadeIn)
        {
            if(timer >= 1f)
            gameObject.SetActive(false);
        }
    }
}
