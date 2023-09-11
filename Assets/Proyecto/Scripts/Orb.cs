using UnityEngine;
using System.Collections;

public class Orb : MonoBehaviour
{
    public float initialMovementSpeed = 5f;
    public float maxMovementSpeed = 15f;
    public float speedIncreaseRate = 1f;
    public float collectionDistance = 1.0f;
    public float activationDistance = 3.0f;
    public float orbLifetime = 5.0f; // Duración de vida de la orbe en segundos

    private Transform player;
    private bool isMovingToPlayer = false;
    private float currentMovementSpeed;
    private bool isPaused = false; // Variable para controlar si el script está pausado

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentMovementSpeed = initialMovementSpeed;

        OrbActive();
    }

    private void Update()
    {
        if (!isPaused) // Comprobar si el script está pausado
        {
            if (isMovingToPlayer && player != null)
            {
                Vector3 directionToPlayer = player.position - transform.position;

                transform.position = Vector3.MoveTowards(transform.position, player.position, currentMovementSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, player.position) < collectionDistance)
                {
                    CollectOrb();
                }

                if (currentMovementSpeed < maxMovementSpeed)
                {
                    currentMovementSpeed += speedIncreaseRate * Time.deltaTime;
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, player.position) < activationDistance)
                {
                    isMovingToPlayer = true;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Vector3.Distance(transform.position, player.position) < activationDistance)
            {
                isMovingToPlayer = true;
            }
        }
    }

    private void CollectOrb()
    {
        OrbCollector playerOrbCollector = player.GetComponent<OrbCollector>();
        if (playerOrbCollector != null)
        {
            playerOrbCollector.CollectOrb();
            Destroy(gameObject);
        }
    }

    // Método para pausar el script durante un segundo
    public void PauseOrbScript()
    {
        isPaused = true;
        Invoke("ResumeOrbScript", 2.0f); // Después de dos segundos, reanuda el script
    }

    // Método para reanudar el script
    public void ResumeOrbScript()
    {
        isPaused = false;
    }

    private IEnumerator OrbActive()
    {
        yield return new WaitForSeconds(orbLifetime);
        Destroy(gameObject);
    }
}
