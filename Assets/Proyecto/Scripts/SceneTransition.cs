using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    private Animator animator;
    public GameObject statsUpgrade;
    private StatsUpgrade stats;

    private void Start()
    {
        animator = GetComponent<Animator>();
        stats = statsUpgrade.GetComponent<StatsUpgrade>();

        // Activa las animaciones de escala según el número de mejoras de vida compradas
        if (stats.lifeUpgradesBought == 1)
        {
            animator.SetTrigger("Scale1");
            Debug.Log("Vida1");
        }
        else if (stats.lifeUpgradesBought == 2)
        {
            animator.SetTrigger("Scale1");
            animator.SetTrigger("Scale2");
            Debug.Log("Vida2");
        }
        else if (stats.lifeUpgradesBought == 3)
        {
            animator.SetTrigger("Scale1");
            animator.SetTrigger("Scale2");
            animator.SetTrigger("Scale3");
            Debug.Log("Vida3");
        }
    }
}
