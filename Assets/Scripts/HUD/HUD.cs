using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HUD : MonoBehaviour
{
	private GameObject player;
	public UnityEngine.UI.Text text;
	
    // Start is called before the first frame update
	void Awake()
    {
	    this.player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
	{
		double healthValue = Math.Round(this.player.GetComponent<CharacterAttributes>().characterAttributes.Health);
		text.text = string.Format("Health: {0}", healthValue);
	    
    }
    
	
}
