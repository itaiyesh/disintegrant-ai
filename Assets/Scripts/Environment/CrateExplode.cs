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
        Explosion.Stop();
        boom = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Physics.Raycast(Player.transform.position, Player.transform.TransformDirection(Vector3.forward), out RaycastHit hitInfo, 20f);
            Debug.Log("hit.tag: " + hitInfo.transform.tag);
            if (hitInfo.transform.tag == "ExplodableCrate" && boom == true)
            {
                Explosion.Play();
                Destroy(ExplodableCrate);
                explosionSound.Play();
                boom = false;
            }
        }


    }
}
