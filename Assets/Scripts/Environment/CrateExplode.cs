using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateExplode : MonoBehaviour
{
    public GameObject ExplodableCrate;
    public ParticleSystem Explosion;
    public bool boom;
    public GameObject Player;
    public AudioSource explosionSound;


    // Start is called before the first frame update
    void Start()
    {
        if (Explosion.isPlaying)
        { Explosion.Stop(); }
        boom = true;
        
    }

    
    void OnMouseDown()
    {
        StartCoroutine(PauseGame(0.0f));
        if (!Explosion.isPlaying)
            {
            Explosion.Play(); }
            Destroy(ExplodableCrate);
            BoxCollider boxCollider = ExplodableCrate.GetComponent<BoxCollider>();
            Destroy(boxCollider);
            explosionSound.Play();
            boom = false;
            if (!Explosion.isPlaying)
            { 
                Destroy(gameObject);
                Destroy(explosionSound);
                Destroy(Explosion);
            }
        
        
    }

    public IEnumerator PauseGame(float pauseTime)
    {
        Time.timeScale = 0f;
        float pauseEndTime = Time.realtimeSinceStartup + pauseTime;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
        Time.timeScale = 1f;
    }

}