using System.Collections;
using UnityEngine;

public class ActivarObjetos : MonoBehaviour
{
    public GameObject boxColliderChangeScene;

    private Animator animatorManos;
    public GameObject[] manosAtaque; // Cambiamos manosAtaque a un arreglo de GameObjects
    private int currentIndex = 0; // Índice actual del arreglo

    // Start is called before the first frame update
    void Start()
    {
        foreach (var mano in manosAtaque)
        {
            mano.SetActive(false);
        }

        animatorManos = manosAtaque[currentIndex].GetComponent<Animator>();
        boxColliderChangeScene.SetActive(false);
    }

    public void ActivarObjetoCollider()
    {
        boxColliderChangeScene.SetActive(true);
    }

    public void ActivarAtaque()
    {
        if (currentIndex < manosAtaque.Length)
        {
            manosAtaque[currentIndex].SetActive(true);
            StartCoroutine(DesactivarManos());
        }
    }

    private IEnumerator DesactivarManos()
    {
        yield return new WaitForSeconds(10f);

        animatorManos.SetTrigger("End");

        yield return new WaitForSeconds(1.5f);
        manosAtaque[currentIndex].SetActive(false);

        currentIndex++; // Incrementar el índice para el próximo objeto

        if (currentIndex >= manosAtaque.Length)
        {
            currentIndex = 0; // Reiniciar el índice si llegamos al final del arreglo
        }
    }
}
