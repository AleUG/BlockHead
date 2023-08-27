using UnityEngine;

public class ItemVida : MonoBehaviour
{
    public int cantidadRecuperacion = 5; // Cantidad de vida a recuperar al tomar el item

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerVida = other.GetComponent<PlayerHealth>();
            if (playerVida != null)
            {
                playerVida.Heal(cantidadRecuperacion); // Llama al método Curar del script PlayerVida
            }

            Destroy(gameObject); // Destruye el objeto del item
        }
    }
}