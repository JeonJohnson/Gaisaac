using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>
{

    public Player player;

    private void FindPlayer()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");

            if (playerObj)
            {
                player = playerObj.GetComponent<Player>();
            }
        }
    }


	private void Awake()
	{
        FindPlayer();
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
