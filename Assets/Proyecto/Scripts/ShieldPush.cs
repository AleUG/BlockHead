using UnityEngine;
using System.Collections;

public class ShieldPush : MonoBehaviour
{
    public float pushForce = 10f; // La fuerza con la que empujar� a los enemigos
    private Animator playerAnimator;

    public AudioClip sound;
    public AudioSource shieldSound;

    private int golpesRecibidos = 0; // Variable para llevar el registro de golpes recibidos
    public int golpesMaximos = 3; // N�mero m�ximo de golpes que el escudo puede aguantar

    private bool isCooldown = false; // Variable para controlar el enfriamiento
    public float cooldownDuration = 2f; // Duraci�n del enfriamiento
    private PlayerBlock playerBlock;

    private void Start()
    {
        // Obt�n la referencia al Animator del jugador
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        shieldSound = FindObjectOfType<AudioSource>();

        playerBlock = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBlock>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que colision� tiene un componente Rigidbody y tiene el tag "Enemy"
        if (other.CompareTag("Enemy") && golpesRecibidos < golpesMaximos && !isCooldown)
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                // Calcula la direcci�n desde el escudo hacia el enemigo
                Vector3 pushDirection = other.transform.position - transform.position;
                pushDirection.y = 0; // No queremos empujar en la direcci�n vertical
                pushDirection.Normalize();

                // Aplica una fuerza al Rigidbody del enemigo para empujarlo hacia atr�s
                rb.AddForce(pushDirection * pushForce, ForceMode.Impulse);

                // Activa la animaci�n de bloqueo en el jugador
                playerAnimator.SetTrigger("Bloqueo");

                // Reproduce el sonido del escudo sin interrumpir el sonido anterior
                if (shieldSound != null && sound != null)
                {
                    shieldSound.PlayOneShot(sound);
                }

                // Incrementa el contador de golpes recibidos
                golpesRecibidos++;

                // Verifica si se alcanz� el n�mero m�ximo de golpes
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
