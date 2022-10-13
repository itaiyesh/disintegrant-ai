using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class HealthbarUI : MonoBehaviour
{
	public Transform TransformToFollow;
	private VisualElement healthBar;
	private Camera mainCamera;
	
	private VisualElement[] bars;
	private int healthBars = 10;
	
	private void Start()
	{
		mainCamera = Camera.main;
		healthBar = GetComponent<UIDocument>().rootVisualElement.Q("Container");

		SetPosition();
	}
	
	private void LateUpdate()
	{
		if (TransformToFollow != null)
		{
			AnimateBar();
			SetPosition();
		}
	}
	
	public void AnimateBar()
	{
		// TODO: This is problably not amazing for perforamnce. Better to use a health update event.
		int newHealthBars = (int) Math.Ceiling(GetComponent<CharacterAttributes>().characterAttributes.Health / 10);
		
		if (newHealthBars != healthBars) 
		{
			int i = 0;
			foreach(VisualElement bar in healthBar.Children())
			{
				if (i <= newHealthBars)
				{
					bar.style.visibility = Visibility.Visible;
				} else {
					bar.style.visibility = Visibility.Hidden;
				}
				i += 1;
			}
			
		}
	}
	
	public void SetPosition()
	{
		Vector2 newPosition = RuntimePanelUtils.CameraTransformWorldToPanel(
			healthBar.panel,
			TransformToFollow.position,
			mainCamera
		);
		
		healthBar.transform.position = new Vector2(newPosition.x - healthBar.layout.width / 2, newPosition.y);
	}
	
}
