using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour
{
    public EventSound3D eventSound3DPrefab;

    public AudioClip audioClip;

    public float volume = 0.5f;
    public float minDistance = 0f;
    public float maxDistance = 10f;

    // Start is called before the first frame update
    void Start()
    {
        var sound = Instantiate(eventSound3DPrefab, transform.position, Quaternion.identity, null);
        sound.transform.SetParent(gameObject.transform);
        sound.audioSrc.spatialBlend = 1f;
        sound.audioSrc.rolloffMode = AudioRolloffMode.Linear;
        sound.audioSrc.minDistance = minDistance;
        sound.audioSrc.maxDistance = maxDistance;
        sound.audioSrc.volume = volume;
        sound.audioSrc.loop = true;
        sound.audioSrc.clip = audioClip;
        sound.audioSrc.Play();
    }
}
