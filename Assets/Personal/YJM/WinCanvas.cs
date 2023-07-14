using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCanvas : MonoBehaviour
{
    CanvasGroup canvasGroup;
    [SerializeField] GameObject fadeImage;
    [SerializeField] GameObject button;

    public void Activate()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(FadeCoro());
        fadeImage.SetActive(true);
        button.SetActive(true);
    }

    IEnumerator FadeCoro()
    {
        while(true)
        {
            yield return null;
            canvasGroup.alpha += Time.deltaTime;
            if(canvasGroup.alpha > 1) { canvasGroup.alpha = 1; yield break; }
        }
    }

    public void GoToLobbyScene()
    {
        LoadSceneController.Instance.LoadScene(1);
    }
}
