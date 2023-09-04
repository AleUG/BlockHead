using UnityEngine;
using System.Collections;
using TMPro;

public class PlayerAttack : MonoBehaviour
{
    private KeyCode attackKey = KeyCode.J;
    public int attackDamage = 1;
    public float comboWindow = 1.5f;
    public float maxTurnDistance = 5.0f;

    private Animator animator;
    private Coroutine attackCoroutine;

    private bool isAttacking = false;
    private float lastAttackTime = 0f;
    public float coroutineTime = 0.5f;

    public TextMeshProUGUI comboText; // Referencia al TextMeshPro que muestra el contador de combo
    private int comboCount = 0;
    private float lastComboTime = 0f;


    private void Start()
    {
        animator = GetComponent<Animator>();
        comboText = GameObject.Find("ComboText").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(attackKey))
        {
            PerformAttack();
        }

        if (Time.time - lastAttackTime > comboWindow)
        {
            isAttacking = false;

            // Verifica si ha pasado suficiente tiempo desde el último combo
            if (Time.time - lastComboTime > comboWindow)
            {
                comboCount = 0;
                comboText.text = "0"; // Actualiza el texto del contador de combos
            }
        }

        if (comboCount <= 0)
        {
            comboText.gameObject.SetActive(false);
        }
        else
        {
            comboText.gameObject.SetActive(true);
        }
    }

    void PerformAttack()
{
    if (!isAttacking)
    {
        // Verificar si el personaje está en el estado de salto
        bool isJumping = animator.GetBool("Jump");

        // Si el personaje está saltando, puede realizar un ataque especial
        if (isJumping)
        {
                isAttacking = true;
                // Ejecuta la animación de ataque especial para el salto
                animator.SetTrigger("JumpAttack");
        }
        else
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

            lastAttackTime = Time.time;
            isAttacking = true;
            animator.SetTrigger("Ataque");
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
            }
            attackCoroutine = StartCoroutine(DisableAttackCollider());
        }
    }

    else if (Time.time - lastAttackTime <= comboWindow)
    {
        if (!animator.GetBool("ComboAttack1"))
        {
            lastAttackTime = Time.time;
            animator.SetBool("ComboAttack1", true);
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
            }
            attackCoroutine = StartCoroutine(DisableAttackCollider());
            return;
        }
        else if (!animator.GetBool("ComboAttack2"))
        {
            lastAttackTime = Time.time;
            animator.SetBool("ComboAttack2", true);
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
            }
            attackCoroutine = StartCoroutine(DisableAttackCollider());
            return;
        }
    }
}


    IEnumerator DisableAttackCollider()
    {
        yield return new WaitForSeconds(coroutineTime);
        isAttacking = false;
        animator.SetBool("Ataque", false);
        animator.SetBool("ComboAttack1", false);
        animator.SetBool("ComboAttack2", false);
    }

    GameObject FindNearestEnemy()
    {
        int enemyLayerMask = LayerMask.GetMask("Enemy");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, maxTurnDistance, enemyLayerMask);

        GameObject nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Collider collider in hitColliders)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, collider.transform.position);

            if (distanceToEnemy < nearestDistance)
            {
                nearestDistance = distanceToEnemy;
                nearestEnemy = collider.gameObject;
            }

        }

        return nearestEnemy;
    }

    void OnTriggerEnter(Collider other)
    {
        if ((animator.GetBool("Ataque") || animator.GetBool("ComboAttack1")) || animator.GetBool("ComboAttack2") && other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);

                // Incrementa el contador de combos
                comboCount++;

                // Actualiza el texto del contador de combos
                comboText.text = "" + comboCount;

                // Actualiza el tiempo del último combo
                lastComboTime = Time.time;
            }
        }
    }
}
