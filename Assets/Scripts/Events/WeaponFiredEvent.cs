using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/**
 * An event to be used when a weapon is fired. It sends the audio clip of the sound and the position.
 * GameObject firingPlayer
 * GameObject weapon
 * AudioClip fireSound
 * Vector3 bulletSpawnPosition
 */
public class WeaponFiredEvent : UnityEvent<GameObject, GameObject, AudioClip, Vector3>{}
