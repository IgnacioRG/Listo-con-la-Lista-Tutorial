using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System;

public class Juego : MonoBehaviour
{
    public Text contadorNivel, textoTienda, textoTiempo, contadorAciertos, contadorFallos;    
    public Text avisos;
    public GameObject objetosCasa, objetosTienda, panelAvisos, panelMensaje, panelEstacionamiento, panelTiempoTerminado, panelRespuesta;
    public GameObject panelAcierto, panelAvanzaNivel, panelError, panelRegresaNivel;
    public float tiempoVisualizacion;
    public int elegirNivel;    

    [SerializeField] Button _continuar, _salir, _botonPausa, _finalizar;

    [SerializeField] Button _empezarEtapa2;

    [SerializeField] private GameObject _menuPausa;
    
    public GameObject[] productos;
    public GameObject[] platosAlacena;

    //En caso de que el tiempo se acabe (se puede optimizar)
    public GameObject[] posicionesRespuesta;

    //En caso de que usuario se equivoco
    public GameObject[] posicionesRespuestaEsperada;

    
    public GameObject[] platosEstante;
    public GameObject contenedorObjetosAlacena;

    public Button[] posicionesEstanteria;

    public AudioClip selected_audioclip;
    public AudioClip pasaNivel_audioclip;
    public AudioClip bajaNivel_audioclip;
    public AudioClip victoria_audioclip;
    public AudioClip derrota_audioclip;
    public AudioSource bocina;

    [SerializeField]
    int _NeurocoinsRecibidasAlGanar = 5;
    [SerializeField]
    int _NeurocoinsRecibidasAlSubirDeNivel = 20;

    public GameObject mascota;
    public GameObject coins_panel;
    public Text coins_text;

    bool pausa;
    private int _nivel;

    private int _aciertos = 0;

    private int _errores = 0;

    private float _tiempoRestante = 10;

    private bool _activarTemporizador = false;

    private bool _etapa1 = false;
    private bool _etapa3 = false;

    private Image _imagenesPlatos;
    
    System.Random _selectorDeProductos = new System.Random();
    System.Random _selectorDePosiciones = new System.Random();

    private int[] _productosEstanteria = new int[15];

    private List<int> _listaSeleccionProductos = new List<int>();
    
    private List<int> _listaSeleccionPosiciones = new List<int>();
    
    private List<int> _productosSeleccionados = new List<int>();
    
    private List<int> _distractores = new List<int>();

    private List<int> _respuestaJugador = new List<int>(); 

    private Dictionary<Button, int> _posicionesElegidas = new Dictionary<Button, int>(); 
    
    private void Awake()
    {
        if (elegirNivel <= 0 || elegirNivel > 10)

            _nivel = 1;
        
        else

            _nivel = elegirNivel;

        _posicionesElegidas.Add(posicionesEstanteria[0],0);
        _posicionesElegidas.Add(posicionesEstanteria[1],1);
        _posicionesElegidas.Add(posicionesEstanteria[2],2);
        _posicionesElegidas.Add(posicionesEstanteria[3],3);
        _posicionesElegidas.Add(posicionesEstanteria[4],4);
        _posicionesElegidas.Add(posicionesEstanteria[5],5);
        _posicionesElegidas.Add(posicionesEstanteria[6],6);
        _posicionesElegidas.Add(posicionesEstanteria[7],7);
        _posicionesElegidas.Add(posicionesEstanteria[8],8);
        _posicionesElegidas.Add(posicionesEstanteria[9],9);
        _posicionesElegidas.Add(posicionesEstanteria[10],10);
        _posicionesElegidas.Add(posicionesEstanteria[11],11);
        _posicionesElegidas.Add(posicionesEstanteria[12],12);
        _posicionesElegidas.Add(posicionesEstanteria[13],13);
        _posicionesElegidas.Add(posicionesEstanteria[14],14);

    }

    // Start is called before the first frame update
    void Start()
    {
    
        contadorNivel.text = "NIVEL: " + _nivel.ToString();
        contadorAciertos.text = "ACIERTOS: \n" + _aciertos.ToString() + "/3";
        contadorFallos.text = "FALLOS: \n" + _errores.ToString() + "/3";
        textoTienda.text = "Articulos seleccionados: ";
        textoTiempo.text = _tiempoRestante.ToString();

        contadorNivel.gameObject.SetActive(true);
        textoTienda.gameObject.SetActive(false);
        textoTiempo.gameObject.SetActive(true);
        avisos.gameObject.SetActive(false);
        
        panelAvisos.SetActive(true);
        panelMensaje.SetActive(false);
        panelEstacionamiento.SetActive(false);
        panelTiempoTerminado.SetActive(false);
        objetosCasa.SetActive(true);
        objetosTienda.SetActive(false);

        _finalizar.enabled = false;

        pausa = false;
        ActivadorDeBotones();
        StartCoroutine(GenerarNuevoNivel());
    }

    //Se utiliza Update cuando se necesita utilizar un tiempo de espera

    void Update (){

        if (_activarTemporizador == false)
        {
            return;
        }

        if (_tiempoRestante > 0){
            _tiempoRestante -= Time.deltaTime;
            textoTiempo.text = Mathf.RoundToInt(_tiempoRestante).ToString();
        }
        else{
            _activarTemporizador = false;
            if (_etapa3){
                StartCoroutine(MostrarTiempoTerminado());
            }
            else if(_etapa1){
                StartCoroutine(EmpezarEtapa2());
            }
        }
        

    }

    private bool ComparaListas(List<int> respuestaCorrecta, List<int> respuestaJugador)
    {
        bool iguales = true;
        int _indice = 0;
        if (respuestaJugador.Count != respuestaCorrecta.Count)
            return false;
        else{
            respuestaCorrecta.Sort();
            respuestaJugador.Sort();            
            do
            {
                if (respuestaCorrecta[_indice] != respuestaJugador[_indice])
                
                    iguales = false;

                _indice++;

            } while (_indice < respuestaCorrecta.Count && iguales);
        }
        return iguales;
    }

    //Metodo para regresar el juego al estado cuando se inicia un nivel
    private void IniciarNuevoNivel(){

        contadorNivel.text = "NIVEL: " + _nivel.ToString();
        contadorAciertos.text = "ACIERTOS: \n" + _aciertos.ToString() + "/3";
        contadorFallos.text = "FALLOS: \n" + _errores.ToString() + "/3";

        contadorNivel.gameObject.SetActive(true);
        textoTienda.gameObject.SetActive(false);
        textoTiempo.gameObject.SetActive(true);
        avisos.gameObject.SetActive(false);
        
        panelAvisos.SetActive(true);
        panelMensaje.SetActive(false);
        panelEstacionamiento.SetActive(false);
        panelTiempoTerminado.SetActive(false);
        objetosCasa.SetActive(true);
        objetosTienda.SetActive(false);

        _finalizar.enabled = false;

        pausa = false;
        _etapa3 = false;
        _respuestaJugador.Clear();
        _productosSeleccionados.Clear();
        _distractores.Clear();
        StartCoroutine(GenerarNuevoNivel());

    }


    //Metodo para elegir aleatoriamente productos y sus posiciones en la
    //alacena
    private void ElegirProductosYPosiciones_Casa(int repeticiones, int numeroDistractores){

        foreach (GameObject plato in platosAlacena)
        {
            _imagenesPlatos = plato.GetComponent<Image>();
            _imagenesPlatos.enabled = true;
        }

        for (int _i = 0;  _i < repeticiones; _i++){

            int _temporalProductos = _selectorDeProductos.Next(0, _listaSeleccionProductos.Count);
            _productosSeleccionados.Add(_listaSeleccionProductos[_temporalProductos]);
            

            int _temporalPosiciones = _selectorDePosiciones.Next(0, _listaSeleccionPosiciones.Count);
            productos[_listaSeleccionProductos[_temporalProductos]].SetActive(true);
            platosAlacena[_listaSeleccionPosiciones[_temporalPosiciones]].SetActive(true);
            productos[_listaSeleccionProductos[_temporalProductos]].transform.SetParent(platosAlacena[_listaSeleccionPosiciones[_temporalPosiciones]].transform);
            productos[_listaSeleccionProductos[_temporalProductos]].GetComponent<RectTransform>().anchoredPosition = new Vector3(0f,20f,0f);
            productos[_listaSeleccionProductos[_temporalProductos]].transform.SetParent(contenedorObjetosAlacena.transform);
            _listaSeleccionProductos.RemoveAt(_temporalProductos);
            _listaSeleccionPosiciones.RemoveAt(_temporalPosiciones);

        }

        if (numeroDistractores > 0){

            for (int _i = 0;  _i < numeroDistractores; _i++){

                int _temporalProductos = _selectorDeProductos.Next(0, _listaSeleccionProductos.Count);
                _distractores.Add(_listaSeleccionProductos[_temporalProductos]);

                 int _temporalPosiciones = _selectorDePosiciones.Next(0, _listaSeleccionPosiciones.Count);
                productos[_listaSeleccionProductos[_temporalProductos]].SetActive(true);
                platosAlacena[_listaSeleccionPosiciones[_temporalPosiciones]].SetActive(true);
                _imagenesPlatos = platosAlacena[_listaSeleccionPosiciones[_temporalPosiciones]].GetComponent<Image>();
                _imagenesPlatos.enabled = false;
                productos[_listaSeleccionProductos[_temporalProductos]].transform.SetParent(platosAlacena[_listaSeleccionPosiciones[_temporalPosiciones]].transform);
                productos[_listaSeleccionProductos[_temporalProductos]].GetComponent<RectTransform>().anchoredPosition = new Vector3(0f,20f,0f);
                productos[_listaSeleccionProductos[_temporalProductos]].transform.SetParent(contenedorObjetosAlacena.transform);
                _listaSeleccionProductos.RemoveAt(_temporalProductos);
                _listaSeleccionPosiciones.RemoveAt(_temporalPosiciones);

            
            }

        }

    }

    //Metodo para elegir aleatoriamente productos y sus posiciones en el
    //estante del supermercado
    private void ElegirProductosYPosiciones_Supermercado(){
        
        _listaSeleccionPosiciones.Clear();
        for (int _i = 0;  _i < 15; _i++)

                _listaSeleccionPosiciones.Add(_i);

        foreach (int producto in _productosSeleccionados){

            int _temporalPosiciones = _selectorDePosiciones.Next(0, _listaSeleccionPosiciones.Count);
            productos[producto].SetActive(true);
            platosEstante[_listaSeleccionPosiciones[_temporalPosiciones]].SetActive(true);
            productos[producto].transform.SetParent(platosEstante[_listaSeleccionPosiciones[_temporalPosiciones]].transform);
            productos[producto].GetComponent<RectTransform>().anchoredPosition = new Vector3(0f,20f,0f);
            productos[producto].transform.SetParent(contenedorObjetosAlacena.transform);
            _productosEstanteria[_listaSeleccionPosiciones[_temporalPosiciones]] = producto;
            _listaSeleccionPosiciones.RemoveAt(_temporalPosiciones);

        }
       
        if (_distractores.Count > 0){

            foreach (int distractor in _distractores){

                int _temporalPosiciones = _selectorDePosiciones.Next(0, _listaSeleccionPosiciones.Count);
                productos[distractor].SetActive(true);
                platosEstante[_listaSeleccionPosiciones[_temporalPosiciones]].SetActive(true);
                productos[distractor].transform.SetParent(platosEstante[_listaSeleccionPosiciones[_temporalPosiciones]].transform);
                productos[distractor].GetComponent<RectTransform>().anchoredPosition = new Vector3(0f,20f,0f);
                productos[distractor].transform.SetParent(contenedorObjetosAlacena.transform);
                _productosEstanteria[_listaSeleccionPosiciones[_temporalPosiciones]] = distractor;
                _listaSeleccionPosiciones.RemoveAt(_temporalPosiciones);

            }

        }

        //Revisar productos que ya estan en la estanteria
        foreach (int posicion in _listaSeleccionPosiciones){

            int _temporalProductos = _selectorDeProductos.Next(0, _listaSeleccionProductos.Count);
            productos[_listaSeleccionProductos[_temporalProductos]].SetActive(true);
            platosEstante[posicion].SetActive(true);
            productos[_listaSeleccionProductos[_temporalProductos]].transform.SetParent(platosEstante[posicion].transform);
            productos[_listaSeleccionProductos[_temporalProductos]].GetComponent<RectTransform>().anchoredPosition = new Vector3(0f,20f,0f);
            productos[_listaSeleccionProductos[_temporalProductos]].transform.SetParent(contenedorObjetosAlacena.transform);
             _productosEstanteria[posicion] = _listaSeleccionProductos[_temporalProductos];
            _listaSeleccionProductos.RemoveAt(_temporalProductos);
            
        }

    }

    //Metodo para que se presenten los productos a memorizar
    IEnumerator GenerarNuevoNivel(){

        contadorAciertos.text = "ACIERTOS: \n" + _aciertos.ToString() + "/3";
        contadorFallos.text = "FALLOS: \n" + _errores.ToString() + "/3";
        _listaSeleccionPosiciones.Clear();
        _listaSeleccionProductos.Clear();
        for (int _i = 0;  _i < 15; _i++)
                _listaSeleccionPosiciones.Add(_i);
        for (int _i = 0;  _i < productos.Length; _i++)
                _listaSeleccionProductos.Add(_i);
        _productosSeleccionados.Clear();
        _tiempoRestante = 120;
        _etapa1 = true;
        _empezarEtapa2.enabled = true;

        //Ajustes realizados por nivel
        switch(_nivel){
            case 1:
                ElegirProductosYPosiciones_Casa(2, 0);
            break;
            case 2:            
                ElegirProductosYPosiciones_Casa(3, 0);

            break;
            case 3:
            
                ElegirProductosYPosiciones_Casa(2, 2);

            break;
            case 4:

                ElegirProductosYPosiciones_Casa(3, 2);

            break;
            case 5:
            
                ElegirProductosYPosiciones_Casa(4, 3);

            break;
            case 6:
            
                ElegirProductosYPosiciones_Casa(5, 2);

            break;
            case 7:
            
                ElegirProductosYPosiciones_Casa(6, 3);

            break;
            case 8:
            
                ElegirProductosYPosiciones_Casa(6, 4);

            break;
            case 9:
            
                ElegirProductosYPosiciones_Casa(7, 5);

            break;
            case 10:
            
                ElegirProductosYPosiciones_Casa(7, 6);

            break;

            default:

                Debug.Log("Nivel inexistente");

            break;
        }
        textoTiempo.text = _tiempoRestante.ToString();

        //Comienza etapa 1
        panelAvisos.SetActive(true);
        yield return new WaitForSeconds(3f);
        panelAvisos.SetActive(false);
        avisos.gameObject.SetActive(false);
        _activarTemporizador = true;
        
    }

    //Metodo para pasar a etapa2 cuando se presiona boton de continuar
    //o se termina el tiempo de memorizar productos de la alacena
    IEnumerator EmpezarEtapa2(){

        _etapa1 = false;
        _empezarEtapa2.enabled = false;
        foreach (int producto in _productosSeleccionados){

            productos[producto].SetActive(false);
        }
        foreach (int producto in _distractores){

            productos[producto].SetActive(false);
        }

        foreach (GameObject plato in platosAlacena)

            plato.SetActive(false);
        
        objetosCasa.SetActive(false);

        //Se cambia a etapa 2
        panelMensaje.SetActive(true);
        avisos.gameObject.SetActive(true);
        avisos.text = "¡HORA DE IR AL SUPERMERCADO!";
        panelEstacionamiento.SetActive(true);
        yield return new WaitForSeconds(5f);
        avisos.gameObject.SetActive(false);
        panelMensaje.SetActive(false);
        panelEstacionamiento.SetActive(false);
        //Se cambia a etapa 3
        objetosTienda.SetActive(true);
        _tiempoRestante = 120;

        if (_nivel < 6){
            
            textoTienda.text = "Articulos seleccionados: " + _respuestaJugador.Count + "/" + _productosSeleccionados.Count;
            textoTienda.gameObject.SetActive(true);        
        }
        else
            textoTienda.gameObject.SetActive(false);

        ElegirProductosYPosiciones_Supermercado();
        _activarTemporizador = true;
        _etapa3 = true;
        foreach (Button botonesEstante in posicionesEstanteria)
            botonesEstante.enabled = true;

    }

    //Metodo para mostrar que se acabo el tiempo sin que el usuario terminara
    IEnumerator MostrarTiempoTerminado(){
        GuardaPartida(false, true);
        panelTiempoTerminado.SetActive(true);
        foreach (GameObject producto in productos)
            producto.SetActive(false);

        int coordenada = 0;

        foreach (int producto in _productosSeleccionados){

            productos[producto].SetActive(true);
            productos[producto].transform.SetParent(posicionesRespuesta[coordenada].transform);
            productos[producto].GetComponent<RectTransform>().anchoredPosition = new Vector3(0f,0f,0f);
            productos[producto].transform.SetParent(contenedorObjetosAlacena.transform);
            coordenada++;

       }
        yield return new WaitForSeconds(5f);
        
        foreach (GameObject producto in productos)
            producto.SetActive(false);

        panelTiempoTerminado.SetActive(false);
        _aciertos = 0;
        _errores++;

        if (_errores >= 3)
        {
            if (_nivel != 1)
                _nivel--;
            else
                _nivel = 1;
            bocina.clip = bajaNivel_audioclip;
            bocina.Play();
            panelRegresaNivel.gameObject.SetActive(true);
            yield return new WaitForSeconds(5f);
            panelRegresaNivel.gameObject.SetActive(false);
            _errores = 0;
            contadorNivel.text = _nivel.ToString();
        }
        else {
            bocina.clip = derrota_audioclip;
            bocina.Play();
        }


        foreach (GameObject plato in platosEstante){

            _imagenesPlatos = plato.GetComponent<Image>();
            _imagenesPlatos.enabled = false;
        }

        IniciarNuevoNivel();


    }

    //Metodo cuando se tuvo una respuesta correcta
    IEnumerator AumentarAciertos(){
        GuardaPartida(true,false);
         if (_aciertos < 3)
        {
            bocina.clip = victoria_audioclip;
            bocina.Play();
            mascota.GetComponent<Animator>().Rebind();
            mascota.GetComponent<Animator>().SetInteger("Estado", 1); //aplaudiendo
            coins_panel.SetActive(true);
            coins_text.text = "¡+" + _NeurocoinsRecibidasAlGanar + " Neurocoins!";
            ActualizaNeurocoins(_NeurocoinsRecibidasAlGanar);
            panelAcierto.gameObject.SetActive(true);
            yield return new WaitForSeconds(5f);
            coins_panel.SetActive(false);
            panelAcierto.gameObject.SetActive(false);

        }
        else
        {

            if (_nivel != 10)

                _nivel++;

            else

                _nivel = 10;
            bocina.clip = pasaNivel_audioclip;
            bocina.Play();
            mascota.GetComponent<Animator>().Rebind();
            mascota.GetComponent<Animator>().SetInteger("Estado", 0); //celebrando
            coins_panel.SetActive(true);
            coins_text.text = "¡+" + _NeurocoinsRecibidasAlSubirDeNivel + " Neurocoins!";
            ActualizaNeurocoins(_NeurocoinsRecibidasAlSubirDeNivel);
            panelAvanzaNivel.gameObject.SetActive(true);
            yield return new WaitForSeconds(5f);
            coins_panel.SetActive(false);
            panelAvanzaNivel.gameObject.SetActive(false);
            contadorNivel.text = _nivel.ToString();
            _aciertos = 0;
        
        }
        IniciarNuevoNivel();

    }

    //Metodo cuando se tuvo una respuesta incorrecta
    IEnumerator AumentarErrores(){
        GuardaPartida(false,false);
        panelError.gameObject.SetActive(true);
        bocina.clip = derrota_audioclip;
        bocina.Play();
        yield return new WaitForSeconds(2f);
        panelError.gameObject.SetActive(false);
        panelRespuesta.SetActive(true);
        foreach (GameObject producto in productos)
            producto.SetActive(false);
        int coordenada = 0;
        foreach (int producto in _productosSeleccionados){
            productos[producto].SetActive(true);
            productos[producto].transform.SetParent(posicionesRespuestaEsperada[coordenada].transform);
            productos[producto].GetComponent<RectTransform>().anchoredPosition = new Vector3(0f,0f,0f);
            productos[producto].transform.SetParent(contenedorObjetosAlacena.transform);
            coordenada++;
        }
        yield return new WaitForSeconds(5f);        
        foreach (GameObject producto in productos)
            producto.SetActive(false);
        panelRespuesta.SetActive(false);    
        if (_errores >= 3)
        {
            if (_nivel != 1)
                _nivel--;
            else
                _nivel = 1;
            bocina.clip = bajaNivel_audioclip;
            bocina.Play();
            mascota.GetComponent<Animator>().Rebind();
            mascota.GetComponent<Animator>().SetInteger("Estado", 2); //triste
            coins_panel.SetActive(true);
            coins_text.text = "¡No te rindas!";
            panelRegresaNivel.gameObject.SetActive(true);
            yield return new WaitForSeconds(5f);
            coins_panel.SetActive(false);
            panelRegresaNivel.gameObject.SetActive(false);
            contadorNivel.text = _nivel.ToString();
            _errores = 0;
        
        }
        IniciarNuevoNivel();

    }

    void ActivadorDeBotones(){

        _botonPausa.onClick.AddListener(() => Pausar());
        _continuar.onClick.AddListener(() => ContinuarJuego());
        _salir.onClick.AddListener(() => SalirDeJuego());
        _finalizar.onClick.AddListener(() => RevisarRespuesta());
        _empezarEtapa2.onClick.AddListener(() => TerminarEtapa1());

        foreach (Button boton in posicionesEstanteria){

            boton.onClick.AddListener(() => ElegirProductoRespuesta(boton));
        }

    }

    //Metodo para terminar la etapa1 al presionar el boton de contiuar
    void TerminarEtapa1(){
        _tiempoRestante = 0;
        textoTiempo.text = "0";
    }


    //Metodo para revisar la respuesta del usuario al interactuar
    //con el boton de finalizar
    void RevisarRespuesta(){
        bocina.pitch = 1.0f;
        _finalizar.enabled = false;
        foreach (Button botonesEstante in posicionesEstanteria)
            botonesEstante.enabled = false;

        foreach (GameObject producto in productos)
            producto.SetActive(false);

        foreach (GameObject plato in platosEstante){

            _imagenesPlatos = plato.GetComponent<Image>();
            _imagenesPlatos.enabled = false;
        }

        _activarTemporizador = false;
        if (ComparaListas(_productosSeleccionados,_respuestaJugador)){

            _aciertos++;
            _errores = 0;
            StartCoroutine(AumentarAciertos());
        }
        else{

            _errores++;
            _aciertos = 0;
            StartCoroutine(AumentarErrores());
        }

    }

    //Metodo para que el usuario seleccione productos del estante del
    //supermercado como respuesta
    void ElegirProductoRespuesta(Button posicionElegida){
        bocina.clip = selected_audioclip;
        
        _imagenesPlatos = platosEstante[_posicionesElegidas[posicionElegida]].GetComponent<Image> ();
        if (_imagenesPlatos.IsActive()){
            bocina.pitch = 0.7f;
            _imagenesPlatos.enabled = false;
            _respuestaJugador.Remove(_productosEstanteria[_posicionesElegidas[posicionElegida]]);

            if (_respuestaJugador.Count == 0)

                _finalizar.enabled = false;
        }
        else{
            bocina.pitch = 1.0f;
            _imagenesPlatos.enabled = true;
            _respuestaJugador.Add(_productosEstanteria[_posicionesElegidas[posicionElegida]]);
            _finalizar.enabled = true;
        }
        bocina.Play();
        textoTienda.text = "Articulos seleccionados: " + _respuestaJugador.Count + "/" + _productosSeleccionados.Count;

    }

     //Metodo para salir de juego en pausa
    public void SalirDeJuego()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MenuListoConLaLista");

    }

    //Metodo para abrir menu de pausa
    public void Pausar()
    {

        Time.timeScale = 0f;
        _menuPausa.SetActive(true);
        _botonPausa.enabled = false;

    }

    //Metodo para cerrar menu de pausa
    public void ContinuarJuego()
    {

        Time.timeScale = 1.0f;
        _menuPausa.SetActive(false);
        _botonPausa.enabled = true;

    }

    public void GuardaPartida(bool victoria, bool agoto_tiempo)
    {
        Partida p = new Partida();
        p.nivel = _nivel;
        p.juego = "Listo con la Lista";
        p.fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        p.victoria = victoria;
        p.agoto_tiempo = agoto_tiempo;
        StartCoroutine(AduanaCITAN.SubePartidaA_CITAN(p));
    }

    public void ActualizaNeurocoins(int coins)
    {
        StartCoroutine(AduanaCITAN.ActualizaNeurocoins_CITAN(coins));
    }

}
