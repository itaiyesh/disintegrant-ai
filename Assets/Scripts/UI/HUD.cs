using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HUD : MonoBehaviour
{
	public GameObject player;
	public UnityEngine.UI.Text text;


    // Update is called once per frame
    void Update()
	{
		double healthValue = Math.Round(player.GetComponent<CharacterAttributes>().characterAttributes.Health);
		text.text = string.Format("Health: {0}", healthValue);
	    
    }
    
	
}
