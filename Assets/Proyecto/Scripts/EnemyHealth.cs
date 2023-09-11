using UnityEngine;
using System.Collections;
using System.Collections.Generic; // Agrega esta línea para usar List.

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

    private Rigidbody enemyRigidbody;
    private Transform playerTransform;

    private Orb orbScript;

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
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            EnemyDamage();
            Die();

            // Verificar si el enemigo soltará el prefab
            if (Random.value <= dropProbability && dropPrefab != null)
            {
                Instantiate(dropPrefab, transform.position, Quaternion.identity);
            }

            // Generar una cantidad aleatoria de orbes entre 1 y 10
            int randomOrbCount = Random.Range(1, 11);

            // Generar varios orbes
            for (int i = 0; i < randomOrbCount; i++)
            {
                // Calcular una dirección aleatoria para cada orb
                Vector3 randomDirection = Random.insideUnitSphere;
                randomDirection.y = 0; // No aplicar movimiento en la dirección vertical
                randomDirection.Normalize();

                // Instanciar el orb en una posición ligeramente desplazada
                Vector3 spawnPosition = transform.position + randomDirection * 1.5f; // Puedes ajustar la distancia de dispersión
                Instantiate(dropOrb, spawnPosition, Quaternion.identity);
            }
        }
        else
        {
            EnemyDamage();
        }
    }

    void Die()
    {
        Destroy(gameObject, 0.05f);
    }

    private void EnemyDamage()
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
