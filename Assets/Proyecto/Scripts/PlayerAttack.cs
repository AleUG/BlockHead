using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public KeyCode attackKey = KeyCode.F;
    public int attackDamage = 20;
    public float attackRange = 2.0f;

    private void Update()
    {
        if (Input.GetKeyDown(attackKey))
        {
            PerformAttack();
        }
    }

    void PerformAttack()
    {
        RaycastHit hit;

        // Lanzar un rayo hacia adelante desde la posición del jugador
        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();

                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(attackDamage);
                }
            }
        }
    }

    // Para visualizar el rango de ataque en la escena (opcional)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward * attackRange, 0.1f);
    }
}
