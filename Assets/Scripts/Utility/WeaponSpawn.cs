using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WeaponSpawn : MonoBehaviour
{
    public GameObject WeaponCollectable;
    public Text CountdownText;
    public GameObject SpawnPending;

    // public GameObject SpawnFX;
    public float Frequency = 20f; //Elapsed time between spawns

    public bool InitialSpawn = true;
    private float pickupTime;

    private GameObject spawnedCollectable;

    private bool isTriggered = false;

    private GameObject weapon;
    // Start is called before the first frame update
    void Start()
    {
        if (InitialSpawn)
        {
            spawnedCollectable = Instantiate(WeaponCollectable, transform.position, transform.rotation);
            SpawnPending.SetActive(false);
        }
        else
        {
            pickupTime = Time.fixedTime;
            SpawnPending.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnedCollectable == null && (Time.fixedTime - pickupTime >= Frequency))
        {
            spawnedCollectable = Instantiate(WeaponCollectable, transform.position, transform.rotation);
            SpawnPending.SetActive(false);
            // Instantiate(SpawnFX, transform.position, transform.rotation);
        }

        if (spawnedCollectable == null)
        {
            int timeLeft = 1 + (int)Mathf.Max(Frequency + pickupTime - Time.fixedTime, 0);
            //Start rotating 10 mins before spawn
            // float timeLeftRatio = Mathf.Clamp(10 - timeLeft, 0, 10) / 10;
            transform.Rotate(new Vector3(0, Mathf.Lerp(0, 200, Mathf.Clamp(5 - timeLeft, 0, 5)), 0) * Time.deltaTime);
            CountdownText.text = string.Format("{0}", timeLeft);
        }
    }
    public void OnTriggerEnter(Collider c)
    {
        if (spawnedCollectable != null && spawnedCollectable.GetComponent<BaseCollectable>().TryCollect(c))
        {
            pickupTime = Time.fixedTime;
            spawnedCollectable = null;
            SpawnPending.SetActive(true);
        }
    }
}
