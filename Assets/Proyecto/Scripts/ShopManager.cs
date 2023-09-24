using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class ShopManager : MonoBehaviour
{
    public TextMeshProUGUI orbCountText; // Referencia al TextMeshPro Text para mostrar la cantidad de orbes
    public GameObject canvasShop;
    public List<ShopItem> shopItems; // Lista de �tems de la tienda

    private StatsUpgrade stats;
    private OrbCollector orbScript;
    private bool isShopOpen = false; // Variable para controlar el estado del canvas
    private Animator animator;
    public float delayBeforeClosing = 1.0f; // Tiempo de retraso antes de cerrar la tienda

    public TextMeshProUGUI precioHP;
    public TextMeshProUGUI precioSpeed;
    public TextMeshProUGUI precioDamage;

    public GameObject conjunto1;
    public GameObject conjunto2;
    public GameObject conjunto3;

    private void Start()
    {
        orbScript = FindObjectOfType<OrbCollector>();
        stats = GetComponent<StatsUpgrade>();
        animator = canvasShop.GetComponent<Animator>();

        canvasShop.SetActive(false);

        // Verifica las compras y desactiva los conjuntos seg�n corresponda
        CheckPurchasesAndDisableConjuntos();

        // Actualiza los textos de precio en el canvas de la tienda al iniciar, usando PlayerPrefs si est�n disponibles
        for (int i = 0; i < shopItems.Count; i++)
        {
            shopItems[i].itemPrice = GetPriceFromPlayerPrefs(i);
        }

        // Actualiza los TextMeshProUGUI con los precios actualizados
        precioHP.text = shopItems[0].itemPrice.ToString();
        precioSpeed.text = shopItems[1].itemPrice.ToString();
        precioDamage.text = shopItems[2].itemPrice.ToString();
    }

    private void Update()
    {
        // Abre o cierra el men� de compra cuando se presiona el bot�n TAB
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
            ShopItem currentItem = shopItems[itemIndex]; // Obtiene el �tem seleccionado

            // Comprueba si el jugador ha comprado el art�culo m�s de 3 veces
            if (currentItem.purchasesCount >= 3)
            {
                Debug.Log("No puedes comprar este �tem m�s de 3 veces.");
                return; // No permite comprar m�s de 3 veces
            }

            // Comprueba si el jugador tiene suficientes orbes para comprar el art�culo
            if (FindObjectOfType<OrbCollector>().SpendOrbs(currentItem.itemPrice))
            {
                // Realiza aqu� las acciones necesarias para dar al jugador el art�culo comprado
                Debug.Log("Has comprado el art�culo.");

                if (currentItem.purchasesCount >= 2)
                {
                    // Desactiva el componente TextMeshProUGUI correspondiente
                    DisableConjunto(itemIndex);
                }

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

                // Incrementa el precio del �tem para la siguiente compra
                currentItem.itemPrice += 50; // Puedes ajustar esto

                // Incrementa el contador de compras para este �tem
                currentItem.purchasesCount++;

                // Actualiza el contador de orbes en el TextMeshPro Text
                orbScript.UpdateOrbText();

                // Actualiza el texto del precio correspondiente
                UpdatePriceText(itemIndex, currentItem.itemPrice);

                // Guarda el valor actualizado en PlayerPrefs
                SavePurchasesCountToPlayerPrefs(itemIndex, currentItem.purchasesCount);

                // Guarda el precio actualizado en PlayerPrefs
                SavePriceToPlayerPrefs(itemIndex, currentItem.itemPrice);

                // Cierra la tienda despu�s de una compra
                StartCoroutine(DelayedCloseShop());
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


    private void CheckPurchasesAndDisableConjuntos()
    {
        foreach (var item in shopItems)
        {
            int itemIndex = shopItems.IndexOf(item);

            // Recupera el valor de purchasesCount de PlayerPrefs
            item.purchasesCount = PlayerPrefs.GetInt($"PurchasesCount{itemIndex}", 0);

            if (item.purchasesCount >= 3)
            {
                // Desactiva el conjunto correspondiente
                DisableConjunto(itemIndex);
            }
        }
    }

    private void SavePurchasesCountToPlayerPrefs(int itemIndex, int purchasesCount)
    {
        // Guarda el valor de purchasesCount en PlayerPrefs
        PlayerPrefs.SetInt($"PurchasesCount{itemIndex}", purchasesCount);
    }

    // Agrega este m�todo para desactivar los conjuntos
    private void DisableConjunto(int itemIndex)
    {
        switch (itemIndex)
        {
            case 0:
                conjunto1.SetActive(false);
                break;
            case 1:
                conjunto2.SetActive(false);
                break;
            case 2:
                conjunto3.SetActive(false);
                break;
            default:
                break;
        }
    }

    // M�todo para actualizar el texto del precio en funci�n del �ndice del art�culo
    private void UpdatePriceText(int itemIndex, int price)
    {
        switch (itemIndex)
        {
            case 0:
                precioHP.text = price.ToString();
                break;
            case 1:
                precioSpeed.text = price.ToString();
                break;
            case 2:
                precioDamage.text = price.ToString();
                break;
            default:
                break;
        }
    }

    private void SavePriceToPlayerPrefs(int itemIndex, int price)
    {
        // Guarda el precio del �tem en PlayerPrefs usando una clave �nica por �tem
        PlayerPrefs.SetInt($"PrecioItem{itemIndex}", price);
    }

    private int GetPriceFromPlayerPrefs(int itemIndex)
    {
        // Recupera el precio del �tem de PlayerPrefs usando la clave �nica
        return PlayerPrefs.GetInt($"PrecioItem{itemIndex}", shopItems[itemIndex].itemPrice);
    }


    private IEnumerator DelayedCloseShop()
    {
        // Espera un tiempo antes de desactivar la tienda
        yield return new WaitForSeconds(delayBeforeClosing);

        // Cierra la tienda
        canvasShop.SetActive(false);
        isShopOpen = false;
    }

    public void ActivarCanvas()
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

[System.Serializable]
public class ShopItem
{
    public int itemPrice; // Precio del �tem
    public int purchasesCount = 0; // Contador de compras realizadas

    // Puedes agregar m�s atributos aqu� seg�n sea necesario para cada �tem
}
