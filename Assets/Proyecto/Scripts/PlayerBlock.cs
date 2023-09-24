using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    private Animator animator;
    private bool isBlocking = false;
    private KeyCode blockKey = KeyCode.K;

    private PlayerVida playerVida;

    public GameObject shield;
    private ShieldPush shieldPush;

    public AudioSource sound;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerVida = GetComponent<PlayerVida>();
        shieldPush = shield.GetComponent<ShieldPush>();
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
            shieldPush.golpesRecibidos = 0;
        }
    }

    private void StartBlocking()
    {
        sound.Play();

        isBlocking = true;
        animator.SetBool("Block", true);

        if(isBlocking == true)
        {
            animator.SetBool("Ataque", false);
            animator.SetBool("ComboAttack1", false);
            animator.SetBool("ComboAttack2", false);
        }

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
