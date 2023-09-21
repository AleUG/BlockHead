using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class ShopManager : MonoBehaviour
{
    public TextMeshProUGUI orbCountText; // Referencia al TextMeshPro Text para mostrar la cantidad de orbes
    public GameObject canvasShop;
    public List<ShopItem> shopItems; // Lista de ítems de la tienda

    private StatsUpgrade stats;
    private OrbCollector orbScript;
    private bool isShopOpen = false; // Variable para controlar el estado del canvas
    private Animator animator;
    public float delayBeforeClosing = 1.0f; // Tiempo de retraso antes de cerrar la tienda

    private void Start()
    {
        orbScript = FindObjectOfType<OrbCollector>();
        stats = GetComponent<StatsUpgrade>();
        animator = canvasShop.GetComponent<Animator>();

        canvasShop.SetActive(false);
    }

    private void Update()
    {
        // Abre o cierra el menú de compra cuando se presiona el botón TAB
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isShopOpen)
            {
                canvasShop.SetActive(true);
                isShopOpen = true;
                animator.SetBool("End", false);
                Time.timeScale = 0f;
            }
            else
            {
                animator.SetBool("End", true);
                StartCoroutine(DelayedCloseShop());
                Time.timeScale = 1f;
            }
        }
    }

    public void BuyItem(int itemIndex)
    {
        if (itemIndex >= 0 && itemIndex < shopItems.Count)
        {
            ShopItem currentItem = shopItems[itemIndex]; // Obtiene el ítem seleccionado

            // Comprueba si el jugador tiene suficientes orbes para comprar el artículo
            if (FindObjectOfType<OrbCollector>().SpendOrbs(currentItem.itemPrice))
            {
                // Realiza aquí las acciones necesarias para dar al jugador el artículo comprado
                Debug.Log("Has comprado el artículo.");

                if (itemIndex == 0)
                {
                    stats.UpgradeLife();
                }
                else if (itemIndex == 1)
                {
                    stats.UpgradeVelocity();
                }
                else if (itemIndex == 2)
                {
                    stats.UpgradeDamage();
                }

                // Aumenta el precio del ítem para la siguiente compra
                currentItem.itemPrice += 50; // Puedes ajustar esto

                // Actualiza el contador de orbes en el TextMeshPro Text
                orbScript.UpdateOrbText();

                // Cierra la tienda después de una compra
                StartCoroutine(DelayedCloseShop());
            }
            else
            {
                // El jugador no tiene suficientes orbes para comprar el artículo
                Debug.Log("No tienes suficientes orbes para comprar este artículo.");
                //Condición para que no se ejecute el botón
            }
        }
        else
        {
            Debug.LogError("Índice de artículo fuera de rango.");
        }
    }

    private IEnumerator DelayedCloseShop()
    {
        // Espera un tiempo antes de desactivar la tienda
        yield return new WaitForSeconds(delayBeforeClosing);

        // Cierra la tienda
        canvasShop.SetActive(false);
        isShopOpen = false;
    }
}

[System.Serializable]
public class ShopItem
{
    public int itemPrice; // Precio del ítem
    // Puedes agregar más atributos aquí según sea necesario para cada ítem
}
