using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceDeath : MonoBehaviour
{
    private GameManager GameManager;
    void Start()
    {
        GameManager = FindObjectOfType<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        {
            if (other.tag == "Player")
            {
                GameManager.Die(other.gameObject);
            }
        }
    }
}
