using UnityEngine;
using TMPro;

public class OrbCollector : MonoBehaviour
{
    private int currentSceneOrbs = 0; // Orbes recolectados en la escena actual
    private static int totalOrbs = 0; // Total de orbes recolectados en todas las escenas (estático)
    public TextMeshProUGUI orbText; // Referencia al TextMeshPro Text

    private void Start()
    {
        orbText = GameObject.Find("OrbText").GetComponent<TextMeshProUGUI>();

        LoadTotalOrbs(); // Carga el total de orbes recolectados al iniciar
        UpdateOrbText(); // Actualiza el TextMeshPro Text al iniciar
    }

    // Método para incrementar el contador de orbes en la escena actual
    public void CollectOrb()
    {
        currentSceneOrbs++;
        totalOrbs++;
        UpdateOrbText(); // Actualiza el TextMeshPro Text al recolectar un orbe
        SaveTotalOrbs(); // Guarda el total de orbes después de recolectar
    }

    // Método para obtener el número actual de orbes en la escena actual
    public int GetCurrentSceneOrbs()
    {
        return currentSceneOrbs;
    }

    // Método para obtener el número total de orbes en todas las escenas
    public static int GetTotalOrbs()
    {
        return totalOrbs;
    }

    // Método para actualizar el TextMeshPro Text con el contador actual
    public void UpdateOrbText()
    {
        if (orbText != null)
        {
            orbText.text = totalOrbs.ToString(); // Actualiza el texto con las orbes de la escena actual
        }
    }

    // Método para guardar el total de orbes en PlayerPrefs
    private void SaveTotalOrbs()
    {
        PlayerPrefs.SetInt("TotalOrbs", totalOrbs);
        PlayerPrefs.Save(); // Guarda los cambios en PlayerPrefs
    }

    // Método para cargar el total de orbes desde PlayerPrefs
    private void LoadTotalOrbs()
    {
        if (PlayerPrefs.HasKey("TotalOrbs"))
        {
            totalOrbs = PlayerPrefs.GetInt("TotalOrbs");

            if (totalOrbs <= 0)
            {
                totalOrbs = 0;
            }
        }
    }

    // Puedes llamar a este método cuando el jugador alcance un punto de control o complete la escena
    public void SaveSceneOrbs()
    {
        PlayerPrefs.SetInt("SceneOrbs", currentSceneOrbs);
        PlayerPrefs.Save();
    }

    // Puedes llamar a este método cuando el jugador muera para restar las orbes recolectadas en la escena actual
    public void SubtractSceneOrbs()
    {
        totalOrbs -= currentSceneOrbs;
        SaveTotalOrbs();
        PlayerPrefs.DeleteKey("SceneOrbs");
    }

    // Método para gastar orbes
    public bool SpendOrbs(int amount)
    {
        if (totalOrbs >= amount)
        {
            totalOrbs -= amount;
            SaveTotalOrbs();
            return true; // Devuelve verdadero si el jugador tenía suficientes orbes para gastar
        }
        else
        {
            return false; // Devuelve falso si el jugador no tenía suficientes orbes
        }
    }
}
