using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;
    public List<Renderer> enemyRenderers; // Cambia el nombre de la variable a "enemyRenderers".
    private List<Color> originalColors = new List<Color>();

    public Color damageColor = Color.red;
    public float damageFlashDuration = 0.2f;
    public float pushBackForce = 5.0f;

    public GameObject dropPrefab; // Prefab que el enemigo puede soltar
    public GameObject dropOrb; // Prefab que el enemigo puede soltar

    private float dropProbability = 0.05f; // Probabilidad de soltar el prefab (0.3 significa 30%)

    public GameObject bloodParticlesPrefab; // Prefab de part�culas de sangre

    private Rigidbody enemyRigidbody;
    private Transform playerTransform;

    private Animator animator;

    // Tiempo de duraci�n de la animaci�n de da�o
    public float damageAnimationDuration = 0.4f;
    private bool isDamaged = false;

    void Start()
    {
        currentHealth = maxHealth;

        foreach (Renderer renderer in enemyRenderers)
        {
            if (renderer != null)
            {
                originalColors.Add(renderer.material.color);
            }
        }

        enemyRigidbody = GetComponent<Rigidbody>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            EnemyDamage();
            Die();

            // Instancia part�culas de sangre al morir
            if (bloodParticlesPrefab != null)
            {
                Instantiate(bloodParticlesPrefab, transform.position, Quaternion.identity);
            }

            // Verificar si el enemigo soltar� el prefab
            if (Random.value <= dropProbability && dropPrefab != null)
            {
                Instantiate(dropPrefab, transform.position, Quaternion.identity);
            }

            // Generar una cantidad aleatoria de orbes entre 1 y 10
            int randomOrbCount = Random.Range(2, 15);

            // Generar varios orbes
            for (int i = 0; i < randomOrbCount; i++)
            {
                // Calcular una direcci�n aleatoria para cada orb
                Vector3 randomDirection = Random.insideUnitSphere;
                randomDirection.y = 0; // No aplicar movimiento en la direcci�n vertical
                randomDirection.Normalize();

                // Instanciar el orb en una posici�n ligeramente desplazada
                Vector3 spawnPosition = transform.position + randomDirection * 1.5f; // Puedes ajustar la distancia de dispersi�n
                Instantiate(dropOrb, spawnPosition, Quaternion.identity);
            }
        }
        else
        {
            // El enemigo est� da�ado, activa la animaci�n de da�o y establece el tiempo de duraci�n
            if (!isDamaged)
            {
                isDamaged = true;
                animator.SetBool("Damage", true);
                StartCoroutine(ResetDamageAnimation());
            }
            EnemyDamage();
        }
    }

    private IEnumerator ResetDamageAnimation()
    {
        // Espera el tiempo de duraci�n de la animaci�n de da�o
        yield return new WaitForSeconds(damageAnimationDuration);
        // Desactiva la animaci�n de da�o
        animator.SetBool("Damage", false);
        isDamaged = false;
    }

    void Die()
    {
        Destroy(gameObject, 0.05f);
    }

    public void EnemyDamage()
    {
        if (enemyRigidbody != null)
        {
            Vector3 pushDirection = transform.position - playerTransform.position;
            pushDirection.y = 0;
            pushDirection.Normalize();
            enemyRigidbody.AddForce(pushDirection * pushBackForce, ForceMode.Impulse);
        }

        foreach (Renderer renderer in enemyRenderers)
        {
            StartCoroutine(FlashDamageColor(renderer));
        }

        animator.SetBool("Ataque", false);
    }

    private IEnumerator FlashDamageColor(Renderer renderer)
    {
        if (renderer != null)
        {
            renderer.material.color = damageColor;
            yield return new WaitForSeconds(damageFlashDuration);
            renderer.material.color = originalColors[enemyRenderers.IndexOf(renderer)];
        }
    }
}
