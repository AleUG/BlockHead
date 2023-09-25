using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void DeletePlayerPrefs()
    {
        // Limpia PlayerPrefs al iniciar una nueva partida
        PlayerPrefs.DeleteAll();
    }
}

