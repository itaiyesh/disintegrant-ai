using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class HealthbarUI : MonoBehaviour
{
	public Transform TransformToFollow;
	private Vector3 lastTransformToFollow;
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
			lastTransformToFollow = TransformToFollow.position;
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
		Vector3 lerpedTransformToFollow;
		
		if (lastTransformToFollow != null) 
		{
			lerpedTransformToFollow = Vector3.Lerp(
				lastTransformToFollow,
				TransformToFollow.position,
				Time.deltaTime * 0.5f);
		} else 
		{
			 lerpedTransformToFollow = TransformToFollow.position;
		}
		
			
		Vector2 newPosition = RuntimePanelUtils.CameraTransformWorldToPanel(
			healthBar.panel,
			lerpedTransformToFollow,
			mainCamera
		);
		
		healthBar.transform.position = new Vector2(newPosition.x - healthBar.layout.width / 2, newPosition.y);
	}
	
}
