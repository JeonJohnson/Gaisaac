using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleController : MonoBehaviour
{
    public void GoInGameScene()
    {
        LoadSceneController.Instance.LoadScene(2);
    }
}
