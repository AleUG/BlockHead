using System.Collections;
using UnityEngine;
using Cinemachine;

public class CambioCamara : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera; // Asigna tu c�mara de Cinemachine en el Inspector
    public float nuevaDistancia = 10f; // Nueva distancia de la c�mara
    public float nuevoFOV = 60f; // Nuevo campo de visi�n (FOV)

    private float distanciaOriginal;
    private float fovOriginal;
    private bool cambioRealizado = false;

    private void Start()
    {
        if (virtualCamera != null)
        {
            distanciaOriginal = virtualCamera.m_Lens.OrthographicSize;
            fovOriginal = virtualCamera.m_Lens.FieldOfView;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!cambioRealizado && other.CompareTag("Player")) // Aseg�rate de que sea el jugador
        {
            CambiarCamara();
        }
    }

    private void CambiarCamara()
    {
        if (virtualCamera != null)
        {
            virtualCamera.m_Lens.OrthographicSize = nuevaDistancia;
            virtualCamera.m_Lens.FieldOfView = nuevoFOV;
            cambioRealizado = true; // Evitar que se realice el cambio nuevamente
        }
    }

    // Puedes agregar una funci�n para revertir los cambios si es necesario
    public void RevertirCambios()
    {
        if (virtualCamera != null)
        {
            virtualCamera.m_Lens.OrthographicSize = distanciaOriginal;
            virtualCamera.m_Lens.FieldOfView = fovOriginal;
            cambioRealizado = false; // Marcar que el cambio se ha revertido
        }
    }
}
