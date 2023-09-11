using UnityEngine;
using System.Collections;

public class ShieldPush : MonoBehaviour
{
    public float pushForce = 10f; // La fuerza con la que empujará a los enemigos
    private Animator playerAnimator;

    public AudioClip sound;
    public AudioSource shieldSound;

    private int golpesRecibidos = 0; // Variable para llevar el registro de golpes recibidos
    public int golpesMaximos = 3; // Número máximo de golpes que el escudo puede aguantar

    private bool isCooldown = false; // Variable para controlar el enfriamiento
    public float cooldownDuration = 2f; // Duración del enfriamiento
    private PlayerBlock playerBlock;

    private void Start()
    {
        // Obtén la referencia al Animator del jugador
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        shieldSound = FindObjectOfType<AudioSource>();

        playerBlock = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBlock>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que colisionó tiene un componente Rigidbody y tiene el tag "Enemy"
        if (other.CompareTag("Enemy") && golpesRecibidos < golpesMaximos && !isCooldown)
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                // Calcula la dirección desde el escudo hacia el enemigo
                Vector3 pushDirection = other.transform.position - transform.position;
                pushDirection.y = 0; // No queremos empujar en la dirección vertical
                pushDirection.Normalize();

                // Aplica una fuerza al Rigidbody del enemigo para empujarlo hacia atrás
                rb.AddForce(pushDirection * pushForce, ForceMode.Impulse);

                // Activa la animación de bloqueo en el jugador
                playerAnimator.SetTrigger("Bloqueo");

                // Reproduce el sonido del escudo sin interrumpir el sonido anterior
                if (shieldSound != null && sound != null)
                {
                    shieldSound.PlayOneShot(sound);
                }

                // Incrementa el contador de golpes recibidos
                golpesRecibidos++;

                // Verifica si se alcanzó el número máximo de golpes
                if (golpesRecibidos >= golpesMaximos)
                {

                    playerBlock.StopBlocking();
                    golpesRecibidos = 0;

                    playerAnimator.Play("Idle");

                }
            }
        }
    }
}
