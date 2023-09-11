using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic; // Agrega esta línea para usar List.

public class PlayerVida : MonoBehaviour
{
    public float vidaMaxima; // Vida máxima del jugador
    public float vidaActual; // Vida actual del jugador
    public GameObject gameOverCanvas; // Referencia al canvas de Game Over

    public Image healthBarImage;
    public Image damageIndicator; // Referencia al sprite de indicación de daño

    private float targetDamageFillAmount = 1f;

    public List<Renderer> playerRender; // Lista de Renderers
    private List<Color> originalColors = new List<Color>();


    public AudioSource gameplayMusicSource; // Referencia al AudioSource de la música del gameplay
    public AudioSource gameOverMusicSource; // Referencia al AudioSource de la música del Game Over
    public AudioSource damageAudioSource; // Referencia al AudioSource para reproducir el sonido de daño

    private float gameOverMusicVolume = 0.5f; // Volumen de la música especial para el Game Over

    private bool invulnerable = false;
    private bool isTakingDamage = false; // Indica si el jugador está recibiendo daño actualmente
    private Rigidbody rb;
    public float reboteForce = 10f;
    public float invulnerabilityDuration = 2f;
    public float blinkInterval = 0.2f;

    private void Start()
    {
        vidaActual = vidaMaxima; // Establece la vida actual al valor máximo al iniciar
        rb = GetComponent<Rigidbody>();

        // Guarda los colores originales de los Renderers en la lista originalColors
        foreach (Renderer renderer in playerRender)
        {
            if (renderer != null)
            {
                originalColors.Add(renderer.material.color);
            }
        }
    }


    public void RecibirDaño(int cantidad)
    {
        if (invulnerable || isTakingDamage)
        {
            return;
        }

        vidaActual -= cantidad; // Resta la cantidad de daño a la vida actual

        // Calcula el valor de fillAmount en función del daño recibido
        float damageRatio = (float)cantidad / vidaMaxima;
        float newFillAmount = Mathf.Clamp01(healthBarImage.fillAmount - damageRatio);

        if (vidaActual <= 0)
        {
            // El jugador ha perdido toda su vida

            // Deshabilitar los controles del jugador (opcional)
            // Aquí puedes desactivar otros componentes relacionados con el jugador si es necesario

            // Reproducir el sonido de daño
            if (damageAudioSource != null)
            {
                damageAudioSource.Play();
            }

            // Detener la música del gameplay
            //gameplayMusicSource.Stop();

            gameOverCanvas.SetActive(true);
            // Reproducir la música especial para el Game Over
            //gameOverMusicSource.volume = gameOverMusicVolume;
            //gameOverMusicSource.Play(); 

            // Desactivar el objeto después de la duración del sonido de daño
            gameObject.SetActive(false);
        }
        else
        {
            // Rebote del jugador al recibir daño
            Vector3 lookDirection = transform.forward;
            rb.velocity = Vector3.zero;
            rb.AddForce(-lookDirection * reboteForce, ForceMode.Impulse);

            // Aplica el efecto de cambio de color a los Renderers
            foreach (Renderer renderer in playerRender)
            {
                StartCoroutine(FlashDamageColor(renderer));
            }

            // Activar invulnerabilidad y el parpadeo del sprite
            ActivarInvulnerabilidad();
            //InvokeRepeating("ToggleSpriteRenderer", 0f, blinkInterval);
            Invoke("DesactivarInvulnerabilidad", invulnerabilityDuration);

            // Reproducir el sonido de daño
            if (damageAudioSource != null)
            {
                damageAudioSource.Play();
            }

            // Indicar que el jugador está recibiendo daño
            isTakingDamage = true;

            // Si el damageIndicator ya está activo, calcula targetDamageFillAmount
            if (damageIndicator.gameObject.activeSelf)
            {
                targetDamageFillAmount = newFillAmount;
            }
            else
            {
                // Si no, actívalo y establece targetDamageFillAmount
                damageIndicator.gameObject.SetActive(true);
                targetDamageFillAmount = newFillAmount;
            }
        }
    }

    private IEnumerator ScrollDamageIndicator(float newFillAmount)
    {
        // Define la duración de la animación
        float duration = 1.0f; // Puedes ajustar este valor

        // Obtiene el fillAmount actual
        float initialFillAmount = damageIndicator.fillAmount;

        // Realiza la animación
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            // Interpola gradualmente el fillAmount desde el valor inicial al nuevo valor
            damageIndicator.fillAmount = Mathf.Lerp(initialFillAmount, newFillAmount, elapsedTime / duration);

            // Incrementa el tiempo transcurrido
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Establece el fillAmount en el nuevo valor
        damageIndicator.fillAmount = newFillAmount;

        // Desactiva el sprite de indicación de daño si está vacío
        if (newFillAmount <= 0)
        {
            damageIndicator.gameObject.SetActive(false);
        }
    }


    public void ActivarInvulnerabilidad()
    {
        invulnerable = true;
    }

    public void DesactivarInvulnerabilidad()
    {
        invulnerable = false;
        isTakingDamage = false;
    }


    public void Curar(int cantidad)
    {
        vidaActual += cantidad; // Suma la cantidad de curación a la vida actual

        if (vidaActual > vidaMaxima)
        {
            vidaActual = vidaMaxima; // Limita la vida actual al valor máximo
        }
    }

    public void SetVidaMaxima()
    {
        vidaActual = vidaMaxima;
        // Lógica adicional, si es necesario
    }

    private IEnumerator FlashDamageColor(Renderer renderer)
    {
        if (renderer != null)
        {
            // Cambia el color del material del Renderer al color de daño
            renderer.material.color = Color.red;

            // Espera un tiempo
            yield return new WaitForSeconds(0.25f);

            // Restaura el color original del material del Renderer
            renderer.material.color = originalColors[playerRender.IndexOf(renderer)];
        }
    }

    void Update()
    {
        // Actualiza gradualmente el fillAmount de la barra de vida
        healthBarImage.fillAmount = Mathf.Lerp(healthBarImage.fillAmount, vidaActual / vidaMaxima, Time.deltaTime * 25f);

        // Actualiza gradualmente el fillAmount del damageIndicator
        damageIndicator.fillAmount = Mathf.Lerp(damageIndicator.fillAmount, targetDamageFillAmount, Time.deltaTime * 3f);

        // Si el fillAmount del damageIndicator se acerca al objetivo, desactívalo
        if (Mathf.Approximately(damageIndicator.fillAmount, targetDamageFillAmount))
        {
            damageIndicator.fillAmount = targetDamageFillAmount;
            if (targetDamageFillAmount == 0f)
            {
                damageIndicator.gameObject.SetActive(false);
            }
        }
    }

}
