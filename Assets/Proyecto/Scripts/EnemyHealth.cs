using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;
    public Renderer enemyRenderer; // Referencia al componente Renderer del enemigo
    public Color damageColor = Color.red; // Color para representar el daño
    public float damageFlashDuration = 0.2f; // Duración de la animación de cambio de color
    public float pushBackForce = 5.0f; // Fuerza de empuje hacia atrás al recibir daño

    private Color originalColor; // Almacenar el color original del enemigo
    private Rigidbody enemyRigidbody; // Referencia al componente Rigidbody del enemigo

    private Transform playerTransform; // Referencia al transform del jugador

    void Start()
    {
        currentHealth = maxHealth;

        // Guardar el color original del enemigo
        if (enemyRenderer != null)
        {
            originalColor = enemyRenderer.material.color;
        }

        // Obtener la referencia al componente Rigidbody del enemigo
        enemyRigidbody = GetComponent<Rigidbody>();

        // Obtener la referencia al transform del jugador
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Aplicar empuje hacia atrás al recibir daño
            if (enemyRigidbody != null)
            {
                Vector3 pushDirection = transform.position - playerTransform.position; // Dirección desde el jugador al enemigo
                pushDirection.y = 0; // No aplicar fuerza en la dirección vertical
                pushDirection.Normalize();
                enemyRigidbody.AddForce(pushDirection * pushBackForce, ForceMode.Impulse);
            }

            // Iniciar la animación de cambio de color al recibir daño
            if (enemyRenderer != null)
            {
                StartCoroutine(FlashDamageColor());
            }
        }
    }

    void Die()
    {
        Destroy(gameObject, 0.01f);
    }

    // Corrutina para animar el cambio de color al recibir daño
    private IEnumerator FlashDamageColor()
    {
        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = damageColor;
            yield return new WaitForSeconds(damageFlashDuration);
            enemyRenderer.material.color = originalColor;
        }
    }
}
