using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuListoLista : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("Demo_Memoria");
    }

    public void Salir()
    {
        SceneManager.LoadScene("Menu Juegos");
    }
}
