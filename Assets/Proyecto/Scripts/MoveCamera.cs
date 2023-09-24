using System.Collections;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform targetPosition; // La posici�n final a la que queremos que la c�mara se mueva.
    public float moveSpeed = 2.0f;   // La velocidad de movimiento de la c�mara.

    private bool isMoving = false;    // Flag para indicar si la c�mara est� en movimiento.

    void Update()
    {
        // Verificamos si el bot�n del mouse (en este caso, el bot�n izquierdo) est� siendo presionado.
        if (Input.GetMouseButtonDown(0))
        {
            // Comenzamos la transici�n hacia la posici�n objetivo.
            StartCoroutine(MoveToTargetPosition());
        }
    }

    IEnumerator MoveToTargetPosition()
    {
        if (isMoving)
            yield break; // Si la c�mara ya est� en movimiento, salimos de la corutina.

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

        // Asegur�monos de que la posici�n final sea exacta.
        transform.position = targetPosition.position;

        isMoving = false;
    }
}
