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
        Debug.Log(" MOUSE HIIIIIT!");
            if (!Explosion.isPlaying)
            { Explosion.Play(); }
            Destroy(ExplodableCrate);
            explosionSound.Play();
            boom = false;
        
    }

}