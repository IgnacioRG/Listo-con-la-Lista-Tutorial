using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JuegoMemoriaSupermercado
{
    public class EnemigoE : MonoBehaviour
    {
        public delegate void EnemyAction(int vida);
        public static event EnemyAction RevelaVida;
        public static int Vida = 60;
        public int _vida = 60;
        //public static int Vida
        //{
        //    get
        //    {
        //        return _vida;
        //    }        
        //}


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                if (RevelaVida != null)
                    RevelaVida(_vida);
            }
        }
    }
}
