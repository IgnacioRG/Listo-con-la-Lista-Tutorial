using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuListoLista : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("JuegoListoConLaLista");
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("TutorialListoConLaLista");
    }

    public void Salir()
    {
        SceneManager.LoadScene("Menu Juegos");
    }
}
