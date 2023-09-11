using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public string levelToLoad;

    private void OnMouseDown()
    {
        // Carga el nivel correspondiente cuando el jugador haga clic en el elemento de selección
        SceneManager.LoadScene(levelToLoad);
    }
}
