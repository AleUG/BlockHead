using UnityEngine;

public class ItemVida : MonoBehaviour
{
    public int cantidadRecuperacion = 5; // Cantidad de vida a recuperar al tomar el item

    public AudioClip audioHealth;
    private AudioSource sfxAudio;

    private void Start()
    {
        sfxAudio = GameObject.Find("SFX_Audio").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerVida playerVida = other.GetComponent<PlayerVida>();
            if (playerVida != null)
            {
                playerVida.Curar(cantidadRecuperacion); // Llama al m�todo Curar del script PlayerVida
                sfxAudio.PlayOneShot(audioHealth);
            }

            // Opcionalmente, puedes destruirlo despu�s de un tiempo si lo deseas
            Destroy(gameObject); // Esto destruir� el objeto despu�s de 2 segundos (ajusta el tiempo seg�n tus necesidades)
        }
    }
}
