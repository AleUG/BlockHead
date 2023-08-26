using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;  // Número máximo de corazones
    public int currentHealth;  // Salud actual
    public Image[] hearts;     // Array de imágenes de corazones
    public Sprite fullHeart;   // Sprite de corazón lleno
    public Sprite emptyHeart;  // Sprite de corazón vacío

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < maxHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;

            Destroy(gameObject);
            // Aquí puedes llamar a una función de "Game Over" o realizar alguna acción cuando el jugador pierde todas las vidas.
        }

        UpdateHearts();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UpdateHearts();
    }
}
