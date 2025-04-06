using System;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraShaker : MonoBehaviour
{
    private CinemachineVirtualCamera _virtualCamera;

    private CinemachineBasicMultiChannelPerlin _noise;
    private Coroutine _shakeRoutine;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _noise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        StopShake();
    }

    public void StartShake(float amplitude = 2f, float frequency = 5f)
    {
        _noise.m_AmplitudeGain = amplitude;
        _noise.m_FrequencyGain = frequency;
    }

    public void StopShake()
    {
        _noise.m_AmplitudeGain = 0f;
        _noise.m_FrequencyGain = 0f;
    }
}