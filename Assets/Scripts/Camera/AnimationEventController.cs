using System.Collections;
using UnityEngine;
using Cinemachine;

public class AnimationEventController : MonoBehaviour
{
    //NPC must set this to null because it causes the camera to shake upon fire.
    public GameObject ShakeCamera;

    public float amplitude = 1f;
    public float frequency = 1f;
    private CinemachineVirtualCamera playerFollowVirtualCamera;

    private void Awake()
    {

        if (ShakeCamera)
        {
            playerFollowVirtualCamera = ShakeCamera.GetComponent<CinemachineVirtualCamera>();
        }
    }

    public void OnShakeCamera(float duration)
    {
        //Only player may cause a camera shake
        if (playerFollowVirtualCamera)
        {
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
