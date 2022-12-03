using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicTrigger : MonoBehaviour
{
    // public GameObject SrcObject;
    public AudioSource src;
    private GameManager GameManager;

    void Start()
    {
        // src = SrcObject.GetComponent<AudioSource>();
        GameManager = FindObjectOfType<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        {
            if (other.tag == "Player")
            {
                GameManager.SwitchBackgroundMusic(src);
            }
        }
    }
}
