using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour
{
    public EventSound3D eventSound3DPrefab;

    public AudioClip audioClip;

    private EventSound3D sound;

    private Transform player;

    private float colliderRadius;

    public float minVolume = 0f;
    public float maxVolume = 0.5f;

    // public AudioSource audioObject;
    // Start is called before the first frame update
    void Start()
    {
        colliderRadius = GetComponent<SphereCollider>().radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (player && sound)
        {
            float distanceVolume = Mathf.Lerp(maxVolume, minVolume, Vector3.Distance(player.position, transform.position) / colliderRadius);
            //Facing direction
            var targetDirection = transform.position - player.position;
            float angle = Mathf.Abs(Vector3.Angle(player.transform.forward, targetDirection));
            float directionVolume = Mathf.Lerp(1f, 0.2f, angle / 180);

            sound.audioSrc.volume = distanceVolume * directionVolume;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player = other.transform;

            if (audioClip)
            {
                sound = Instantiate(eventSound3DPrefab, transform.position, Quaternion.identity, null);
                sound.audioSrc.clip = audioClip;
                sound.audioSrc.minDistance = 5f;
                sound.audioSrc.maxDistance = 100f;
                sound.audioSrc.loop = true;
                sound.audioSrc.Play();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (sound)
            {
                sound.audioSrc.Stop();
                Destroy(sound);
            }
            player = null;
        }
    }

}
