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

    public int lifeUpgradesBought = 0;
    private int speedUpgradesBought = 0;
    private int damageUpgradesBought = 0;

    // Variables para llevar un registro de las imágenes actuales
    private int currentLifeImage = 0;
    private int currentSpeedImage = 0;
    private int currentDamageImage = 0;

    public GameObject[] lifeImageLVL;
    public GameObject[] speedImageLVL;
    public GameObject[] damageImageLVL;

    private void Start()
    {
        canvasLife = GameObject.Find("LifeManager");

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        animator = canvasLife.GetComponent<Animator>();

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

        // Cargar los valores de las imágenes actuales desde PlayerPrefs
        currentLifeImage = PlayerPrefs.GetInt("CurrentLifeImage", 0);
        currentSpeedImage = PlayerPrefs.GetInt("CurrentSpeedImage", 0);
        currentDamageImage = PlayerPrefs.GetInt("CurrentDamageImage", 0);

        // Actualizar las imágenes de mejora según los valores cargados
        UpdateUpgradeImages();
    }

    private void SaveUpgradeCounts()
    {
        // Guardar el número de mejoras realizadas en PlayerPrefs
        PlayerPrefs.SetInt("LifeUpgrades", lifeUpgradesBought);
        PlayerPrefs.SetInt("SpeedUpgrades", speedUpgradesBought);
        PlayerPrefs.SetInt("DamageUpgrades", damageUpgradesBought);

        // Guardar los valores de las imágenes actuales en PlayerPrefs
        PlayerPrefs.SetInt("CurrentLifeImage", currentLifeImage);
        PlayerPrefs.SetInt("CurrentSpeedImage", currentSpeedImage);
        PlayerPrefs.SetInt("CurrentDamageImage", currentDamageImage);
    }

    private void UpdateUpgradeImages()
    {
        // Desactiva todas las imágenes de vida
        for (int i = 0; i < lifeImageLVL.Length; i++)
        {
            lifeImageLVL[i].SetActive(false);
        }

        // Activa la imagen de vida según el número de mejoras compradas
        if (currentLifeImage > 0)
        {
            lifeImageLVL[currentLifeImage - 1].SetActive(true);
        }

        // Desactiva todas las imágenes de velocidad
        for (int i = 0; i < speedImageLVL.Length; i++)
        {
            speedImageLVL[i].SetActive(false);
        }

        // Activa la imagen de velocidad según el número de mejoras compradas
        if (currentSpeedImage > 0)
        {
            speedImageLVL[currentSpeedImage - 1].SetActive(true);
        }

        // Desactiva todas las imágenes de daño
        for (int i = 0; i < damageImageLVL.Length; i++)
        {
            damageImageLVL[i].SetActive(false);
        }

        // Activa la imagen de daño según el número de mejoras compradas
        if (currentDamageImage > 0)
        {
            damageImageLVL[currentDamageImage - 1].SetActive(true);
        }
    }


    private void ActivateNextLifeImage()
    {
        if (currentLifeImage < 3)
        {
            // Activa el nuevo GameObject
            lifeImageLVL[currentLifeImage].SetActive(true);

            // Desactiva el GameObject anterior si existe
            if (currentLifeImage > 0)
            {
                lifeImageLVL[currentLifeImage - 1].SetActive(false);
            }

            currentLifeImage++;
        }
    }

    private void ActivateNextSpeedImage()
    {
        if (currentSpeedImage < 3)
        {
            // Activa el nuevo GameObject
            speedImageLVL[currentSpeedImage].SetActive(true);

            // Desactiva el GameObject anterior si existe
            if (currentSpeedImage > 0)
            {
                speedImageLVL[currentSpeedImage - 1].SetActive(false);
            }

            currentSpeedImage++;
        }
    }

    private void ActivateNextDamageImage()
    {
        if (currentDamageImage < 3)
        {
            // Activa el nuevo GameObject
            damageImageLVL[currentDamageImage].SetActive(true);

            // Desactiva el GameObject anterior si existe
            if (currentDamageImage > 0)
            {
                damageImageLVL[currentDamageImage - 1].SetActive(false);
            }

            currentDamageImage++;
        }
    }

    public void UpgradeLife()
    {
        if (playerVida != null && lifeUpgradesBought < 3)
        {
            playerVida.vidaMaxima += lifeUpgrade;
            playerVida.vidaActual += lifeUpgrade;

            ActivateNextLifeImage();

            // Activa las animaciones de escala según el número de mejoras de vida compradas
            if (lifeUpgradesBought == 0)
            {
                animator.SetTrigger("Scale1");
                Debug.Log("Vida1");
            }
            else if (lifeUpgradesBought == 1)
            {
                animator.SetTrigger("Scale2");
                Debug.Log("Vida2");
            }
            else if (lifeUpgradesBought == 2)
            {
                animator.SetTrigger("Scale3");
                Debug.Log("Vida3");
            }

            lifeUpgradesBought++;

            SaveUpgradeCounts();

            PlayerPrefs.SetFloat("MaxLife", playerVida.vidaMaxima);
            PlayerPrefs.SetFloat("CurrentLife", playerVida.vidaActual);
        }
    }

    public void UpgradeVelocity()
    {
        if (playerMove != null && speedUpgradesBought < 3)
        {
            playerMove.speed += moveSpeedUpgrade;

            ActivateNextSpeedImage();

            speedUpgradesBought++;

            SaveUpgradeCounts();

            PlayerPrefs.SetFloat("MoveSpeed", playerMove.speed);
        }
    }

    public void UpgradeDamage()
    {
        if (playerAttack != null && damageUpgradesBought < 3)
        {
            playerAttack.attackDamage += attackUpgrade;

            ActivateNextDamageImage();

            damageUpgradesBought++;

            SaveUpgradeCounts();

            PlayerPrefs.SetInt("AttackDamage", playerAttack.attackDamage);
        }
    }
}
