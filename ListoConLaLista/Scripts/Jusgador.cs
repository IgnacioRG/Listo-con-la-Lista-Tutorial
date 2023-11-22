using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JuegoMemoriaSupermercado
{
    public class Jusgador : MonoBehaviour
    {
        enum EstadoJugador { Vivo, Muerto, Corriendo, Disparando, Agachado, Saltando, Recargando, Cubriendose }
        
        public int vida;
        private int _num_armas;
        EstadoJugador _miEstado;
        [SerializeField]
        int _balas = 5;
        bool _corutinaEjecutandose = false;

        public EnemigoE _miEnemigo;


        private void OnEnable()
        {
            EnemigoE.RevelaVida += ImprimeVidaEnemigo;
        }

        private void OnDisable()
        {
            EnemigoE.RevelaVida -= ImprimeVidaEnemigo;
        }

        void ImprimeVidaEnemigo(int _vidaEnemigoEvento)
        {
            Debug.Log("Vida Enemigo" + _vidaEnemigoEvento);
        }

        private void Awake()
        {
            _miEnemigo = GameObject.Find("Enemigo").GetComponent<EnemigoE>();
            Debug.Log("Dispara");
            _balas = _balas - 1;
            StartCoroutine(RecargaDisparo());
            Debug.Log("¿Cuántas balas tengo?" + _balas);
            Debug.Log("Primero");
        }

        IEnumerator RecargaDisparo()
        {
            Debug.Log("Recargando");
            yield return new WaitForSeconds(10f);
            _balas = 10;
            yield return new WaitForSeconds(10f);
            Debug.Log("Termine de Recargar");
            Debug.Log("¿Cuántas balas tengo cORTUINA?" + _balas);
            //Mnada llamar función de lo que sea
        }

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("Vida Enemigo" + _miEnemigo._vida);

            Debug.Log("Segundo");
            switch (_miEstado)
            {
                case EstadoJugador.Vivo:
                    break;
                case EstadoJugador.Muerto:

                    break;
                case EstadoJugador.Corriendo:
                    break;
                case EstadoJugador.Disparando:
                    break;
                case EstadoJugador.Agachado:
                    break;
                case EstadoJugador.Saltando:
                    break;
                case EstadoJugador.Recargando:
                    break;
                case EstadoJugador.Cubriendose:
                    break;
                default:
                    break;
            }
        }



        // Update is called once per frame
        void Update()
        {
            //if (!_corutinaEjecutandose) {
            //    _corutinaEjecutandose = true;
            //    StartCoroutine(RecargaDisparo());
            //}
        }

        private void FixedUpdate()
        {

        }

        private void LateUpdate()
        {

        }


    }
}
