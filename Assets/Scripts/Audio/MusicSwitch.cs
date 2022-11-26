using UnityEngine;
using System.Collections;
using UnityEditor;

public class MusicSwitch : MonoBehaviour
{
    public AudioSource musicSource1, musicSource2;
    float defaultVolume = 0.5f;
    float transitionTime = 0.75f;
    bool switchTrigger = false;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void switchMusic()
    {
        AudioSource nowPlaying = musicSource1;
        AudioSource target = musicSource2;

        if (nowPlaying.isPlaying == false)
        {
            nowPlaying = musicSource2;
            target = musicSource1;
        }

        StartCoroutine(MixSources(nowPlaying, target));
    }

    IEnumerator MixSources(AudioSource nowPlaying, AudioSource target)
    {
        float percentage = 0;
        while (nowPlaying.volume > 0)
        {
            nowPlaying.volume = Mathf.Lerp(defaultVolume, 0, percentage);
            percentage += Time.deltaTime / transitionTime;
            yield return null;
        }

        nowPlaying.Pause();
        if (target.isPlaying == false)
            target.Play();
        target.UnPause();
        percentage = 0;

        while (target.volume < defaultVolume)
        {
            target.volume = Mathf.Lerp(0, defaultVolume, percentage);
            percentage += 1.2f*(Time.deltaTime / transitionTime);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    { { 
        if (other.tag == "Player")
        switchMusic();
        }
    }
}