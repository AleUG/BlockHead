using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public TextMeshProUGUI orbCountText; // Referencia al TextMeshPro Text para mostrar la cantidad de orbes
    public int itemCost = 10; // Costo del artículo en orbes
    private int orbCount; // Cantidad de orbes del jugador
    public GameObject canvasShop;

    private void Start()
    {
        // Puedes obtener la cantidad actual de orbes del jugador desde el OrbCollector u otra fuente
        orbCount = FindObjectOfType<OrbCollector>().GetOrbCount();
        UpdateOrbCountText();
    }

    private void Update()
    {
        // Abre o cierra el menú de compra cuando se presiona el botón TAB
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleShopMenu();
        }
    }

    public void BuyItem()
    {
        // Comprueba si el jugador tiene suficientes orbes para comprar el artículo
        if (FindObjectOfType<OrbCollector>().SpendOrbs(itemCost))
        {
            // Realiza aquí las acciones necesarias para dar al jugador el artículo comprado
            Debug.Log("Has comprado el artículo.");

            // Actualiza el contador de orbes en el TextMeshPro Text
            UpdateOrbCountText();
        }
        else
        {
            // El jugador no tiene suficientes orbes para comprar el artículo
            Debug.Log("No tienes suficientes orbes para comprar este artículo.");
        }
    }


    private void ToggleShopMenu()
    {
        // Activa o desactiva el Canvas de compra
        canvasShop.SetActive(!canvasShop.activeSelf);
    }

    private void UpdateOrbCountText()
    {
        // Actualiza el TextMeshPro Text para mostrar la cantidad de orbes
        orbCountText.text = orbCount.ToString();
    }
}
