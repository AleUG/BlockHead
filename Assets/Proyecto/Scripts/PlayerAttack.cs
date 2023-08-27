using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private KeyCode attackKey = KeyCode.J;
    public int attackDamage = 20;
    public float comboWindow = 0.5f; // Ventana de tiempo para activar el segundo ataque
    public float maxTurnDistance = 5.0f; // Distancia m�xima para girar hacia un enemigo

    private Animator animator;
    public BoxCollider attackCollider; // Referencia al BoxCollider Trigger del jugador

    private bool isAttacking = false; // Indica si el jugador est� atacando
    private float lastAttackTime = 0f; // Momento del �ltimo ataque

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

        // Si ha pasado suficiente tiempo desde el �ltimo ataque, restablece el combo
        if (Time.time - lastAttackTime > comboWindow)
        {
            isAttacking = false;
        }
    }

    void PerformAttack()
    {
        if (!isAttacking)
        {
            // Encuentra el enemigo m�s cercano
            GameObject nearestEnemy = FindNearestEnemy();

            if (nearestEnemy != null)
            {
                // Calcula la distancia al enemigo m�s cercano
                float distanceToEnemy = Vector3.Distance(transform.position, nearestEnemy.transform.position);

                // Comprueba si la distancia es menor que la distancia m�xima para girar
                if (distanceToEnemy < maxTurnDistance)
                {
                    // Gira al jugador hacia el enemigo m�s cercano
                    Vector3 targetDirection = nearestEnemy.transform.position - transform.position;
                    targetDirection.y = 0f;
                    transform.rotation = Quaternion.LookRotation(targetDirection);
                }
            }

            lastAttackTime = Time.time; // Actualiza el tiempo del �ltimo ataque
            isAttacking = true; // Indica que el jugador est� atacando
            animator.SetTrigger("Ataque"); // Activa la animaci�n de ataque
            attackCollider.enabled = true; // Activa el BoxCollider Trigger
            Invoke("EndAttackAnimation", 0.5f); // Cambia el tiempo seg�n la duraci�n de la animaci�n
        }
        else
        {
            // Si el jugador ataca nuevamente dentro de la ventana de combo, ejecuta el segundo ataque
            if (Time.time - lastAttackTime <= comboWindow)
            {
                lastAttackTime = Time.time; // Actualiza el tiempo del �ltimo ataque
                animator.SetTrigger("ComboAttack"); // Activa la animaci�n de combo de ataque
                attackCollider.enabled = true; // Activa el BoxCollider Trigger
                Invoke("EndAttackAnimation", 0.5f); // Cambia el tiempo seg�n la duraci�n de la animaci�n
            }
        }
    }

    void EndAttackAnimation()
    {
        isAttacking = false; // Restablece el estado de ataque
        animator.ResetTrigger("Ataque"); // Resetea el trigger de la animaci�n de ataque
        animator.ResetTrigger("ComboAttack"); // Resetea el trigger de la animaci�n de combo
    }

    // Funci�n para encontrar el enemigo m�s cercano
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

    // Funci�n para manejar colisiones con enemigos
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
