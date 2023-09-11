using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsUpgrade : MonoBehaviour
{
    private PlayerVida playerVida;
    private PlayerAttack playerAttack;
    private PlayerMovement playerMove;

    public float lifeUpgrade = 25.0f;
    public float moveSpeedUpgrade = 0.5f;
    public int attackUpgrade = 2;

    public GameObject canvasLife;
    private Animator animator;

    private int lifeUpgradesBought = 0; // Variable para llevar un registro de las vidas compradas

    // Start is called before the first frame update
    void Start()
    {
        // Buscamos al jugador con la etiqueta "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        animator = canvasLife.GetComponent<Animator>();

        // Comprobamos si encontramos al jugador
        if (player != null)
        {
            playerVida = player.GetComponent<PlayerVida>();
            playerAttack = player.GetComponent<PlayerAttack>();
            playerMove = player.GetComponent<PlayerMovement>();
        }
    }

    public void UpgradeLife()
    {
        if (playerVida != null)
        {
            playerVida.vidaMaxima += lifeUpgrade;
            playerVida.vidaActual += lifeUpgrade;

            // Aplica una animación diferente en la primera compra de vida
            if (lifeUpgradesBought == 0)
            {
                animator.SetTrigger("Scale1");
            }
            else if (lifeUpgradesBought == 1)
            {
                animator.SetTrigger("Scale2");
                // Aquí puedes aplicar una animación diferente para compras posteriores de vida
                // animator.SetTrigger("OtraAnimacion");
            }
            else if (lifeUpgradesBought == 2)
            {
                animator.SetTrigger("Scale3");
            }

            lifeUpgradesBought++;
        }
    }

    public void UpgradeVelocity()
    {
        if (playerMove != null)
        {
            playerMove.speed += moveSpeedUpgrade;
        }
    }

    public void UpgradeDamage()
    {
        if (playerAttack != null)
        {
            playerAttack.attackDamage += attackUpgrade;
        }
    }
}
