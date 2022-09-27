//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

// adapted from https://gamedevsolutions.com/camera-shake-effect-in-unity3d/ to simulated gun fire recoil
// need to set up camera shake and camera that follows player independent:
//https://forum.unity.com/threads/camera-shake-effect-when-camera-following-player.781820/

public class CameraShake : MonoBehaviour
{
    // Camera Information
    public Transform cameraTransform;
    private Vector3 orignalCameraPos;

    // Shake Parameters
    public float shakeAmount = 0.7f;



    // Start is called before the first frame update
    void Start()
    {
        orignalCameraPos = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //orignalCameraPos = cameraTransform.localPosition;
        while (Input.GetButtonDown("Fire1"))
        {

            Debug.Log("Mouse button hit!");
            cameraTransform.localPosition = orignalCameraPos + Random.insideUnitSphere * shakeAmount;
        }

        cameraTransform.localPosition = orignalCameraPos;
        
    }

}
