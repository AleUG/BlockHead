using UnityEngine;

public class StatsUpgrade : MonoBehaviour
{
    private PlayerVida playerVida;
    private PlayerAttack playerAttack;
    private PlayerMovement playerMove;

    private Animator animator;
    private GameObject canvasLife;

    public float lifeUpgrade = 25.0f;
    public float moveSpeedUpgrade = 0.5f;
    public int attackUpgrade = 2;

    public int lifeUpgradesBought = 0; // Variable para llevar un registro de las vidas compradas
    private int speedUpgradesBought = 0; // Variable para llevar un registro de las mejoras de velocidad compradas
    private int damageUpgradesBought = 0; // Variable para llevar un registro de las mejoras de daño compradas

    private void Start()
    {
        canvasLife = GameObject.Find("LifeManager");

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

        // Cargar el número de mejoras realizadas desde PlayerPrefs
        lifeUpgradesBought = PlayerPrefs.GetInt("LifeUpgrades", 0);
        speedUpgradesBought = PlayerPrefs.GetInt("SpeedUpgrades", 0);
        damageUpgradesBought = PlayerPrefs.GetInt("DamageUpgrades", 0);
    }


    private void SaveUpgradeCounts()
    {
        // Guardar el número de mejoras realizadas en PlayerPrefs
        PlayerPrefs.SetInt("LifeUpgrades", lifeUpgradesBought);
        PlayerPrefs.SetInt("SpeedUpgrades", speedUpgradesBought);
        PlayerPrefs.SetInt("DamageUpgrades", damageUpgradesBought);
    }

    public void UpgradeLife()
    {
        if (playerVida != null && lifeUpgradesBought < 3)
        {
            playerVida.vidaMaxima += lifeUpgrade;
            playerVida.vidaActual += lifeUpgrade;

            // Activa las animaciones de escala según el número de mejoras de vida compradas
            if (lifeUpgradesBought == 0)
            {
                animator.SetTrigger("Scale1");
            }
            else if (lifeUpgradesBought == 1)
            {
                animator.SetTrigger("Scale2");
            }
            else if (lifeUpgradesBought == 2)
            {
                animator.SetTrigger("Scale3");
            }

            lifeUpgradesBought++;

            // Guardar el número de mejoras realizadas
            SaveUpgradeCounts();

            // Guardar las estadísticas actualizadas en PlayerPrefs
            PlayerPrefs.SetFloat("MaxLife", playerVida.vidaMaxima);
            PlayerPrefs.SetFloat("CurrentLife", playerVida.vidaActual);
        }
    }

    public void UpgradeVelocity()
    {
        if (playerMove != null && speedUpgradesBought < 3)
        {
            playerMove.speed += moveSpeedUpgrade;
            speedUpgradesBought++;

            // Guardar el número de mejoras realizadas
            SaveUpgradeCounts();

            // Guardar la velocidad actualizada en PlayerPrefs
            PlayerPrefs.SetFloat("MoveSpeed", playerMove.speed);
        }
    }

    public void UpgradeDamage()
    {
        if (playerAttack != null && damageUpgradesBought < 3)
        {
            playerAttack.attackDamage += attackUpgrade;
            damageUpgradesBought++;

            // Guardar el número de mejoras realizadas
            SaveUpgradeCounts();

            // Guardar el daño actualizado en PlayerPrefs
            PlayerPrefs.SetInt("AttackDamage", playerAttack.attackDamage);
        }
    }
}
