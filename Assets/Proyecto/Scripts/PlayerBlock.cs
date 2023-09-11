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
        // Mantén presionada la tecla para bloquear
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

        // Aquí puedes agregar lógica adicional relacionada con el bloqueo
    }

    public void StopBlocking()
    {
        isBlocking = false;
        animator.SetBool("Block", false);

        playerVida.DesactivarInvulnerabilidad();

        // Aquí puedes agregar lógica adicional cuando dejas de bloquear
    }
}
