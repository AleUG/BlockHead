using UnityEngine;

public class EnemyAtaque2 : MonoBehaviour
{
    public float visionRadius = 10f; // Radio de visión del enemigo.
    public float attackRange = 5f; // Rango de ataque del enemigo.
    public Transform gunTransform; // Transform para la posición del cañón del enemigo.
    public float bulletSpeed = 10f; // Velocidad de la bala.
    public float bulletLifetime = 5f; // Tiempo de vida de la bala en segundos.
    public float timeBetweenAttacks = 2f; // Tiempo entre cada ataque en segundos.
    public GameObject bulletPrefab; // Deja esto vacío en el Inspector.

    private Transform player;
    private Animator animator;
    private float timeSinceLastAttack;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();

        // Inicializa el tiempo del último ataque al inicio para permitir el primer ataque inmediatamente.
        timeSinceLastAttack = timeBetweenAttacks;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= visionRadius)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

            if (distanceToPlayer <= attackRange)
            {
                // Comprueba si ha pasado suficiente tiempo desde el último ataque.
                if (Time.time - timeSinceLastAttack >= timeBetweenAttacks)
                {
                    // Si el jugador está dentro del rango de ataque y ha pasado suficiente tiempo, inicia la animación de ataque y realiza el ataque.
                    animator.SetTrigger("Attack");
                    timeSinceLastAttack = Time.time;
                }
            }
            else
            {
                // El jugador está en el radio de visión pero fuera del rango de ataque.
                // Aquí puedes agregar lógica adicional si lo necesitas.
            }
        }
        else
        {
            // El jugador está fuera del radio de visión, detén la animación de ataque y permite que el enemigo ataque nuevamente si el jugador regresa.
            //animator.SetBool("Attacking", false);
        }
    }

    // Esta función se llamará desde un evento de animación al final de la animación de ataque.
    public void Shoot()
    {
        // Calcula la dirección hacia el jugador.
        Vector3 directionToPlayer = (player.position - gunTransform.position).normalized;

        // Crea la bala y aplica la dirección y velocidad.
        GameObject bullet = Instantiate(bulletPrefab, gunTransform.position, Quaternion.identity);
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

        // Aplica velocidad a la bala en la dirección calculada.
        bulletRigidbody.velocity = directionToPlayer * bulletSpeed;

        // Destruye la bala después del tiempo especificado.
        Destroy(bullet, bulletLifetime);
    }
}
