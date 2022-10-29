using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public bool isFlickering = false;
    public float timeDelay;
    public GameObject lightsOffObject;

    // Update is called once per frame
    void Update()
    {
        if (isFlickering == false)
        {
            StartCoroutine(FlickeringLight());
        }
    }

    IEnumerator FlickeringLight()
    {
        isFlickering = true;
        this.gameObject.GetComponent<Light>().enabled = false;
        timeDelay = Random.Range(0.1f, 0.5f);
        lightsOffObject.SetActive(true);
        yield return new WaitForSeconds(timeDelay);
        this.gameObject.GetComponent<Light>().enabled = true;
        lightsOffObject.SetActive(false);
        timeDelay = Random.Range(0.1f, 0.5f);
        yield return new WaitForSeconds(timeDelay);
        isFlickering = false;
    }
}
