using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaMáxima : MonoBehaviour
{
    private PlayerVida playerVida;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        playerVida = player.GetComponent<PlayerVida>();

        playerVida.SetVidaMaxima();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
