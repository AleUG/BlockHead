using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    public float followDistance = 5.0f; // Distancia a partir de la cual el enemigo seguir� al jugador
    public float rotationSpeed = 10f;

    public float attackDistance = 2.0f; // Distancia a partir de la cual el enemigo comenzar� a atacar
    private bool isAttacking = false;
    private bool isCooldown = false; // Para evitar que el enemigo siga inmediatamente despu�s del ataque
    private float attackCooldown = 0.5f; // Tiempo de enfriamiento despu�s del ataque

    private EnemyHealth enemyHealth;

    private Transform player;
    private Animator animator;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();

        enemyHealth = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 directionToPlayer = player.position - transform.position;
            directionToPlayer.y = 0; // No queremos mover en la direcci�n vertical

            if (!isAttacking)
            {
                if (directionToPlayer.magnitude <= followDistance)
                {
                    // Calcula la direcci�n hacia la posici�n del jugador
                    Vector3 moveDirection = directionToPlayer.normalized;

                    // Mueve al enemigo hacia la posici�n del jugador
                    transform.position += moveDirection * moveSpeed * Time.deltaTime;

                    // Calcula la rotaci�n hacia la direcci�n del jugador
                    Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

                    // Suaviza la rotaci�n usando Slerp
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                    animator.SetBool("Walk", true);

                    if (directionToPlayer.magnitude <= attackDistance)
                    {
                        isAttacking = true;
                        animator.SetTrigger("Ataque");
                        // Detener el movimiento durante la animaci�n de ataque
                        moveSpeed = 0f;
                    }
                }
                else
                {
                    // Si el enemigo est� fuera de rango de ataque, desactiva la animaci�n de caminar
                    animator.SetBool("Walk", false);
                }
            }
            else
            {
                // El enemigo est� atacando, inicia el enfriamiento
                if (!isCooldown)
                {
                    isCooldown = true;
                    Invoke("FinishAttack", attackCooldown);
                }
            }
        }
    }

    private void FinishAttack()
    {
        // La animaci�n de ataque ha terminado, permite que el enemigo vuelva a moverse y seguir al jugador
        moveSpeed = 3.5f;
        isAttacking = false;
        isCooldown = false;
    }
}
