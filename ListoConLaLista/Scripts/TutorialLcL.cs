using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialLcL : MonoBehaviour
{
    //Gameobjects de la UI
    public GameObject paso_img;
    public GameObject explicacion_txt;

    //Gameobjects de tipo boton
    public GameObject sig_boton;

    //Imagenes para la explicacion tutorial.
    public List<Sprite> lore1_lsp;
    public List<Sprite> lore2_lsp;
    public List<Sprite> mecanica1_lsp;
    public List<Sprite> mecanica2_lsp;
    public Sprite victoria_sp;
    public Sprite derrota_sp;

    //bandera de pasos.
    private bool _sig;

    //Inicializacion de atributos.
    private void Start()
    {
        _sig = false;
        sig_boton.SetActive(true);
        sig_boton.GetComponent<Button>().onClick.RemoveAllListeners();
        sig_boton.GetComponent <Button>().onClick.AddListener(gameObject.GetComponent<TutorialLcL>().SiguientePaso);
        sig_boton.transform.GetChild(0).GetComponent<Text>().text = "Siguiente";

        StartCoroutine(TutorialFlujo());
    }

    /**
     * Metodo que cambia el estado del bool sig para desplegar el siguiente paso del tutorial.
     */
    public void SiguientePaso()
    {
        _sig = true;
    }

    /**
     * Metodo para volver al menu.
     */
    public void VolverAlMenu()
    {
        SceneManager.LoadScene("MenuListoConLaLista");
    }

    /**
     * Corrutina que implementa el flujo del tutorial explicacion del juego en el que
     * cambiamos los sprites que ilustran el punto y una breve descripcion de una mecanica,
     * trama, etc.
     *  1. Lore (VRain casa)
     *  2. Lore (VRain en el supermercado)
     *  3. Mecanica de memorizacion (fase 1)
     *  4. Mecanica de memorizacion (fase 2)
     *  5. Condiciones de Victoria
     *  6. Condiciones de Derrota
     */
    IEnumerator TutorialFlujo()
    {
        explicacion_txt.GetComponent<Text>().text = "¡VRain se ha quedado sin cosas en su despensa!";
        while (!_sig)
        {
            foreach (Sprite sp in lore1_lsp)
            {
                paso_img.GetComponent<Image>().sprite = sp;
                yield return new WaitForSeconds(1);
            }
        }
        _sig = false;

        explicacion_txt.GetComponent<Text>().text = "Para comprar lo necesario, se dirige al Supermercado™ (el de la vaquita)";
        while (!_sig)
        {
            foreach (Sprite sp in lore2_lsp)
            {
                paso_img.GetComponent<Image>().sprite = sp;
                yield return new WaitForSeconds(1);
            }
        }
        _sig = false;

        explicacion_txt.GetComponent<Text>().text = "Memoriza cuáles artículos debes comprar (resaltados en una aura blanca).";
        while (!_sig)
        {
            foreach (Sprite sp in mecanica1_lsp)
            {
                paso_img.GetComponent<Image>().sprite = sp;
                yield return new WaitForSeconds(1);
            }
        }
        _sig = false;

        explicacion_txt.GetComponent<Text>().text = "Compra solo los artículos que VRain necesita en el Supermercado™ (el de la vaquita).";
        while (!_sig)
        {
            foreach (Sprite sp in mecanica2_lsp)
            {
                paso_img.GetComponent<Image>().sprite = sp;
                yield return new WaitForSeconds(1);
            }
        }
        _sig = false;

        explicacion_txt.GetComponent<Text>().text = "¡Acierta tres fases seguidas y sube de nivel!";
        paso_img.GetComponent<Image>().sprite = victoria_sp;
        while (!_sig)
        {
            yield return null;
        }

        explicacion_txt.GetComponent<Text>().text = "¡Cuidado! Si pierdes tres veces seguidas, VRain no tendrá bien su despensa y bajarás de nivel.";
        paso_img.GetComponent<Image>().sprite = derrota_sp;

        sig_boton.GetComponent<Button>().onClick.RemoveAllListeners();
        sig_boton.GetComponent<Button>().onClick.AddListener(gameObject.GetComponent<TutorialLcL>().VolverAlMenu);
        sig_boton.transform.GetChild(0).GetComponent<Text>().text = "Comenzar";
    }
}
