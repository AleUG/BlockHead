using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    public TextMeshProUGUI orbCountText; // Referencia al TextMeshPro Text para mostrar la cantidad de orbes
    public GameObject canvasShop;
    public List<ShopItem> shopItems; // Lista de �tems de la tienda

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
        // Abre o cierra el men� de compra cuando se presiona el bot�n TAB
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleShopMenu();
        }
    }

    public void BuyItem(int itemIndex)
    {
        if (itemIndex >= 0 && itemIndex < shopItems.Count)
        {
            ShopItem currentItem = shopItems[itemIndex]; // Obtiene el �tem seleccionado

            // Comprueba si el jugador tiene suficientes orbes para comprar el art�culo
            if (FindObjectOfType<OrbCollector>().SpendOrbs(currentItem.itemPrice))
            {
                // Realiza aqu� las acciones necesarias para dar al jugador el art�culo comprado
                Debug.Log("Has comprado el art�culo.");

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

                // Aumenta el precio del �tem para la siguiente compra
                currentItem.itemPrice += 50; // Puedes ajustar esto

                // Actualiza el contador de orbes en el TextMeshPro Text
                orbScript.UpdateOrbText();
            }
            else
            {
                // El jugador no tiene suficientes orbes para comprar el art�culo
                Debug.Log("No tienes suficientes orbes para comprar este art�culo.");
                //Condici�n para que no se ejecute el bot�n
            }
        }
        else
        {
            Debug.LogError("�ndice de art�culo fuera de rango.");
        }
    }

    private void ToggleShopMenu()
    {
        // Activa o desactiva el Canvas de compra
        canvasShop.SetActive(!canvasShop.activeSelf);

        // Pausa o reanuda el juego seg�n si el men� de la tienda est� abierto o cerrado
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

    [System.Serializable]
    public class ShopItem
    {
        public int itemPrice; // Precio del �tem
                              // Puedes agregar m�s atributos aqu� seg�n sea necesario para cada �tem
    }
}
