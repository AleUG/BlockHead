using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic; // Agrega esta l�nea para usar List.

public class PlayerVida : MonoBehaviour
{
    public float vidaMaxima; // Vida m�xima del jugador
    public float vidaActual; // Vida actual del jugador
    public GameObject gameOverCanvas; // Referencia al canvas de Game Over

    public Image healthBarImage;
    public Image damageIndicator; // Referencia al sprite de indicaci�n de da�o

    private float targetDamageFillAmount = 1f;

    public List<Renderer> playerRender; // Lista de Renderers
    private List<Color> originalColors = new List<Color>();


    public AudioSource gameplayMusicSource; // Referencia al AudioSource de la m�sica del gameplay
    public AudioSource gameOverMusicSource; // Referencia al AudioSource de la m�sica del Game Over
    public AudioSource damageAudioSource; // Referencia al AudioSource para reproducir el sonido de da�o

    private float gameOverMusicVolume = 0.5f; // Volumen de la m�sica especial para el Game Over

    private bool invulnerable = false;
    private bool isTakingDamage = false; // Indica si el jugador est� recibiendo da�o actualmente
    private Rigidbody rb;
    public float reboteForce = 10f;
    public float invulnerabilityDuration = 2f;
    public float blinkInterval = 0.2f;

    private void Start()
    {
        vidaActual = vidaMaxima; // Establece la vida actual al valor m�ximo al iniciar
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


    public void RecibirDa�o(int cantidad)
    {
        if (invulnerable || isTakingDamage)
        {
            return;
        }

        vidaActual -= cantidad; // Resta la cantidad de da�o a la vida actual

        // Calcula el valor de fillAmount en funci�n del da�o recibido
        float damageRatio = (float)cantidad / vidaMaxima;
        float newFillAmount = Mathf.Clamp01(healthBarImage.fillAmount - damageRatio);

        if (vidaActual <= 0)
        {
            // El jugador ha perdido toda su vida

            // Deshabilitar los controles del jugador (opcional)
            // Aqu� puedes desactivar otros componentes relacionados con el jugador si es necesario

            // Reproducir el sonido de da�o
            if (damageAudioSource != null)
            {
                damageAudioSource.Play();
            }

            // Detener la m�sica del gameplay
            //gameplayMusicSource.Stop();

            gameOverCanvas.SetActive(true);
            // Reproducir la m�sica especial para el Game Over
            //gameOverMusicSource.volume = gameOverMusicVolume;
            //gameOverMusicSource.Play(); 

            // Desactivar el objeto despu�s de la duraci�n del sonido de da�o
            gameObject.SetActive(false);
        }
        else
        {
            // Rebote del jugador al recibir da�o
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

            // Reproducir el sonido de da�o
            if (damageAudioSource != null)
            {
                damageAudioSource.Play();
            }

            // Indicar que el jugador est� recibiendo da�o
            isTakingDamage = true;

            // Si el damageIndicator ya est� activo, calcula targetDamageFillAmount
            if (damageIndicator.gameObject.activeSelf)
            {
                targetDamageFillAmount = newFillAmount;
            }
            else
            {
                // Si no, act�valo y establece targetDamageFillAmount
                damageIndicator.gameObject.SetActive(true);
                targetDamageFillAmount = newFillAmount;
            }
        }
    }

    private IEnumerator ScrollDamageIndicator(float newFillAmount)
    {
        // Define la duraci�n de la animaci�n
        float duration = 1.0f; // Puedes ajustar este valor

        // Obtiene el fillAmount actual
        float initialFillAmount = damageIndicator.fillAmount;

        // Realiza la animaci�n
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

        // Desactiva el sprite de indicaci�n de da�o si est� vac�o
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
        vidaActual += cantidad; // Suma la cantidad de curaci�n a la vida actual

        if (vidaActual > vidaMaxima)
        {
            vidaActual = vidaMaxima; // Limita la vida actual al valor m�ximo
        }
    }

    public void SetVidaMaxima()
    {
        vidaActual = vidaMaxima;
        // L�gica adicional, si es necesario
    }

    private IEnumerator FlashDamageColor(Renderer renderer)
    {
        if (renderer != null)
        {
            // Cambia el color del material del Renderer al color de da�o
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

        // Si el fillAmount del damageIndicator se acerca al objetivo, desact�valo
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
