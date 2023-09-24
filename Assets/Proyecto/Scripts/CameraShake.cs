using UnityEngine;
using System.Collections;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        // Asegúrate de asignar la cámara virtual en el Inspector.
        if (virtualCamera == null)
        {
            Debug.LogError("Asigna la cámara virtual en el Inspector.");
        }
    }

    public void ShakeCamera(float amplitude, float frequency, float duration)
    {
        // Activa el ruido de la cámara con los parámetros proporcionados.
        var noiseSettings = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noiseSettings.m_AmplitudeGain = amplitude;
        noiseSettings.m_FrequencyGain = frequency;

        // Programa la desactivación del temblor después de la duración deseada.
        StartCoroutine(StopShake(duration));
    }

    private IEnumerator StopShake(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Desactiva el ruido de la cámara.
        var noiseSettings = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noiseSettings.m_AmplitudeGain = 0f;
        noiseSettings.m_FrequencyGain = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShakeCamera(2.5f, 1f, 60f);
        }
    }
}
