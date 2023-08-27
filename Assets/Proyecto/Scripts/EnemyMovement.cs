using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float followDistance = 5.0f; // Distancia a partir de la cual el enemigo seguirá al jugador

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 directionToPlayer = player.position - transform.position;
            directionToPlayer.y = 0; // No queremos mover en la dirección vertical

            if (directionToPlayer.magnitude <= followDistance)
            {
                transform.Translate(directionToPlayer.normalized * moveSpeed * Time.deltaTime);
            }
        }
    }
}
