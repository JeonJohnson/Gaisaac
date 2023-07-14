using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class UIManager : Singleton<UIManager>
{

    public TextMeshProUGUI bulletCnt;

    public Transform hpImgHolder;
    [HideInInspector]
    public List<Image> hpImg;

    public RectTransform crossHairHolder;

	private void Awake()
	{
		
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
