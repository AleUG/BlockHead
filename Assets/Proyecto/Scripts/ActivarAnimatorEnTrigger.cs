using UnityEngine;

public class ActivarAnimatorEnTrigger : MonoBehaviour
{
    private Animator animator; // Arrastra el Animator que deseas activar en el Inspector.
    public GameObject manitos;

    private void Start()
    {
        animator = manitos.GetComponent<Animator>();

    }
    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra en el trigger es el jugador (puedes ajustar esto según tu juego).
        if (other.CompareTag("Player"))
        {
            // Activa el trigger en el Animator.
            animator.SetTrigger("Manitos");
        }
    }
}
