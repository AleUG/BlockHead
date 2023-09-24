using UnityEngine;
using System.Collections;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        // Aseg�rate de asignar la c�mara virtual en el Inspector.
        if (virtualCamera == null)
        {
            Debug.LogError("Asigna la c�mara virtual en el Inspector.");
        }
    }

    public void ShakeCamera(float amplitude, float frequency, float duration)
    {
        // Activa el ruido de la c�mara con los par�metros proporcionados.
        var noiseSettings = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noiseSettings.m_AmplitudeGain = amplitude;
        noiseSettings.m_FrequencyGain = frequency;

        // Programa la desactivaci�n del temblor despu�s de la duraci�n deseada.
        StartCoroutine(StopShake(duration));
    }

    private IEnumerator StopShake(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Desactiva el ruido de la c�mara.
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
