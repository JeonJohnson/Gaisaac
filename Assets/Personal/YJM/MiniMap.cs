using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    [SerializeField] Image[] tiles;
    [SerializeField] GameObject[] dots;

    Color tileCol;

    private void Awake()
    {
        tileCol = tiles[2].color;
    }


    public void UpdateMiniMap(int round)
    {
        if(round == 0)
        {
            tiles[0].color = tileCol;
            tiles[1].color = Color.gray;
            tiles[2].color = Color.gray;
            dots[0].SetActive(true);
            dots[1].SetActive(false);
            dots[2].SetActive(false);
        }
        else if(round == 1)
        {
            tiles[0].color = tileCol;
            tiles[1].color = Color.white;
            tiles[2].color = tileCol;
            dots[0].SetActive(false);
            dots[1].SetActive(true);
            dots[2].SetActive(false);
        }
        else if (round == 2) 
        {
            tiles[0].color = tileCol;
            tiles[1].color = tileCol;
            tiles[2].color = Color.white;
            dots[0].SetActive(false);
            dots[1].SetActive(false);
            dots[2].SetActive(true);
        }
    }
}
