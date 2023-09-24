using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnemyHealthBoss : MonoBehaviour
{
    public float vidaMaxima = 50f;
    public float vidaActual;
    public List<Renderer> enemyRenderers;
    private List<Color> originalColors = new List<Color>();

    public Color damageColor = Color.red;
    public float damageFlashDuration = 0.2f;
    public float pushBackForce = 5.0f;

    public GameObject bloodParticlesPrefab;

    public GameObject dropOrb; // Prefab que el enemigo puede soltar

    private Rigidbody enemyRigidbody;
    private Transform playerTransform;

    private Animator animator;

    public Image healthBarImage;
    public Image damageIndicator;

    public float damageAnimationDuration = 0.4f;
    private bool isDamaged = false;

    private float targetDamageFillAmount = 0f; // Inicializado en 0 para que comience en 1

    void Start()
    {
        vidaActual = vidaMaxima;

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

    public void TakeDamage(int damageAmount, int cantidad)
    {
        vidaActual -= damageAmount;

        // Calcula el valor de fillAmount en función del daño recibido
        float damageRatio = (float)cantidad / vidaMaxima;
        float newFillAmount = Mathf.Clamp01(healthBarImage.fillAmount - damageRatio);

        if (vidaActual <= 0)
        {
            EnemyDamage();
            Die();

            if (bloodParticlesPrefab != null)
            {
                Instantiate(bloodParticlesPrefab, transform.position, Quaternion.identity);
            }

            // Generar una cantidad aleatoria de orbes entre 1 y 10
            int randomOrbCount = Random.Range(40, 75);

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
            if (!isDamaged)
            {
                isDamaged = true;
                StartCoroutine(ResetDamageAnimation());
            }
            EnemyDamage();

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

    private IEnumerator ResetDamageAnimation()
    {
        yield return new WaitForSeconds(damageAnimationDuration);
        isDamaged = false;
    }

    void Die()
    {
        animator.SetTrigger("Dead");
        Destroy(gameObject, 1f);
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
