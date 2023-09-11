using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBullet : MonoBehaviour
{
    public float lifetime = 5f; // Tiempo de vida de la bala en segundos.
    private GameObject player;
    private GameObject shield;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        shield = GameObject.FindGameObjectWithTag("Shield");

        // Destruye el objeto de la bala después del tiempo especificado.
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Comprueba si la bala colisionó con el jugador.
        if (other.gameObject == player || other.gameObject == shield)
        {
            // Destruye la bala cuando choca con el jugador.
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player || collision.gameObject == shield)
        {
            // Destruye la bala cuando choca con el jugador.
            Destroy(gameObject);
        }
    }
}
