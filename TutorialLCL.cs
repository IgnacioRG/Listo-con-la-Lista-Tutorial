using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialLCL : MonoBehaviour
{
    //Gameobjects de tipo UI
    public GameObject paso;
    public GameObject explicacion;

    //Gameobjects de tipo boton
    public GameObject sig_boton;
    public GameObject fin_boton;

    //Sprites de explicacion.
    public List<Sprite> lore1_lsp;
    public List<Sprite> lore2_lsp;
    public List<Sprite> mecanica1_lsp;
    public List<Sprite> mecanica2_lsp;
    public Sprite victoria_sp;
    public Sprite derrota_sp;

    private bool _sig;

    //Inicializamos algunos parametros y desactivamos el boton final.
    void Start()
    {
        _sig = false;
        fin_boton.SetActive(false);

        StartCoroutine(TutorialFlujo());
    }

    /**
     * SiguientePaso se llama cada vez que el jugador termina de leer la instruccion
     * actual (boton siguiente).
     */
    public void SiguientePaso()
    {
        _sig = true;
    }

    /**
     * BotonFinal desactiva el boton siguiente y activa el boton final para terminar el tutorial.
     */
    public void BotonFinal()
    {
        sig_boton.SetActive(false);
        fin_boton.SetActive(true);
    }

    /**
     * Metodo para volver al menu.
     */
    public void VolverPrincipal()
    {
        SceneManager.LoadScene("MenuListoConLaLista");
    }

    /**
     * Flujo normal del tutorial en el que se explica en 4 pasos las mecanicas del juego.
     * 1. Mecanica principal de memorizacion.
     * 2. Mecanica de Borrar y Distracciones.
     * 3. Condiciones de Victoria.
     * 4. Condiciones de Derrota.
     */
    IEnumerator TutorialFlujo()
    {
        explicacion.GetComponent<Text>().text = "¡VRain se ha quedado sin despensa!";
        while (!_sig)
        {
            foreach (Sprite sp in lore1_lsp)
            {
                paso.GetComponent<Image>().sprite = sp;
                yield return new WaitForSeconds(1);
            }
            yield return null;
        }
        _sig = false;

        explicacion.GetComponent<Text>().text = "Necesita ir al Supermercado™ (el de la vaquita) a comprar justo lo que necesita.";
        while (!_sig)
        {
            foreach (Sprite sp in lore2_lsp)
            {
                paso.GetComponent<Image>().sprite = sp;
                yield return new WaitForSeconds(1);
            }
            yield return null;
        }
        _sig = false;

        explicacion.GetComponent<Text>().text = "Memoriza cuáles artículos debes comprar (resaltados en una aura blanca).";
        while (!_sig)
        {
            foreach (Sprite sp in mecanica1_lsp)
            {
                paso.GetComponent<Image>().sprite = sp;
                yield return new WaitForSeconds(1);
            }
            yield return null;
        }
        _sig = false;

        explicacion.GetComponent<Text>().text = "Compra solo los artículos que necesitas en el Supermercado™ (el de la vaquita).";
        while (!_sig)
        {
            foreach (Sprite sp in mecanica2_lsp)
            {
                paso.GetComponent<Image>().sprite = sp;
                yield return new WaitForSeconds(1);
            }
            yield return null;
        }
        _sig = false;

        paso.GetComponent<Image>().sprite = victoria_sp;
        explicacion.GetComponent<Text>().text = "¡Consigue superar tres fases seguidas y sube de nivel!";
        while (!_sig)
        {
            yield return null;
        }
        BotonFinal();

        paso.GetComponent<Image>().sprite = derrota_sp;
        explicacion.GetComponent<Text>().text = "¡Cuidado! Si pierdes tres veces seguidas, VRain no tendrá bien su despensa y bajarás de nivel.";
    }
}
