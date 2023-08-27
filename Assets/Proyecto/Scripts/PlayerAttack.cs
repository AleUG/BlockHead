using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private KeyCode attackKey = KeyCode.J;
    public int attackDamage = 20;
    public float comboWindow = 0.5f; // Ventana de tiempo para activar el segundo ataque
    public float maxTurnDistance = 5.0f; // Distancia máxima para girar hacia un enemigo

    private Animator animator;
    public BoxCollider attackCollider; // Referencia al BoxCollider Trigger del jugador

    private bool isAttacking = false; // Indica si el jugador está atacando
    private float lastAttackTime = 0f; // Momento del último ataque

    private void Start()
    {
        animator = GetComponent<Animator>();
        attackCollider = GetComponentInChildren<BoxCollider>(); // Asigna el BoxCollider Trigger
    }

    private void Update()
    {
        if (Input.GetKeyDown(attackKey))
        {
            PerformAttack();
        }

        // Si ha pasado suficiente tiempo desde el último ataque, restablece el combo
        if (Time.time - lastAttackTime > comboWindow)
        {
            isAttacking = false;
        }
    }

    void PerformAttack()
    {
        if (!isAttacking)
        {
            // Encuentra el enemigo más cercano
            GameObject nearestEnemy = FindNearestEnemy();

            if (nearestEnemy != null)
            {
                // Calcula la distancia al enemigo más cercano
                float distanceToEnemy = Vector3.Distance(transform.position, nearestEnemy.transform.position);

                // Comprueba si la distancia es menor que la distancia máxima para girar
                if (distanceToEnemy < maxTurnDistance)
                {
                    // Gira al jugador hacia el enemigo más cercano
                    Vector3 targetDirection = nearestEnemy.transform.position - transform.position;
                    targetDirection.y = 0f;
                    transform.rotation = Quaternion.LookRotation(targetDirection);
                }
            }

            lastAttackTime = Time.time; // Actualiza el tiempo del último ataque
            isAttacking = true; // Indica que el jugador está atacando
            animator.SetTrigger("Ataque"); // Activa la animación de ataque
            attackCollider.enabled = true; // Activa el BoxCollider Trigger
            Invoke("EndAttackAnimation", 0.5f); // Cambia el tiempo según la duración de la animación
        }
        else
        {
            // Si el jugador ataca nuevamente dentro de la ventana de combo, ejecuta el segundo ataque
            if (Time.time - lastAttackTime <= comboWindow)
            {
                lastAttackTime = Time.time; // Actualiza el tiempo del último ataque
                animator.SetTrigger("ComboAttack"); // Activa la animación de combo de ataque
                attackCollider.enabled = true; // Activa el BoxCollider Trigger
                Invoke("EndAttackAnimation", 0.5f); // Cambia el tiempo según la duración de la animación
            }
        }
    }

    void EndAttackAnimation()
    {
        isAttacking = false; // Restablece el estado de ataque
        animator.ResetTrigger("Ataque"); // Resetea el trigger de la animación de ataque
        animator.ResetTrigger("ComboAttack"); // Resetea el trigger de la animación de combo
    }

    // Función para encontrar el enemigo más cercano
    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < nearestDistance)
            {
                nearestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    // Función para manejar colisiones con enemigos
    void OnTriggerEnter(Collider other)
    {
        if (animator.GetBool("Ataque") || animator.GetBool("ComboAttack") && other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
            }
        }
    }
}
