using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    private Animator animator;
    private bool isBlocking = false;
    private KeyCode blockKey = KeyCode.K;

    private PlayerVida playerVida;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerVida = GetComponent<PlayerVida>();
    }

    private void Update()
    {
        // Mant�n presionada la tecla para bloquear
        if (Input.GetKeyDown(blockKey))
        {
            StartBlocking();
        }
        else if (Input.GetKeyUp(blockKey))
        {
            StopBlocking();
        }
    }

    private void StartBlocking()
    {
        isBlocking = true;
        animator.SetBool("Block", true);

        playerVida.ActivarInvulnerabilidad();

        // Aqu� puedes agregar l�gica adicional relacionada con el bloqueo
    }

    public void StopBlocking()
    {
        isBlocking = false;
        animator.SetBool("Block", false);

        playerVida.DesactivarInvulnerabilidad();

        // Aqu� puedes agregar l�gica adicional cuando dejas de bloquear
    }
}
