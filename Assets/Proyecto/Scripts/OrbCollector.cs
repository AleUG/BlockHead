using UnityEngine;
using TMPro;

public class OrbCollector : MonoBehaviour
{
    private int orbCount = 0; // Contador de orbes
    public TextMeshProUGUI orbText; // Referencia al TextMeshPro Text

    private void Start()
    {
        UpdateOrbText(); // Actualiza el TextMeshPro Text al iniciar
    }

    // Método para incrementar el contador de orbes
    public void CollectOrb()
    {
        orbCount++;
        UpdateOrbText(); // Actualiza el TextMeshPro Text al recolectar un orbe

        // Puedes agregar aquí cualquier efecto o sonido cuando se recolecta un orbe
        // Por ejemplo: AudioSource.PlayClipAtPoint(sonidoOrbe, transform.position);
    }

    // Método para obtener el número actual de orbes
    public int GetOrbCount()
    {
        return orbCount;
    }

    public bool SpendOrbs(int amount)
    {
        if (orbCount >= amount)
        {
            orbCount -= amount;
            return true; // Devuelve verdadero si el jugador tenía suficientes orbes para gastar
        }
        else
        {
            return false; // Devuelve falso si el jugador no tenía suficientes orbes
        }
    }

    // Método para actualizar el TextMeshPro Text con el contador actual
    public void UpdateOrbText()
    {
        if (orbText != null)
        {
            orbText.text = orbCount.ToString(); // Actualiza el texto
        }
    }

    // Puedes llamar a este método para reiniciar el contador de orbes
    public void ResetOrbCount()
    {
        orbCount = 0;
        UpdateOrbText(); // Actualiza el TextMeshPro Text al reiniciar
    }
}
