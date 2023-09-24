using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossFight : MonoBehaviour
{
    private Transform player; // Referencia al objeto del jugador
    public float attackDistance = 3f; // Distancia a la que el jefe atacar� al jugador
    public float attackCooldown = 2f; // Tiempo entre ataques
    private Animator bossAnimator; // Animator del jefe
    private NavMeshAgent navMeshAgent;
    private float attackTimer = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bossAnimator = GetComponent<Animator>();

        navMeshAgent = GetComponent<NavMeshAgent>();
        if (player == null)
        {
            Debug.LogError("No se ha asignado una referencia al objeto del jugador.");
        }
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Si el jugador est� dentro de la distancia de ataque
        if (distanceToPlayer <= attackDistance)
        {
            // Detener al jefe y mirar al jugador
            navMeshAgent.isStopped = true;
            transform.LookAt(player);

            // Comprobar el cooldown antes de atacar nuevamente
            if (attackTimer <= 0f)
            {
                // Ejecutar la animaci�n de ataque
                bossAnimator.SetTrigger("Attack");

                // Aqu� puedes agregar l�gica adicional para causar da�o al jugador

                // Reiniciar el temporizador de ataque
                attackTimer = attackCooldown;
            }
        }
        else
        {
            // Si el jugador est� fuera de la distancia de ataque, seguir al jugador
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(player.position);
        }

        // Actualizar el temporizador de ataque
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }
    }
}
