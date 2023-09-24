using System.Collections;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform targetPosition; // La posición final a la que queremos que la cámara se mueva.
    public float moveSpeed = 2.0f;   // La velocidad de movimiento de la cámara.

    private bool isMoving = false;    // Flag para indicar si la cámara está en movimiento.

    void Update()
    {
        // Verificamos si el botón del mouse (en este caso, el botón izquierdo) está siendo presionado.
        if (Input.GetMouseButtonDown(0))
        {
            // Comenzamos la transición hacia la posición objetivo.
            StartCoroutine(MoveToTargetPosition());
        }
    }

    IEnumerator MoveToTargetPosition()
    {
        if (isMoving)
            yield break; // Si la cámara ya está en movimiento, salimos de la corutina.

        isMoving = true;

        Vector3 initialPosition = transform.position;
        float journeyLength = Vector3.Distance(initialPosition, targetPosition.position);
        float startTime = Time.time;

        while (Time.time - startTime < journeyLength / moveSpeed)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            transform.position = Vector3.Lerp(initialPosition, targetPosition.position, fractionOfJourney);

            yield return null;
        }

        // Asegurémonos de que la posición final sea exacta.
        transform.position = targetPosition.position;

        isMoving = false;
    }
}
