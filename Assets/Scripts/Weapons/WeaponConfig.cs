using System;
using UnityEngine;

public enum WeaponCategory {
    PROJECTILE,
    HITSCAN
}
//Rename to weapon
[Serializable]
public class WeaponConfig {
    public string tag;
    public GameObject projectile;

    public WeaponCategory weaponType;

    public Transform bulletSpawnPosition;

    public virtual void Attack(Transform target){
        //TODO: For better performance, move switch to init
        switch(weaponType) 
        {
            case WeaponCategory.PROJECTILE:
                Vector3 aimDirection = (target.position - bulletSpawnPosition.position).normalized;
                //Constraining to horizontal aiming only
                aimDirection.y = 0;
		        GameObject.Instantiate(projectile, bulletSpawnPosition.position, Quaternion.LookRotation(aimDirection, Vector3.up));
                break;
            case WeaponCategory.HITSCAN:
                //
                break;
            default:
                Debug.LogError("Unsupported weapon type");
                break;
        }
    }

}