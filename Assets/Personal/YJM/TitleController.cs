using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleController : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
    }

    public void GoInGameScene()
    {
        LoadSceneController.Instance.LoadScene(1);
    }
}
