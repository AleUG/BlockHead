using UnityEngine;
using System.Collections;

public class BossMovement : MonoBehaviour
{
    public float originalMoveSpeed = 3.5f;
    public float moveSpeed = 3.5f;
    public float followDistance = 5.0f;
    public float extraDistance = 8.0f; // Nueva distancia extra
    public float rotationSpeed = 10f;
    public float attackDistance = 2.0f;

    private Animator animator;

    private bool isAttacking = false;
    private bool isCooldown = false;
    private bool hasPerformedExtraAttack = false; // Para evitar repetir la animación extra
    private float attackCooldown = 0.5f;

    private EnemyHealthBoss enemyHealthBoss;
    private Transform player;

    private bool isExtraAttackActive = false; // Nuevo bool para controlar la animación extra

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyHealthBoss = GetComponent<EnemyHealthBoss>();

        animator = GetComponent<Animator>();

        originalMoveSpeed = moveSpeed;
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
                    animator.SetBool("Ataque2", false);

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
                    animator.SetBool("Walk", false);
                    animator.SetBool("Ataque", false);
                }

                // Verificar si el jugador está a la distancia extra y aún no se ha realizado la animación extra
                if (directionToPlayer.magnitude < followDistance && directionToPlayer.magnitude >= extraDistance && !isAttacking && !hasPerformedExtraAttack)
                {
                    animator.SetTrigger("Ataque2");
                    animator.Play("Ataque_2");
                    hasPerformedExtraAttack = true;

                    StartCoroutine(ResetAtaque2());
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

    private IEnumerator ResetAtaque2()
    {
        yield return new WaitForSeconds(12f);
        hasPerformedExtraAttack = false;
    }

    private void FinishAttack()
    {
        moveSpeed = originalMoveSpeed;
        isAttacking = false;
        isCooldown = false;
    }
}
