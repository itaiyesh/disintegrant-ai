using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum WeaponType {
	MACHINE_GUN,
	ROCKET_LAUNCHER
}

// Weapon class, contains all details relevant to a weapon implementation
public class Weapon {
	public WeaponType Type;
	public GameObject WeaponPrefab;
	public GameObject ProjectilePrefab;
	public float ProjectileDamageMultiplier = 1.0f;
	public float ProjectileSpeedMultiplier = 1.0f;
	public float ProjectileAccelerationMultiplier = 1.0f;
	
	public Weapon(
		WeaponType type,
		string weaponPrefabPath,
		string projectilePrefabPath,
		float projectileDamageMultiplier,
		float projectileSpeedMultiplier,
		float projectileAccelerationMultiplier
	)
	{
		Type = type;
		
		WeaponPrefab = (GameObject) AssetDatabase.LoadAssetAtPath(weaponPrefabPath, typeof(GameObject)); // Load weapon prefab
		ProjectilePrefab = (GameObject) AssetDatabase.LoadAssetAtPath(projectilePrefabPath, typeof(GameObject)); // Load projectile prefab
		
		ProjectileDamageMultiplier = projectileDamageMultiplier;
		ProjectileSpeedMultiplier = projectileSpeedMultiplier;
		ProjectileAccelerationMultiplier = projectileAccelerationMultiplier;
	}
}

// Weapons class that stores all weapon types and parameters
public class Weapons {
	public Weapon MACHINE_GUN = new Weapon(
		type: WeaponType.MACHINE_GUN,
		weaponPrefabPath: "Assets/3rd Party Assets/PolygonSciFiSpace/Models/SM_Wep_Rifle_01.fbx",
		projectilePrefabPath: "",
		projectileDamageMultiplier: 1.0f,
		projectileSpeedMultiplier: 1.0f,
		projectileAccelerationMultiplier: 1.0f
	);
	
	public Weapon ROCKET_LAUNCHER = new Weapon(
		type: WeaponType.ROCKET_LAUNCHER,
		weaponPrefabPath: "",
		projectilePrefabPath: "",
		projectileDamageMultiplier: 1.0f,
		projectileSpeedMultiplier: 1.0f,
		projectileAccelerationMultiplier: 1.0f
	);
}

