using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    public TextMeshProUGUI orbCountText; // Referencia al TextMeshPro Text para mostrar la cantidad de orbes
    public GameObject canvasShop;
    public List<int> itemPrices; // Lista de precios de los artículos

    private StatsUpgrade stats;
    private PauseManager pauseManager;
    private OrbCollector orbScript;

    private bool isShopOpen;

    private void Start()
    {
        orbScript = FindObjectOfType<OrbCollector>();
        stats = GetComponent<StatsUpgrade>();
        pauseManager = GetComponent<PauseManager>();
    }

    private void Update()
    {
        // Abre o cierra el menú de compra cuando se presiona el botón TAB
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleShopMenu();
        }
    }

    public void BuyItem(int itemIndex)
    {
        if (itemIndex >= 0 && itemIndex < itemPrices.Count)
        {
            int itemCost = itemPrices[itemIndex]; // Obtiene el precio del artículo seleccionado

            // Comprueba si el jugador tiene suficientes orbes para comprar el artículo
            if (FindObjectOfType<OrbCollector>().SpendOrbs(itemCost))
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

                // Actualiza el contador de orbes en el TextMeshPro Text
                orbScript.UpdateOrbText();
            }
            else
            {
                // El jugador no tiene suficientes orbes para comprar el artículo
                Debug.Log("No tienes suficientes orbes para comprar este artículo.");
                //Condición para que no se ejecute el el botón
            }
        }
        else
        {
            Debug.LogError("Índice de artículo fuera de rango.");
        }
    }

    private void ToggleShopMenu()
    {
        // Activa o desactiva el Canvas de compra
        canvasShop.SetActive(!canvasShop.activeSelf);

        // Pausa o reanuda el juego según si el menú de la tienda está abierto o cerrado
        isShopOpen = !isShopOpen;
        if (isShopOpen)
        {
            pauseManager.PauseGame();
        }
        else
        {
            pauseManager.ResumeGame();
        }
    }
}
