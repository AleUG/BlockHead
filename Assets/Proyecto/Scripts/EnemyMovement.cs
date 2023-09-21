using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float originalMoveSpeed = 3.5f;
    public float moveSpeed = 3.5f;
    public float followDistance = 5.0f;
    public float rotationSpeed = 10f;
    public float attackDistance = 2.0f;

    private bool isAttacking = false;
    private bool isCooldown = false;
    private float attackCooldown = 0.5f;

    private EnemyHealth enemyHealth;
    private Transform player;
    private Animator animator;

    private float wanderTimer = 2.0f;
    private float wanderInterval = 2.0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();

        originalMoveSpeed = moveSpeed;
        wanderTimer = wanderInterval;
    }

    void Update()
    {

        if (player != null)
        {
            Vector3 directionToPlayer = player.position - transform.position;
            directionToPlayer.y = 0;

            if (!isAttacking)
            {
                if (directionToPlayer.magnitude <= followDistance)
                {
                    // Código para seguir al jugador
                    Vector3 moveDirection = directionToPlayer.normalized;
                    transform.position += moveDirection * moveSpeed * Time.deltaTime;
                    Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    animator.SetBool("Walk", true);

                    if (directionToPlayer.magnitude <= attackDistance)
                    {
                        isAttacking = true;
                        animator.SetBool("Ataque", true);
                        moveSpeed = 0f;
                    }
                    else
                    {
                        animator.SetBool("Ataque", false);

                    }
                }
                else
                {
                    // Caminata aleatoria
                    wanderTimer -= Time.deltaTime;
                    if (wanderTimer <= 0)
                    {
                        Vector3 randomDirection = Random.insideUnitCircle.normalized;
                        Vector3 targetPosition = transform.position + randomDirection * 5f;
                        Vector3 moveDirection = targetPosition - transform.position;
                        moveDirection.y = 0;
                        moveDirection.Normalize(); // Normalizar la dirección
                        transform.position += moveDirection * moveSpeed * Time.deltaTime;
                        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                        animator.SetBool("Walk", true);
                        animator.SetBool("Ataque", false);

                        wanderTimer = wanderInterval;
                    }
                    else
                    {
                        animator.SetBool("Walk", false);
                        animator.SetBool("Ataque", false);
                    }
                }
            }
            else
            {
                // Enfriamiento después del ataque
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
        moveSpeed = originalMoveSpeed;
        isAttacking = false;
        isCooldown = false;
    }
}
