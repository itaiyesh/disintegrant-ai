using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// adapted from https://gamedevsolutions.com/camera-shake-effect-in-unity3d/ to simulated gun fire recoil
// need to set up camera shake and camera that follows player independent:
//https://forum.unity.com/threads/camera-shake-effect-when-camera-following-player.781820/
//https://www.youtube.com/watch?v=BQGTdRhGmE4
public class CameraShake : MonoBehaviour
{
    public Transform CamTransform;
    //public Transform agent;
    public float scale = 0.03f;
    private void Update()
    
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(Shaking());
        }
     
    }

    IEnumerator Shaking()
    {
        Vector3 startPosition = CamTransform.position;
        //Vector3 agentStartPosition = agent.transform.position
        
        while (Input.GetMouseButton(0))
        {
            //CamTransform.position = startPosition + Random.insideUnitSphere * scale;
            CamTransform.position = CamTransform.position + Random.insideUnitSphere * scale;
            yield return null;
        }
        CamTransform.position = CamTransform.position;
    }

}
