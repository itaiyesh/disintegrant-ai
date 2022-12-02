using System.Collections;
using UnityEngine;
using Cinemachine;
using System.Collections.Generic;
using System.Globalization;
public class AnimationEventController : MonoBehaviour
{
    //NPC must set this to null because it causes the camera to shake upon fire.
    public GameObject ShakeCamera;
    private CinemachineVirtualCamera playerFollowVirtualCamera;

    private void Awake()
    {

        if (ShakeCamera)
        {
            playerFollowVirtualCamera = ShakeCamera.GetComponent<CinemachineVirtualCamera>();
        }
    }

    public void OnShakeCamera(string p)
    {
        //Only player may cause a camera shake
        if (playerFollowVirtualCamera)
        {
            p = "2., 1.5, 0.1";
            string[] args = p.Split(',');
            float amplitude = float.Parse(args[0], CultureInfo.InvariantCulture.NumberFormat);
            float frequency = float.Parse(args[1], CultureInfo.InvariantCulture.NumberFormat);
            float duration = float.Parse(args[2], CultureInfo.InvariantCulture.NumberFormat);
            StartCoroutine(CameraShakeCoroutine(amplitude, frequency, duration));
        }
    }
    IEnumerator CameraShakeCoroutine(float amplitude, float frequency, float duration)
    {
        var perlin = playerFollowVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = amplitude;
        perlin.m_FrequencyGain = frequency;
        yield return new WaitForSeconds(duration);
        perlin.m_AmplitudeGain = 0f;
        perlin.m_FrequencyGain = 0f;
    }

}
